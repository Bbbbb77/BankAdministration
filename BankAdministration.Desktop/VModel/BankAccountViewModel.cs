using System;
using System.Collections.Generic;
using System.Text;
using BankAdministration.Persistence.DTOS;

namespace BankAdministration.Desktop.VModel
{
    public class BankAccountViewModel : ViewModelBase
    {
        private String number_;
        private Int64 balance_;
        private Boolean isLocked_;
        private DateTime createdDate_;

        public String Number
        {
            get => number_;
            set
            {
                number_ = value;
                OnPropertyChanged();
            }
        }

        public Int64 Balance
        {
            get => balance_;
            set
            {
                balance_ = value;
                OnPropertyChanged();
            }
        }

        public Boolean IsLocked
        {
            get => isLocked_;
            set
            {
                isLocked_ = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreatedDate
        {
            get => createdDate_;
            set
            {
                createdDate_ = value;
                OnPropertyChanged();
            }
        }

        public static explicit operator BankAccountViewModel(BankAccountDto dto) => new BankAccountViewModel
        {
            Number = dto.Number,
            Balance = dto.Balance,
            IsLocked = dto.IsLocked,
            CreatedDate = dto.CreatedDate
        };

        public static explicit operator BankAccountDto(BankAccountViewModel vm) => new BankAccountDto
        {
            Number = vm.Number,
            Balance = vm.Balance,
            IsLocked = vm.IsLocked,
            CreatedDate = vm.CreatedDate
        };
    }
}
