using AuraHotel.Models;
using AuraHotel.Services;
using AuraHotel.ViewModels;

namespace AuraHotel.Tests;

[Collection("UserSession")]
public class RoomTypeViewModelTests
{
    public RoomTypeViewModelTests()
    {
        UserSession.Clear();
    }

    [Fact]
    public async Task LoadRoomTypesCommand_TransitionsThroughLoadingToPopulatedSuccessWithFormattedPricesAndDeleteLocks()
    {
        var loadTcs = new TaskCompletionSource<IReadOnlyList<RoomType>>();
        var backend = new FakeRoomTypeBackend
        {
            OnGetAllAsync = () => loadTcs.Task
        };
        var viewModel = backend.CreateViewModel();

        var loadTask = viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        Assert.True(viewModel.IsLoading);
        Assert.False(viewModel.IsEmptyStateVisible);
        Assert.False(viewModel.IsTableVisible);
        Assert.False(viewModel.OpenCreateDialogCommand.CanExecute(null));

        loadTcs.SetResult(
        [
            new RoomType { RoomTypeId = 1, TypeName = "Đơn", PricePerDay = 500000m, RoomCount = 3 },
            new RoomType { RoomTypeId = 5, TypeName = "Penthouse", PricePerDay = 1200000m, RoomCount = 0 }
        ]);
        await loadTask;

        Assert.False(viewModel.IsLoading);
        Assert.False(viewModel.IsEmpty);
        Assert.False(viewModel.IsEmptyStateVisible);
        Assert.True(viewModel.IsTableVisible);
        Assert.False(viewModel.HasError);
        Assert.Equal(2, viewModel.FilteredRoomTypes.Count);

        var singleRoom = viewModel.FilteredRoomTypes[0];
        Assert.Equal("500.000 đ", singleRoom.PricePerDayDisplay);
        Assert.Equal(3, singleRoom.RoomCount);
        Assert.False(singleRoom.CanDelete);
        Assert.False(viewModel.DeleteRoomTypeCommand.CanExecute(singleRoom));
        Assert.Contains("3", singleRoom.DeleteToolTip, StringComparison.Ordinal);

        var penthouse = viewModel.FilteredRoomTypes[1];
        Assert.Equal("1.200.000 đ", penthouse.PricePerDayDisplay);
        Assert.Equal(0, penthouse.RoomCount);
        Assert.True(penthouse.CanDelete);
        Assert.True(viewModel.DeleteRoomTypeCommand.CanExecute(penthouse));
    }

    [Fact]
    public async Task EmptyState_DistinguishesEmptyCatalogFromUnmatchedSearchEvenAfterPriorBlockedDeleteError()
    {
        var backend = new FakeRoomTypeBackend();
        var viewModel = backend.CreateViewModel();

        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        Assert.True(viewModel.IsEmpty);
        Assert.True(viewModel.IsEmptyStateVisible);
        Assert.Equal("Chưa có loại phòng nào", viewModel.EmptyStateTitle);
        Assert.False(viewModel.HasActiveSearch);

        backend.Seed(
            new RoomType { RoomTypeId = 1, TypeName = "Đơn", PricePerDay = 500000m, RoomCount = 3 },
            new RoomType { RoomTypeId = 4, TypeName = "VIP", PricePerDay = 1200000m, RoomCount = 2 });
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        // Trigger a blocked-delete error first, then verify unmatched search clears stale error and shows search empty state
        await viewModel.DeleteRoomTypeCommand.ExecuteAsync(viewModel.FilteredRoomTypes[0]);
        Assert.True(viewModel.HasError);

        viewModel.SearchText = "Presidential";

        Assert.False(viewModel.HasError);
        Assert.True(viewModel.IsEmpty);
        Assert.True(viewModel.IsEmptyStateVisible);
        Assert.True(viewModel.HasActiveSearch);
        Assert.Equal("Không tìm thấy loại phòng phù hợp", viewModel.EmptyStateTitle);
        Assert.True(viewModel.ClearSearchCommand.CanExecute(null));

        viewModel.ClearSearchCommand.Execute(null);

        Assert.Equal(string.Empty, viewModel.SearchText);
        Assert.False(viewModel.HasActiveSearch);
        Assert.False(viewModel.IsEmpty);
        Assert.Equal(2, viewModel.FilteredRoomTypes.Count);
    }

