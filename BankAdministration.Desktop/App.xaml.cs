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
        private MainWindow mainView_;
        private LoginViewModel loginViewModel_;
        private LoginWindow loginView_;
        private UserSelectionViewModel userSelectionViewModel_;
        private UserWindow userView_;

        private DepositWindow depositView_;
        private LockBankAccountWindow lockBankAccount_;
        private TransferWindow transferView_;
        private WithdrawnWindow withdrawnWindow_;

        private LockBankAccountViewModel lockBankAccountViewModel_;
        private DepositViewModel depositViewModel_;
        private TransferViewModel transferViewModel_;
        private WithdrawnViewModel withdrawnViewModel_;

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

            userSelectionViewModel_ = new UserSelectionViewModel(service_);
            userSelectionViewModel_.LogOutSucceeded += ViewModel_LogOutSucceeded;
            userSelectionViewModel_.UserSelectedSucceded += ViewModel_SelectUserSucceded;
            userSelectionViewModel_.MessageApplication += ViewModel_MessageApplication;
            userView_ = new UserWindow
            {
                DataContext = userSelectionViewModel_
            };

            mainViewModel_ = new MainViewModel(service_);
            mainViewModel_.LogOutSucceeded += ViewModel_LogOutSucceeded;
            mainViewModel_.MessageApplication += ViewModel_MessageApplication;
            mainViewModel_.LockRequested += ViewModel_LockBankAccount;
            mainViewModel_.TransferRequested += ViewModel_NewTransfer;
            mainViewModel_.DepositRequested += ViewModel_NewDeposit;
            mainViewModel_.WithdrawnRequested += ViewModel_NewWithdrawn;
            mainView_ = new MainWindow
            {
                DataContext = mainViewModel_
            };

            lockBankAccountViewModel_ = new LockBankAccountViewModel(service_);
            lockBankAccountViewModel_.YesEvent += ViewModel_LockBankAccountFinishedYes;
            lockBankAccountViewModel_.NoEvent += ViewModel_LockBankAccountFinishedNo;

            depositViewModel_ = new DepositViewModel();
            depositViewModel_.YesEvent += ViewModel_NewDepositFinishedYes;
            depositViewModel_.NoEvent += ViewModel_NewDepositFinishedNo;

            transferViewModel_ = new TransferViewModel();
            transferViewModel_.YesEvent += ViewModel_NewTransferFinishedYes;
            transferViewModel_.NoEvent += ViewModel_NewTransferFinishedNo;

            withdrawnViewModel_ = new WithdrawnViewModel();
            withdrawnViewModel_.YesEvent += ViewModel_NewWithdrawnFinishedYes;
            withdrawnViewModel_.NoEvent += ViewModel_NewWithdrawnFinishedNo;

            loginView_.Show();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "BankAdministration", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_LogOutSucceeded(object sender, EventArgs e)
        {
            mainView_.Hide();
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
            userSelectionViewModel_.LoadUsersAsync();
            userView_.Show();
        }

        private void ViewModel_SelectUserSucceded(object sender, EventArgs e)
        {
            userView_.Hide();
            mainViewModel_.UserName = userSelectionViewModel_.UserName;
            mainViewModel_.LoadBankAccountsAsync();
            mainView_.Show();
        }

        private void ViewModel_NewDeposit(object sender, EventArgs e) {
            depositView_ = new DepositWindow
            {
                DataContext = depositViewModel_
            };
            depositView_.ShowDialog();
        }
        private void ViewModel_NewDepositFinishedYes(object sender, EventArgs e) { 
            if(depositView_.IsActive)
            {
                depositView_.Close();
            }
            var amount = depositViewModel_.DepositAmount;
            var sourceNumber = mainViewModel_.SelectedBankAccount.Number;

            service_.SetDeposit(sourceNumber, amount);
        }
        private void ViewModel_NewDepositFinishedNo(object sender, EventArgs e)
        {
            if (depositView_.IsActive)
            {
                depositView_.Close();
            }
        }

        private void ViewModel_LockBankAccount(object sender, EventArgs e) {
            lockBankAccount_ = new LockBankAccountWindow
            {
                DataContext = lockBankAccountViewModel_
            };
            var b = mainViewModel_.SelectedBankAccount.IsLocked;

            lockBankAccountViewModel_.setText(b);

            lockBankAccount_.ShowDialog();
        }
        private void ViewModel_LockBankAccountFinishedYes(object sender, EventArgs e) {
            if (lockBankAccount_.IsActive)
            {
                lockBankAccount_.Close();
            }
            service_.SetBankAccountLock(!mainViewModel_.SelectedBankAccount.IsLocked, 
                                        mainViewModel_.SelectedBankAccount.Number);
        }
        private void ViewModel_LockBankAccountFinishedNo(object sender, EventArgs e)
        {
            if (lockBankAccount_.IsActive)
            {
                lockBankAccount_.Close();
            }
        }

        private void ViewModel_NewTransfer(object sender, EventArgs e) {
            transferView_ = new TransferWindow
            {
                DataContext = transferViewModel_
            };
            transferView_.ShowDialog();
        }
        private void ViewModel_NewTransferFinishedYes(object sender, EventArgs e) {
            if (transferView_.IsActive)
            {
                transferView_.Close();
            }
            var amount = transferViewModel_.Amount;
            var destNumber = transferViewModel_.DestNumber;
            var destUserName = transferViewModel_.DestUserName;
            var sourceNumber = mainViewModel_.SelectedBankAccount.Number;

            service_.SetTransfer(sourceNumber, destNumber, destUserName, amount);
        }
        private void ViewModel_NewTransferFinishedNo(object sender, EventArgs e)
        {
            if (transferView_.IsActive)
            {
                transferView_.Close();
            }
        }

        private void ViewModel_NewWithdrawn(object sender, EventArgs e) {
            withdrawnWindow_ = new WithdrawnWindow
            {
                DataContext = withdrawnViewModel_
            };
            withdrawnWindow_.ShowDialog();
        }
        private void ViewModel_NewWithdrawnFinishedYes(object sender, EventArgs e) { 
            if(withdrawnWindow_.IsActive)
            {
                withdrawnWindow_.Close();
            }
            var amount = withdrawnViewModel_.WithdrawnAmount;
            var sourceNumber = mainViewModel_.SelectedBankAccount.Number;
            service_.SetWithdrawn(sourceNumber, amount);
        }
        private void ViewModel_NewWithdrawnFinishedNo(object sender, EventArgs e)
        {
            if (withdrawnWindow_.IsActive)
            {
                withdrawnWindow_.Close();
            }
        }
    }
}
