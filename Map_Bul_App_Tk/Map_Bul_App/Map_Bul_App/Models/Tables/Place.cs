using System;
using System.Linq;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using SQLite.Net.Attributes;
using TK.CustomMap;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.Models.Tables
{

    [Table("Place")]
    public class Place
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Buliding { get; set; }
        public string Phone { get; set; }
        public string BaseCategoryName { get; set; }
        public string RootCategoryName { get; set; }
        public string PinImage { get; set; }
        public string Photo { get; set; }
        public string Icon { get; set; }
        public string InfoImage { get; set; }
        public string Tags { get; set; }
        public string CategoriesBranch { get; set; }
        public string Color { get; set; }
        public TimeSpan? OpenTime1 { get; set; }
        public TimeSpan? OpenTime2 { get; set; }
        public TimeSpan? OpenTime3 { get; set; }
        public TimeSpan? OpenTime4 { get; set; }
        public TimeSpan? OpenTime5 { get; set; }
        public TimeSpan? OpenTime6 { get; set; }
        public TimeSpan? OpenTime7 { get; set; }

        public TimeSpan? CloseTime1 { get; set; }
        public TimeSpan? CloseTime2 { get; set; }
        public TimeSpan? CloseTime3 { get; set; }
        public TimeSpan? CloseTime4 { get; set; }
        public TimeSpan? CloseTime5 { get; set; }
        public TimeSpan? CloseTime6 { get; set; }
        public TimeSpan? CloseTime7 { get; set; }

        public bool IsFavorite { get; set; }
        public string Email { get; set; }
        public bool Wifi { get; set; }
        public string Number { get; set; }
        public string Site { get; set; }
        public string SubcategoryName { get; set; }
        public int Discount { get; set; }
        public string EntryTicket { get; set; }
        public string Floor { get; set; }
        public int BaseCategoryId { get; set; }
        public string SubCategories { get; set; }
        public bool IsNowOpen => true;
        public string OwnerServerId { get; set; }
        public Place()
        {
        }

        public Place(DeserializeGetMarkerDetailsData p, bool isFavorite)
        {
            ServerId = p.Id;
            Name = p.Name;
            Lat = p.Lat;
            Lng = p.Lng;
            Introduction = p.Introduction;
            Description = p.Description;
            PinImage = p.Pin;
            Icon = p.Icon;
            Photo = p.Photo;
            InfoImage = p.Logo;
            Color = p.Color;
            City = p.CityName;
            Street = p.Street;
            House = p.House;
            Buliding = p.Buliding;
            Phone = p.Phones.FirstOrDefault(item => item.Primary)?.Number ??
                    string.Empty;

            OpenTime1 = p.WorkTime.FirstOrDefault(item => item.Id == 1)?.OpenTime;
            OpenTime2 = p.WorkTime.FirstOrDefault(item => item.Id == 2)?.OpenTime;
            OpenTime3 = p.WorkTime.FirstOrDefault(item => item.Id == 3)?.OpenTime;
            OpenTime4 = p.WorkTime.FirstOrDefault(item => item.Id == 4)?.OpenTime;
            OpenTime5 = p.WorkTime.FirstOrDefault(item => item.Id == 5)?.OpenTime;
            OpenTime6 = p.WorkTime.FirstOrDefault(item => item.Id == 6)?.OpenTime;
            OpenTime7 = p.WorkTime.FirstOrDefault(item => item.Id == 7)?.OpenTime;

            CloseTime1 = p.WorkTime.FirstOrDefault(item => item.Id == 1)?.CloseTime;
            CloseTime2 = p.WorkTime.FirstOrDefault(item => item.Id == 2)?.CloseTime;
            CloseTime3 = p.WorkTime.FirstOrDefault(item => item.Id == 3)?.CloseTime;
            CloseTime4 = p.WorkTime.FirstOrDefault(item => item.Id == 4)?.CloseTime;
            CloseTime5 = p.WorkTime.FirstOrDefault(item => item.Id == 5)?.CloseTime;
            CloseTime6 = p.WorkTime.FirstOrDefault(item => item.Id == 6)?.CloseTime;
            CloseTime7 = p.WorkTime.FirstOrDefault(item => item.Id == 7)?.CloseTime;

            Tags = p.Subcategories.Select(item => item.Name).ToTagsString();
            CategoriesBranch = p.CategoriesBranch.Any()
                ? string.Join(",", p.CategoriesBranch.Select(item => item.Id))
                : string.Empty;
            BaseCategoryName = p.BaseCategoryName;
            RootCategoryName = p.RootCategoryName;
        }

        public TKCustomMapPin ToTkCustomMapPin()
        {
            return new TKCustomMapPin
            {
                Id = ServerId,
                DefaultPinColor = Xamarin.Forms.Color.FromHex(Color),
                Image = PinImage,
                InfoImage = InfoImage,
                IsDraggable = false,
                IsSmall = false,
                Position = new Position(Lat, Lng),
                ShowCallout = false,
                IsVisible = true,
                WorkTime = CurrentWorkTime(),
                Title = Name,
                Subtitle = RootCategoryName,
                Tags = Tags,
                Type = BaseCategoryName
            };
        }

        private string CurrentWorkTime()
        {
            var today = DateTime.Now.DayOfWeek;
            TimeSpan? openTime = null;
            TimeSpan? closeTime = null;
            switch (today)
            {
                case DayOfWeek.Monday:
                    {
                        openTime = OpenTime1;
                        closeTime = CloseTime1;
                        break;
                    }

                case DayOfWeek.Tuesday:
                    {
                        openTime = OpenTime2;
                        closeTime = CloseTime2;
                    }
                    break;
                case DayOfWeek.Wednesday:
                    {
                        openTime = OpenTime3;
                        closeTime = CloseTime3;
                    }
                    break;
                case DayOfWeek.Thursday:
                    {
                        openTime = OpenTime4;
                        closeTime = CloseTime4;
                    }
                    break;
                case DayOfWeek.Friday:
                    {
                        openTime = OpenTime5;
                        closeTime = CloseTime5;
                    }
                    break;
                case DayOfWeek.Saturday:
                    {
                        openTime = OpenTime6;
                        closeTime = CloseTime6;
                    }
                    break;
                case DayOfWeek.Sunday:
                    {
                        openTime = OpenTime7;
                        closeTime = CloseTime7;
                    }
                    break;
            }
            var workTime = (openTime != null && closeTime != null) ? openTime.Value.Hours + ":" + (openTime.Value.Minutes < 10 ? "0" + openTime.Value.Minutes : openTime.Value.Minutes.ToString()) + " - " + closeTime.Value.Hours + ":" + (closeTime.Value.Minutes < 10 ? "0" + closeTime.Value.Minutes : closeTime.Value.Minutes.ToString()) : TextResource.Closed;
            return workTime;
        }
    }
}
