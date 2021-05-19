using System;
using System.Collections.Generic;
using System.Text;
using BankAdministration.Persistence.Models;
using BankAdministration.Persistence.DTOS;

namespace BankAdministration.Desktop.VModel
{
    public class TransactionViewModel : ViewModelBase
    {
        private TransactionTypeEnum transactionType_;
        private string sourceAccountNumber_;
        private string destinationAccountNumber_;
        private String destinationAccountUserName_;
        private Int64 amount_;
        private Int64 oldBalance_;
        private Int64 newBalance_;
        private DateTime transactionTime_;

        public TransactionTypeEnum TransactionType
        {
            get => transactionType_;
            set
            {
                transactionType_ = value;
                OnPropertyChanged();
            }
        }

        public string SourceAccountNumber
        {
            get => sourceAccountNumber_;
            set
            {
                sourceAccountNumber_ = value;
                OnPropertyChanged();
            }
        }

        public string DestinationAccountNumber
        {
            get => destinationAccountNumber_;
            set
            {
                destinationAccountNumber_ = value;
                OnPropertyChanged();
            }
        }

        public String DestinationAccountUserName
        {
            get => destinationAccountUserName_;
            set
            {
                destinationAccountUserName_ = value;
                OnPropertyChanged();
            }
        }

        public Int64 Amount
        {
            get => amount_;
            set
            {
                amount_ = value;
                OnPropertyChanged();
            }
        }

        public Int64 OldBalance
        {
            get => oldBalance_;
            set
            {
                oldBalance_ = value;
                OnPropertyChanged();
            }
        }

        public Int64 NewBalance
        {
            get => newBalance_;
            set
            {
                newBalance_ = value;
                OnPropertyChanged();
            }
        }

        public DateTime TransactionTime
        {
            get => transactionTime_;
            set
            {
                transactionTime_ = value;
                OnPropertyChanged();
            }
        }

        public static explicit operator TransactionViewModel(TransactionDto dto) => new TransactionViewModel
        {
            TransactionType = dto.TransactionType,
            SourceAccountNumber = dto.SourceAccountNumber,
            DestinationAccountNumber = dto.DestinationAccountNumber,
            DestinationAccountUserName = dto.DestinationAccountUserName,
            Amount = dto.Amount,
            OldBalance = dto.OldBalance,
            NewBalance = dto.NewBalance,
            TransactionTime = dto.TransactionTime
        };

        public static explicit operator TransactionDto(TransactionViewModel vm) => new TransactionDto
        {
            TransactionType = vm.TransactionType,
            SourceAccountNumber = vm.SourceAccountNumber,
            DestinationAccountNumber = vm.DestinationAccountNumber,
            DestinationAccountUserName = vm.DestinationAccountUserName,
            Amount = vm.Amount,
            OldBalance = vm.OldBalance,
            NewBalance = vm.NewBalance,
            TransactionTime = vm.TransactionTime
        };
    }
}
