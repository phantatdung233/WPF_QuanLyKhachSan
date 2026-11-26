using HotelManagement.Services;
using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace HotelManagement.Views;

public sealed class NavigationSymbolConverter : IValueConverter
{
    public static SymbolRegular Resolve(NavigationKey key) => key switch
    {
        NavigationKey.RoomMap => SymbolRegular.GridDots24,
        NavigationKey.Booking => SymbolRegular.CalendarLtr24,
        NavigationKey.Room => SymbolRegular.Bed24,
        NavigationKey.RoomType => SymbolRegular.Tag24,
        NavigationKey.Customer => SymbolRegular.People24,
        NavigationKey.Service => SymbolRegular.DrinkCoffee24,
        NavigationKey.Invoice => SymbolRegular.ReceiptMoney24,
        NavigationKey.Report => SymbolRegular.DataTrending24,
        _ => SymbolRegular.GridDots24
    };

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is NavigationKey key ? Resolve(key) : SymbolRegular.GridDots24;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Binding.DoNothing;
}
