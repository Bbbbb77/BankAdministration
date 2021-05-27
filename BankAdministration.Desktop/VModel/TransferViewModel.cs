using System;
using System.Collections.Generic;
using System.Text;

namespace BankAdministration.Desktop.VModel
{
    public class TransferViewModel : ViewModelBase
    {
        private Int64 amount_;
        private string destNumber_;
        private string destUserName_;

        public Int64 Amount
        {
            get => amount_;
            set
            {
                amount_ = value;
                OnPropertyChanged();
            }
        }

        public string DestNumber
        {
            get => destNumber_;
            set
            {
                destNumber_ = value;
                OnPropertyChanged();
            }
        }

        public string DestUserName
        {
            get => destUserName_;
            set
            {
                destUserName_ = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand YesCommand { get; private set; }
        public DelegateCommand NoCommand { get; private set; }

        public EventHandler YesEvent;
        public EventHandler NoEvent;

        public TransferViewModel()
        {
            YesCommand = new DelegateCommand(_ => YesAsync());
            NoCommand = new DelegateCommand(_ => NoAsync());
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
