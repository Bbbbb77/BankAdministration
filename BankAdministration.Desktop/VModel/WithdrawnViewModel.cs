using System;
using System.Collections.Generic;
using System.Text;

namespace BankAdministration.Desktop.VModel
{
    public class WithdrawnViewModel : ViewModelBase
    {
        private Int64 amount_;

        public Int64 WithdrawnAmount
        {
            get => amount_;
            set
            {
                amount_ = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand YesCommand { get; private set; }
        public DelegateCommand NoCommand { get; private set; }

        public EventHandler YesEvent;
        public EventHandler NoEvent;

        public WithdrawnViewModel()
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
