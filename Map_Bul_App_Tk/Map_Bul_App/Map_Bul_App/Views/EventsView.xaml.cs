using Map_Bul_App.Models;
using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Views
{
    public partial class EventsView : ContentPage
    {
        public EventsView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false); //скрыть ActionBar
        }

        public int MarkerId { get; set; }

        public ArticlesViewModel CurrentViewModel => BindingContext as ArticlesViewModel;

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void OnAppearing()
        {
            //ArticlesListView.SelectedItem = null;
            CurrentViewModel.MarkerId = MarkerId;
            CurrentViewModel.Initialize(ArticleType.Event);
            base.OnAppearing();

        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tempArticle = e.Item as ArticleEventItem;
            if (tempArticle != null)
            {
                ApplicationSettings.DataBase.InsertArticleEventToShowedArticleEvents(tempArticle.ServerId, 0);
                tempArticle.OnPropertyChanged("IsShowed");
                CurrentViewModel.SelectedArticleEventItem = tempArticle;
            }
        }

        private void ArticlesListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            //CurrentViewModel.LazyLoadCommand.Execute(e.Item);
        }
    }
}