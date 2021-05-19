using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BankAdministration.Desktop.Model;
using BankAdministration.Desktop.VModel;
using BankAdministration.Desktop.View;

namespace BankAdministration.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private BankAdministrationApiService service_;
        private MainViewModel mainViewModel_;
        private MainWindow view_;
        private LoginViewModel loginViewModel_;
        private LoginWindow loginView_;


        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            service_ = new BankAdministrationApiService(ConfigurationManager.AppSettings["baseAddress"]);

            loginViewModel_ = new LoginViewModel(service_);
            loginViewModel_.LoginSucceeded += ViewModel_LoginSucceeded;
            loginViewModel_.LoginFailed += ViewModel_LoginFailed;
            loginViewModel_.MessageApplication += ViewModel_MessageApplication;
            loginView_ = new LoginWindow
            {
                DataContext = loginViewModel_
            };

            mainViewModel_ = new MainViewModel(service_);
            mainViewModel_.LogOutSucceeded += ViewModel_LogOutSucceeded;
            mainViewModel_.MessageApplication += ViewModel_MessageApplication;

            view_ = new MainWindow
            {
                DataContext = mainViewModel_
            };

            loginView_.Show();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "BankAdministration", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_LogOutSucceeded(object sender, EventArgs e)
        {
            view_.Hide();
            mainViewModel_.BankAccounts = null;
            mainViewModel_.Transactions = null;
            loginView_.Show();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Login failed!", "BankAdministration", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_LoginSucceeded(object sender, EventArgs e)
        {
            loginView_.Hide();
            mainViewModel_.UserName = loginViewModel_.UserName;
            mainViewModel_.LoadBankAccountsAsync();
            view_.Show();
        }
    }
}
