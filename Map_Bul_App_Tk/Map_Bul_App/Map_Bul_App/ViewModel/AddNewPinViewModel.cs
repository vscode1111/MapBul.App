using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Map_Bul_App.Models;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel.Design;
using Map_Bul_App.Views;
using Plugin.Toasts;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.ViewModel
{
    internal class AddNewPinViewModel : BaseViewModel
    {
        public event EventHandler<KeyValuePair<object, bool>> TimePlusMinusClicked;
        public event EventHandler<KeyValuePair<object, bool>> PhonePlusMinusClicked;

        public AddNewPinViewModel(Position position)
        {
            _mapCenter = position;
            ValidCities = new List<DeserializeCitiesData>();
            _pinCategories = new ObservableCollection<PinCategory>(ApplicationSettings.PinCategories.Select(item =>
            {
                var temp = new PinCategory
                {
                    CategoriesBranch = item.CategoriesBranch,
                    Color = item.Color,
                    Icon = item.Icon,
                    Id = item.Id,
                    PinIcon = item.PinIcon,
                    IsVisible = true,
                    ItemSelected = false,
                    Name = item.Name,
                    ParentId = item.ParentId
                };
                temp.ItemTapped += Category_ItemTapped;
                return temp;
            }));
            _pinSubCategories = new ObservableCollection<PinCategory>(ApplicationSettings.PinSubCategories.Select(item =>
            {
                var temp = new PinCategory
                {
                    CategoriesBranch = item.CategoriesBranch,
                    Color = item.Color,
                    Icon = item.Icon,
                    PinIcon = item.PinIcon,
                    Id = item.Id,
                    IsVisible = false,
                    ItemSelected = false,
                    Name = item.Name,
                    ParentId = item.ParentId
                };
                temp.ItemTapped += Tag_ItemTapped;
                return temp;
            }));
            _weekViewModel = new WeekViewModel();
            _baseCategoryImage = "empty_object_icon.png";
            //_photo = "add_picture_btn.png";
            _workTimeViewModels = new List<WorkTimeViewModel>();
            _phoneViewModels = new List<PhoneViewModel>();
            var firstWoktimeVm = new WorkTimeViewModel(new List<MyDayOfWeek>(), true);
            firstWoktimeVm.PlusMinusClicked += WoktimeVm_PlusMinusClicked;
            firstWoktimeVm.CalendarClicked += WoktimeVm_CalendarClicked;
            _workTimeViewModels.Add(firstWoktimeVm);
            var firstPhone = new PhoneViewModel(new Phone {Primary = true}, true);
            firstPhone.PlusMinusClicked += Phone_PlusMinusClicked;
            _phoneViewModels.Add(firstPhone);
            _isRedact = false;
            Photos.CollectionChanged += Photos_CollectionChanged;
        }

        public AddNewPinViewModel(UserPinDescriptor pin)
        {
            Pin = pin;
            _mapCenter = new Position(pin.Lat, pin.Lng);
            ValidCities = new List<DeserializeCitiesData>();
            _pinCategories = new ObservableCollection<PinCategory>(ApplicationSettings.PinCategories.Select(item =>
            {
                var temp = new PinCategory
                {
                    CategoriesBranch = item.CategoriesBranch,
                    Color = item.Color,
                    Icon = item.Icon,
                    Id = item.Id,
                    PinIcon = item.PinIcon,
                    IsVisible = true,
                    ItemSelected = false,
                    Name = item.Name,
                    ParentId = item.ParentId
                };
                temp.ItemTapped += Category_ItemTapped;
                return temp;
            }));
            _pinSubCategories = new ObservableCollection<PinCategory>(ApplicationSettings.PinSubCategories.Select(item =>
            {
                var temp = new PinCategory
                {
                    CategoriesBranch = item.CategoriesBranch,
                    Color = item.Color,
                    Icon = item.Icon,
                    PinIcon = item.PinIcon,
                    Id = item.Id,
                    IsVisible = false,
                    ItemSelected = false,
                    Name = item.Name,
                    ParentId = item.ParentId
                };
                temp.ItemTapped += Tag_ItemTapped;
                return temp;
            }));

            foreach (var item in _pinSubCategories.Where(psc => psc.ParentId == Pin.BaseCategoryId))
            {
                item.IsVisible = true;
                item.ItemSelected = Pin.Subcategories.Any(p => p.Id == item.Id);
            }

            OldPhotos = new ObservableCollection<string>(pin.PhotosMini);

            #region [ Для времени работы ]

            _weekViewModel = new WeekViewModel();

            _workTimeViewModels = new List<WorkTimeViewModel>();

            if (Pin.WorkTime != null && Pin.WorkTime.Count > 0)
            {
                foreach (var tempListOfDay in Pin.WorkTime.GroupBy(w => w.CloseTime.ToString() + w.OpenTime.ToString()))
                {
                    var newWorkTimeViewModel = new WorkTimeViewModel();
                    if (_workTimeViewModels.All(wt => !wt.IsAdd))
                    {
                        newWorkTimeViewModel.IsAdd = true;
                    }
                    newWorkTimeViewModel.PlusMinusClicked += WoktimeVm_PlusMinusClicked;
                    newWorkTimeViewModel.CalendarClicked += WoktimeVm_CalendarClicked;
                    foreach (var workTime in tempListOfDay)
                    {
                        newWorkTimeViewModel.Days.Add(new MyDayOfWeek((DayOfWeek) (workTime.Id == 7 ? 0 : workTime.Id))
                        {
                            IsSelected = true
                        });
                        newWorkTimeViewModel.StartTime = workTime.OpenTime;
                        newWorkTimeViewModel.StopTime = workTime.CloseTime;
                        _weekViewModel.Days.FirstOrDefault(
                            d => d.Day == (DayOfWeek) (workTime.Id == 7 ? 0 : workTime.Id))
                            .IsSelected = true;
                    }
                    _workTimeViewModels.Add(newWorkTimeViewModel);
                }
            }
            else
            {
                var firstWoktimeVm = new WorkTimeViewModel(new List<MyDayOfWeek>(), true);
                firstWoktimeVm.PlusMinusClicked += WoktimeVm_PlusMinusClicked;
                firstWoktimeVm.CalendarClicked += WoktimeVm_CalendarClicked;
                _workTimeViewModels.Add(firstWoktimeVm);
            }

            #endregion [ Для времени работы ]

            #region [ Для телефонов ]

            _phoneViewModels = new List<PhoneViewModel>();
            if (Pin.Phones != null && Pin.Phones.Count > 0)
            {
                foreach (var phone in Pin.Phones)
                {
                    var newPhone = new PhoneViewModel(phone);
                    if (newPhone.Phone.Primary)
                    {
                        newPhone.IsAdd = true;
                    }
                    _phoneViewModels.Add(newPhone);
                    newPhone.PlusMinusClicked += Phone_PlusMinusClicked;
                }
            }
            else
            {
                var firstPhone = new PhoneViewModel(new Phone { Primary = true }, true);
                firstPhone.PlusMinusClicked += Phone_PlusMinusClicked;
                _phoneViewModels.Add(firstPhone);
            }

            #endregion [ Для телефонов ]

            _baseCategoryString = Pin.BaseCategoryName;
            _baseCategoryColor = _pinCategories.FirstOrDefault(c => c.Id == Pin.BaseCategoryId).Color;
            _baseCategoryImage = _pinCategories.FirstOrDefault(c => c.Id == Pin.BaseCategoryId).Icon;
            BaseCategorySelected = true;
            PinCategories.FirstOrDefault(c => c.Id == Pin.BaseCategoryId).ItemSelected = true;
            _isRedact = true;
            Photos.CollectionChanged += Photos_CollectionChanged;
            PropertyChanged += AddNewPinViewModel_PropertyChanged;
            Pin.PropertyChanged += Pin_PropertyChanged;
        }

        /// <summary>
        /// Ловит изменения свойств в редактируемой метке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    CheckFields();
                    break;
                case "Introduction":
                    CheckFields();
                    break;
                case "Description":
                    CheckFields();
                    break;
                case "Site":
                    CheckFields();
                    break;
                case "Email":
                    CheckFields();
                    break;
            } 
        }

        /// <summary>
        /// Ловит изменения свойств в редактируемой метке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewPinViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Photos":
                    CheckFields();
                    break;
                case "PinCategories":
                    CheckFields();
                    break;
                case "PinSubCategories":
                    CheckFields();
                    break;
                case "SelectedDiscountIndex":
                    CheckFields();
                    break;
                case "PhoneViewModels":
                    CheckFields();
                    break;
            }
        }

        private void Photos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CheckFields();
            //if (Photos.Count > 0)
            //{
            //    if (Photos.Count > _photosBase64Dictionary.Count)
            //    {
            //        var fileProvider = DependencyService.Get<IFileSystemWork>();
            //        foreach (
            //            var newPhotoPath in Photos.Where(path => _photosBase64Dictionary.All(b64 => b64.Key != path)))
            //        {
            //            if (!newPhotoPath.Contains("http://185.76.145.214/"))
            //            {
            //                var tempBase64Photo = Convert.ToBase64String(fileProvider.GetFile(newPhotoPath));
            //                _photosBase64Dictionary.Add(newPhotoPath, tempBase64Photo);
            //            }
            //            else
            //            {
            //                _photosBase64Dictionary.Add(newPhotoPath, newPhotoPath);
            //            }
            //        }
            //    }
            //    else if (Photos.Count < _photosBase64Dictionary.Count)
            //    {
            //        var tempToRemove = _photosBase64Dictionary.Where(b64 => !Photos.Contains(b64.Key))
            //            .Select(b64 => b64.Key);
            //        foreach (var itemToDelete in tempToRemove)
            //        {
            //            _photosBase64Dictionary.Remove(itemToDelete);
            //        }
            //    }
            //}
            //else
            //{
            //    _photosBase64Dictionary.Clear();
            //}
        }

        public override void InitilizeFunc(object obj = null)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (!_isRedact)
                {
                    Pin = new UserPinDescriptor
                    {
                        Pin = new TKCustomMapPin
                        {
                            Position = _mapCenter,
                            IsSmall = true,
                            Scale = 3,
                            Image = "empty_pin.png",
                            ShowCallout = false,
                            IsDraggable = true
                        },
                        IsVisible = true,
                        CategoriesBranch = new List<int>()
                    };
                    IsPersonalMarker = !IsGuide;
                }
                else
                {
                    IsPersonalMarker = true;
                }
                if (Device.OS == TargetPlatform.Android)
                    Pin.Pin.IsDraggable = false;
                Pins = new ObservableCollection<TKCustomMapPin> {Pin.Pin};
                MapCenter = new Position(Pin.Pin.Position.Latitude, Pin.Pin.Position.Longitude);
            });
            //CheckRegion();
        }

        #region Fields

        public string Title => IsRedact ? ResX.TextResource.EditMarkerTitle : ResX.TextResource.AddMarkerTitile;
        
        private readonly bool _isRedact;
        public bool IsRedact => _isRedact;

        private ObservableCollection<PinCategory> _pinCategories;
        private ObservableCollection<PinCategory> _pinSubCategories;
        private string _baseCategoryString;
        private Color _baseCategoryColor;
        private ImageSource _baseCategoryImage;
        private int _selectedDiscountIndex;
        //private ImageSource _photo;
        private bool _isValidLocation;
        private List<WorkTimeViewModel> _workTimeViewModels;
        private List<PhoneViewModel> _phoneViewModels;
        private WeekViewModel _weekViewModel;
        private UserPinDescriptor _pin;
        private Position _mapCenter;
        private ObservableCollection<TKCustomMapPin> _pins;
        private bool _categoriesPopupVisible;
        private bool _tagsPopupVisible;

        private bool _daysPopupVisible;

        #endregion

        #region [Property]

        public bool IsGuide => ApplicationSettings.CurrentUser.UserType == UserTypesMobile.Guide;

        private bool _isPersonalMarker;
        public bool IsPersonalMarker
        {
            get { return _isPersonalMarker; }
            set
            {
                if (value != _isPersonalMarker)
                {
                    _isPersonalMarker = value;
                    OnPropertyChanged();
                    CheckFields();
                }
            }
        }


        private List<DeserializeCitiesData> ValidCities { get; set; }
        private string City { get; set; }
        private int CytiId { get; set; }
        private string Street { get; set; }
        private string House { get; set; }
        private string Building { get; set; }
        private bool BaseCategorySelected { get; set; }

        private ObservableCollection<string> _oldPhotos;
        public ObservableCollection<string> OldPhotos
        {
            get { return _oldPhotos; }
            set
            {
                if (value != _oldPhotos)
                {
                    _oldPhotos = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _photos = new ObservableCollection<string>();
        public ObservableCollection<string> Photos
        {
            get { return _photos; }
            set
            {
                if (value != _photos)
                {
                    _photos = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _photosBase64Dictionary;

        public ObservableCollection<string> PhotosBase64Dictionary
        {
            get { return _photosBase64Dictionary; }
            set
            {
                if (value != _photosBase64Dictionary)
                {
                    _photosBase64Dictionary = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<PinCategory> PinCategories
        {
            get { return _pinCategories; }
            set
            {
                if (value == _pinCategories) return;
                _pinCategories = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedSubCategories));
            }
        }

        public ObservableCollection<PinCategory> PinSubCategories
        {
            get { return _pinSubCategories; }
            set
            {
                if (value == _pinSubCategories) return;
                _pinSubCategories = value;
                OnPropertyChanged();
                CheckFields();
                OnPropertyChanged(nameof(SelectedSubCategories));
            }
        }

        public string BaseCategoryString
        {
            get { return _baseCategoryString; }
            set
            {
                if (value == _baseCategoryString) return;
                _baseCategoryString = value;
                OnPropertyChanged();
                CheckFields();
            }
        }

        public Color BaseCategoryColor
        {
            get { return _baseCategoryColor; }
            set
            {
                if (value == _baseCategoryColor) return;
                _baseCategoryColor = value;
                OnPropertyChanged();
            }
        }

        public ImageSource BaseCategoryImage
        {
            get { return _baseCategoryImage; }
            set
            {
                if (value == _baseCategoryImage) return;
                _baseCategoryImage = value;
                OnPropertyChanged();
            }
        }

        public int SelectedDiscountIndex
        {
            get { return _selectedDiscountIndex; }
            set
            {
                if (value == _selectedDiscountIndex) return;
                _selectedDiscountIndex = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsValidLocation
        {
            get { return _isValidLocation; }
            set
            {
                if (value == _isValidLocation) return;
                CheckFields();
                _isValidLocation = value;
                OnPropertyChanged();
            }
        }

        public bool IsAllFieldsValid
        {
            get
            {
                var tempValidResult = !string.IsNullOrEmpty(Pin?.Name) && !string.IsNullOrEmpty(Pin.Introduction) &&
                                      !string.IsNullOrEmpty(Pin.Description) && Pin.BaseCategoryId != 0 &&
                                      (!PinSubCategories.Any(item => item.IsVisible) ||
                                       PinSubCategories.Any(item => item.ItemSelected)) &&
                                      WorkTimeViewModels.Any() &&
                                      WorkTimeViewModels.Any(item => item.Days.Any()) &&
                                      IsValidLocation;

                /*
                if (!_isRedact)
                {
                    tempValidResult = tempValidResult && Photos.Count(photoPath => !string.IsNullOrEmpty(photoPath))>0;
                }
                */

                return tempValidResult;
            }
        }

        public IEnumerable<PinCategory> VisibleTags => PinSubCategories.Where(item => item.IsVisible).ToList();
        public IEnumerable<PinCategory> SelectedTags => VisibleTags.Where(item => item.ItemSelected);

        public List<int> SelectedSubCategories
        {
            get
            {
                return PinSubCategories.Where(itemm => itemm.ItemSelected)
                    .Select(item => item.Id)
                    .ToList();
            }
        }

        public string Tags => SelectedTags.Any() ? SelectedTags.Select(item => item.Name).ToTagsString() : string.Empty;

        #region ViewModels

        public List<WorkTimeViewModel> WorkTimeViewModels
        {
            get { return _workTimeViewModels; }
            set
            {
                if (value == _workTimeViewModels) return;
                _workTimeViewModels = value;
                OnPropertyChanged();
            }
        }

        public List<PhoneViewModel> PhoneViewModels
        {
            get { return _phoneViewModels; }
            set
            {
                if (value == _phoneViewModels) return;
                _phoneViewModels = value;
                OnPropertyChanged();
            }
        }

        public WeekViewModel WeekViewModel
        {
            get { return _weekViewModel; }
            set
            {
                if (value == _weekViewModel) return;
                _weekViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Карта

        public UserPinDescriptor Pin
        {
            get { return _pin; }
            set
            {
                if (value != null && _pin != value)
                    _pin = value;
                OnPropertyChanged();
            }
        }


        public Position MapCenter
        {
            get { return _mapCenter; }
            set
            {
                if (_mapCenter == value) return;
                _mapCenter = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<TKCustomMapPin> Pins
        {
            get { return _pins; }
            set
            {
                if (_pins == value) return;
                _pins = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Popups

        public bool CategoriesPopupVisible
        {
            get { return _categoriesPopupVisible; }
            set
            {
                if (value == _categoriesPopupVisible) return;
                _categoriesPopupVisible = value;
                OnPropertyChanged();
            }
        }

        public bool TagsPopupVisible
        {
            get { return _tagsPopupVisible; }
            set
            {
                if (value == _tagsPopupVisible) return;
                _tagsPopupVisible = value;
                OnPropertyChanged();
            }
        }

        public bool DaysPopupVisible
        {
            get { return _daysPopupVisible; }
            set
            {
                if (value == _daysPopupVisible) return;
                _daysPopupVisible = value;
                OnPropertyChanged();
            }
        }

        #endregion
        #endregion

        #region [Method]
        public void CheckFields()
        {
            if (!_haveChecked)
            {
                CheckRegion();
            }
            OnPropertyChanged(nameof(IsAllFieldsValid));
        }

        private void UpdateSubcatgoriesVisible(/*int changedCategoryId, bool isSelected*/)
        {
            var selected = PinCategories.FirstOrDefault(item => item.ItemSelected);
            if (selected != null)
            {
                BaseCategoryColor = selected.Color;
                Pin.BaseCategoryId = selected.Id;
                BaseCategoryString = selected.Name;
                BaseCategoryImage = selected.Icon;
                Pin.Pin.Image = selected.PinIcon;
                BaseCategorySelected = true;
            }
            else
            {
                Pin.BaseCategoryId = 0;
                BaseCategoryColor = Color.Black;
                BaseCategoryString = string.Empty;
                BaseCategoryImage = "empty_object_icon.png";
                Pins.FirstOrDefault().Image = "empty_pin.png";
                BaseCategorySelected = false;
            }

            foreach (var sc in PinSubCategories)
            {
                sc.IsVisible = selected != null && sc.ParentId == selected.Id;
            }
            OnPropertyChanged(nameof(SelectedTags));
            OnPropertyChanged(nameof(VisibleTags));
            OnPropertyChanged(nameof(Tags));
            CheckFields();
        }

        private string countryCode;

        private async Task<KeyValuePair<bool, int>> GetRegion()
        {
            var serverId = 0;
            if (!ValidCities.Any() || IsPersonalMarker)
            {
                var validCities = ApplicationSettings.Service.GetPermittedCities(ApplicationSettings.CurrentUser.Guid, IsPersonalMarker);
                if (validCities != null)
                    ValidCities = validCities;
            }
            var fullAdress = string.Empty;
            var location =
                (Pin.Pin.Position.Latitude.ToString().Replace(",", ".") + "," +
                 Pin.Pin.Position.Longitude.ToString().Replace(",", ".")).Replace(" ", "");
            var request = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + location + "&language=" + "ru";
            var response = await
                WebRequest.Create(request).GetResponseAsync();
            var stream = response.GetResponseStream();
            if (stream == null) return new KeyValuePair<bool, int>();
            var result = Serializer.Deserialize<GoogleJsonResponse>(new StreamReader(stream).ReadToEnd());
            var firstresult = result.results.FirstOrDefault();
            if (firstresult == null) return new KeyValuePair<bool, int>();
            firstresult.address_components.Reverse();

            #region Validation

            var id = string.Empty;
            var found = false;
            var localities = 0;
            if (ValidCities.Any(i => i.Id == 0 || i.Name == "Без города"))
            {
                countryCode = firstresult.address_components.FirstOrDefault(ac => ac.types.Any(t => t == "country")).short_name;
            }
                //country
                foreach (var component in firstresult.address_components)
            {
                if (!found)
                {
                    if (component.types.Any(item => item == "postal_code"))
                        continue;
                    fullAdress += component.long_name;
                    if (component.types.Any(item => item == "locality") ||
                        component.types.Any(item => item == "administrative_area_level_3"))
                        //      if (component.types.Any(item => item == "locality") )
                    {
                        localities++;
                        request = "https://maps.googleapis.com/maps/api/geocode/json?address=" + fullAdress;
                        response = await WebRequest.Create(request).GetResponseAsync();
                        stream = response.GetResponseStream();
                        var tempresult = Serializer.Deserialize<GoogleJsonResponse>(new StreamReader(stream).ReadToEnd());
                        if (tempresult.status == "OK")
                        {
                            id = tempresult.results.FirstOrDefault()?.place_id;
                            if (id == null) continue;
                            if (ValidCities.Select(item => item.PlaceId).Contains(id))
                            {
                                serverId = ValidCities.FirstOrDefault(item => item.PlaceId == id).Id;
                                found = true;
                            }
                        }
                    }
                    fullAdress += ", ";
                }
            }

            #endregion Validation

            if (found)
            {
                City = ValidCities.FirstOrDefault(item => item.PlaceId == id).Name;
                foreach (var component in firstresult.address_components)
                {
                    if (component.types.Any(item => item == "street_number"))
                    {
                        if (component.short_name.Contains("с"))
                        {
                            var index = component.short_name.IndexOf('с');
                            House = component.short_name.Substring(0, index);
                            Building = component.short_name.Substring(index + 1);
                        }
                        else
                        {
                            House = component.short_name;
                        }
                    }
                    if (component.types.Any(item => item == "route"))
                    {
                        Street = component.short_name;
                    }
                }
            }
            return new KeyValuePair<bool, int>(found, serverId);
        }

        /// <summary>
        /// Указывает была ли произведена проверка по доступным регионам
        /// </summary>
        private bool _haveChecked;
        private async void CheckRegion()
        {
            _haveChecked = true;
            CytiId = 0;
            IsValidLocation = true;

            /*
            _haveChecked = true;
            await Task.Run(() =>
            {
                var result = GetRegion().Result;
                if (result.Key)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //ApplicationSettings.CreateToast(ToastNotificationType.Success, "Правильная геолокация.");
                        IsValidLocation = true;
                        CytiId = result.Value;
                    });
                }
                else
                {
                    if (!IsPersonalMarker)
                    {
                        if (ValidCities.Any(vc => vc.Id == 0 || vc.Name == "Без города"))
                        {
                            var permittedCountry =
                                ApplicationSettings.Service.GetPermittedCountry(ApplicationSettings.CurrentUser.Guid);
                            if (permittedCountry?.Count > 0)
                            {
                                if (permittedCountry.Any(i => i.Code == countryCode))
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        CytiId = 0;
                                        IsValidLocation = true;
                                    });
                                    return;
                                }
                            }
                        }
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (IsPersonalMarker)
                        {
                            IsValidLocation = true;
                        }
                        else
                        {
                            
                            ApplicationSettings.CreateToast(ToastNotificationType.Error,
                                ExceptionResources.NotAllowedToAddPins);
                            IsValidLocation = false;
                        }
                    });
                }
            });
            */
        }

        #region Event Handlers

        private void Category_ItemTapped(object sender, int e)
        {
            var category = PinCategories.FirstOrDefault(item => item.Id == e);
            if (category != null)
            {
                category.ItemSelected = !category.ItemSelected;
                if (category.ItemSelected)
                    foreach (var c in PinCategories.Where(item => item.Id != category.Id))
                    {
                        c.ItemSelected = false;
                    }
                UpdateSubcatgoriesVisible(/*category.Id, category.ItemSelected*/);
            }
            if (category == null || category.ItemSelected)
            {
                foreach (var subCategory in PinSubCategories)
                {
                    subCategory.ItemSelected = false;
                }
                OpenCloseCategoriesCommand.Execute(null);
            }
            CheckFields();
        }

        private void Tag_ItemTapped(object sender, int e)
        {
            var tag = PinSubCategories.FirstOrDefault(item => item.Id == e);
            if (tag != null)
                tag.ItemSelected = !tag.ItemSelected;
            OnPropertyChanged(nameof(SelectedTags));
            OnPropertyChanged(nameof(Tags));

            CheckFields();
        }

        public void WoktimeVm_CalendarClicked(object sender, EventArgs e)
        {
            var workTimevm = sender as WorkTimeViewModel;

            if (workTimevm == null) return;

            foreach (var item in WeekViewModel.Days)
            {
                item.IsEnabled =
                    WorkTimeViewModels.Where(vm => vm != workTimevm).SelectMany(vm => vm.Days).All(d => d.Day != item.Day);//.Contains(item);
                //Деактивировать выбранные в других строках дни недели.
                item.IsSelected = workTimevm.Days.Any(d=>d.Day==item.Day);
                    //workTimevm.Days.Contains(item);
                //Активировать и выбрать выбранные в данной строке дни недели.
            }
            WeekViewModel.CurrId = workTimevm.Id;
            DaysPopupVisible = !DaysPopupVisible;
        }

        public void Phone_PlusMinusClicked(object sender, bool e)
        {
            PhonePlusMinusClicked?.Invoke(this, new KeyValuePair<object, bool>(sender, e));
        }

        public void WoktimeVm_PlusMinusClicked(object sender, bool e)
        {
            TimePlusMinusClicked?.Invoke(this, new KeyValuePair<object, bool>(sender, e));
        }

        #endregion
        #endregion

        #region Commands

        public ICommand SelectPersonalMarkerTypeCommand=>new Command(act =>
        {
            IsPersonalMarker = true;
            //ApplicationSettings.CreateToast(ToastNotificationType.Success, "Личная метка.");
            CheckRegion();
        });

        public ICommand SelectGuideMarkerTypeCommand => new Command(act =>
        {
            if (IsGuide)
            {
                IsPersonalMarker = false;
                //ApplicationSettings.CreateToast(ToastNotificationType.Success, "Метка для отправки на проверку.");
                CheckRegion();
            }
        });

        public ICommand AddPinCommand => new Command(() =>
        {
            Task.Run(() =>
            {
                if (!_haveChecked)
                {
                    CheckRegion();
                }
                var addResult = false;
                try
                {
                    StartLoading();
                    var openTimes = new List<WorkTimeDay>();
                    var closeTimes = new List<WorkTimeDay>();

                    foreach (var workTime in WorkTimeViewModels)
                    {
                        foreach (var day in workTime.Days)
                        {
                            openTimes.Add(new WorkTimeDay
                            {
                                Time = workTime.StartTime,
                                WeekDayId = day.Day.ToRussian()
                            });
                            closeTimes.Add(new WorkTimeDay
                            {
                                Time = workTime.StopTime,
                                WeekDayId = day.Day.ToRussian()
                            });
                        }
                    }
                    int discount = SelectedDiscountIndex >= 0 ? SelectedDiscountIndex*5 : 0;
                    var entryTicket = "";
                    var phones =
                        PhoneViewModels.Where(item => !string.IsNullOrEmpty(item.Phone.Number))
                            .Select(item => item.Phone.Number)
                            .ToArray();
                    
                    DeserializeBase result;
                    var fileProvider = DependencyService.Get<IFileSystemWork>();
                    Photos.Remove(string.Empty);
                    if (!_isRedact)
                    {
                        var tempListOfBase64Photo = new List<string>();
                        foreach (var photo in Photos)
                        {
                            var temp = fileProvider.GetFile(photo);
                            if (temp != null)
                                tempListOfBase64Photo.Add(Convert.ToBase64String(temp));
                        }

                        result = ApplicationSettings.Service.CreateMarker(ApplicationSettings.CurrentUser.Guid, Pin.Name,
                            Pin.Introduction, Pin.Description, CytiId, Pin.BaseCategoryId, Pin.Pin.Position.Latitude,
                            Pin.Pin.Position.Longitude, entryTicket, discount, Street, House, Building, "", Pin.Site,
                            Pin.Email, tempListOfBase64Photo.ToArray(), SelectedSubCategories.ToArray(), phones, openTimes.ToArray(),
                            closeTimes.ToArray(), IsPersonalMarker);
                    }
                    else
                    {
                        if (Photos.Count(path=>!string.IsNullOrEmpty(path))==0 || Photos==OldPhotos)//нет новых фото
                        {
                            result = ApplicationSettings.Service.EditMarker(ApplicationSettings.CurrentUser.Guid, Pin.Name,
                                Pin.Introduction, Pin.Description, CytiId, Pin.BaseCategoryId, Pin.Pin.Position.Latitude,
                                Pin.Pin.Position.Longitude, entryTicket, discount, Street, House, Building, "", Pin.Site,
                                Pin.Email/*, Pin.Photo*/, new string[0], SelectedSubCategories.ToArray(), phones,
                                openTimes.ToArray(),
                                closeTimes.ToArray(), IsPersonalMarker, Pin.Id);
                        }
                        else
                        {
                            var tempListOfBase64Photo = new List<string>();
                            foreach (var photoPath in Photos)
                            {
                                if (photoPath.Contains("http://185.76.145.214/"))
                                {
                                    tempListOfBase64Photo.Add(photoPath);
                                }
                                else
                                {
                                    var temp = fileProvider.GetFile(photoPath);
                                    if (temp != null)
                                        tempListOfBase64Photo.Add(Convert.ToBase64String(temp));
                                }
                            }
                            result = ApplicationSettings.Service.EditMarker(ApplicationSettings.CurrentUser.Guid, Pin.Name,
                                Pin.Introduction, Pin.Description, CytiId, Pin.BaseCategoryId, Pin.Pin.Position.Latitude,
                                Pin.Pin.Position.Longitude, entryTicket, discount, Street, House, Building, "", Pin.Site,
                                Pin.Email/*, Pin.Photo*/, tempListOfBase64Photo.ToArray(), SelectedSubCategories.ToArray(), phones,
                                openTimes.ToArray(),
                                closeTimes.ToArray(), IsPersonalMarker, Pin.Id);

                        }
                    }
                    addResult = result != null && result.Success;
                }
                catch (Exception e)
                {
                    // ignored
                }
                finally
                {
                    StopLoading();
                    if (addResult && !IsPersonalMarker && !_isRedact)
                    {
                        ApplicationSettings.CreateToast(ToastNotificationType.Success,
                            TextResource.ThePinIsAddedToast);
                        Device.BeginInvokeOnMainThread(GoBack);
                    }
                    else if (addResult && IsPersonalMarker)
                    {
                        if (_isRedact)
                        {
                            ApplicationSettings.CreateToast(ToastNotificationType.Success,
                                TextResource.ThePinIsAddedToast);
                        }
                        else
                        {
                            ApplicationSettings.CreateToast(ToastNotificationType.Success,
                                TextResource.PinAddedToast);
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ApplicationSettings.GoToPage(new TKMapView());
                            var temp =
                                ApplicationSettings.MainPage.Navigation.NavigationStack.Where(
                                    i => i.GetType() == typeof (AddPinView)).ToList();
                            foreach (var page in temp)
                            {
                                ApplicationSettings.MainPage.Navigation.RemovePage(page);
                            }
                            var tempList = ApplicationSettings.MainPage.Navigation.NavigationStack.Where(
                                i => i.GetType() == typeof (TKMapView)).ToList();
                            if (tempList.Count > 1)
                            {
                                for (int i = 0; i < tempList.Count - 1; i++)
                                {
                                    ApplicationSettings.MainPage.Navigation.RemovePage(tempList[i]);
                                }
                            }
                        });
                    }
                }
            });
        });

        public Command<TKCustomMapPin> DragEndCommand
        {
            get
            {
                return new Command<TKCustomMapPin>(pin =>
                {
                    Pin.Pin.Position = pin.Position;
                    /*await */CheckRegion();
                });
            }
        }

        public ICommand OpenCloseCategoriesCommand
            => new Command(() => { CategoriesPopupVisible = !CategoriesPopupVisible; });

        public ICommand OpenCloseTagsCommand => new Command(() =>
        {
            if (BaseCategorySelected)
                TagsPopupVisible = !TagsPopupVisible;
        });

        public ICommand CloseDaysCommand => new Command<ButtonAction>(action =>
        {
            switch (action)
            {
                case ButtonAction.Ok:
                    var workTimeViewModel = WorkTimeViewModels.FirstOrDefault(item => item.Id == WeekViewModel.CurrId);
                    workTimeViewModel.Days =
                        WeekViewModel.Days.Where(item => item.IsSelected && item.IsEnabled).ToList();
                    break;
                case ButtonAction.Cancel:
                    break;
            }
            DaysPopupVisible = !DaysPopupVisible;
        });

        #endregion
        
    }
}
