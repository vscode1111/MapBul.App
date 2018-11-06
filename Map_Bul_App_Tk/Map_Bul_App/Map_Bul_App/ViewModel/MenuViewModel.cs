using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Map_Bul_App.Settings;
using Map_Bul_App.Views;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {
            ApplicationSettings.CurrentUser.PropertyChanged += CurrentUser_PropertyChanged;
            IsFavoritesVisible = ApplicationSettings.CurrentUser.UserType != UserTypesMobile.Guest;
        }

        private void CurrentUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UserType")
            {
                var type = ApplicationSettings.CurrentUser.UserType;
                IsFavoritesVisible = type != UserTypesMobile.Guest;
            }
        }

        //private static void GoToSelectPage(Type type)
        //{
        //    if (ApplicationSettings.MainPage.Navigation.NavigationStack.Last().GetType() == type) return;
        //    var newPage = (ContentPage)Activator.CreateInstance(type);
        //    ApplicationSettings.GoToPage(newPage);
        //}

        private static void OnMenuItemClicked(Pages page)
        {
            try
            {
                ((MasterDetailPage) ApplicationSettings.MainApp.MainPage).IsPresented = false;
                Type type;
                switch (page)
                {
                    case Pages.Map:
                        type = typeof (TKMapView);
                        break;
                    case Pages.Calendar:
                        type = typeof (EventsView);
                        break;
                    case Pages.Articles:
                        type = typeof (ArticlesView);
                        break;
                    case Pages.Favorites:
                    {
                        type = Device.OS == TargetPlatform.iOS
                            ? typeof (FavoritesAppleView)
                            : typeof (FavoritesAndroidView);
                    }
                        break;
                    default:
                        return;
                }
                if (ApplicationSettings.MainPage.Navigation.NavigationStack.Last().GetType() == type) return;
                var newPage = (Page) Activator.CreateInstance(type);
                ApplicationSettings.GoToPage(newPage);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        #region Fields

        private bool _isFavoritesVisible;

        #endregion

        #region [Properties]

        public bool IsFavoritesVisible
        {
            get { return _isFavoritesVisible; }
            set
            {
                if (_isFavoritesVisible == value) return;
                _isFavoritesVisible = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region [Commands]

        public ICommand LoginLogoutCommand => new Command(() =>
        {
            ((MasterDetailPage) ApplicationSettings.MainApp.MainPage).IsPresented = false;
            ApplicationSettings.GoToAuth();
        });

        public ICommand MenuItemClickedCommand => new Command<Pages>(OnMenuItemClicked);

        #endregion

        public override void InitilizeFunc(object obj = null)
        {
        }
    }
}
