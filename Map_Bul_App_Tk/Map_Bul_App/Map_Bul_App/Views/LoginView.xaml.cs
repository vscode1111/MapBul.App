using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Views
{
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            ApplicationSettings.MainApp.MainPage.SetValue(MasterDetailPage.IsGestureEnabledProperty, false);// запретить показ меню
        }
    }
}
