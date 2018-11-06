using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Map_Bul_App.ResX;
using Plugin.Toasts;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
namespace Map_Bul_App.Settings
{
    public interface IMapBulService
    {
        string Login(string login, string password);

        string RegisterTenant(string email, string firstName, string middleName, string lastName,
            DateTime birthDate, string gender, string phone, string address = default(string));

        string RecoverPassword(string email);
        

        string GetCategories();

        
        string GetSessionMarkers(double p1Lat, double p1Lng, double p2Lat, double p2Lng, string sessionToken);
        string GetMarkerDetails(int id);

        string GetArticles(bool isRefresh, DateTime? existingDateTime);
        string GetEvents(bool isRefresh, DateTime? existingDateTime);

        string GetPermittedCities(string userGuid, bool isPersonal);

        string GetPermittedCountry(string userGuid);

        string CreateMarker(string userGuid, string name, string introduction, string description, int cityId,
            int baseCategoryId, double lat, double lng, string entryTicket, int discount, string street, string house,
            string building, string floor, string site, string email, string[] photoBase64, int[] subCategoryIds,
            string[] phones, IEnumerable<WorkTimeDay> openTimes, IEnumerable<WorkTimeDay> closeTimes, bool isPersonal);

        string ClearSession(string sessionToken);

        string SaveFavoriteArticleAndEvent(string userGuid, int articleEventSertverId);
        string RemoveFavoriteArticleAndEvent(string userGuid, int articleEventSertverId);
        string GetFavoritsArticlAndEvent(string userGuid);

        string SaveFavoriteMarker(string userGuid, int markerSertverId);

        string RemoveFavoriteMarker(string userGuid, int markerSertverId);

        string GetFavoritsMarker(string userGuid);

        string GetRelatedEventsFromMarker(int markerServerId, bool nearest);

        string EditMarker(string userGuid, string name, string introduction, string description, int cityId,
            int baseCategoryId, double lat, double lng, string entryTicket, int discount, string street, string house,
            string building, string floor, string site, string email/*,string photoPath*/, string[] photoBase64, int[] subCategoryIds,
            string[] phones, IEnumerable<WorkTimeDay> openTimes, IEnumerable<WorkTimeDay> closeTimes, bool isPersonal,
            int markerServerId);

        //string GetMarkers(double p1Lat, double p1Lng, double p2Lat, double p2Lng);
        //string GetUserTypeById(int id, string userGuid);
    }


    public sealed class MapBulService
    {
        #region [ Event ]

        public event EventHandler ErrorEvent;

        private void OnErrorEvent(string ex)
        {
            var temp = ErrorEvent;
            temp?.Invoke(ex, null);
        }

        #endregion [ Event ]

        //private CancellationTokenSource _cancelToken;
        private const int Timeout = 5000;

        #region [ Auth ]