    [Fact]
    public async Task SearchText_FiltersRoomTypesInRealTimeByNameAndCode()
    {
        var backend = new FakeRoomTypeBackend(
            new RoomType { RoomTypeId = 1, TypeName = "Đơn", PricePerDay = 500000m, RoomCount = 3 },
            new RoomType { RoomTypeId = 3, TypeName = "Gia đình", PricePerDay = 950000m, RoomCount = 3 },
            new RoomType { RoomTypeId = 4, TypeName = "VIP", PricePerDay = 1200000m, RoomCount = 2 });
        var viewModel = backend.CreateViewModel();
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        viewModel.SearchText = "gia đình";
        Assert.Single(viewModel.FilteredRoomTypes);
        Assert.Equal(3, viewModel.FilteredRoomTypes[0].RoomTypeId);

        viewModel.SearchText = "4";
        Assert.Single(viewModel.FilteredRoomTypes);
        Assert.Equal("VIP", viewModel.FilteredRoomTypes[0].TypeName);
    }

    [Fact]
    public async Task LoadRoomTypesCommand_ClearsPriorSuccessBannerAndSetsErrorMessageWithoutEmptyOverlapWhenDatabaseThrows()
    {
        var backend = new FakeRoomTypeBackend(
            new RoomType { RoomTypeId = 5, TypeName = "Penthouse", PricePerDay = 2000000m, RoomCount = 0 });
        var viewModel = backend.CreateViewModel();
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        await viewModel.DeleteRoomTypeCommand.ExecuteAsync(viewModel.FilteredRoomTypes.Single());
        Assert.True(viewModel.HasSuccessMessage);

        backend.OnGetAllAsync = static () => throw new InvalidOperationException("SQL connection failed");
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        Assert.False(viewModel.IsLoading);
        Assert.False(viewModel.HasSuccessMessage);
        Assert.True(viewModel.HasError);
        Assert.False(viewModel.IsEmpty);
        Assert.False(viewModel.IsEmptyStateVisible);
    }

    [Theory]
    [InlineData("", "500000", true, false)]
    [InlineData("VIP", "500000", true, false)]
    [InlineData("   vip   ", "900000", true, false)]
    [InlineData("Suite", "", false, true)]
    [InlineData("Suite", "0", false, true)]
    [InlineData("Suite", "0.4", false, true)]
    [InlineData("Suite", "-200000", false, true)]
    [InlineData("Suite", "abc", false, true)]
    public async Task SaveCommand_OnCreate_ShowsInlineErrorsAndSkipsDaoForInvalidOrDuplicateInput(
        string typeName,
        string priceInput,
        bool expectedNameError,
        bool expectedPriceError)
    {
        var backend = new FakeRoomTypeBackend(
            new RoomType { RoomTypeId = 4, TypeName = "VIP", PricePerDay = 1200000m, RoomCount = 2 });
        var viewModel = backend.CreateViewModel();
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        viewModel.OpenCreateDialogCommand.Execute(null);
        viewModel.Dialog.TypeName = typeName;
        viewModel.Dialog.PriceInput = priceInput;

        await viewModel.Dialog.SaveCommand.ExecuteAsync(null);

        Assert.Equal(0, backend.InsertCalls);
        Assert.True(viewModel.Dialog.IsOpen);
        Assert.Equal(expectedNameError, viewModel.Dialog.HasTypeNameError);
        Assert.Equal(expectedPriceError, viewModel.Dialog.HasPricePerDayError);
    }

