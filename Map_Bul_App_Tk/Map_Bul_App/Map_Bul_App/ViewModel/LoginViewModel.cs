using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Map_Bul_App.Models;
using Map_Bul_App.Models.Tables;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using Map_Bul_App.Views;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public override void InitilizeFunc(object obj = null)
        {
        }
        #region [ Property ]

        private string _login;

        public string Login
        {
            get { return _login; }
            set
            {
                if (value == _login) return;
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        #endregion [ Property ]

        #region [Method]
        private void GuestLogin()
        {
            StartLoading();
            var user = ApplicationSettings.DataBase.Users.FirstOrDefault(item => item.Type == UserTypesMobile.Guest) ??
                       new User
                       {
                           Name = "Войти",
                           Type = UserTypesMobile.Guest,
                           LastLogin = DateTime.UtcNow
                       };
            ApplicationSettings.CurrentUser.Id = ApplicationSettings.DataBase.SaveUser(user);
            ApplicationSettings.CurrentUser.UserType = UserTypesMobile.GetMobileTypeByServerType(user.Type);
            ApplicationSettings.CurrentUser.Name = user.Name;
            ApplicationSettings.CreateToast(ToastNotificationType.Success, TextResource.GuestLogin);
            ApplicationSettings.MainApp.MainPage.SetValue(MasterDetailPage.IsGestureEnabledProperty, true);
            ApplicationSettings.GoToPage(new TKMapView());
            StopLoading();
        }

        private void LoginToApp()
        {
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Login))
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, ExceptionResources.WrongAuth);
                return;
            }

            Task.Run(() =>
            {
                StartLoading();
                var user =
                    ApplicationSettings.DataBase.Users.FirstOrDefault(
                        item => item.Email == Login && item.Password == Password);
                if (user == null)
                {
                    var result = ApplicationSettings.Service.Login(Login, Password);
                    if (result == null)
                    {
                        StopLoading();
                        return;
                    }
                    ApplicationSettings.CurrentUser.Id = ApplicationSettings.SaveNewUser(new User
                    {
                        ServerId = result.Id,
                        Guid = result.Guid,
                        Name = result.FirstName,
                        Type = result.UserTypeTag,
                        Password = Password,
                        Email = result.Email,
                        LastLogin = DateTime.UtcNow
                    });

                    ApplicationSettings.CurrentUser.Guid = result.Guid;
                    ApplicationSettings.CurrentUser.Name = result.FirstName;
                    ApplicationSettings.CurrentUser.UserType = UserTypesMobile.GetMobileTypeByServerType(result.UserTypeTag);

                    //При авторизации мы закачиваем избранные места, статьи и события

                    ApplicationSettings.LoadFavorites();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ApplicationSettings.CurrentUser.IsLogined = true;
                        //ApplicationSettings.CreateToast(ToastNotificationType.Success, TextResource.LocalEntering);
                        ApplicationSettings.MainApp.MainPage.SetValue(MasterDetailPage.IsGestureEnabledProperty, true);
                        ApplicationSettings.GoToPage(new TKMapView());
                        StopLoading();
                    });
                    return;
                }

                user.LastLogin = DateTime.UtcNow;
                ApplicationSettings.DataBase.SaveUser(user);
                ApplicationSettings.CurrentUser.Id = user.Id;
                ApplicationSettings.CurrentUser.Guid = user.Guid;
                ApplicationSettings.CurrentUser.Name = user.Name;
                ApplicationSettings.CurrentUser.UserType = user.Type;
                Device.BeginInvokeOnMainThread(() =>
                {
                    ApplicationSettings.SetCurrentSession();
                    ApplicationSettings.CurrentUser.IsLogined = true;
                    //ApplicationSettings.CreateToast(ToastNotificationType.Success, TextResource.LocalEntering);
                    ApplicationSettings.MainApp.MainPage.SetValue(MasterDetailPage.IsGestureEnabledProperty, true);
                    ApplicationSettings.GoToPage(new TKMapView());
                    StopLoading();
                });
            });
        }

        private void GotoRegistration()
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(StartLoading);
                ApplicationSettings.GoToPage(new RegistrationView());
                Device.BeginInvokeOnMainThread(StopLoading);
            });
        }

        private void RecoverPassword()
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(StartLoading);
                ApplicationSettings.GoToPage(new RecoverPasswordView());
                Device.BeginInvokeOnMainThread(StopLoading);
            });
        }
        #endregion

        public ICommand LoginCommand => new Command(LoginToApp);
        public ICommand GuestLoginCommand => new Command(GuestLogin);
        public ICommand RecoverPasswordCommand => new Command(RecoverPassword);
        public ICommand RegistrationCommand => new Command(GotoRegistration);
    }
}
