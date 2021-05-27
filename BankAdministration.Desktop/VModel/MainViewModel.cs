using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Windows.Data;
using BankAdministration.Desktop.Model;
using BankAdministration.Persistence.DTOS;

namespace BankAdministration.Desktop.VModel
{
    public class SelectedBankAccountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BankAccountViewModel)
                return value;
            return null;
        }
    }

    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<BankAccountViewModel> bankAccounts_;
        private ObservableCollection<TransactionViewModel> transactions_;
        private BankAccountViewModel selectedBankAccount_;
        private TransactionViewModel selectedTransaction_;
        private readonly BankAdministrationApiService service_;

        public ObservableCollection<BankAccountViewModel> BankAccounts
        {
            get => bankAccounts_;
            set
            {
                bankAccounts_ = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TransactionViewModel> Transactions
        {
            get => transactions_;
            set
            {
                transactions_ = value;
                OnPropertyChanged();
            }
        }

        public BankAccountViewModel SelectedBankAccount
        {
            get => selectedBankAccount_;
            set
            {
                selectedBankAccount_ = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelectedBankAccountNull
        {
            get => selectedBankAccount_ is null;
        }

        public TransactionViewModel SelectedTransaction
        {
            get => selectedTransaction_;
            set
            {
                selectedTransaction_ = value;
                OnPropertyChanged();
            }
        }

        public string UserName { get; set; }

        public DelegateCommand SelectCommand { get; private set; }

        public DelegateCommand RefreshBankAccountCommand { get; private set; }

        public DelegateCommand SelectBankAccountCommand { get; private set; }

        public DelegateCommand AddNewBankAccount { get; private set; }

        public DelegateCommand LogOutCommand { get; private set; }

        public DelegateCommand NewDepositCommand { get; private set; }

        public DelegateCommand NewWithdrawnCommand { get; private set; }

        public DelegateCommand NewTransferCommand { get; private set; }

        public DelegateCommand LockBankAccountCommand { get; private set; }

        public event EventHandler LogOutSucceeded;

        public event EventHandler LockRequested;

        public event EventHandler DepositRequested;

        public event EventHandler TransferRequested;

        public event EventHandler WithdrawnRequested;

        public MainViewModel(BankAdministrationApiService service)
        {
            service_ = service;

            RefreshBankAccountCommand = new DelegateCommand(_ => LoadBankAccountsAsync());
            SelectBankAccountCommand = new DelegateCommand(_ => LoadTransactionsAsync(SelectedBankAccount));
            LogOutCommand = new DelegateCommand(_ => LogOutAsync());

            NewDepositCommand = new DelegateCommand(_ => !(SelectedBankAccount is null),_ => NewDeposit());
            NewWithdrawnCommand = new DelegateCommand(_ => !(SelectedBankAccount is null), _ => NewWithdrawn());
            NewTransferCommand = new DelegateCommand(_ => !(SelectedBankAccount is null), _ => NewTransfer());
            LockBankAccountCommand = new DelegateCommand(_ => !(SelectedBankAccount is null), _ => LockBankAccount());
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

        public async void LoadBankAccountsAsync()
        {
            if (UserName is null)
                return;
            try
            {
                BankAccounts = new ObservableCollection<BankAccountViewModel>((await service_.LoadBankAccountsAsync(UserName)).Select(
                    bankAccount =>
                    {
                        var bankAccountVm = (BankAccountViewModel)bankAccount;
                        return bankAccountVm;
                    }));
                BankAccounts.CollectionChanged += BankAccounts_CollectionChanged;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public async void LoadTransactionsAsync(BankAccountViewModel bankAccount)
        {
            if (bankAccount is null)
                return;
            try
            {
                Transactions = new ObservableCollection<TransactionViewModel>(
                    (await service_.LoadTransactionsAsync(bankAccount.Number))
                    .Select(
                    transaction =>
                    {
                        var transactionVm = (TransactionViewModel)transaction;
                        return transactionVm;
                    }));
                Transactions.CollectionChanged += Transactions_CollectionChanged;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private void BankAccounts_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e){}

        private void Transactions_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e){}

        private void NewDeposit()
        {
            DepositRequested?.Invoke(this, EventArgs.Empty);
        }

        private void NewWithdrawn()
        {
            WithdrawnRequested?.Invoke(this, EventArgs.Empty);
        }

        private void NewTransfer()
        {
            TransferRequested?.Invoke(this, EventArgs.Empty);
        }

        private void LockBankAccount()
        {
            LockRequested?.Invoke(this, EventArgs.Empty);
        }


    }
}
