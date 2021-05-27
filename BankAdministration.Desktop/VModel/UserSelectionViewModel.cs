using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Data;
using BankAdministration.Desktop.Model;
using BankAdministration.Persistence.DTOS;

namespace BankAdministration.Desktop.VModel
{
    public class SelectedUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserViewModel)
                return value;
            return null;
        }
    }

    public class UserSelectionViewModel : ViewModelBase
    {
        private ObservableCollection<UserViewModel> users_;
        private UserViewModel selectedUser_;
        private readonly BankAdministrationApiService service_;
        public string UserName { get; set; }

        public ObservableCollection<UserViewModel> Users
        {
            get => users_;
            set
            {
                users_ = value;
                OnPropertyChanged();
            }
        }

        public UserViewModel SelectedUser
        {
            get => selectedUser_;
            set
            {
                selectedUser_ = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand SelectedUserCommand { get; private set; }
        public DelegateCommand LogOutCommand { get; private set; }
        public event EventHandler LogOutSucceeded;
        public event EventHandler UserSelectedSucceded;

        public UserSelectionViewModel(BankAdministrationApiService service)
        {
            service_ = service;
            UserName = string.Empty;
            SelectedUserCommand = new DelegateCommand(_ => LoadBankAccountsAsync(SelectedUser));
            LogOutCommand = new DelegateCommand(_ => LogOutAsync());
        }

        public async void LoadUsersAsync()
        {
            try
            {
                Users = new ObservableCollection<UserViewModel>((await service_.LoadUsersAsync()).Select(
                    user =>
                    {
                        var userVm = (UserViewModel)user;
                        return userVm;
                    }));
                Users.CollectionChanged += Users_CollectionChanged;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private void LoadBankAccountsAsync(UserViewModel user)
        {
            UserName = user.UserName;
            OnUserSelectedSucces();
        }

        private void OnUserSelectedSucces()
        {
            UserSelectedSucceded?.Invoke(this, EventArgs.Empty);
        }

        private async void LogOutAsync()
        {
            try
            {
                await service_.LogoutAsync();
                OnLogoutSuccess();
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private void OnLogoutSuccess()
        {
            LogOutSucceeded?.Invoke(this, EventArgs.Empty);
        }

        private void Users_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        { }
    }
}
