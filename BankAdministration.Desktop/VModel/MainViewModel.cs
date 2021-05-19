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
        private readonly BankAdministrationApiService service_;
        private BankAccountViewModel selectedBankAccount_;
        private TransactionViewModel selectedTransaction_;

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

        public event EventHandler LogOutSucceeded;

        public MainViewModel(BankAdministrationApiService service)
        {
            service_ = service;

            RefreshBankAccountCommand = new DelegateCommand(_ => LoadBankAccountsAsync());
            SelectBankAccountCommand = new DelegateCommand(_ => LoadTransactionsAsync(SelectedBankAccount));
            LogOutCommand = new DelegateCommand(_ => LogOutAsync());
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

        private async void BankAccounts_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e){}

        private async void Transactions_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e){}
    }
}