    [Fact]
    public async Task TypingIntoDialogInputs_ClearsCorrespondingInlineValidationErrors()
    {
        var backend = new FakeRoomTypeBackend();
        var viewModel = backend.CreateViewModel();
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        viewModel.OpenCreateDialogCommand.Execute(null);
        await viewModel.Dialog.SaveCommand.ExecuteAsync(null);

        Assert.True(viewModel.Dialog.HasTypeNameError);
        Assert.True(viewModel.Dialog.HasPricePerDayError);

        viewModel.Dialog.TypeName = "Deluxe";
        Assert.False(viewModel.Dialog.HasTypeNameError);
        Assert.True(viewModel.Dialog.HasPricePerDayError);

        viewModel.Dialog.PriceInput = "850.000";
        Assert.False(viewModel.Dialog.HasPricePerDayError);
    }

    [Fact]
    public async Task CreateAndEditRoomType_PersistsChangesReloadsFormattedRowsAndClosesDialog()
    {
        var backend = new FakeRoomTypeBackend(
            new RoomType { RoomTypeId = 1, TypeName = "Đơn", PricePerDay = 500000m, RoomCount = 3 });
        var viewModel = backend.CreateViewModel();
        var dialogOpenedCount = 0;
        var dialogClosedCount = 0;
        viewModel.DialogRequested += () => dialogOpenedCount++;
        viewModel.Dialog.CloseRequested += () => dialogClosedCount++;

        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        // Create new RoomType
        viewModel.OpenCreateDialogCommand.Execute(null);
        Assert.True(viewModel.Dialog.IsOpen);
        Assert.False(viewModel.Dialog.IsEditMode);
        Assert.Equal(1, dialogOpenedCount);

        viewModel.Dialog.TypeName = "  Deluxe  ";
        viewModel.Dialog.PriceInput = "850.000";
        await viewModel.Dialog.SaveCommand.ExecuteAsync(null);

        Assert.Equal(1, backend.InsertCalls);
        Assert.False(viewModel.Dialog.IsOpen);
        Assert.Equal(1, dialogClosedCount);
        Assert.True(viewModel.HasSuccessMessage);
        Assert.Equal(2, viewModel.FilteredRoomTypes.Count);

        var createdRow = viewModel.FilteredRoomTypes.Single(item => item.TypeName == "Deluxe");
        Assert.Equal("850.000 đ", createdRow.PricePerDayDisplay);

        // Edit existing RoomType (keeping same case-adjusted name + new price)
        viewModel.OpenEditDialogCommand.Execute(createdRow);
        Assert.True(viewModel.Dialog.IsOpen);
        Assert.True(viewModel.Dialog.IsEditMode);
        Assert.Equal(createdRow.RoomTypeId, viewModel.Dialog.EditingRoomTypeId);

        viewModel.Dialog.TypeName = "Deluxe";
        viewModel.Dialog.PriceInput = "920000";
        await viewModel.Dialog.SaveCommand.ExecuteAsync(null);

        Assert.Equal(1, backend.UpdateCalls);
        Assert.False(viewModel.Dialog.IsOpen);
        Assert.Equal(2, dialogClosedCount);
        Assert.Equal(
            "920.000 đ",
            viewModel.FilteredRoomTypes.Single(item => item.RoomTypeId == createdRow.RoomTypeId).PricePerDayDisplay);
    }

