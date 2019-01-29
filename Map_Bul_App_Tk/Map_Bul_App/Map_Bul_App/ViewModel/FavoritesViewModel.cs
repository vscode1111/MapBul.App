using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Map_Bul_App.Models;
using Map_Bul_App.Settings;
using Map_Bul_App.Views;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel
{
    public class FavoritesViewModel : BaseViewModel
    {
        public FavoritesViewModel()
        {
            _articles = new ObservableCollection<ArticleEventItem>();
            _events = new ObservableCollection<ArticleEventItem>();
            _pins = new ObservableCollection<UserPinDescriptor>();
        }

        public override void InitilizeFunc(object obj = null)
        {
            IsRefreshing = true;
            var user = ApplicationSettings.CurrentUser;
            var eventsArticles = ApplicationSettings.CurrentUser.FavoritesArticleEvents.Select(item =>
            {
                var result = new ArticleEventItem(item);
                result.DeleteItem += Result_DeleteItem;
                return result;
            }).ToList();
            Articles =
                new ObservableCollection<ArticleEventItem>(
                    eventsArticles.Where(item => !item.StartDate.HasValue));
            Events = new ObservableCollection<ArticleEventItem>(
                eventsArticles.Where(item => item.StartDate.HasValue));
            var pins = ApplicationSettings.CurrentUser.FavoritesPlaces.Select(item =>
            {
                var temp = new UserPinDescriptor(item);
                temp.DeleteFromFavorites += Pin_DeleteFromFavorites;
                return temp;
            });
            Pins = new ObservableCollection<UserPinDescriptor>(pins);
            IsRefreshing = false;
        }

        #region [Field]

        private int _selectedFrame;
        private int _selectedTitle;
        private ObservableCollection<ArticleEventItem> _articles;
        private ObservableCollection<ArticleEventItem> _events;
        private ObservableCollection<UserPinDescriptor> _pins;
        private ArticleEventItem _selectedArticleItem;
        private bool _isRefreshing;
        private UserPinDescriptor _selectedPin;

        #endregion

        #region [Property]

        public int SelectedFrame
        {
            get => _selectedFrame;
            set
            {
                _selectedFrame = value;
                SelectedTitle = value;
                OnPropertyChanged();
            }
        }

        public int SelectedTitle
        {
            get => _selectedTitle;
            set
            {
                if (value != _selectedTitle)
                {
                    _selectedTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsScrolling { get; set; }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (value == _isRefreshing) return;
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ArticleEventItem> Articles
        {
            get => _articles;
            set
            {
                if (_articles == value) return;
                _articles = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ArticleEventItem> Events
        {
            get => _events;
            set
            {
                if (_events == value) return;
                _events = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserPinDescriptor> Pins
        {
            get => _pins;
            set
            {
                if (_pins == value) return;
                _pins = value;
                OnPropertyChanged();
            }
        }

        public ArticleEventItem SelectedArticleItem
        {
            get => _selectedArticleItem;
            set
            {
                if (value != null)
                {
                    _selectedArticleItem = value;
                    OnPropertyChanged();
                    ApplicationSettings.GoToPage(new ArticleDetailsView(SelectedArticleItem));
                    _selectedArticleItem = null;
                }
            }
        }

        public UserPinDescriptor SelectedPin
        {
            get => _selectedPin;
            set
            {
                if (value != null)
                {
                    _selectedPin = value;
                    OnPropertyChanged();
                    var detailsViewModel = new PinDetailsViewModel(_selectedPin.Id);
                    detailsViewModel.PinDeleted += DetailsViewModel_PinDeleted;
                    if (detailsViewModel.Pin != null)
                    {
                        ApplicationSettings.GoToPage(new PinDetailsView(detailsViewModel));
                    }
                    _selectedPin = null;
                }
            }
        }

        #endregion

        #region [Command]

        public ICommand SelectFrameCommand => new Command<Pages>(frame =>
        {
            if (Device.OS == TargetPlatform.Android && IsScrolling) return;
            switch (frame)
            {
                case Pages.Map:
                    SelectedFrame = 0;
                    break;
                case Pages.Calendar:
                    SelectedFrame = 1;
                    break;
                case Pages.Articles:
                    SelectedFrame = 2;
                    break;
            }
        }
            );

        public ICommand DeleteArticle => new Command<ArticleEventItem>(item =>
        {
            if (item == null) return;
            item.DeleteItem -= Result_DeleteItem;
            if (item.Type == ArticleType.Article)
            {
                Articles.Remove(item);
            }
            if (item.Type == ArticleType.Event)
            {
                Events.Remove(item);
            }
            ApplicationSettings.DataBase.DeleteArticleEvent(item.Id);
        });

        public ICommand DeletePin => new Command<UserPinDescriptor>(pin =>
        {
            if (pin == null) return;
            ApplicationSettings.DataBase.DeletePlace(pin.Id, IdType.ServerId);
            Pins.Remove(pin);
        });

        #endregion

        #region [Method]

        private void Pin_DeleteFromFavorites(object sender, EventArgs e)
        {
            var pin = sender as UserPinDescriptor;
            if (pin == null) return;
            if (DeletePin.CanExecute(pin))
                DeletePin.Execute(pin);
        }

        private void Result_DeleteItem(object sender, EventArgs e)
        {
            var item = sender as ArticleEventItem;
            if (item == null) return;
            if (DeleteArticle.CanExecute(item))
                DeleteArticle.Execute(item);
        }

        private void DetailsViewModel_PinDeleted(object sender, EventArgs e)
        {
            var pinVm = sender as PinDetailsViewModel;
            if (pinVm != null)
            {
                var toDelete = Pins.FirstOrDefault(item => item.Id == pinVm.PinId);
                if (toDelete != null)
                    Pins.Remove(toDelete);
            }
        }

        #endregion
    }
}
