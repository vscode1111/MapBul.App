using System;
using Map_Bul_App.Models;
using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Views
{
    public partial class ArticleDetailsView : ContentPage
    {
        public ArticleDetailsView(ArticleEventItem articleEventItem)
        {
            BindingContext = new ArticleDetailsViewModel(articleEventItem);
            InitializeComponent();
            
            if (articleEventItem.Type == ArticleType.Article)
            {
                WatermarkGrid.IsVisible = false;
            }
            else
            {
                AuthorNameLabel.IsVisible = false;
            }
            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
        }

        protected override void OnAppearing()
        {
            
            base.OnAppearing();
            SourceUriLabel.Text = ResX.TextResource.Source + "<u>"+ CurrentViewModel.ArticleEventItem.SourceUrl + "</u>";
            SourceUriLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(act =>
                {
                    if (!string.IsNullOrEmpty(CurrentViewModel.ArticleEventItem.SourceUrl))
                    {
                        try
                        {
                            var tempUrl = CurrentViewModel.ArticleEventItem.SourcePhoto;
                            if (!tempUrl.Contains("https://") && !tempUrl.Contains("http://"))
                            {
                                tempUrl = "http://" + tempUrl;
                            }
                            Device.OpenUri(new Uri(tempUrl));
                        }
                        catch
                        {
                            //ignored
                        }
                    }
                })
            });
            PhotoUriLabel.Text = ResX.TextResource.Photo + "<u>" + CurrentViewModel.ArticleEventItem.SourcePhoto + "</u>";
            PhotoUriLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(act =>
                {
                    if (!string.IsNullOrEmpty(CurrentViewModel.ArticleEventItem.SourcePhoto))
                    {
                        try
                        {
                            var tempUrl = CurrentViewModel.ArticleEventItem.SourcePhoto;
                            if (!tempUrl.Contains("https://") && !tempUrl.Contains("http://"))
                            {
                                tempUrl = "http://" + tempUrl;
                            }
                            Device.OpenUri(new Uri(tempUrl));
                        }
                        catch
                        {
                            //ignored
                        }
                    }
                })
            });
        }

        private ArticleDetailsViewModel CurrentViewModel => BindingContext as ArticleDetailsViewModel;

    }
}
