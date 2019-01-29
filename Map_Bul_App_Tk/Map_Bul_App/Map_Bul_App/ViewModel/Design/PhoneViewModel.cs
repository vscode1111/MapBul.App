using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Map_Bul_App.Annotations;
using Map_Bul_App.Settings;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel.Design
{
    internal class PhoneViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<bool> PlusMinusClicked;
        public PhoneViewModel(Phone phone, bool isAdd = false)
        {
            IsAdd = isAdd;
            _phone = phone;
        }

        public PhoneViewModel()
        {

        }
        
        private Phone _phone;

        public Phone Phone
        {
            get => _phone;
            set
            {if(value==_phone) return;
                _phone = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdd;

        public string PlusMinusImage => IsAdd ? "plus_icon.png" : "minus_icon.png";

        public ICommand PlusMinusCommand => new Command(o =>
        {
            OnPlusMinusClicked();
        });

        protected virtual void OnPlusMinusClicked()
        {
            PlusMinusClicked?.Invoke(this, IsAdd);
        }
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