        /// <summary>
        ///     Метод авторизации пользователя на сервере
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public DeserializeAuthorizeData Login(string login, string password)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() => { responce = DependencyService.Get<IMapBulService>().Login(login, password); },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeAuthorize>(responce);
                if (result.Success)
                {
                    return result.Data.FirstOrDefault();
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        /// <summary>
        ///     Регситрация жителя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <param name="birthDate"></param>
        /// <param name="gender"></param>
        /// <param name="phone"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public DeserializeBase RegisterTenant(string email, string firstName, string middleName, string lastName,
            DateTime birthDate, string gender, string phone, string address = default(string))
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(
                    () =>
                    {
                        responce = DependencyService.Get<IMapBulService>()
                            .RegisterTenant(email, firstName, middleName, lastName, birthDate, gender, phone, address);
                    },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeAuthorize>(responce);
                if (result.Success)
                {
                    return result;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        /// <summary>
        ///     Восстановление пароля
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public DeserializeBase RecoverPassord(string email)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() => { responce = DependencyService.Get<IMapBulService>().RecoverPassword(email); },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeAuthorize>(responce);
                if (result.Success)
                {
                    return result;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        #endregion [ Auth ]

        #region [MAP]

        /// <summary>
        ///     Получение списка категорий
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DeserializeCategory> GetCategories()
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() => { responce = DependencyService.Get<IMapBulService>().GetCategories(); },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeCategories>(responce);
                if (result.Success)
                {
                    return result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        public IEnumerable<DeserializeGetMarkersData> GetSessionMarkers(Position p1, Position p2)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                var session = ApplicationSettings.DataBase.GetCurrentSession;
                Task.Run(
                    () =>
                    {
                        responce = DependencyService.Get<IMapBulService>()
                            .GetSessionMarkers(p1.Latitude, p1.Longitude, p2.Latitude, p2.Longitude, session?.Token);
                    },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeGetMarkers>(responce);
                if (result.Success)
                {
                    return result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        public DeserializeGetMarkerDetailsData GetMarkerDetails(int id)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() => { responce = DependencyService.Get<IMapBulService>().GetMarkerDetails(id); },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeGetMarkerDetails>(responce);
                if (result.Success)
                {
                    return result.Data.FirstOrDefault();
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        #endregion

        #region Create marker

        /// <summary>
        ///     Создание метки
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="name"></param>
        /// <param name="introduction"></param>
        /// <param name="description"></param>
        /// <param name="cityId"></param>
        /// <param name="baseCategoryId"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="entryTicket"></param>
        /// <param name="discount"></param>
        /// <param name="street"></param>
        /// <param name="house"></param>
        /// <param name="building"></param>
        /// <param name="floor"></param>
        /// <param name="site"></param>
        /// <param name="email"></param>
        /// <param name="photoBase64"></param>
        /// <param name="subCategoryIds"></param>
        /// <param name="phones"></param>
        /// <param name="openTimes"></param>
        /// <param name="closeTimes"></param>
        /// <param name="isPersonal">Указывает, является ли метка персональной</param>
        /// <returns></returns>
        public DeserializeBase CreateMarker(string userGuid, string name, string introduction, string description,
            int cityId,
            int baseCategoryId, double lat, double lng, string entryTicket, int discount, string street, string house,
            string building, string floor, string site, string email, string[] photoBase64, int[] subCategoryIds,
            string[] phones, IEnumerable<WorkTimeDay> openTimes, IEnumerable<WorkTimeDay> closeTimes, bool isPersonal)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var cancel = new CancellationTokenSource(60000);
                var responce = "";
                /*Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>()
                        .CreateMarker(userGuid, name, introduction, description, cityId,
                            baseCategoryId, lat, lng, entryTicket, discount, street, house,
                            building, floor, site, email, photoBase64, subCategoryIds,
                            phones, openTimes, closeTimes);
                }, cancel.Token).Wait(cancel.Token);*/
                //responce = DependencyService.Get<IMapBulService>()
                //    .CreateMarker(userGuid, name, introduction, description, cityId,
                //        baseCategoryId, lat, lng, entryTicket, discount, street, house,
                //        building, floor, site, email, photoBase64, subCategoryIds,
                //        phones, openTimes, closeTimes);
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>()
                        .CreateMarker(userGuid, name, introduction, description, cityId,
                            baseCategoryId, lat, lng, entryTicket, discount, street, house,
                            building, floor, site, email, photoBase64, subCategoryIds,
                            phones, openTimes, closeTimes, isPersonal);
                }).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeBase>(responce);
                if (result.Success)
                {
                    return result;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }

            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }

        }

        public DeserializeBase EditMarker(string userGuid, string name, string introduction, string description,
            int cityId,
            int baseCategoryId, double lat, double lng, string entryTicket, int discount, string street, string house,
            string building, string floor, string site, string email/*,string photoPath*/, string[] photoBase64, int[] subCategoryIds,
            string[] phones, IEnumerable<WorkTimeDay> openTimes, IEnumerable<WorkTimeDay> closeTimes, bool isPersonal, int markerServerId)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var cancel = new CancellationTokenSource(60000);
                var responce = "";
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>()
                        .EditMarker(userGuid, name, introduction, description, cityId,
                            baseCategoryId, lat, lng, entryTicket, discount, street, house,
                            building, floor, site, email/*, photoPath*/, photoBase64, subCategoryIds,
                            phones, openTimes, closeTimes, isPersonal, markerServerId);
                }).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeBase>(responce);
                if (result.Success)
                {
                    return result;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }

            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }

        }

