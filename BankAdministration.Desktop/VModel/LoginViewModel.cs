using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using BankAdministration.Desktop.Model;

namespace BankAdministration.Desktop.VModel
{
    class LoginViewModel : ViewModelBase
    {
        private readonly BankAdministrationApiService service_;
        private bool isLoading_;

        public DelegateCommand LoginCommand { get; private set; }
        public string UserName { get; set; }
        public bool IsLoading
        {
            get => isLoading_;
            set
            {
                isLoading_ = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler LoginSucceeded;
        public event EventHandler LoginFailed;

        public LoginViewModel(BankAdministrationApiService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            service_ = service;
            UserName = string.Empty;
            IsLoading = false;

            LoginCommand = new DelegateCommand(_ => !IsLoading, p => LoginAsync(p as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null)
                return;

            try
            {
                IsLoading = true;
                bool result = await service_.LoginAsync(UserName, passwordBox.Password);
                IsLoading = false;

                if (result)
                    OnLoginSuccess();
                else
                    OnLoginFailed();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private void OnLoginSuccess()
        {
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
