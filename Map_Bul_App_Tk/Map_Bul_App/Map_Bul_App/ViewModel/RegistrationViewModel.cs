using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel
{
    public class RegistrationViewModel : BaseViewModel
    {
        public override void InitilizeFunc(object obj = null)
        {
            OnPropertyChanged(nameof(AllFieldsIsFilled));
        }

        #region [Properties & Fields]

        #region [Properties]
        
        private string _login;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private int _selectedSexIndex;
        private string _phone;
        private DateTime _birthDate = DateTime.Now.AddYears(-5);

        #endregion

        #region [Fields]


        private bool AllFieldsIsFilled
            => (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName));
        public string Login
        {
            get { return _login; }
            set
            {
                if (value == _login) return;
                _login = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllFieldsIsFilled));
            }
        }

        public string FirstName // Имя
        {
            get { return _firstName; }
            set
            {
                if (value == _firstName) return;
                _firstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllFieldsIsFilled));
            }
        }

        public string MiddleName //Фамилия
        {
            get { return _middleName; }
            set
            {
                if (value == _middleName) return;
                _middleName = value;
                OnPropertyChanged();
            }
        }

        public string LastName //Фамилия
        {
            get { return _lastName; }
            set
            {
                if (value == _lastName) return;
                _lastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllFieldsIsFilled));
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value == _phone) return;
                _phone = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                if (value == _birthDate) return;
                _birthDate = value;
                OnPropertyChanged();
            }
        }
        public int SelectedSexIndex
        {
            get { return _selectedSexIndex; }
            set
            {
                if (value == _selectedSexIndex) return;
                _selectedSexIndex = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region [Commands]

        public ICommand RegisterCommand => new Command(() =>
        {
            Task.Run(() =>

            {
                StartLoading();
                var sex = "М";
                if (SelectedSexIndex == 1)
                    sex = "Ж";
                var result = ApplicationSettings.Service.RegisterTenant(Login, FirstName, MiddleName, LastName,
                    BirthDate, sex, Phone);
                if (result == null)
                {
                    ApplicationSettings.CreateToast(ToastNotificationType.Error, TextResource.ErrorToast);
                    StopLoading();
                    return;
                }
                if (result.Success)
                {
                    ApplicationSettings.CreateToast(ToastNotificationType.Success,
                        TextResource.SuccessfulRegistrationToast);
                    Device.BeginInvokeOnMainThread(GoBack);
                }
                else
                {
                    ApplicationSettings.CreateToast(ToastNotificationType.Error, result.ErrorReason);
                }
                StopLoading();
            });

            BackClickCommand.Execute(null);
        });




        #endregion
    }
}