    [Fact]
    public async Task DeleteRoomTypeCommand_BlocksWhenRoomCountPositiveAndDeletesWhenRoomCountZero()
    {
        var backend = new FakeRoomTypeBackend(
            new RoomType { RoomTypeId = 1, TypeName = "Đơn", PricePerDay = 500000m, RoomCount = 3 },
            new RoomType { RoomTypeId = 5, TypeName = "Penthouse", PricePerDay = 2000000m, RoomCount = 0 });
        var viewModel = backend.CreateViewModel();
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        var lockedRow = viewModel.FilteredRoomTypes.Single(item => item.RoomTypeId == 1);
        var deletableRow = viewModel.FilteredRoomTypes.Single(item => item.RoomTypeId == 5);

        await viewModel.DeleteRoomTypeCommand.ExecuteAsync(lockedRow);
        Assert.Equal(0, backend.DeleteCalls);
        Assert.True(viewModel.HasError);
        Assert.Equal(2, viewModel.FilteredRoomTypes.Count);

        await viewModel.DeleteRoomTypeCommand.ExecuteAsync(deletableRow);
        Assert.Equal(1, backend.DeleteCalls);
        Assert.False(viewModel.HasError);
        Assert.True(viewModel.HasSuccessMessage);
        Assert.Single(viewModel.FilteredRoomTypes);
    }

    [Fact]
    public async Task EditAndSave_BlocksDuplicateWithAnotherRoomTypeAndHandlesSaveFailureInsideDialog()
    {
        var backend = new FakeRoomTypeBackend(
            new RoomType { RoomTypeId = 1, TypeName = "Đơn", PricePerDay = 500000m, RoomCount = 3 },
            new RoomType { RoomTypeId = 4, TypeName = "VIP", PricePerDay = 1200000m, RoomCount = 2 });
        var viewModel = backend.CreateViewModel();
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        var vipRow = viewModel.FilteredRoomTypes.Single(item => item.RoomTypeId == 4);
        viewModel.OpenEditDialogCommand.Execute(vipRow);
        viewModel.Dialog.TypeName = "Đơn";

        await viewModel.Dialog.SaveCommand.ExecuteAsync(null);
        Assert.Equal(0, backend.UpdateCalls);
        Assert.True(viewModel.Dialog.HasTypeNameError);
        Assert.True(viewModel.Dialog.IsOpen);

        backend.OnUpdateAsync = static _ => throw new InvalidOperationException("DB write failed");
        viewModel.Dialog.TypeName = "VIP Thượng Hạng";
        await viewModel.Dialog.SaveCommand.ExecuteAsync(null);

        Assert.True(viewModel.Dialog.HasError);
        Assert.True(viewModel.Dialog.IsOpen);
        Assert.False(viewModel.Dialog.IsSaving);
    }

    [Fact]
    public async Task DeleteRoomTypeCommand_WhenConcurrentRoomAddedInDatabase_RefreshesLockStateAndShowsError()
    {
        var backend = new FakeRoomTypeBackend(
            new RoomType { RoomTypeId = 5, TypeName = "Penthouse", PricePerDay = 2000000m, RoomCount = 0 });
        var viewModel = backend.CreateViewModel();
        await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);

        var targetRow = viewModel.FilteredRoomTypes.Single();
        Assert.True(targetRow.CanDelete);

        // Simulate another user assigning 2 rooms right before safe DELETE runs
        backend.Seed(new RoomType { RoomTypeId = 5, TypeName = "Penthouse", PricePerDay = 2000000m, RoomCount = 2 });

        await viewModel.DeleteRoomTypeCommand.ExecuteAsync(targetRow);

