using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Map_Bul_App.Annotations;
using Map_Bul_App.Models.Tables;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.Models
{
    public class PinDescriptor
    {
        public TKCustomMapPin Pin { get; set; }

        public bool IsVisible { get; set; }

        public List<int> CategoriesBranch { get; set; }

        public int RootCategory => CategoriesBranch.LastOrDefault();

        public IEnumerable<int> SubCategories => CategoriesBranch.Take(CategoriesBranch.Count - 1).ToList();

        public TimeSpan? OpenTime { get; set; }

        public TimeSpan? CloseTime { get; set; }

        public bool WiFi { get; set; }

        public bool Personal { get; set; }

        public bool NowOpen
        {
            get
            {
                if (OpenTime != null && CloseTime != null)
                {
                    return OpenTime < DateTime.Now.TimeOfDay &&
                           CloseTime > DateTime.Now.TimeOfDay;
                }
                else
                {
                    return true;
                }
            }
        }
            //=>
            //    OpenTime != null && CloseTime != null && OpenTime < DateTime.Now.TimeOfDay &&
            //    CloseTime > DateTime.Now.TimeOfDay;


        public PinDescriptor()
        {
        }

        public PinDescriptor(DeserializeGetMarkersData item)
        {
            var tags = item.SubCategories.ToTagsString();
            var todayWorkTime =
                item.WorkTime.FirstOrDefault(
                    d => d.Id.ToDayOfWeek() == DateTime.Now.DayOfWeek);
            var workTime = (todayWorkTime != null) ? todayWorkTime.OpenTime.Hours + ":" + (todayWorkTime.OpenTime.Minutes < 10 ? "0" + todayWorkTime.OpenTime.Minutes : todayWorkTime.OpenTime.Minutes.ToString()) + " - " + todayWorkTime.CloseTime.Hours + ":" + (todayWorkTime.CloseTime.Minutes < 10 ? "0" + todayWorkTime.CloseTime.Minutes : todayWorkTime.CloseTime.Minutes.ToString()) : TextResource.Closed;
            OpenTime = todayWorkTime?.OpenTime;
            CloseTime = todayWorkTime?.CloseTime;
            if (todayWorkTime != null && todayWorkTime.OpenTime == todayWorkTime.CloseTime)
            {
                workTime = TextResource.EvryTime;
                OpenTime = null;
                CloseTime = null;
            }
            Pin = new TKCustomMapPin
            {
                Id = item.Id,
                Position = new Position(item.Lat, item.Lng),
                Title = item.Name,
                Image = item.Icon,
                InfoImage = item.Logo,
                ShowCallout = false,
                IsDraggable = false,
                WorkTime = workTime,
                Tags = tags
            };

            CategoriesBranch = item.CategoriesBranch;
            IsVisible = false;
            WiFi = item.Wifi;
            Personal = item.Personal;
        }
    }


    public class UserPinDescriptor : PinDescriptor, INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _introduction;
        public string Introduction
        {
            get { return _introduction; }
            set
            {
                if (value != _introduction)
                {
                    _introduction = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CityId { get; set; }
        public int BaseCategoryId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string EntryTicket { get; set; }
        public int DiscountId { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Buliding { get; set; }
        public string Floor { get; set; }

        private string _site;
        public string Site
        {
            get { return _site; }
            set
            {
                if (value != _site)
                {
                    _site = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _email;
        public string Email
        {
            get {return _email;}
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> Photos { get; set; }
        public List<string> PhotosMini { get; set; }
        public string Photo { get; set; }
        public string Icon { get; set; }
        public string InfoImage { get; set; }
        public string PinImage { get; set; }

        public List<Subcategory> Subcategories { get; set; } //Теги
        public List<WorkTime> WorkTime { get; set; }
        public Phone Phone => Phones.FirstOrDefault(item => item.Primary);//{ get; set; }
        public List<Phone> Phones { get; set; } 
        //public List<int> Subcategories { get; set; }
        public string Adress { get; set; }
        public string BaseCategoryName { get; set; }
        public string RootCategoryName { get; set; }
        public List<string> Tags { get; set; }
        public string StringTags { get; set; }
            
        public string HexColor { get; set; }

        public Color Color=>Color.FromHex(HexColor);

        public event EventHandler DeleteFromFavorites;

        public bool HaveRelatedEvents { get; set; }

        public UserPinDescriptor()
        {

        }
        public UserPinDescriptor(Place item)
        {
            Name = item.Name;
            InfoImage = item.InfoImage;
            Icon = item.Icon;
            HexColor = item.Color;
            StringTags = item.Tags;
            BaseCategoryName = item.BaseCategoryName;
            RootCategoryName = item.RootCategoryName;
            Id = item.ServerId;
    }
        public ICommand DeleteCommand=> new Command(OnDeleteFromFavorites);
        protected virtual void OnDeleteFromFavorites()
        {
            DeleteFromFavorites?.Invoke(this, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