        /// <summary>
        ///     Получение доступных городов
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="isPersonal"></param>
        /// <returns></returns>
        public List<DeserializeCitiesData> GetPermittedCities(string userGuid, bool isPersonal)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() => { responce = DependencyService.Get<IMapBulService>().GetPermittedCities(userGuid, isPersonal); },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeCities>(responce);
                if (result.Success)
                {
                    return result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        public List<DeserializeCountriesData> GetPermittedCountry(string userGuid)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() => { responce = DependencyService.Get<IMapBulService>().GetPermittedCountry(userGuid); },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeCountries>(responce);
                if (result.Success)
                {
                    return result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        #endregion

        public List<DeserializeGetArticlesData> GetArticles(ArticleType type, bool isRefresh,
            DateTime? existingDateTime)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var cancel = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() =>
                {
                    switch (type)
                    {
                        case ArticleType.Article:
                            responce = DependencyService.Get<IMapBulService>().GetArticles(isRefresh, existingDateTime);
                            break;
                        case ArticleType.Event:
                            responce = DependencyService.Get<IMapBulService>().GetEvents(isRefresh, existingDateTime);
                            break;
                        default:
                            responce = DependencyService.Get<IMapBulService>().GetArticles(isRefresh, existingDateTime);
                            break;
                    }
                }, cancel.Token).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeGetArticles>(responce);
                if (result.Success)
                {
                    return type == ArticleType.Article ? result.Data.Where(item => item.StartDate == null).ToList() : result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        public DeserializeBase ClearSession(string sessionToken)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() => { responce = DependencyService.Get<IMapBulService>().ClearSession(sessionToken); },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeBase>(responce);
                if (result.Success)
                {
                    return result;
                }
                return null;
            }
            catch (AggregateException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void SaveFavoriteArticleAndEvent(string userGuid, int articleEventSertverId)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return;
            }
            try
            {
                var cancel = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>().SaveFavoriteArticleAndEvent(userGuid, articleEventSertverId);
                }, cancel.Token).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeBase>(responce);
                if (!result.Success)
                    OnErrorEvent(result.ErrorReason);
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
            }
        }

        public void RemoveFavoriteArticleAndEvent(string userGuid, int articleEventSertverId)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return;
            }
            try
            {
                var cancel = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>().RemoveFavoriteArticleAndEvent(userGuid, articleEventSertverId);
                }, cancel.Token).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeBase>(responce);
                if (!result.Success)
                    OnErrorEvent(result.ErrorReason);
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
            }
        }

        public IEnumerable<DeserializeGetArticlesData> GetFavoritsArticlAndEvent(string userGuid)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var cancel = new CancellationTokenSource(15000);
                var responce = "";
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>().GetFavoritsArticlAndEvent(userGuid);

                }, cancel.Token).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeGetArticles>(responce);
                if (result.Success)
                {
                    return result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        public void SaveFavoriteMarker(string userGuid, int markerSertverId)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return;
            }
            try
            {
                var cancel = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>().SaveFavoriteMarker(userGuid, markerSertverId);
                }, cancel.Token).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeBase>(responce);
                if (!result.Success)
                    OnErrorEvent(result.ErrorReason);
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
            }
        }

        public void RemoveFavoriteMarker(string userGuid, int markerSertverId)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return;
            }
            try
            {
                var cancel = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>().RemoveFavoriteMarker(userGuid, markerSertverId);
                }, cancel.Token).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeBase>(responce);
                if (!result.Success)
                    OnErrorEvent(result.ErrorReason);
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
            }
        }

        public IEnumerable<DeserializeGetMarkerDetailsData> GetFavoritsMarker(string userGuid)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var cancel = new CancellationTokenSource(15000);
                var responce = "";
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>().GetFavoritsMarker(userGuid);

                }, cancel.Token).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeGetMarkerDetails>(responce);
                if (result.Success)
                {
                    return result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        public IEnumerable<DeserializeGetArticlesData> GetRelatedEventsFromMarker(int markerServerId, bool nearest)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var cancel = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() =>
                {
                    responce = DependencyService.Get<IMapBulService>().GetRelatedEventsFromMarker(markerServerId, nearest);

                }, cancel.Token).Wait(cancel.Token);
                var result = Serializer.Deserialize<DeserializeGetArticles>(responce);
                if (result.Success)
                {
                    return result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }

        // Старая функция получения маркеров
        /*        public DeserializeUserType GetUserTypeById(int id, string guid)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(() => { responce = DependencyService.Get<IMapBulService>().GetUserTypeById(id, guid); },
                    _cancelToken.Token).Wait();
                var result = Serializer.Deserialize<DeserializeUserType>(responce);
                if (result.Success)
                {
                    return new DeserializeUserType();
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DeserializeGetMarkersData> GetMarkers(Position p1, Position p2)
        {
            if (!ApplicationSettings.IsConnectToInternet)
            {
                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.NoInternetConnectionToast);
                return null;
            }
            try
            {
                var _cancelToken = new CancellationTokenSource(Timeout);
                var responce = "";
                Task.Run(
                    () =>
                    {
                        responce = DependencyService.Get<IMapBulService>()
                            .GetMarkers(p1.Latitude, p1.Longitude, p2.Latitude, p2.Longitude);
                    }, _cancelToken.Token)
                    .Wait();
                var result = Serializer.Deserialize<DeserializeGetMarkers>(responce);
                if (result.Success)
                {
                    return result.Data;
                }
                OnErrorEvent(result.ErrorReason);
                return null;
            }
            catch (AggregateException e)
            {
                OnErrorEvent(ExceptionResources.TimeOutException);
                return null;
            }
            catch (Exception e)
            {
                OnErrorEvent(ExceptionResources.RequestException);
                return null;
            }
        }*/
    }
}
