using System;
using System.Collections.Generic;
using System.Text;
using BankAdministration.Desktop.Model;

namespace BankAdministration.Desktop.VModel
{
    public class LockBankAccountViewModel : ViewModelBase
    {
        private readonly BankAdministrationApiService service_;
        private string text_;
        private bool isLocked_;

        public string Text
        {
            get => text_;
            set
            {
                text_ = value;
                OnPropertyChanged();
            }
        }

        /*public bool IsLocked
        {
            get => isLocked_;
            set
            {
                isLocked_ = value;
                OnPropertyChanged();
            }
        }*/

        public DelegateCommand YesCommand { get; private set; }
        public DelegateCommand NoCommand { get; private set; }

        public EventHandler YesEvent;
        public EventHandler NoEvent;

        public LockBankAccountViewModel(BankAdministrationApiService servie)
        {
            service_ = servie;
            YesCommand = new DelegateCommand(_ => YesAsync());
            NoCommand = new DelegateCommand(_ => NoAsync());
        }

        public void setText(bool isLocked)
        {
            ////IsLocked = isLocked;
            if (isLocked)
            {
                Text = "Are your sure to unlock this bank account?";
            }
            else
            {
                Text = "Are your sure to lock this bank account?";
            }
        }

        private async void YesAsync()
        {
            YesEvent?.Invoke(this, EventArgs.Empty);
        }

        private async void NoAsync()
        {
            NoEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
