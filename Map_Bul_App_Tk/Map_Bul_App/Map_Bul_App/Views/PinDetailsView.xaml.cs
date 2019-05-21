using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Views
{
    public partial class PinDetailsView : ContentPage
    {
        public PinDetailsView(PinDetailsViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
            SelectedPhotoStackLayout.BackgroundColor = Color.FromRgba(0, 0, 0, 255);
            //LargeSelectedImage.PropertyChanged += (sender, e) =>
            //{
            //    if (e.PropertyName == Image.IsLoadingProperty.PropertyName)
            //    {
            //        ImageActivityIndicator.IsRunning = LargeSelectedImage.IsLoading;
            //        ImageActivityIndicator.IsVisible = LargeSelectedImage.IsLoading;
            //    }
            //};
            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
        }

        private PinDetailsViewModel CurrentViewModel => BindingContext as PinDetailsViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CurrentViewModel.Initialize();
            CurrentViewModel.PropertyChanged += CurrentViewModel_PropertyChanged;
        }

        private void CurrentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPhoto")
            {
                if (string.IsNullOrEmpty(CurrentViewModel.SelectedPhoto))
                {
                    SelectedPhotoStackLayout.FadeTo(0);
                    SelectedPhotoStackLayout.IsVisible = false;
                }
                else
                {
                    SelectedPhotoStackLayout.IsVisible = true;
                    SelectedPhotoStackLayout.FadeTo(1);
                }
            }
        }
    }
}
