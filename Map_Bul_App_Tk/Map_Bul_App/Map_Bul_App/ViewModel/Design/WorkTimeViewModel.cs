using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Map_Bul_App.Annotations;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel.Design
{
    public class WorkTimeViewModel : INotifyPropertyChanged
    {
        public event EventHandler<bool> PlusMinusClicked;
        public event EventHandler CalendarClicked;

       
        public WorkTimeViewModel()
        {
            _days = new List<MyDayOfWeek>();
            Id = Guid.NewGuid();
        }
        public WorkTimeViewModel(List<MyDayOfWeek> days, bool isAdd=false)
        {
            _days = days;
            IsAdd = isAdd;
            _startTime=new TimeSpan(10,0,0);
            _stopTime = new TimeSpan(20, 0, 0);
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        private List<MyDayOfWeek> _days;
        public List<MyDayOfWeek> Days
        {
            get { return _days; }
            set
            {
                if (value == _days) return;
                _days = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DaysFormattedString));
            }
        }
        public FormattedString DaysFormattedString
        {
            get
            {
                var result = new FormattedString();
                if (!Days.Any()) return result;
                foreach (var day in Days)
                {
                    result.Spans.Add(new Span
                    {
                        Text = day==Days.LastOrDefault()? day.ShortName.ToLower(): day.ShortName.ToLower()+", ",
                        ForegroundColor = day.Color,
                        
                    });
                    
                }
                return result;
            }
        }

        private TimeSpan _startTime;

        public TimeSpan StartTime
        {
            get { return _startTime; }
            set
            {
                if (value == _startTime) return;
                _startTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _stopTime;

       



        public TimeSpan StopTime
        {
            get { return _stopTime; }
            set
            {
                if (value == _stopTime) return;
                _stopTime = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlusMinusCommand => new Command(o =>
        {
            OnPlusMinusClicked();
        });
        public ICommand CalendarCommand=> new Command(o =>
        {
            OnCalendarClicked();
        });
        public bool IsAdd { get; set; }

        public bool IsFilled =>  Days.Any();

        public string PlusMinusImage => IsAdd ? "plus_icon.png" : "minus_icon.png";

        protected virtual void OnPlusMinusClicked()
        {
            PlusMinusClicked?.Invoke(this, IsAdd);
        }

        protected virtual void OnCalendarClicked()
        {
            CalendarClicked?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
