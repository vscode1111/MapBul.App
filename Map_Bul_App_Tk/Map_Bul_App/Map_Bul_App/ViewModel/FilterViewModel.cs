using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Map_Bul_App.Design;
using Map_Bul_App.Models;
using Map_Bul_App.Models.Tables;
using Map_Bul_App.Settings;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.ViewModel
{
    public class FilterViewModel : BaseViewModel
    {
        public FilterViewModel()
        {
            _pinCategories = new List<PinCategory>();
            _mapRegion = new MapSpan(new Position(0, 0), 0, 0);
            _pinSubCategories = new List<PinCategory>();
            LoadCategories();
        }

        public override void InitilizeFunc(object obj = null)
        {
        }

        #region [Method]


        public void LoadPins(MapSpan region)
        {
            if (ApplicationSettings.LoadingMarkers || !ApplicationSettings.PinCategories.Any()) return;
            Device.BeginInvokeOnMainThread(() => { ApplicationSettings.LoadingMarkers = true; });
            var p1 = new Position(region.Center.Latitude + region.LatitudeDegrees,
                region.Center.Longitude - region.LongitudeDegrees);
            var p2 = new Position(region.Center.Latitude - region.LatitudeDegrees,
                region.Center.Longitude + region.LongitudeDegrees);
            var categories = ApplicationSettings.GetAllCAtegories();

            var pinToRemove = ApplicationSettings.LoadedPins.Pins.Where(p => p.Personal).ToList();
            foreach (var item in pinToRemove)
            {
                item.IsVisible = false;
                ApplicationSettings.LoadedPins.Pins.Remove(item);
            }

            var fromServer = ApplicationSettings.Service.GetSessionMarkers(p1, p2);

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

            ApplicationSettings.LoadedPins.Pins.AddRange(
                pinsFromServer.Where(pfs => ApplicationSettings.LoadedPins.Pins.All(lp => lp.Pin.Id != pfs.Pin.Id)));
            ApplicationSettings.LoadedPins.MapRegions.Add(region);
            Device.BeginInvokeOnMainThread(() => { ApplicationSettings.LoadingMarkers = false; });
            UpdatePinsVisible();
        }

        public void UpdateSubcatgoriesVisible(int? changedCategoryId = null, bool? isSelected = null)
        {
            if (changedCategoryId != null && isSelected != null)
            {
            }
            else
            {
                var selected = SelectedCategories.ToList();
                foreach (var subCategory in PinSubCategories)
                {
                    if (subCategory.ParentId != null && selected.Contains(subCategory.ParentId.Value))
                        subCategory.IsVisible = true;
                    else
                    {
                        subCategory.IsVisible = subCategory.ItemSelected = false;
                    }
                }
            }
            UpdatePinsVisible();
        }

        public void UpdatePinsVisible(int? changedCategoryId = null, bool? isSelected = null,
            bool? isRootCategory = null)
        {
            var selectedRootcategories = SelectedRootCategories;
            var selectedSubcategories = SelectedSubCategories;


            if (!selectedRootcategories.Any())
            {
                foreach (var pin in Pins)
                    pin.IsVisible = CheckVisiblePinByTags(pin);
            }
            else
            {
                foreach (var pin in Pins.Where(pin => selectedRootcategories.Contains(pin.RootCategory)))
                {
                    if (!CheckVisiblePinByTags(pin))
                    {
                        pin.IsVisible = false;
                        continue;
                    }

                    if (!pin.SubCategories.Any()) //Если нет вложенных категорий, или ни одна из подкатегорий не выбрана
                        pin.IsVisible = true;
                    else
                    {
                        var subcategories =
                            PinSubCategories.Where(item => item.ParentId == pin.RootCategory).Select(item => item.Id);
                        // Если не отмечены никакие подкатегории в категории
                        var selectedByNoSelectedSubcategoriesIncategory =
                            !selectedSubcategories.Intersect(subcategories).Any();
                        var visibleBySelectedSubCategory =
                            pin.SubCategories.Any(item => selectedSubcategories.Contains(item));
                        //Если выбрана подкатегория с пином
                        pin.IsVisible = (visibleBySelectedSubCategory || selectedByNoSelectedSubcategoriesIncategory);
                    }
                }
                foreach (var pin in Pins.Where(pin => !selectedRootcategories.Contains(pin.RootCategory)))
                    pin.IsVisible = false;
            }
            OnPropertyChanged(nameof(SelectedPinsCnt));
        }

        public IEnumerable<TKCustomMapPin> GetPinsByTag(string tag)
        {
            tag = tag.ToLower();
            var categories =
                PinCategories.Where(item => item.Name.ToLower().Contains(tag)).Select(item => item.Id).ToList();
            categories.AddRange(
                PinSubCategories?.Where(item => item.Name.ToLower().Contains(tag)).Select(item => item.Id));
            return (ApplicationSettings.LoadedPins.Pins.Where(
                pin => pin.Pin.Title.ToLower().Contains(tag) || pin.CategoriesBranch.Intersect(categories).Any())
                .Select(pin => pin.Pin)).ToList();
        }

        public void LoadFromSettings(FilterSettings settings)
        {
            var settingsSelectedCategories =
                settings.Categories.Replace(" ", "")
                    .Split(',')
                    .Where(item => !string.IsNullOrEmpty(item))
                    .Select(item => Convert.ToInt32(item))
                    .ToList();
            var settingsSelectedSubCategories =
                settings.SubCategories.Replace(" ", "")
                    .Split(',')
                    .Where(item => !string.IsNullOrEmpty(item))
                    .Select(item => Convert.ToInt32(item))
                    .ToList();
            if (settingsSelectedCategories.Any())
            {
                CategoriesVisible = true;
                foreach (var c in PinCategories)
                {
                    c.ItemSelected = settingsSelectedCategories.Contains(c.Id);
                }
            }
            if (settingsSelectedSubCategories.Any())
            {
                SubCategoriesVisible = true;
                foreach (var c in PinSubCategories)
                {
                    c.ItemSelected = settingsSelectedSubCategories.Contains(c.Id);
                }
            }
            MyMarkerSelected = settings.MyMarkerSelected;
            WifiSelected = settings.WifiSelected;
            NowOpenSelected = settings.NowOpenSelected;
            MapRegion = settings.MapRegion;
            LoadPins(MapRegion);
        }

        private bool CheckVisiblePinByTags(PinDescriptor pin)
        {
            bool byMyMarker, byWifi, byNowOpen;
            byNowOpen = !NowOpenSelected || pin.NowOpen;
            byWifi = !WifiSelected || pin.WiFi;
            byMyMarker = !pin.Personal || MyMarkerSelected;
            return byMyMarker && byWifi && byNowOpen;
        }
        private void ShowMap()
        {
            GoBack();
            SaveSettings();
        }

        public void SaveSettings(MapSpan region=null)
        {

            var settings = new FilterSettings
            {
                Categories = string.Join(",", SelectedRootCategories),
                SubCategories = string.Join(",", SelectedSubCategories),
                MyMarkerSelected = _myMarkerSelected,
                WifiSelected = _wifiSelected,
                NowOpenSelected = _nowOpenSelected,
                CenterLat =region?.Center.Latitude ?? MapRegion.Center.Latitude,
                CenterLng = region?.Center.Longitude ?? MapRegion.Center.Longitude,
                MRadius = region?.Radius.Meters ?? MapRegion.Radius.Meters,
                DateStamp = DateTime.UtcNow
            };
            ApplicationSettings.DataBase.SaveFilter(settings);
        }

        private void LoadCategories()
        {
            PinCategories = ApplicationSettings.PinCategories;
            PinSubCategories = ApplicationSettings.PinSubCategories;
        }



        private void ClearFilter()
        {
            foreach (var item in PinCategories)
            {
                item.ItemSelected = false;
            }
            foreach (var item in PinSubCategories)
            {
                item.ItemSelected = false;
            }
            MyMarkerSelected = WifiSelected = NowOpenSelected = CategoriesVisible = SubCategoriesVisible = false;
        }
        #endregion

        #region [Properties and Fields]

        public bool IsNotGuest => ApplicationSettings.CurrentUser.UserType != UserTypesMobile.Guest;

        private MapSpan _mapRegion;

        public MapSpan MapRegion
        {
            get => _mapRegion;
            set
            {
                if (_mapRegion == value) return;
                _mapRegion = value;
                OnPropertyChanged();
                UpdatePinsVisible();
            }
        }

        private List<PinDescriptor> _pins;

        public List<PinDescriptor> Pins
        {
            get
            {
                return
                    ApplicationSettings.LoadedPins.Pins.Where(item => MapRegion.Contains(item.Pin.Position)).ToList() ??
                    new List<PinDescriptor>();
            }
            set
            {
                if (value == ApplicationSettings.LoadedPins.Pins) return;
                ApplicationSettings.LoadedPins.Pins = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedPinsCnt));
            }
        }

        public List<PinDescriptor> SelectedPins
        {
            get { return Pins.Where(item => item.IsVisible).ToList(); }
        }

        public int SelectedPinsCnt => SelectedPins?.Count ?? 0;

        private List<PinCategory> _pinCategories;

        public List<PinCategory> PinCategories
        {
            get => _pinCategories;
            set
            {
                if (value == _pinCategories) return;
                _pinCategories = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedCategories));
                OnPropertyChanged(nameof(SelectedRootCategories));
            }
        }

        private List<PinCategory> _pinSubCategories;

        public List<PinCategory> PinSubCategories
        {
            get => _pinSubCategories;
            set
            {
                if (value == _pinSubCategories) return;
                _pinSubCategories = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedCategories));
                OnPropertyChanged(nameof(SelectedSubCategories));
            }
        }

        public List<int> SelectedCategories
        {
            get
            {
                return
                    PinCategories.ToList()
                        .Concat(PinSubCategories.ToList())
                        .Where(itemm => itemm.ItemSelected)
                        .Select(item => item.Id)
                        .ToList();
            }
        }

        public List<int> SelectedRootCategories
        {
            get
            {
                return
                    PinCategories.ToList().Where(item => item.ItemSelected).Select(item => item.Id).ToList();
            }
        }

        public List<int> SelectedSubCategories
        {
            get
            {
                return
                    PinSubCategories.ToList().Where(item => item.ItemSelected).Select(item => item.Id).ToList();
            }
        }

        private bool _myMarkerSelected;

        public bool MyMarkerSelected
        {
            get => _myMarkerSelected;
            set
            {
                if (value == _myMarkerSelected) return;
                _myMarkerSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _wifiSelected;

        public bool WifiSelected
        {
            get => _wifiSelected;
            set
            {
                if (value == _wifiSelected) return;
                _wifiSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _nowOpenSelected;

        public bool NowOpenSelected
        {
            get => _nowOpenSelected;
            set
            {
                if (value == _nowOpenSelected) return;
                _nowOpenSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _categoriesVisible;

        public bool CategoriesVisible
        {
            get => _categoriesVisible;
            set
            {
                if (value == _categoriesVisible) return;
                _categoriesVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _subCategoriesVisible;

        public bool SubCategoriesVisible
        {
            get => _subCategoriesVisible;
            set
            {
                if (value == _subCategoriesVisible) return;
                _subCategoriesVisible = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region [ Command ]

        public ICommand ChangeShowMyMarkerCommand=>new Command(() =>
        {
            MyMarkerSelected = !MyMarkerSelected;
            UpdatePinsVisible();
        });

        public ICommand ChangeWifiEnabledCommand=> new Command(() =>
        {
            WifiSelected = !WifiSelected;
            UpdatePinsVisible();
        });

        public ICommand ChangeNowOpenEnabledCommand => new Command(() =>
        {
            NowOpenSelected = !NowOpenSelected;
            UpdatePinsVisible();
        });

        public ICommand OpenCloseCategories =>  new Command(() =>
        {
            CategoriesVisible = !CategoriesVisible;
        });

        public ICommand OpenCloseSubCategories => new Command(() =>
        {
            SubCategoriesVisible = !SubCategoriesVisible;
        });

        public ICommand ClearFilterCommand => new Command(ClearFilter);

        public ICommand ShowOnMapCommand => new Command(ShowMap);

        #endregion [ Command ]
    }
}
