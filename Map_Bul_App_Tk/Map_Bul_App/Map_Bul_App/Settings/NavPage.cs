using Xamarin.Forms;

namespace Map_Bul_App.Settings
{
    public class CustomNavPage : NavigationPage { }

    public class NavPage : CustomNavPage
    {
        public NavPage()
        {
            ApplicationSettings.MainPage = this;
        }
        public async void GoToPage(Page page)
        {
            await Navigation.PushAsync(page, true);
        }

        public int CountOfPageInStack => this.Navigation.NavigationStack.Count;
    }
}
