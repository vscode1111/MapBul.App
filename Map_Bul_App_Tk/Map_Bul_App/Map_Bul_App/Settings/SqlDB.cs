using System.Collections.Generic;
using System.Linq;
using Map_Bul_App.Models;
using Map_Bul_App.Models.Tables;
using SQLite.Net;
using Xamarin.Forms;

namespace Map_Bul_App.Settings
{
    public interface ISqLite
    {
        SQLiteConnection GetConnection(string filename);
    }

    public class MapBulDataBaseRepository
    {
        private readonly SQLiteConnection _db;

        public MapBulDataBaseRepository(string filename)
        {
            _db = DependencyService.Get<ISqLite>().GetConnection(filename);
            _db.CreateTable<User>();
            _db.CreateTable<Place>();
            _db.CreateTable<FilterSettings>();
            _db.CreateTable<ArticleEvent>();
            _db.CreateTable<Session>();
            _db.CreateTable<ShowedArticleEvent>();
        }

        public IEnumerable<User> Users => (from t in _db.Table<User>() select t).ToList();
        public IEnumerable<Place> Places => (from t in _db.Table<Place>() select t).ToList();
        public IEnumerable<Session> Sessions => (from t in _db.Table<Session>() select t).ToList();
        public IEnumerable<ArticleEvent> ArticlesEvents => (from t in _db.Table<ArticleEvent>() select t).ToList();
        public IEnumerable<FilterSettings> FilterSettingses => (from t in _db.Table<FilterSettings>() select t).ToList();

        public IEnumerable<ShowedArticleEvent> ShowedArticleEvents
            => (from t in _db.Table<ShowedArticleEvent>() select t).ToList();

        /// <summary>
        /// Занесение id просмотренных событий и статей в базу
        /// </summary>
        /// <param name="serverIdArticleEvent">Серверный id для статьи или события</param>
        /// <param name="type">Тип - событие(0) или статья(1)</param>
        public void InsertArticleEventToShowedArticleEvents(int serverIdArticleEvent, int type)
        {
            if (ShowedArticleEvents.All(u => u.ServerId != serverIdArticleEvent))
            {
                var tempId = _db.Insert(new ShowedArticleEvent {ServerId = serverIdArticleEvent, Type = type});
            }
        }

        public int SaveUser(User item)
        {
            if (_db.Table<User>().Any(u => u.Guid == item.Guid))
            {
                var user = _db.Table<User>().FirstOrDefault(u => u.Guid == item.Guid);
                item.Id = user.Id;
            }
            if (item.Id == 0) return _db.Insert(item);
           _db.Update(item);
            return item.Id;
        }

        public int SavePlace(Place item)
        {
            item.OwnerServerId = ApplicationSettings.CurrentUser.Guid;
            if (
                Places.Any(
                    i => i.OwnerServerId == ApplicationSettings.CurrentUser.Guid && i.ServerId == item.ServerId))
            {
                _db.Update(item);
                return item.Id;
            }
            else
            {
                var a = _db.Insert(item);
                return a;
            }

            //item.OwnerServerId = ApplicationSettings.CurrentUser.Guid;
            //if (item.Id == 0) return _db.Insert(item);
            //_db.Update(item);
            //return item.Id;
        }

        public int DeletePlace(int id, IdType type = IdType.LocalId)
        {
            switch (type)
            {
                case IdType.LocalId:
                    return _db.Delete<Place>(id);
                case IdType.ServerId:
                {
                    var pin = _db.Table<Place>().FirstOrDefault(item => item.ServerId == id && item.OwnerServerId==ApplicationSettings.CurrentUser.Guid);
                    if (pin != null)
                    {
                        return _db.Delete<Place>(pin.Id);
                    }
                    return 0;
                }
            }
            return 0;
        }

        public Session GetCurrentSession => _db.Table<Session>().LastOrDefault();

        public int SetSession(string token)
        {
            var session = new Session
            {
                Token = token
            };
            return _db.Insert(session);
        }

        public int DeleteSession(Session item)
        {
            return _db.Delete(item);
        }




        public int SaveFilter(FilterSettings item)
        {
            ClearFilterSettings();
            _db.Insert(item);
            return item.Id;
        }

        public int ClearFilterSettings()
        {
           return _db.DeleteAll<FilterSettings>();
        }


        public int SaveArticleEvent(ArticleEventItem item)
        {
            var dbItem = new ArticleEvent(item) { OwnerServerId = ApplicationSettings.CurrentUser.Guid };
            if (
                ArticlesEvents.Any(
                    i => i.OwnerServerId == ApplicationSettings.CurrentUser.Guid && i.ServerId == item.ServerId))
            {
                _db.Update(dbItem);
                return item.Id;
            }
            else
            {
                var a = _db.Insert(dbItem);
                return a;
            }
            //if (dbItem.Id == 0)
            //{
            //    var a = _db.Insert(dbItem);
            //    return a;
            //}

            //_db.Update(dbItem);
            //return item.Id;
        }

        public int DeleteArticleEvent(int id, IdType type = IdType.LocalId)
        {
            switch (type)
            {
                case IdType.LocalId:
                    return _db.Delete<ArticleEvent>(id);
                case IdType.ServerId:
                {
                    var item = ArticlesEvents.FirstOrDefault(a => a.ServerId == id);
                    return item != null ? _db.Delete<ArticleEvent>(item.Id) : 0;
                }


                default:
                    return 0;
            }
        }
    }
}
