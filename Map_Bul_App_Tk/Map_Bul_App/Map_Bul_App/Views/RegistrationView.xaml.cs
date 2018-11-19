using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Views
{
    public partial class RegistrationView : ContentPage
    {
        public RegistrationView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false); //скрыть ActionBar
            SexPicker.Items.Add(ResX.TextResource.MSex);
            SexPicker.Items.Add(ResX.TextResource.FSex);
            SexPicker.SelectedIndex = 0;
            if (ApplicationSettings.GetLanguage != "ru")
            {
                RegistrationMiddleNameEntry.IsVisible = false;
            }
        }

        public RegistrationViewModel CurrentViewModel => BindingContext as RegistrationViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CurrentViewModel.Initialize();
        }


    }
}
