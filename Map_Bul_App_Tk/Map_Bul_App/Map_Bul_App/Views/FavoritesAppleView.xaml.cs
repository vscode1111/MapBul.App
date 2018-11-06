using System;
using System.Collections.Generic;
using Map_Bul_App.Models;
using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;
using XLabs.Platform.Device;

namespace Map_Bul_App.Views
{
    public partial class FavoritesAppleView : ContentPage
    {
        public FavoritesAppleView()
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
            var display = ApplicationSettings.ThisDevice.Display;
            _widthRequest = ApplicationSettings.ThisDevice.WidthRequestInInches(
                ApplicationSettings.ThisDevice.Display.ScreenWidthInches());
            PScrollView.WidthRequest = _widthRequest * 3;
            if (display == null) return;
            foreach (var frame in _profileFrames)
            {
                frame.WidthRequest = _widthRequest;
            }

            if (CurrentViewModel != null)
            {
                CurrentViewModel.PropertyChanged += _viewModel_PropertyChanged;
            }
            if (Device.OS == TargetPlatform.Android)
            {
                PScrollView.Scrolled += PScrollView_Scrolled;
            }
            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
            Application.Current.MainPage.SetValue(MasterDetailPage.IsGestureEnabledProperty, false);// запретить показ меню
        }
        private double _widthRequest { get; set; }
        private void PScrollView_Scrolled(ScrollView arg1, Rectangle arg2)
        {
            if (CurrentViewModel.IsScrolling) return;
            var leftX = arg1.ScrollX + _widthRequest / 2;
            if (leftX < _widthRequest)
            {
                CurrentViewModel.SelectedTitle = 0;
                return;
            }

            if (leftX > _widthRequest && leftX < 2 * _widthRequest)
            {
                CurrentViewModel.SelectedTitle = 1;
                return;
            }

            if (leftX > 2 * _widthRequest)
                CurrentViewModel.SelectedTitle = 2;
        }

        private async void _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedFrame":
                    CurrentViewModel.IsScrolling = true;
                    await PScrollView.ScrollToAsync(_profileFrames[CurrentViewModel.SelectedFrame], ScrollToPosition.Center, true);
                    CurrentViewModel.IsScrolling = false;
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
        public void SetFrame(int frame)
        {
            CurrentViewModel.SelectedFrame = frame;
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

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {

        }
    }
}