        Assert.True(viewModel.HasError);
        Assert.Contains("2", viewModel.ErrorMessage!, StringComparison.Ordinal);
        Assert.False(viewModel.FilteredRoomTypes.Single().CanDelete);
    }

    [Fact]
    public async Task StaffSession_CannotNavigateToRoomTypeOrExecuteRoomTypeViewModelOperations()
    {
        var staffUser = new User
        {
            UserId = 2,
            Username = "staff",
            Password = "123456",
            FullName = "Lê Thu Ngân",
            Role = RolePermissions.Staff
        };
        UserSession.Start(staffUser);

        var mainViewModel = new MainViewModel(staffUser);
        Assert.DoesNotContain(mainViewModel.NavigationItems, item => item.Key == NavigationKey.RoomType);

        mainViewModel.NavigateCommand.Execute(NavigationKey.RoomType);
        Assert.IsNotType<RoomTypeViewModel>(mainViewModel.CurrentView);
        Assert.True(mainViewModel.HasError);

        var backend = new FakeRoomTypeBackend(
            new RoomType { RoomTypeId = 5, TypeName = "Penthouse", PricePerDay = 2000000m, RoomCount = 0 });
        var roomTypeViewModel = backend.CreateViewModel(fallbackRole: RolePermissions.Staff);

        Assert.False(roomTypeViewModel.IsAdministratorAccess);
        Assert.False(roomTypeViewModel.OpenCreateDialogCommand.CanExecute(null));

        await roomTypeViewModel.LoadRoomTypesCommand.ExecuteAsync(null);
        Assert.Equal(0, backend.GetAllCalls);
        Assert.True(roomTypeViewModel.HasError);
    }

    private sealed class FakeRoomTypeBackend
    {
        private readonly List<RoomType> _items = [];
        private int _nextId = 10;

        public Func<Task<IReadOnlyList<RoomType>>>? OnGetAllAsync { get; set; }

        public Func<RoomType, Task<bool>>? OnUpdateAsync { get; set; }

        public int GetAllCalls { get; private set; }

        public int InsertCalls { get; private set; }

        public int UpdateCalls { get; private set; }

        public int DeleteCalls { get; private set; }

        public FakeRoomTypeBackend(params RoomType[] initialItems)
        {
            Seed(initialItems);
        }

        public void Seed(params RoomType[] items)
        {
            _items.Clear();
            _items.AddRange(items.Select(Clone));
            _nextId = _items.Count == 0 ? 1 : _items.Max(i => i.RoomTypeId) + 1;
        }

        public RoomTypeViewModel CreateViewModel(string fallbackRole = RolePermissions.Administrator) =>
            new(
                getAllAsync: () =>
                {
                    GetAllCalls++;
                    if (OnGetAllAsync is not null)
                    {
                        return OnGetAllAsync();
                    }

                    IReadOnlyList<RoomType> snapshot = _items
                        .OrderBy(item => item.RoomTypeId)
                        .Select(Clone)
                        .ToList();
                    return Task.FromResult(snapshot);
                },
                insertAsync: roomType =>
                {
                    InsertCalls++;
                    var assignedId = _nextId++;
                    _items.Add(new RoomType
                    {
                        RoomTypeId = assignedId,
                        TypeName = roomType.TypeName.Trim(),
                        PricePerDay = roomType.PricePerDay,
                        RoomCount = 0
                    });
                    return Task.FromResult(assignedId);
                },
                updateAsync: roomType =>
                {
                    UpdateCalls++;
                    if (OnUpdateAsync is not null)
                    {
                        return OnUpdateAsync(roomType);
                    }

                    var existing = _items.FirstOrDefault(item => item.RoomTypeId == roomType.RoomTypeId);
                    if (existing is null)
                    {
                        return Task.FromResult(false);
                    }

                    existing.TypeName = roomType.TypeName.Trim();
                    existing.PricePerDay = roomType.PricePerDay;
                    return Task.FromResult(true);
                },
                deleteAsync: roomTypeId =>
                {
                    DeleteCalls++;
                    var existing = _items.FirstOrDefault(item => item.RoomTypeId == roomTypeId);
                    if (existing is null || existing.RoomCount > 0)
                    {
                        return Task.FromResult(false);
                    }

                    _items.Remove(existing);
                    return Task.FromResult(true);
                },
                fallbackRole: fallbackRole);

        private static RoomType Clone(RoomType source) =>
            new()
            {
                RoomTypeId = source.RoomTypeId,
                TypeName = source.TypeName,
                PricePerDay = source.PricePerDay,
                RoomCount = source.RoomCount
            };
    }
}
