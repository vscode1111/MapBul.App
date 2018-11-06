using Map_Bul_App.Models;
using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Views
{
    public partial class ArticlesView 
    {
        public ArticlesView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
        }
        public ArticlesViewModel CurrentViewModel => BindingContext as ArticlesViewModel;

        protected override void OnAppearing()
        {
            //ArticlesListView.SelectedItem = null;
            CurrentViewModel.Initialize(ArticleType.Article);
            base.OnAppearing();
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tempArticle = e.Item as ArticleEventItem;
            if (tempArticle != null)
            {
                ApplicationSettings.DataBase.InsertArticleEventToShowedArticleEvents(tempArticle.ServerId, 1);
                tempArticle.OnPropertyChanged("IsShowed");
                CurrentViewModel.SelectedArticleEventItem = tempArticle;
            }
        }
    }
}
