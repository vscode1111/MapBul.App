using System;
using System.Collections.Generic;
using System.Linq;
using Map_Bul_App.Design;
using Map_Bul_App.Models;
using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Map_Bul_App.ViewModel.Design;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XLabs.Platform.Device;

namespace Map_Bul_App.Views
{
    public partial class AddPinView
    {
        public AddPinView(Position position)
        {
            Position = position;
            CurrentViewModel = new AddNewPinViewModel(Position);
            InitializeComponent();
            Init();
        }

        public AddPinView(UserPinDescriptor pin)
        {
            CurrentViewModel = new AddNewPinViewModel(pin);
            InitializeComponent();
            Init();
        }

        private Position Position { get; set; }

        private void CurrentViewModel_PhonePlusMinusClicked(object sender, KeyValuePair<object, bool> e)
        {
            var senderVm = (PhoneViewModel)e.Key;
            if (e.Value) //Добавление
            {
                if (PhoneStack.Children.Count < 5 && CurrentViewModel.PhoneViewModels.All(item=>!string.IsNullOrEmpty(item.Phone.Number)))
                {
                    var newPhoneVm = new PhoneViewModel(new Phone());
                    newPhoneVm.PlusMinusClicked += CurrentViewModel.Phone_PlusMinusClicked;
                    CurrentViewModel.PhoneViewModels.Add(newPhoneVm);
                    var phoneCellView = new PhoneCell
                    {
                        BindingContext = newPhoneVm
                    };
                    PhoneStack.Children.Add(phoneCellView);
                }
            }
            else //Удаление
            {
                var todelete = PhoneStack.Children.FirstOrDefault(item => item.BindingContext == e.Key);
                if (todelete == null) return;
                try
                {
                    var result = PhoneStack.Children.Remove(todelete);
                    if (result)
                    {
                        CurrentViewModel.PhoneViewModels.Remove(senderVm);
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }

        private void CurrentViewModelTimeTimePlusMinusClicked(object sender, KeyValuePair<object, bool> e)
        {
            var senderVm = (WorkTimeViewModel)e.Key;
            if (e.Value)
            {
                if (WorkTimeStack.Children.Count >= 7 ||
                    !CurrentViewModel.WorkTimeViewModels.All(
                        item =>
                            (item.IsFilled) && CurrentViewModel.WorkTimeViewModels.SelectMany(vm => vm.Days).Count() < 7))
                    return;
                var newWorkTimeVm = new WorkTimeViewModel(new List<MyDayOfWeek>());
                newWorkTimeVm.PlusMinusClicked += CurrentViewModel.WoktimeVm_PlusMinusClicked;
                newWorkTimeVm.CalendarClicked += CurrentViewModel.WoktimeVm_CalendarClicked;
                CurrentViewModel.WorkTimeViewModels.Add(newWorkTimeVm);
                var workTimeView = new WorkTimeCell
                {
                    BindingContext = newWorkTimeVm
                };
                WorkTimeStack.Children.Add(workTimeView);
            }
            else
            {
                var todelete = WorkTimeStack.Children.FirstOrDefault(item => item.BindingContext == e.Key);
                if (todelete == null) return;
                try
                {
                  var result=  WorkTimeStack.Children.Remove(todelete);
                    if (result)
                    {
                        CurrentViewModel.WorkTimeViewModels.Remove(senderVm);
                    }
                }
                catch (Exception ex)
                {
                        
                    //  throw;
                }
            }
        }

        private AddNewPinViewModel CurrentViewModel
        {
            get => BindingContext as AddNewPinViewModel;
            set => BindingContext = value;
        }

        protected override void OnAppearing()
        {
            CurrentViewModel.Initialize();
            base.OnAppearing();
            if (Device.OS == TargetPlatform.Android)
                TkMap.PropertyChanged += TkMap_PropertyChanged;

            ImagesView.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "HaveChange")
                    CurrentViewModel.CheckFields();
            };
        }

        private void TkMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MapCenter")
                CurrentViewModel.MapCenter = CurrentViewModel.Pins.FirstOrDefault().Position;
        }

        private void Init()
        {
            var inches = ApplicationSettings.ThisDevice.Display.ScreenHeightInches();
            CategoryPopup.HeightRequest = TagsPopup.HeightRequest = DaysPopup.HeightRequest =
                ApplicationSettings.ThisDevice.Display.HeightRequestInInches(inches);
            CurrentViewModel.TimePlusMinusClicked += CurrentViewModelTimeTimePlusMinusClicked;
            CurrentViewModel.PhonePlusMinusClicked += CurrentViewModel_PhonePlusMinusClicked;

            #region [ Для времени работы ]

            foreach (var workTime in CurrentViewModel.WorkTimeViewModels)
            {
                var newWorkTimeView = new WorkTimeCell
                {
                    BindingContext = workTime
                };
                WorkTimeStack.Children.Add(newWorkTimeView);
            }

            #endregion [ Для времени работы ]

            #region [ Для телефонов ]

            foreach (var phone in CurrentViewModel.PhoneViewModels.ToList())
            {
                var newPhoneView = new PhoneCell
                {
                    BindingContext = phone
                };
                PhoneStack.Children.Add(newPhoneView);
            }

            #endregion [ Для телефонов ]

            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
            if (Device.OS == TargetPlatform.Android)
            {
                TkMap.HasZoomEnabled = false;
                TkMap.HasScrollEnabled = false;
            }
            DiscountPicker.Items.Add("0 %");
            DiscountPicker.Items.Add("5 %");
            DiscountPicker.Items.Add("10 %");
            DiscountPicker.Items.Add("15 %");
            DiscountPicker.Items.Add("20 %");
            DiscountPicker.Items.Add("25 %");
            DiscountPicker.Items.Add("30 %");
            DiscountPicker.SelectedIndex = 0;
        }

        private void CheckFields(object sender, EventArgs e)
        {
            CurrentViewModel.CheckFields();
        }

    }
}
