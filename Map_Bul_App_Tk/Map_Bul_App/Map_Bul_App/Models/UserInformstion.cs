using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Map_Bul_App.Models.Tables;
using Map_Bul_App.Settings;
using Place = Map_Bul_App.Models.Tables.Place;

namespace Map_Bul_App.Models
{
    public sealed class UserInformation : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _userType;
        private string _guid;
        private bool _isLogined;
        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null || value == _name) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string UserType
        {
            get { return _userType; }
            set
            {
                if (value == _userType) return;
                _userType = value;
                OnPropertyChanged();
            }
        }
        public string Guid
        {
            get { return _guid; }
            set
            {
                if (value == null || value == _guid) return;
                _guid = value;
                OnPropertyChanged(nameof(Guid));
            }
        }
        public bool IsLogined
        {
            get { return _isLogined; }
            set
            {
                if (value == _isLogined) return;
                _isLogined = value;
                OnPropertyChanged(nameof(IsLogined));
            }
        }
        public void ResetUser()
        {
            Id = default(int);
            Name = Guid=UserType=default(string);
            IsLogined = false;
        }

        public void SetUser(User user)
        {
            if (user == null) return;
            _id = user.Id;
            _guid = user.Guid;
            _name = user.Name;
            _userType = user.Type;
            if(user.Type!=UserTypesMobile.Guest)
            _isLogined = true;
        }
        public UserInformation()
        {
            _isLogined = false;
        }
        
/*
        public UserInformation(User user)
        {
            _id = user.Id;
            _guid = user.Guid;
            _name = user.Name;
            _userType = user.Type;
            _isLogined = true;
        }
*/
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<ArticleEvent> FavoritesArticleEvents
            => ApplicationSettings.DataBase.ArticlesEvents.Where(item => item.OwnerServerId == Guid);
        public IEnumerable<Place> FavoritesPlaces
            => ApplicationSettings.DataBase.Places.Where(item => item.OwnerServerId == Guid);


        private void OnPropertyChanged([CallerMemberName]string propertyName=null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

    }
}
