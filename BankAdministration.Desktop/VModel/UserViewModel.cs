using System;
using System.Collections.Generic;
using System.Text;
using BankAdministration.Persistence.Models;
using BankAdministration.Persistence.DTOS;

namespace BankAdministration.Desktop.VModel
{
    public class UserViewModel : ViewModelBase
    {
        private string userName_;
        private string fullName_;

        public string UserName
        {
            get => userName_;
            set
            {
                userName_ = value;
                OnPropertyChanged();
            }
        }

        public string FullName
        {
            get => fullName_;
            set
            {
                fullName_ = value;
                OnPropertyChanged();
            }
        }

        public static explicit operator UserViewModel(UserDto dto) => new UserViewModel
        {
            UserName = dto.UserName,
            FullName = dto.FullName
        };

        public static explicit operator UserDto(UserViewModel vm) => new UserDto
        {
            UserName = vm.UserName,
            FullName = vm.FullName
        };
    }
}
