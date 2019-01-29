using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Map_Bul_App.Settings;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected BaseViewModel()
        {
            _isActivityIndicatorRuning = _isActivityIndicatorRuning = false;
        }

        protected static void GoBack()
        {
            ApplicationSettings.MainPage.Navigation.PopAsync(true);
        }

        #region [Events]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        public event PropertyChangedEventHandler CancelInitialize;
        protected virtual void OnCancelInitialize()
        {
            CancelInitialize?.Invoke(IsInitialize,
                new PropertyChangedEventArgs("CancelInitialize"));
        }
        #endregion

        #region [Properties]
        #region [Private]
        private bool _isActivityIndicatorVisible;
        private bool _isActivityIndicatorRuning;
        private bool _isInitialize;

        


        #endregion

        #region [Public]


        public  Color BackgroundColorOfActivityIndicator => Color.FromRgba(0, 0, 0, 175);
        public bool IsActivityIndicatorVisible
        {
            get => _isActivityIndicatorVisible;
            set
            {
                if (value == _isActivityIndicatorVisible) return;
                _isActivityIndicatorVisible = value;
                OnPropertyChanged(nameof(IsActivityIndicatorVisible));
            }
        }
        public bool IsActivityIndicatorRuning
        {
            get => _isActivityIndicatorRuning;
            set
            {
                if (value == _isActivityIndicatorRuning) return;
                _isActivityIndicatorRuning = value;
                IsActivityIndicatorVisible = value;
                OnPropertyChanged(nameof(IsActivityIndicatorRuning));
            }
        }

        public bool IsInitialize
        {
            get => _isInitialize;
            set
            {
                if (value == _isInitialize) return;
                _isInitialize = value;
                OnPropertyChanged(nameof(IsInitialize));
                OnCancelInitialize();
            }
        }
        #endregion
        #endregion
        public abstract void InitilizeFunc(object obj=null);


        public void Initialize(object obj = null)
        {
            if (!IsInitialize)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(StartLoading);
                    
                    //IsInitialize = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitilizeFunc(obj);
                        IsInitialize = true;
                        StopLoading();
                    });
                });
            }
        }


        /// <summary>
        /// Запустить индикатор загрузки
        /// </summary>
        public void StartLoading()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsActivityIndicatorRuning = true;
            });
        }


        /// <summary>
        /// Остановить индикатор загрузки
        /// </summary>
        public void StopLoading()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsActivityIndicatorRuning = false;
            });
        }
        #region [ Command ]

        public ICommand MenuShowCloseClickCommand =>  new Command(() =>
        {
            ((MasterDetailPage)ApplicationSettings.MainApp.MainPage).IsPresented =
                !((MasterDetailPage)ApplicationSettings.MainApp.MainPage).IsPresented;
        });

        public ICommand BackClickCommand => new Command(GoBack);

        #endregion [ Command ]
    }
}
