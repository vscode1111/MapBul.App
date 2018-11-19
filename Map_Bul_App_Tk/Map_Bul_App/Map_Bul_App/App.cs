using System;
using System.Linq;
using Map_Bul_App.Settings;
using Map_Bul_App.Views;
using Plugin.Toasts;
using TK.CustomMap.Api.Google;
using Xamarin.Forms;

namespace Map_Bul_App
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                GmsPlace.Init(ApplicationSettings.MapsApiKey);
                GmsDirection.Init(ApplicationSettings.MapsApiKey);
                Current.Resources = new ResourceDictionary();
                ApplicationSettings.Notificator = DependencyService.Get<IToastNotificator>();
                ApplicationSettings.Service.ErrorEvent += Service_ErrorEvent;
                ApplicationSettings.MainApp = this;
                ApplicationSettings.DataBase = new MapBulDataBaseRepository("MapBulDb.db");
                Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
                DateTime lastLogin;
                ApplicationSettings.LoadCategories();
                lastLogin = ApplicationSettings.DataBase.Users.Any()
                    ? ApplicationSettings.DataBase.Users.Max(item => item.LastLogin)
                    : DateTime.MinValue;
                var currentUser = ApplicationSettings.DataBase.Users.FirstOrDefault(item => item.LastLogin == lastLogin);
                if (currentUser != null)
                {
                    currentUser.LastLogin = DateTime.UtcNow;
                    ApplicationSettings.DataBase.SaveUser(currentUser);
                }
                ApplicationSettings.SetCurrentSession();
                ApplicationSettings.CurrentUser.SetUser(currentUser);
                NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
                MainPage = new MyMasterDetailPage();

                if (ApplicationSettings.CurrentUser.IsLogined || ApplicationSettings.CurrentUser.UserType == UserTypesMobile.Guest)
                {
                    ApplicationSettings.LoadFavorites();
                    ApplicationSettings.MainPage.Navigation.PushAsync(new TKMapView());
                }
                else
                {
                    ApplicationSettings.MainPage.Navigation.PushAsync(new LoginView());
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static void Service_ErrorEvent(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => { ApplicationSettings.CreateToast(ToastNotificationType.Error, sender as string); });
        }

        private static void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, ResX.ExceptionResources.ConnectException);
            }
            if (e.IsConnected)
            {
                ApplicationSettings.LoadCategories();
               ApplicationSettings.ClearServerSession();
            }
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

