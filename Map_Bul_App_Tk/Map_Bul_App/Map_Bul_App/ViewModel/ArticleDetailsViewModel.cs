using System.Linq;
using System.Threading.Tasks;
using Map_Bul_App.Models;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using Map_Bul_App.Views;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel
{
    internal class ArticleDetailsViewModel : BaseViewModel
    {
        public ArticleDetailsViewModel(ArticleEventItem articleEventItem)
        {
            var fromDb =
                ApplicationSettings.CurrentUser.FavoritesArticleEvents.FirstOrDefault(
                    item => item.ServerId == articleEventItem.ServerId);
            _articleEventItem = articleEventItem;
            if (fromDb != null && fromDb.IsFavorite)
                _articleEventItem.IsFavorite = true;
        }


        public override void InitilizeFunc(object obj = null)
        {
            if (obj != null)
            {
                ArticleEventItem = obj as ArticleEventItem;
            }
        }



        #region Property & Field
        public string Title
        {
            get
            {
                switch (ArticleEventItem.Type)
                {
                    case ArticleType.Article:
                        return TextResource.ArticlesButtonText;
                    case ArticleType.Event:
                        return TextResource.EventTitleString;
                    default:
                        return TextResource.ArticlesButtonText;
                }
            }
        }
        private ArticleEventItem _articleEventItem;
        public ArticleEventItem ArticleEventItem
        {
            get { return _articleEventItem; }
            set
            {
                if (value == null) return;

                _articleEventItem = value;
                OnPropertyChanged();
            }
        }
        public bool IsFavorite
        {
            get { return ArticleEventItem.IsFavorite; }
            set
            {
                if (_articleEventItem.IsFavorite == value) return;
                _articleEventItem.IsFavorite = value;
                OnPropertyChanged();
            }
        }
       

        #endregion

        public Command GoToArticleMarkerCommand
        {
            get
            {
                return new Command(act =>
                {
                    if (!string.IsNullOrEmpty(ArticleEventItem.AddressName) && ArticleEventItem.MarkerId.HasValue)
                    {
                        var detailsViewModel = new PinDetailsViewModel(ArticleEventItem.MarkerId.Value);
                        if (detailsViewModel.Pin != null)
                        {
                            ApplicationSettings.GoToPage(new PinDetailsView(detailsViewModel));
                        }
                    }
                });
            }
        }

        public Command AddToFavoritesCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsFavorite = !ArticleEventItem.IsFavorite;
                    OnPropertyChanged(nameof(ArticleEventItem.IsFavorite));
                    Task.Run(() =>
                    {
                        if (ArticleEventItem.IsFavorite)
                        {
                            ApplicationSettings.DataBase.SaveArticleEvent(ArticleEventItem);
                            ApplicationSettings.Service.SaveFavoriteArticleAndEvent(
                                ApplicationSettings.CurrentUser.Guid, ArticleEventItem.ServerId);
                        }
                        else
                        {
                            ApplicationSettings.DataBase.DeleteArticleEvent(ArticleEventItem.ServerId, IdType.ServerId);
                            ArticleEventItem.OnDeleteItem();
                            ApplicationSettings.Service.RemoveFavoriteArticleAndEvent(
                                ApplicationSettings.CurrentUser.Guid, ArticleEventItem.ServerId);
                        }
                    });
                });
            }
        }
    }
}
