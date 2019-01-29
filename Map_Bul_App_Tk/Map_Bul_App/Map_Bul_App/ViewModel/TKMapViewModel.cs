using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Map_Bul_App.Design;
using Map_Bul_App.Models;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using Map_Bul_App.Views;
using Plugin.Geolocator;
using Plugin.Toasts;
using TK.CustomMap;
using TK.CustomMap.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.ViewModel
{
    public class TKMapViewModel : BaseViewModel
    {

        public event EventHandler<bool> PinPreviewVisibleChanged;
        public FilterPage Filter;
        public TKMapViewModel()
        {
            var lastFilterSettings =
                ApplicationSettings.DataBase.FilterSettingses.LastOrDefault();
            if (lastFilterSettings != null)
            {

                _mapCenter = lastFilterSettings.Center;
                _mapRegion = lastFilterSettings.MapRegion;
            }
            else
            {
                _mapCenter = new Position(39, 37);
                _mapRegion= new MapSpan(_mapCenter,90,112);
               Task.Run(async () =>
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 100;
                    var userPosition = await locator.GetPositionAsync();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MapCenter = new Position(userPosition.Latitude, userPosition.Longitude);
                        MapRegion = MapSpan.FromCenterAndRadius(MapCenter,new Distance(1000));
                    });
                });
            }

            _pins = new ObservableCollection<TKCustomMapPin>();
            _pinDetailsViewModelList = new List<PinDetailsViewModel>();
            ApplicationSettings.CategoriesLoaded += ApplicationSettings_CategoriesLoaded;
        }

        private void ApplicationSettings_CategoriesLoaded(object sender, EventArgs e)
        {
            LoadMarkersCommand.Execute(null);
        }

        public override void InitilizeFunc(object obj = null)
        {
            LoadingMarkers = true;
            var lastFilterSettings =
                ApplicationSettings.DataBase.FilterSettingses.LastOrDefault();
            if (lastFilterSettings == null)
            {
                LoadingMarkers = false;
                Filter = new FilterPage(MapRegion);
            }
            else
            {
                Task.Run((() =>
                {
                    Filter = new FilterPage(lastFilterSettings);
                    Device.BeginInvokeOnMainThread(
                        () =>
                        {
                            Pins =
                                new ObservableCollection<TKCustomMapPin>(
                                    Filter.CurrentViewModel.SelectedPins.Select(item => item.Pin));
                        });
                }));
            }
            LoadingMarkers = false;
        }


        #region [Properties]

        #region [Private]

      //  private TKTileUrlOptions _tileUrlOptions;
        private MapSpan _mapRegion;
     //   private MapSpan _mapRegionWithPins;
        private Position _userLocation;
        private Position _mapCenter;
        private TKCustomMapPin _selectedPin;
        private ObservableCollection<TKCustomMapPin> _pins;
        private CurrentPinsOnMap _currentPinsOnMap = CurrentPinsOnMap.Filter;

        public CurrentPinsOnMap CurrentPinsOnMap
        {
            get => _currentPinsOnMap;
            set
            {
                if(_currentPinsOnMap==value) return;
                _currentPinsOnMap = value;
                OnPropertyChanged();
            }
        }

        public bool LoadingMarkers { get; set; }
        public bool OpeningFilters { get; set; }
        public bool OpeningDetails { get; set; }
        public bool FilterByText { get; set; }
        private string _findText { get; set; }
        private List<PinDetailsViewModel> _pinDetailsViewModelList;

        #endregion

        #region [Public]

        #region [Map]

        public IRendererFunctions MapFunctions { get; set; }


        /// <summary>
        ///     Map region bound to <see cref="TKCustomMap" />
        /// </summary>
        public MapSpan MapRegion
        {
            get => _mapRegion;
            set
            {
                var enableLoading = false;
                if (_mapRegion == value) return;
                if (!_mapRegion.Contains(value) && _mapRegion != null)
                {
                    enableLoading = true;
                }
                _mapRegion = value;
                OnPropertyChanged();
                Filter?.CurrentViewModel?.SaveSettings(_mapRegion);
                if (enableLoading)
                    LoadMarkersCommand.Execute(null);
            }
        }
        public Position UserLocation
        {
            get => _userLocation;
            set
            {
                if (_userLocation != value)
                {
                    _userLocation = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        ///     Map region with loadel pins
        /// </summary>
        //public MapSpan MapRegionWithPins
        //{
        //    get { return _mapRegionWithPins; }
        //    set
        //    {
        //        if (_mapRegionWithPins == value) return;

        //        _mapRegionWithPins = value;
        //        OnPropertyChanged();
        //    }
        //}

        /// <summary>
        ///     Pins bound to the <see cref="TKCustomMap" />
        /// </summary>
        public ObservableCollection<TKCustomMapPin> Pins
        {
            get => _pins;
            set
            {
                if (_pins != value)
                {
                    _pins = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Map center bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Position MapCenter
        {
            get => _mapCenter;
            set
            {
                if (_mapCenter != value)
                {
                    _mapCenter = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Selected pin bound to the <see cref="TKCustomMap" />
        /// </summary>
        public TKCustomMapPin SelectedPin
        {
            get => _selectedPin;
            set
            {
                if (_selectedPin == value) return;
                _selectedPin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPreviewVisible));
                OnPinPreviewVisibleChanged(IsPreviewVisible);
            }
        }

        public bool IsPreviewVisible => SelectedPin != null;

        /// <summary>
        ///     Map Long Press bound to the <see cref="TKCustomMap" />
        /// </summary>

        #endregion

        public string FindText
        {
            get { return _findText; }
            set
            {
                if (_findText == value) return;
                _findText = value;
                OnPropertyChanged();
                if (string.IsNullOrEmpty(_findText))
                {
                    FilterByText = false;
                    CurrentPinsOnMap=CurrentPinsOnMap.Filter;
                    LoadMarkersCommand.Execute(null);
                }
            }
        }

        #endregion

        #endregion
        
        #region [Commands]

        #region [App]

        public ICommand OpenFilterCommand => new Command(async () =>
        {
            if (ApplicationSettings.CurrentUser.UserType == UserTypesMobile.Guest)
            {
                OpenFilter();
                return;
            }
            var addMarkerText = TextResource.AddMarkerTitile;
            var favoriteMarkers = TextResource.FavoriteMarkers;
            var openFilterText = TextResource.FilterAction;
            var buttons = new List<string>
            {openFilterText, favoriteMarkers};

            if(ApplicationSettings.CurrentUser.UserType==UserTypesMobile.Guide)
                buttons.Add(addMarkerText);
            var cancelText = TextResource.Cancel;
            
            var action = await Application.Current.MainPage.DisplayActionSheet(
                null,
                cancelText,
                null,
                buttons.ToArray());
            if (action == addMarkerText)
            {
                StartLoading();
                var page = new AddPinView(MapCenter);
                ApplicationSettings.MainPage.GoToPage(page);
                StopLoading();
            }
            else if (action == favoriteMarkers)
            {
                ShowFavorites();
            }
            else if (action == openFilterText)
            {
                OpenFilter();
            }
        });

        public ICommand GoToUserLocation => new Command( () =>
        {
            // var locator = CrossGeolocator.Current;
            // locator.DesiredAccuracy = 100;
            // var userPosition = await locator.GetPositionAsync();
            var userPosition = UserLocation;
            MapRegion = MapSpan.FromCenterAndRadius(new Position(userPosition.Latitude,userPosition.Longitude), new Distance(500));
        });

        public Command FindByTagCommand
        {
            get
            {
                return new Command((() =>
                {
                    if (string.IsNullOrEmpty(FindText)) return;
                    Task.Run(() =>
                    {
                        StartLoading();
                        FilterByText = true;
                        CurrentPinsOnMap=CurrentPinsOnMap.FindByText;
                        if (Filter == null)
                            Filter = new FilterPage(MapRegion);
                        Pins = new ObservableCollection<TKCustomMapPin>(Filter?.CurrentViewModel.GetPinsByTag(FindText));
                        if (Pins.Any())
                        {
                            var minDistance = Pins.Min(item => item.Position.DistanceTo(MapCenter));
                            /*SelectedPin =
                                Pins.FirstOrDefault(
                                    item => Math.Abs(item.Position.DistanceTo(MapCenter) - minDistance) <= 1);*/
                            var position = Pins.FirstOrDefault(
                                item => Math.Abs(item.Position.DistanceTo(MapCenter) - minDistance) <= 1).Position;
                            MapCenter = position;
                        }

                        StopLoading();
                    });
                }));
            }
        }

        public Command LoadMarkersCommand
        {
            get
            {
                return new Command((() =>
                {
                    Task.Run(() =>
                    {
                        if (CurrentPinsOnMap!=CurrentPinsOnMap.Filter || LoadingMarkers || ApplicationSettings.LoadingMarkers ||!ApplicationSettings.PinCategories.Any()) return;
                        LoadingMarkers = true;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ApplicationSettings.LoadingMarkers = true;
                        });

                        if (!ApplicationSettings.LoadedPins.MapRegions.Any(item => item.Contains(MapRegion)))
                        {
                            var p1 = new Position(MapRegion.Center.Latitude + MapRegion.LatitudeDegrees,
                                MapRegion.Center.Longitude - MapRegion.LongitudeDegrees);
                            var p2 = new Position(MapRegion.Center.Latitude - MapRegion.LatitudeDegrees,
                                MapRegion.Center.Longitude + MapRegion.LongitudeDegrees);


                            var pinToRemove = ApplicationSettings.LoadedPins.Pins.Where(p => p.Personal).ToList();
                            foreach (var item in pinToRemove)
                            {
                                item.IsVisible = false;
                                ApplicationSettings.LoadedPins.Pins.Remove(item);
                            }
                            var fromServer = ApplicationSettings.Service.GetSessionMarkers(p1, p2);
                            var categories = ApplicationSettings.GetAllCAtegories();
                            var pinsFromServer = fromServer.Select(
                            item =>
                            {
                                var rootCategory =
                                    categories.FirstOrDefault(c => c.Id == item.CategoriesBranch.LastOrDefault());
                                var baseCategory = categories.FirstOrDefault(c => c.Id == item.CategoriesBranch.FirstOrDefault());
                                var pin = new PinDescriptor(item)
                                {
                                    Pin =
                                    {
                                        DefaultPinColor = rootCategory?.Color ?? CustomColors.Orange,
                                        Subtitle = rootCategory?.Name ?? string.Empty,
                                        Type = baseCategory?.Name ?? string.Empty
                                    }
                                };
                                return pin;
                            });
                            ApplicationSettings.LoadedPins.Pins.AddRange(pinsFromServer);
                            ApplicationSettings.LoadedPins.MapRegions.Add(MapRegion);
                        }
                        if (Filter == null)
                        {
                            LoadingMarkers = false;
                            Device.BeginInvokeOnMainThread(() => { ApplicationSettings.LoadingMarkers = false; });
                            return;
                        }
                        Filter.CurrentViewModel.MapRegion = MapRegion;
                        Filter.CurrentViewModel.UpdatePinsVisible();
                        var pinsToAdd = Filter.CurrentViewModel.SelectedPins.Select(item => item.Pin).ToList();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            foreach (var pin in pinsToAdd.Where(pin => Pins.All(item => item.Id != pin.Id)))
                            {
                                Pins.Add(pin);
                            }
                            LoadingMarkers = false;
                            ApplicationSettings.LoadingMarkers = false;
                        });
                    });
                }));
            }
        }

        #endregion

        #region [Map]

        public Command<Position> MapLongPressCommand
        {
            get
            {
                return new Command<Position>(async position =>
                {
                    if (ApplicationSettings.CurrentUser.UserType == UserTypesMobile.Guest) return;
                    var titleText = TextResource.AddMarkerTitile;
                    var cancelText = TextResource.Cancel;

                    var action = await Application.Current.MainPage.DisplayActionSheet(
                        null,
                        cancelText,
                        null,
                        titleText);

                    if (action == titleText)
                    {
                        StartLoading();
                        var page = new AddPinView(position);
                        ApplicationSettings.MainPage.GoToPage(page);
                        StopLoading();
                    }
                });
            }
        }
        public Command<Position> UserLocationChangedCommand => new Command<Position>(position =>
        {
            UserLocation = position;
        });

        /// <summary>
        ///     Pin Selected bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command PinSelectedCommand
        {
            get
            {
                return
                    new Command(
                        () => { MapRegion = MapSpan.FromCenterAndRadius(SelectedPin.Position, MapRegion.Radius); });
            }
        }

        /// <summary>
        ///     Drag End bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command<TKCustomMapPin> DragEndCommand
        {
            get
            {
                return new Command<TKCustomMapPin>(pin =>
                {
                    var routePin = pin as RoutePin;

                    if (routePin != null)
                    {
                        if (routePin.IsSource)
                        {
                            routePin.Route.Source = pin.Position;
                        }
                        else
                        {
                            routePin.Route.Destination = pin.Position;
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Map Clicked bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public Command<Position> MapClickedCommand
        {
            get
            {
                return new Command<Position>((positon) =>
                {
                    this.SelectedPin = null;
                });
            }
        }
        /// <summary>
        ///     Callout clicked bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command CalloutClickedCommand
        {
            get
            {
                return new Command(() =>
                {
                    Task.Run(() =>
                    {
                        if (OpeningDetails || SelectedPin == null) return;
                        Device.BeginInvokeOnMainThread(StartLoading);
                        OpeningDetails = true;
                        var pinId = SelectedPin.Id;
                        try
                        {
                            var detailsViewModel = new PinDetailsViewModel(pinId);
                            if (detailsViewModel.Pin != null)
                            {
                                ApplicationSettings.GoToPage(new PinDetailsView(detailsViewModel));
                            }
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }
                        Device.BeginInvokeOnMainThread(StopLoading);
                        OpeningDetails = false;
                    });
                });
            }
        }

        public Command ClearMapCommand
        {
            get { return new Command(() => { _pins?.Clear(); }); }
        }

        #endregion

        #endregion

        #region [Methods]
        private void OpenFilter()
        {

            if (OpeningFilters) return;
            FilterByText = false;
            CurrentPinsOnMap = CurrentPinsOnMap.Filter;
            FindText = string.Empty;
            StartLoading();
            OpeningFilters = true;
            if (Filter == null)
                Filter = new FilterPage(MapRegion);
            else
            {
                Filter.CurrentViewModel.MapRegion = MapRegion;
            }
            ApplicationSettings.GoToPage(Filter);
            OpeningFilters = false;
            OpeningDetails = false;
            StopLoading();

        }
        private void ShowFavorites()
        {
            var favorites = ApplicationSettings.CurrentUser.FavoritesPlaces.ToList();
            if (favorites.Any())
            {
                CurrentPinsOnMap = CurrentPinsOnMap.Favorites;
                Pins = new ObservableCollection<TKCustomMapPin>(favorites.Select(item => item.ToTkCustomMapPin()));
            }

            else
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Info, TextResource.NoFavoritePinsToast);
            }
        }
        #endregion

        protected virtual void OnPinPreviewVisibleChanged(bool e)
        {
            PinPreviewVisibleChanged?.Invoke(this, e);
        }
    }
}
