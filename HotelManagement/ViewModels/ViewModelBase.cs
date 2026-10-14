using CommunityToolkit.Mvvm.ComponentModel;

namespace HotelManagement.ViewModels;

// Lớp cơ sở cho mọi ViewModel: cung cấp INotifyPropertyChanged,
// [ObservableProperty] và [RelayCommand] từ MVVM Toolkit.
public abstract class ViewModelBase : ObservableObject
{
}
