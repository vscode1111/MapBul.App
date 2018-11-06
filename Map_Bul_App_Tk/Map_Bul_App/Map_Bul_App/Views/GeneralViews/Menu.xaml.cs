using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Views.GeneralViews
{
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            Title = "";
            InitializeComponent();
            ApplicationSettings.CurrentUser.PropertyChanged += CurrentUser_PropertyChanged;
         }

        private void CurrentUser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLogined")
            {
               
                NavMainLabel.Text = ApplicationSettings.CurrentUser.IsLogined
                    ? ApplicationSettings.CurrentUser.Name
                    : "Войти";
                LogoutButton.IsVisible = ApplicationSettings.CurrentUser.IsLogined;
                LoginButton.IsVisible= !ApplicationSettings.CurrentUser.IsLogined;
            }
        }
        private MenuViewModel CurrentViewModel => BindingContext as MenuViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavMainLabel.Text = ApplicationSettings.CurrentUser.IsLogined
                    ? ApplicationSettings.CurrentUser.Name
                    : ResX.TextResource.LogIn;
            LogoutButton.IsVisible = ApplicationSettings.CurrentUser.IsLogined;
            LoginButton.IsVisible = !ApplicationSettings.CurrentUser.IsLogined;
        }
    }
}
