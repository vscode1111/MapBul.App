using System.Collections.Generic;
using System.Linq;
using Map_Bul_App.Models;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Views
{
    public partial class FavoritesAndroidView : ContentPage
    {
        public FavoritesAndroidView()
        {
            InitializeComponent();
            Init();
        }
        private void Init(int startFrame = 0)
        {
            _startFrame = startFrame;
            _profileFrames = new List<View>
            {
                PinsFrame,
                EventsFrame,
                ArticlesFrame
            };

            if (CurrentViewModel != null)
            {
                CurrentViewModel.PropertyChanged += _viewModel_PropertyChanged;
            }

            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
            Application.Current.MainPage.SetValue(MasterDetailPage.IsGestureEnabledProperty, false);// запретить показ меню
            
        }
        private async  void _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedFrame":
                    var previousFrame = _profileFrames.FirstOrDefault(item => item.IsVisible);
                    await previousFrame.FadeTo(0, 250, Easing.CubicOut);
                    var selectedFrame = _profileFrames[CurrentViewModel.SelectedFrame];
                    selectedFrame.IsVisible = true;
                    previousFrame.IsVisible = false;
                    await  selectedFrame.FadeTo(1, 250, Easing.CubicIn);
                    break;
            }
        }
        public FavoritesViewModel CurrentViewModel => BindingContext as FavoritesViewModel;
        private int _startFrame;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CurrentViewModel.Initialize(_startFrame);
        }
        private List<View> _profileFrames;
        
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {

            CurrentViewModel.SelectedArticleItem = e.Item as ArticleEventItem;
        }

        private void PinsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            CurrentViewModel.SelectedPin = e.Item as UserPinDescriptor;
        }

    }
}
