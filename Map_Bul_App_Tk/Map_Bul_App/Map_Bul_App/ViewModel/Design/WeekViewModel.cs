using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Map_Bul_App.Annotations;

namespace Map_Bul_App.ViewModel.Design
{
    internal class WeekViewModel : INotifyPropertyChanged
    {
        public WeekViewModel()
        {
            _days = new List<MyDayOfWeek>
            {
                new MyDayOfWeek(DayOfWeek.Monday),
                new MyDayOfWeek(DayOfWeek.Tuesday),
                new MyDayOfWeek(DayOfWeek.Wednesday),
                new MyDayOfWeek(DayOfWeek.Thursday),
                new MyDayOfWeek(DayOfWeek.Friday),
                new MyDayOfWeek(DayOfWeek.Saturday),
                new MyDayOfWeek(DayOfWeek.Sunday)
            };
        }

        private List<MyDayOfWeek> _days;

        public List<MyDayOfWeek> Days
        {
            get => _days;
            set
            {
                if (value == _days) return;
                _days = value;
                OnPropertyChanged();
            }
        }

        public Guid CurrId
        {
            get => _currId;
            set
            {
                if (value == _currId) return;
                _currId = value;
                OnPropertyChanged();
            }
        }

        private Guid _currId;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
