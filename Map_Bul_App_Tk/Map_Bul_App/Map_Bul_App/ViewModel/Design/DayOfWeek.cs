using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Map_Bul_App.Annotations;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel.Design
{
    public sealed class MyDayOfWeek : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MyDayOfWeek(DayOfWeek day)
        {
            Day = day;
            _isEnabled = true;
        }

        public DayOfWeek Day;

        public string Name => DateTimeFormatInfo.CurrentInfo.GetDayName(Day);
        public string ShortName => DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(Day);

        public Color Color => Day == DayOfWeek.Saturday || Day == DayOfWeek.Sunday ? Color.Red : Color.Black;

        private bool _isSelected;


        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnabled;


        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
        public ICommand DayTappedCommand => new Command(() =>
        {
            if(_isEnabled)
            IsSelected = !IsSelected;
        });

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }








}
