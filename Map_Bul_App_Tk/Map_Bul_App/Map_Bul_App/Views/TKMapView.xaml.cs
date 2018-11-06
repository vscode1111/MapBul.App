using System;
using System.Collections.ObjectModel;
using System.Linq;
using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.Views
{
    public partial class TKMapView : ContentPage
    {
        public TKMapView()
        {
            InitializeComponent();
            BindingContext = new TKMapViewModel();
            if (Device.OS == TargetPlatform.Android)
            {
                EntryStackLayout.Padding = new Thickness(20, 0, 0, 5);                
                ((Grid) GoToUserLocationStack.Parent).Children.Remove(GoToUserLocationStack);
            }
            BarEntry.Completed += BarEntry_Completed;
            NavigationPage.SetHasNavigationBar(this, false); //скрыть ActionBar
            InitView();
        }

        private void BarEntry_Completed(object sender, EventArgs e)
        {
            CurrentViewModel.FindByTagCommand.Execute(null);
        }


        private void InitView()
        {
            TkMap.SetBinding(TKCustomMap.MapLongPressCommandProperty, "MapLongPressCommand");
            TkMap.SetBinding(TKCustomMap.PinSelectedCommandProperty, "PinSelectedCommand");
            TkMap.SetBinding(TKCustomMap.SelectedPinProperty, "SelectedPin");
            TkMap.SetBinding(TKCustomMap.PinDragEndCommandProperty, "DragEndCommand");
            TkMap.SetBinding(TKCustomMap.CalloutClickedCommandProperty, "CalloutClickedCommand");
            TkMap.SetBinding(TKCustomMap.MapRegionProperty, "MapRegion");
            TkMap.SetBinding(TKCustomMap.MapFunctionsProperty, "MapFunctions");
            TkMap.IsRegionChangeAnimated = true;
            CurrentViewModel.PinPreviewVisibleChanged += CurrentViewModel_PinPreviewVisibleChanged;
        }

        private async void CurrentViewModel_PinPreviewVisibleChanged(object sender, bool e)
        {
            var opacity = e ? 1 : 0;
            if (e)
                PreviewStack.IsVisible = true;
            await PreviewStack.FadeTo(opacity, 250, Easing.SinIn);
            if (!e)
                PreviewStack.IsVisible = false;
        }

        private TKMapViewModel CurrentViewModel => BindingContext as TKMapViewModel;

        protected override void OnAppearing()
        {
            CurrentViewModel?.Initialize();
            if (CurrentViewModel?.Filter != null)
            {
                if (CurrentViewModel.CurrentPinsOnMap == CurrentPinsOnMap.Filter)
                {
                    var newCollection =
                        new ObservableCollection<TKCustomMapPin>(
                            CurrentViewModel.Filter.CurrentViewModel.SelectedPins.Select(item => item.Pin));
                    if (!CurrentViewModel.Pins.SequenceEqual(newCollection, new Statics.ComparePins()))
                    {
                        CurrentViewModel.Pins = newCollection;
                    }
                }
            }

            base.OnAppearing();
        }
    }
}
