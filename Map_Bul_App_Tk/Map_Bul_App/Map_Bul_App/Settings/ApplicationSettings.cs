using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Map_Bul_App.Models;
using Map_Bul_App.Models.Tables;
using Map_Bul_App.Views;
using Plugin.Connectivity;
using Plugin.Toasts;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace Map_Bul_App.Settings
{
    public static class ApplicationSettings
    {
        public static event EventHandler CategoriesLoaded;
        public static App MainApp;
        public static bool LoadingMarkers = false;
        public static readonly MapBulService Service = new MapBulService();
        public static readonly UserInformation CurrentUser = new UserInformation();
        public static readonly LoadedPins LoadedPins = new LoadedPins();
        public static MapBulDataBaseRepository DataBase;
        public static NavPage MainPage;
        public static readonly IDevice ThisDevice = Resolver.Resolve<IDevice>();
        public static IToastNotificator Notificator;
        public const string MapsApiKey = "AIzaSyCwxK-1FnusRJOx1ZpcTS5dpIoJZH9h0Eg";
        public static bool IsConnectToInternet => CrossConnectivity.Current.IsConnected;
        public static readonly List<PinCategory> PinCategories = new List<PinCategory>();
        public static readonly List<PinCategory> PinSubCategories = new List<PinCategory>();

        public static string GetLanguage => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        public static List<PinCategory> GetAllCAtegories()
        {
            return PinCategories.Concat(PinSubCategories).ToList();
        }

        #region Navigation

        /// <summary>
        ///     Переход к заданной странице
        /// </summary>
        /// <param name="page"></param>
        public static void GoToPage(Page page)
        {
            Device.BeginInvokeOnMainThread(() => { MainPage.GoToPage(page); });
        }

        public static void GoToHome()
        {
            var count = MainPage.Navigation.NavigationStack.Count;

            MainPage.Navigation.PushAsync(new TKMapView());
            for (var i = 0; i < count; i++)
            {
                MainPage.Navigation.RemovePage(MainPage.Navigation.NavigationStack[0]);
            }
        }

        public static async void GoToAuth()
        {
            var count = MainPage.Navigation.NavigationStack.Count;
               await MainPage.Navigation.PushAsync(new LoginView());
            for (var i = 0; i < count; i++)
            { 
                    MainPage.Navigation.RemovePage(MainPage.Navigation.NavigationStack[0]);
            }
            CurrentUser.ResetUser();
        }

        #endregion

        #region [ SQL Work ]

        public static int SaveNewUser(User user)
        {
            return DataBase.SaveUser(user);
        }

        #endregion [ SQL Work ]

        public static void LoadCategories()
        {
            Task.Run(() =>
            {
                if (PinCategories.Any() && PinSubCategories.Any()) return;
                var categories = Service.GetCategories();
                if (categories == null) return;
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var rootCategory in categories)
                    {
                        PinCategories.Add(new PinCategory
                        {
                            Color = Color.FromHex(rootCategory.Color),
                            Id = rootCategory.Id,
                            Icon = rootCategory.Icon,
                            PinIcon = rootCategory.Pin,
                            Name = rootCategory.Name
                        });
                        PinSubCategories.AddRange(rootCategory.ChildCategories.Select(item => new PinCategory
                        {
                            Color = Color.FromHex(rootCategory.Color),
                            Icon = item.Icon,
                            PinIcon = item.Pin,
                            Id = item.Id,
                            ParentId = rootCategory.Id,
                            Name = item.Name
                        }));
                    }
                    //CreateToast(ToastNotificationType.Success, "Категории загружены.");
                    OnCategoriesLoaded();
                });
            });
        }

        public static void LoadFavorites()
        {
            if (CurrentUser != null)
            {
                if (CurrentUser.UserType != UserTypesMobile.Guest)
                {
                    var tempListFavoritsArticleAndEvent =
                            Service.GetFavoritsArticlAndEvent(CurrentUser.Guid);
                    if (tempListFavoritsArticleAndEvent != null)
                    {
                        foreach (var item in tempListFavoritsArticleAndEvent)
                        {
                            DataBase.SaveArticleEvent(new ArticleEventItem(item)
                            {
                                IsFavorite = true
                            });
                        }
                    }

                    var tempListFavoritsMarker =
                            Service.GetFavoritsMarker(CurrentUser.Guid);
                    if (tempListFavoritsMarker != null)
                    {
                        foreach (var item in tempListFavoritsMarker)
                        {
                            DataBase.SavePlace(new Place(item, true)
                            {
                                IsFavorite = true
                            });
                        }
                    }
                }
            }
        }

        public static void SetCurrentSession()
        {
            var result = DataBase.SetSession(Guid.NewGuid().ToString());
            ClearServerSession();
        }

        public static void ClearServerSession()
        {
            Task.Run(() =>
            {
                try
                {
                    var cnt = DataBase.Sessions.Count();
                    if (cnt <= 1) return;
                    var sessions = DataBase.Sessions.Take(cnt - 1);
                    foreach (var session in from session in sessions
                                            let result = Service.ClearSession(session.Token)
                                            where result != null && result.Success
                                            select session)
                    {
                        DataBase.DeleteSession(session);
                    }
                }
                catch (Exception e )
                {
                    

                }

            });
        }

        public static void CreateToast(ToastNotificationType toastType, string text)
        {
            try
            {
                string toastTitile;
                switch (toastType)
                {
                    case ToastNotificationType.Error:
                        toastTitile = ResX.TextResource.Error;
                        break;
                    case ToastNotificationType.Success:
                        toastTitile = ResX.TextResource.Success;
                        break;
                    case ToastNotificationType.Warning:
                        toastTitile = ResX.TextResource.Warning;
                        break;
                    case ToastNotificationType.Info:
                        toastTitile = ResX.TextResource.Info;
                        break;
                    default:
                        toastTitile = ResX.TextResource.Info;
                        break;
                }

                Device.BeginInvokeOnMainThread(
                    () => { Notificator.Notify(toastType, toastTitile, text, TimeSpan.FromSeconds(2.0d)); });
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private static void OnCategoriesLoaded()
        {
            CategoriesLoaded?.Invoke(null, EventArgs.Empty);
        }
    }
}
