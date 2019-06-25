using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Map_Bul_App.Models;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using Map_Bul_App.Views;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Map_Bul_App.ViewModel
{
    public class ArticlesViewModel : BaseViewModel
    {
        public ArticlesViewModel()
        {
            _articles = new List<ArticleEventItem>();
        }

        public override void InitilizeFunc(object obj = null)
        {
            _type = obj as ArticleType? ?? ArticleType.Article;
            RefreshCommand.Execute(null);
        }

        #region [Fields]

        private readonly object _locker = new object();
        private bool _isFilterEnable;
        private ArticleType _type;
        private bool _isRefreshing;
        private DateTime _startDate = DateTime.Now;
        private DateTime _stopDate = DateTime.Now;
        private List<ArticleEventItem> _articles;
        private ArticleEventItem _selectedArticleEventItem;

        #endregion

        #region [Property]

        private int _markerId;

        public int MarkerId
        {
            get => _markerId;
            set
            {
                if (value != _markerId)
                {
                    _markerId = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsFilterEnable
        {
            get => _isFilterEnable;
            set
            {
                if (value == _isFilterEnable) return;
                _isFilterEnable = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing == value) return;
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate.Date;
            set
            {
                if (value == _startDate) return;
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime StopDate
        {
            get => _stopDate.Date;
            set
            {
                if (value == _stopDate) return;
                _stopDate = value;
                OnPropertyChanged();
            }
        }

        public List<ArticleEventItem> Articles
        {
            get => _articles;
            set
            {
                if (_articles == value) return;
                _articles = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VisibleArticles));
            }
        }

        public IEnumerable<ArticleEventItem> VisibleArticles => Articles.Where(item => item.IsVisible);

        public ArticleEventItem SelectedArticleEventItem
        {
            get => _selectedArticleEventItem;
            set
            {
                if (value != null)
                {
                    _selectedArticleEventItem = value;
                    OnPropertyChanged();
                    ApplicationSettings.GoToPage(new ArticleDetailsView(SelectedArticleEventItem));
                }
            }
        }

        private int _selectedFilter;

        public int SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (value != _selectedFilter)
                {
                    _selectedFilter = value;
                    //switch (value)
                    //{
                    //    case -1:
                    //        BackgroundPastFiltersColor = Color.FromHex("#e6e6e6");
                    //        BackgroundFutureFiltersColor = Color.FromHex("#e6e6e6");
                    //        break;
                    //    case 0:
                    //        BackgroundPastFiltersColor = Color.FromHex("#e6e6e6");
                    //        BackgroundFutureFiltersColor = Color.White;
                    //        break;
                    //    case 1:
                    //        BackgroundPastFiltersColor = Color.White;
                    //        BackgroundFutureFiltersColor = Color.FromHex("#e6e6e6");
                    //        break;
                    //    case 2:
                    //        BackgroundPastFiltersColor = Color.FromHex("#e6e6e6");
                    //        BackgroundFutureFiltersColor = Color.FromHex("#e6e6e6");
                    //        break;
                    //}
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region [Method]

        /*
                private void SetArticles(IEnumerable<DeserializeGetArticlesData> response)
                {
                    if (response != null)
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            IsRefreshing = true;
                            Articles = response.Select(item => new ArticleEventItem(item)).ToList();
                            IsRefreshing = false;
                        });
                }

                private void AddToArticleList(IEnumerable<ArticleEventItem> articles)
                {
                    Articles.AddRange(articles);
                    OnPropertyChanged(nameof(Articles));
                    OnPropertyChanged(nameof(VisibleArticles));
                }

                private void AddToArticleList(ArticleEventItem article)
                {
                    Articles.Add(article);
                    OnPropertyChanged(nameof(Articles));
                    OnPropertyChanged(nameof(VisibleArticles));
                }
        */

        #endregion

        #region [Command]

        public ICommand SearchFutureCommand => new Command(act =>
        {
            /*
            IsRefreshing = true;
            SelectedFilter = 0;
            Task.Run(() =>
            {
                if ((act != null && (bool) act) || IsFilterEnable)
                {
                    //Выбран участок - будущее
                    if (StartDate > DateTime.Now)
                    {
                        foreach (var article in Articles)
                        {
                            article.IsVisible = article.StartDate >= StartDate && article.StartDate <= StopDate;
                        }
                    }
                    //Выбран участок - прошлое
                    if (StopDate < DateTime.Now)
                    {
                        foreach (var article in Articles)
                        {
                            article.IsVisible = article.StartDate >= DateTime.Now;
                        }
                    }
                    //Выбран участок настоящее
                    if (StartDate <= DateTime.Now && StopDate >= DateTime.Now)
                    {
                        foreach (var article in Articles)
                        {
                            article.IsVisible = article.StartDate > DateTime.Now && article.StartDate <= StopDate;
                        }
                    }
                }
                else
                {
                    foreach (var article in Articles)
                    {
                        article.IsVisible = article.StartDate >= DateTime.Now ||
                                            (article.StopDate != null && article.StopDate.Value > DateTime.Now);
                    }
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    Articles = Articles.OrderBy(i => i.StartDate).ToList();

                    OnPropertyChanged(nameof(VisibleArticles));
                    IsRefreshing = false;
                });
            });
            */
            //if ((act!=null && (bool) act) || IsFilterEnable)
            //{
            //    //Выбран участок - будущее
            //    if (StartDate > DateTime.Now)
            //    {
            //        foreach (var article in Articles)
            //        {
            //            article.IsVisible = article.StartDate >= StartDate && article.StartDate <= StopDate;
            //        }
            //    }
            //    //Выбран участок - прошлое
            //    if (StopDate < DateTime.Now)
            //    {
            //        foreach (var article in Articles)
            //        {
            //            article.IsVisible = article.StartDate >= DateTime.Now;
            //        }
            //    }
            //    //Выбран участок настоящее
            //    if (StartDate <= DateTime.Now && StopDate >= DateTime.Now)
            //    {
            //        foreach (var article in Articles)
            //        {
            //            article.IsVisible = article.StartDate > DateTime.Now && article.StartDate <= StopDate;
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var article in Articles)
            //    {
            //        article.IsVisible = article.StartDate >= DateTime.Now;
            //    }
            //}
            //Articles = Articles.OrderBy(i => i.StartDate).ToList();

            //OnPropertyChanged(nameof(VisibleArticles));
            //IsRefreshing = false;

        });

        public ICommand SearchPastCommand => new Command(act =>
        {
            /*
            IsRefreshing = true;
            SelectedFilter = 1;
            Task.Run(() =>
            {
                if ((act != null && (bool) act) || IsFilterEnable)
                {
                    //Выбран участок - будущее
                    if (StartDate > DateTime.Now)
                    {
                        foreach (var article in Articles)
                        {
                            article.IsVisible = article.StartDate < DateTime.Now;
                        }
                    }
                    //Выбран участок - прошлое
                    if (StopDate < DateTime.Now)
                    {
                        foreach (var article in Articles)
                        {
                            article.IsVisible = article.StartDate >= StartDate && article.StartDate < StopDate;
                            //Если это длящееся событие
                            if (article.StopDate.HasValue && article.StartDate.HasValue)
                            {
                                //Полностью в промежутке
                                var a = article.StartDate.Value >= StartDate && article.StopDate.Value <= StopDate;
                                //конец в промежутке
                                var b = article.StopDate >= StartDate && article.StopDate <= StopDate;
                                //начало в промежутке
                                var c = article.StartDate >= StartDate && article.StartDate <= StopDate;
                                //включает промежуток
                                var d = article.StartDate <= StartDate && article.StopDate >= StopDate;
                                article.IsVisible = a || b || c || d;
                            }
                        }
                    }
                    //Выбран участок настоящее
                    if (StartDate <= DateTime.Now && StopDate >= DateTime.Now)
                    {
                        foreach (var article in Articles)
                        {
                            article.IsVisible = article.StartDate < DateTime.Now && article.StartDate >= StartDate;
                        }
                    }
                }
                else
                {
                    foreach (var article in Articles)
                    {
                        article.IsVisible = article.StartDate < DateTime.Now;
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    Articles = Articles.OrderByDescending(i => i.StartDate).ToList();

                    OnPropertyChanged(nameof(VisibleArticles));
                    IsRefreshing = false;
                });
            });
            */
            //if ((act != null && (bool) act) || IsFilterEnable)
            //{
            //    //Выбран участок - будущее
            //    if (StartDate > DateTime.Now)
            //    {
            //        foreach (var article in Articles)
            //        {
            //            article.IsVisible = article.StartDate < DateTime.Now;
            //        }
            //    }
            //    //Выбран участок - прошлое
            //    if (StopDate < DateTime.Now)
            //    {
            //        foreach (var article in Articles)
            //        {
            //            article.IsVisible = article.StartDate >= StartDate && article.StartDate < StopDate;
            //        }
            //    }
            //    //Выбран участок настоящее
            //    if (StartDate <= DateTime.Now && StopDate >= DateTime.Now)
            //    {
            //        foreach (var article in Articles)
            //        {
            //            article.IsVisible = article.StartDate < DateTime.Now && article.StartDate >= StartDate;
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var article in Articles)
            //    {
            //        article.IsVisible = article.StartDate < DateTime.Now;
            //    }
            //}

            //Articles = Articles.OrderByDescending(i => i.StartDate).ToList();

            //OnPropertyChanged(nameof(VisibleArticles));
            //IsRefreshing = false;
        });

        public ICommand SearchFilterCommand => new Command(act =>
        {
            IsRefreshing = true;
            SelectedFilter = 2;
            OnPropertyChanged(nameof(VisibleArticles));
            IsRefreshing = false;
        });

        public ICommand SearchCommand => new Command(() =>
        {
            IsRefreshing = true;
            if (!IsFilterEnable)
            {
                if (StartDate > StopDate)
                {
                    ApplicationSettings.CreateToast(ToastNotificationType.Warning, ExceptionResources.WrongDate);
                    IsRefreshing = false;
                    return;
                }

                foreach (var article in Articles)
                {
                    var isVisible = false;
                    if (article.StartDate.HasValue)
                    {
                        if (article.StopDate.HasValue)
                        {
                            //Полностью в промежутке
                            var a = article.StartDate.Value >= StartDate && article.StopDate.Value <= StopDate;
                            //Конец в промежутке
                            var b = article.StopDate >= StartDate && article.StopDate <= StopDate;
                            //Начало в промежутке
                            var c = article.StartDate >= StartDate && article.StartDate <= StopDate;
                            //Включает промежуток
                            var d = article.StartDate <= StartDate && article.StopDate >= StopDate;
                            isVisible = a || b || c || d;
                        }
                        else
                        {
                            isVisible = article.StartDate >= StartDate && article.StopDate <= StopDate;
                        }
                    }
                    article.IsVisible = isVisible;
                }

                //Выбран участок - будущее
                if (StartDate.Date > DateTime.Now.Date)
                {
                    SearchFutureCommand.Execute(true);
                }
                //Выбран участок - прошлое
                if (StopDate.Date < DateTime.Now.Date)
                {
                    SearchPastCommand.Execute(true);
                }
                //Выбран участок настоящее
                if (StartDate.Date <= DateTime.Now.Date && StopDate.Date >= DateTime.Now.Date)
                {
                    switch (SelectedFilter)
                    {
                        case 0:
                            SearchFutureCommand.Execute(true);
                            break;
                        case 1:
                            SearchPastCommand.Execute(true);
                            break;
                        case 2:
                            break;
                    }
                }

            }
            else
            {
                StartDate = StopDate = DateTime.Now;
                foreach (var article in Articles)
                {
                    article.IsVisible = true;
                }
                SelectedFilter = -1;
            }
            IsRefreshing = false;
            IsFilterEnable = !IsFilterEnable;
            OnPropertyChanged(nameof(VisibleArticles));
        });

        public ICommand MenuOrBackCommand => new Command(() =>
        {
            if (MarkerId == 0)
            {
                ((MasterDetailPage) ApplicationSettings.MainApp.MainPage).IsPresented =
                    !((MasterDetailPage) ApplicationSettings.MainApp.MainPage).IsPresented;
            }
            else
            {
                GoBack();
            }
        });

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;
            Task.Run(() =>
            {
                // var lastDate = Articles.FirstOrDefault()?.StartDate ?? DateTime.Now;
                var lastDate = DateTime.Now;
                lastDate_last = lastDate;

                var response = MarkerId == 0 
                    ? ApplicationSettings.Service.GetArticles(_type, 1, 100, true, lastDate) 
                    : ApplicationSettings.Service.GetRelatedEventsFromMarker(MarkerId, false).ToList();

                if (response != null && response.Any())
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Articles = response.Select(item => new ArticleEventItem(item)).ToList();
                        IsRefreshing = false;
                        SelectedFilter = -1;
                        OnPropertyChanged(nameof(VisibleArticles));
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Articles.Clear();
                        IsRefreshing = false;
                        OnPropertyChanged(nameof(VisibleArticles));
                    });
                }
            });
        });

        private DateTime? lastDate_last;

        public Command LazyLoadCommand => new Command<ArticleEventItem>(article =>
        {
            //Task.Run(() =>
            //{
            //    lock (_locker)
            //    {
            //        if (Articles.LastOrDefault() == article)
            //        {
            //            var lastDate = Articles.LastOrDefault().StartDate;
            //            if (lastDate != lastDate_last)
            //            {
            //                var newArticles = ApplicationSettings.Service.GetArticles(_type, 1, 1, true, lastDate);
            //                if (newArticles != null && newArticles.Any())
            //                {
            //                    var toAdd = newArticles.Select(item => new ArticleEventItem(item));
            //                    Device.BeginInvokeOnMainThread(() =>
            //                    {
            //                        Articles.AddRange(toAdd);
            //                        OnPropertyChanged(nameof(Articles));
            //                        OnPropertyChanged(nameof(VisibleArticles));
            //                    });
            //                }
            //            }
            //            lastDate_last = lastDate;
            //        }
            //    }
            //});
        });

        #endregion
    }
}
