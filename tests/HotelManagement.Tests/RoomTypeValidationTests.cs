using HotelManagement.Models;
using HotelManagement.Services;

namespace HotelManagement.Tests;

public class RoomTypeValidationTests
{
    [Theory]
    [InlineData(0, "0 đ")]
    [InlineData(500000, "500.000 đ")]
    [InlineData(950000, "950.000 đ")]
    [InlineData(1070000, "1.070.000 đ")]
    [InlineData(1200000, "1.200.000 đ")]
    public void FormatVnd_FormatsAmountsWithDotThousandsSeparatorAndCurrencySuffix(
         int amount,
        string expected)
    {
        var formatted = CurrencyFormatter.FormatVnd(amount);

        Assert.Equal(expected, formatted);
    }

    [Theory]
    [InlineData("500000", 500000)]
    [InlineData("  700000  ", 700000)]
    [InlineData("500.000", 500000)]
    [InlineData("1.200.000", 1200000)]
    [InlineData("950.000 đ", 950000)]
    [InlineData("500,000", 500000)]
    public void TryParseVndAmount_ParsesPlainAndGroupedVietnameseCurrencyInputs(
        string rawInput,
        int expectedAmount)
    {
        var parsed = CurrencyFormatter.TryParseVndAmount(rawInput, out var actual);

        Assert.True(parsed);
        Assert.Equal((decimal)expectedAmount, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateRoomTypeName_RejectsBlankInput(string? input)
    {
        var error = ValidationRules.ValidateRoomTypeName(input, []);

        Assert.NotNull(error);
    }

    [Fact]
    public void ValidateRoomTypeName_RejectsNameExceedingOneHundredCharacters()
    {
        var tooLong = new string('A', 101);

        var error = ValidationRules.ValidateRoomTypeName(tooLong, []);

        Assert.NotNull(error);
    }

    [Theory]
    [InlineData("VIP")]
    [InlineData("vip")]
    [InlineData("  Vip  ")]
    [InlineData("Gia đình")]
    [InlineData("GIA ĐÌNH")]
    public void ValidateRoomTypeName_RejectsCaseInsensitiveDuplicateOnCreate(string candidateName)
    {
        RoomType[] existing =
        [
            new() { RoomTypeId = 3, TypeName = "Gia đình", PricePerDay = 950000m },
            new() { RoomTypeId = 4, TypeName = "VIP", PricePerDay = 1200000m }
        ];

        var error = ValidationRules.ValidateRoomTypeName(candidateName, existing);

        Assert.Equal("Tên loại phòng đã tồn tại.", error);
    }

    [Fact]
    public void ValidateRoomTypeName_AllowsKeepingOwnNameWhenEditingSameRoomTypeAndBlocksOtherDuplicates()
    {
        RoomType[] existing =
        [
            new() { RoomTypeId = 1, TypeName = "Đơn", PricePerDay = 500000m },
            new() { RoomTypeId = 4, TypeName = "VIP", PricePerDay = 1200000m }
        ];

        var ownNameError = ValidationRules.ValidateRoomTypeName("  vip ", existing, editingRoomTypeId: 4);
        var otherDuplicateError = ValidationRules.ValidateRoomTypeName("Đơn", existing, editingRoomTypeId: 4);

        Assert.Null(ownNameError);
        Assert.Equal("Tên loại phòng đã tồn tại.", otherDuplicateError);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc")]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("-500.000")]
    public void ValidateRoomTypePrice_RejectsBlankInvalidZeroAndNegativePrices(string? rawPrice)
    {
        var error = ValidationRules.ValidateRoomTypePrice(rawPrice, out _);

        Assert.NotNull(error);
    }

    [Theory]
    [InlineData("500000", 500000)]
    [InlineData("1.200.000", 1200000)]
    public void ValidateRoomTypePrice_AcceptsPositivePrices(string rawPrice, int expectedPrice)
    {
        var error = ValidationRules.ValidateRoomTypePrice(rawPrice, out var parsedPrice);

        Assert.Null(error);
        Assert.Equal((decimal)expectedPrice, parsedPrice);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(3, false)]
    public void CanDeleteRoomType_LocksDeletionWheneverRoomCountExceedsZero(
        int roomCount,
        bool expectedCanDelete)
    {
        Assert.Equal(expectedCanDelete, ValidationRules.CanDeleteRoomType(roomCount));

        var lockReason = ValidationRules.GetRoomTypeDeleteLockReason(roomCount);
        if (expectedCanDelete)
        {
            Assert.Null(lockReason);
        }
        else
        {
            Assert.NotNull(lockReason);
            Assert.Contains(roomCount.ToString(), lockReason, StringComparison.Ordinal);
        }
    }
}
