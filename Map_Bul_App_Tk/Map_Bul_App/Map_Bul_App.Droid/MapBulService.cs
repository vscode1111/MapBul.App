using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Protocols;
using Map_Bul_App.Droid.MapBulWebReference;
using Map_Bul_App.Settings;
using Xamarin.Forms;
using MapBulService = Map_Bul_App.Droid.MapBulService;

[assembly: Dependency(typeof (MapBulService))]

namespace Map_Bul_App.Droid
{
    public class MapBulService : IMapBulService
    {
        private WebService _service;

        public MapBulService()
        {
            Initialize();
        }

        private void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback = OnValidationCallback;
            _service = new WebService();
        }

        private static bool OnValidationCallback(object sender, X509Certificate cert, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true;
        }

        public string Login(string login, string password)
        {
            return _service.Authorize(login, password);
        }

        public string GetCategories()
        {
            return _service.GetRootCategories(ApplicationSettings.GetLanguage);
        }

        public string GetUserTypeById(int id, string userGuid)
        {
            return _service.GetUserTypeById(id, userGuid);
        }

        /// <summary>
        ///     P1- левый верхний угол.
        ///     P2 - правый нижний угол.
        /// </summary>
        /// <returns></returns>
        public string GetMarkers(double p1Lat, double p1Lng, double p2Lat, double p2Lng)
        {
            string userGuid = "";
            if (ApplicationSettings.CurrentUser.UserType != UserTypesMobile.Guest)
            {
                userGuid = ApplicationSettings.CurrentUser.Guid;
            }
            return _service.GetMarkers(p1Lat, p1Lng, p2Lat, p2Lng, userGuid, ApplicationSettings.GetLanguage);
        }

        public string GetMarkerDetails(int id)
        {
            return _service.GetMarkerDescription(id, ApplicationSettings.GetLanguage);
        }

        public string GetArticles(bool isRefresh,DateTime? existingDateTime)
        {
            return _service.GetRecentArticles(ApplicationSettings.GetLanguage,isRefresh, existingDateTime);
        }

        public string GetEvents(bool isRefresh, DateTime? existingDateTime)
        {
            return _service.GetRecentEvents(ApplicationSettings.GetLanguage,isRefresh, existingDateTime);
        }


        public string CreateMarker(string userGuid, string name, string introduction, string description, int cityId,
            int baseCategoryId, double lat, double lng, string entryTicket, int discount, string street, string house,
            string building, string floor, string site, string email, string[] photoBase64, int[] subCategoryIds,
            string[] phones, IEnumerable<WorkTimeDay> openTimes, IEnumerable<WorkTimeDay> closeTimes, bool isPersonal)
        {
            try
            {
                var serviceOpenkTime = openTimes.Select(item => new KeyValueOfInt32Int32
                {
                    Key = item.WeekDayId,
                    Value = Convert.ToInt32(item.Time.TotalMinutes)
                }).ToArray();

                var serviceClosekTime = closeTimes.Select(item => new KeyValueOfInt32Int32
                {
                    Key = item.WeekDayId,
                    Value = Convert.ToInt32(item.Time.TotalMinutes)
                }).ToArray();
                var result = _service.CreateMarker(userGuid, name, introduction, description, cityId, baseCategoryId, lat, lng,
                    entryTicket, discount, street, house, building, floor, site, email, photoBase64, subCategoryIds, phones,
                    serviceOpenkTime, serviceClosekTime, isPersonal, ApplicationSettings.GetLanguage);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string EditMarker(string userGuid, string name, string introduction, string description, int cityId,
            int baseCategoryId, double lat, double lng, string entryTicket, int discount, string street, string house,
            string building, string floor, string site, string email/*, string photoPath*/, string[] photoBase64, int[] subCategoryIds,
            string[] phones, IEnumerable<WorkTimeDay> openTimes, IEnumerable<WorkTimeDay> closeTimes, bool isPersonal, int markerServerId)
        {
            try
            {
                var serviceOpenkTime = openTimes.Select(item => new KeyValueOfInt32Int32
                {
                    Key = item.WeekDayId,
                    Value = Convert.ToInt32(item.Time.TotalMinutes)
                }).ToArray();

                var serviceClosekTime = closeTimes.Select(item => new KeyValueOfInt32Int32
                {
                    Key = item.WeekDayId,
                    Value = Convert.ToInt32(item.Time.TotalMinutes)
                }).ToArray();
                var result = _service.EditMarker(userGuid, name, introduction, description, cityId, baseCategoryId,
                    lat, lng,
                    entryTicket, discount, street, house, building, floor, site, email/*, photoPath*/, photoBase64, subCategoryIds,
                    phones,
                    serviceOpenkTime, serviceClosekTime, isPersonal, markerServerId, ApplicationSettings.GetLanguage);
                return result;
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string GetPermittedCities(string userGuid, bool isPersonal)
        {
            return _service.GetPermittedCities(userGuid, isPersonal);
        }

        public string GetPermittedCountry(string userGuid)
        {
            return _service.GetPermittedCountry(userGuid);
        }

        public string RegisterTenant(string email, string firstName, string middleName, string lastName, DateTime birthDate, string gender, string phone, string address=default(string))
        {
            return _service.RegisterTenant(email, firstName, middleName, lastName, birthDate, gender, phone, address, ApplicationSettings.GetLanguage);
        }

        public string RecoverPassword(string email)
        {
            return _service.RecoverPassword(email, ApplicationSettings.GetLanguage);
        }



        public string GetSessionMarkers(double p1Lat, double p1Lng, double p2Lat, double p2Lng,string sessionToken)
        {
            string userGuid = "";
            if (ApplicationSettings.CurrentUser.UserType != UserTypesMobile.Guest)
            {
                userGuid = ApplicationSettings.CurrentUser.Guid;
            }
            if (string.IsNullOrEmpty(sessionToken))
                return _service.GetMarkers(p1Lat, p1Lng, p2Lat, p2Lng, userGuid, ApplicationSettings.GetLanguage);
            return _service.GetSessionMarkers( p1Lat,  p1Lng,  p2Lat,  p2Lng, sessionToken, userGuid, ApplicationSettings.GetLanguage);
        }


        public string ClearSession(string sessionToken)
        {
            return _service.RemoveRequestSession(sessionToken);
        }

        public string SaveFavoriteArticleAndEvent(string userGuid, int articleEventSertverId)
        {
            try
            {
                return _service.SaveFavoriteArticleAndEvent(userGuid, articleEventSertverId);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string RemoveFavoriteArticleAndEvent(string userGuid, int articleEventSertverId)
        {
            try
            {
                return _service.RemoveFavoriteArticleAndEvent(userGuid, articleEventSertverId);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public string GetFavoritsArticlAndEvent(string userGuid)
        {
            try
            {
                return _service.GetFavoritsArticlAndEvent(userGuid, ApplicationSettings.GetLanguage);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string SaveFavoriteMarker(string userGuid, int markerSertverId)
        {
            try
            {
                return _service.SaveFavoriteMarker(userGuid, markerSertverId);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string RemoveFavoriteMarker(string userGuid, int markerSertverId)
        {
            try
            {
                return _service.RemoveFavoriteMarker(userGuid, markerSertverId);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string GetFavoritsMarker(string userGuid)
        {
            try
            {
                return _service.GetFavoritsMarker(userGuid, ApplicationSettings.GetLanguage);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string GetRelatedEventsFromMarker(int markerServerId, bool nearest)
        {
            try
            {
                return _service.GetRelatedEventsFromMarker(markerServerId, nearest, ApplicationSettings.GetLanguage);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}