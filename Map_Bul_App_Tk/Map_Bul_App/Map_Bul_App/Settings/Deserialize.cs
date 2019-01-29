using System;
using System.Collections.Generic;
using System.Linq;
using Map_Bul_App.Models;
using Newtonsoft.Json;

namespace Map_Bul_App.Settings
{
    internal static class Serializer
    {
        internal static T Deserialize<T>(string json)
        {
            var res = JsonConvert.DeserializeObject<T>(json);
            return res;
        }
    }

    public class DeserializeBase
    {
        public bool Success { get; set; }
        public string ErrorReason { get; set; }
    }

    public class WorkTimeDay
    {
        public int WeekDayId { get; set; }
        public TimeSpan Time { get; set; }
    }

    #region [ Auth ]

    public class DeserializeAuthorize : DeserializeBase
    {
        public List<DeserializeAuthorizeData> Data { get; set; }
    }

    public class DeserializeAuthorizeData
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Guid { get; set; }
        public string UserTypeTag { get; set; }
        public string Email { get; set; }
        public string UserTypeId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool Deleted { get; set; }
    }


    public class DeserializeUserType : DeserializeBase
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
    }

    #endregion [ Auth ]

    #region [Markers]

    public class DeserializeGetMarkers : DeserializeBase
    {
        public List<DeserializeGetMarkersData> Data { get; set; }
    }

    public class DeserializeGetMarkersData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Icon { get; set; }
        public string Logo { get; set; }
        public List<string> SubCategories { get; set; }
        public List<int> CategoriesBranch { get; set; } // Первый - базовая, последний - корневая
        public List<WorkTime> WorkTime { get; set; }
        public bool Wifi { get; set; }
        public bool Personal { get; set; }
    }

    public class WorkTime
    {
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public int Id { get; set; }
    }

    public class DeserializeGetMarkerDetails : DeserializeBase
    {
        public List<DeserializeGetMarkerDetailsData> Data { get; set; }
    }

    public class DeserializeGetMarkerDetailsData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Description { get; set; }
        public int BaseCategoryId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Pin { get; set; }
        public string Icon { get; set; }
        public string Photo { get; set; }
        public string Logo { get; set; }
        public string Color { get; set; }
        public string CityName { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Buliding { get; set; }
        public List<string> Photos { get; set; }
        public List<string> PhotosMini { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Subcategory> CategoriesBranch { get; set; } // Первый - базовая, последний - корневая
        public List<Subcategory> Subcategories { get; set; } //Теги
        public List<WorkTime> WorkTime { get; set; }
        public bool Wifi { get; set; }
        public bool Personal { get; set; }
        public string BaseCategoryName => CategoriesBranch.FirstOrDefault().Name;
        public string RootCategoryName => CategoriesBranch.LastOrDefault().Name;
        public bool HaveRelatedEvents { get; set; }
    }

    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Phone
    {
        public bool Primary { get; set; }
        public string Number { get; set; }
    }

    #endregion

    #region [Catgories]

    public class DeserializeCategory
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string AddedDate { get; set; }
        public string Icon { get; set; }
        public string Pin { get; set; }
        public List<DeserializeCategory> ChildCategories { get; set; }
    }

    public class DeserializeCategories : DeserializeBase
    {
        public List<DeserializeCategory> Data { get; set; }
    }

    #endregion

    #region Articles

    public class DeserializeGetArticlesData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitlePhoto { get; set; }
        public string Photo { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        //public DateTime? EventDate { get; set; } // Поле уже не используется
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? _stopDate;
        public DateTime? StopDate
        {
          get => _stopDate ?? EndDate;
            set => _stopDate = value;
        }
        public AuthorName AuthorName { get; set; }
        public string MarkerAddress { get; set; }
        public List<string> Subcategories { get; set; }
        public string Text { get; set; }
        public string SourceUrl { get; set; }
        public string SourcePhoto { get; set; }
        public string AddressName { get; set; }
        public int? MarkerId { get; set; }
    }

    public class DeserializeGetArticles : DeserializeBase
    {
        public List<DeserializeGetArticlesData> Data { get; set; }
    }

    #endregion

    #region regions

    public class DeserializeCitiesData
    {
        public string PlaceId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DeserializeCities : DeserializeBase
    {
        public List<DeserializeCitiesData> Data { get; set; }
    }

    public class DeserializeCountriesData
    {
        public string PlaceId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class DeserializeCountries : DeserializeBase
    {
        public List<DeserializeCountriesData> Data { get; set; }
    }


    #endregion
}
