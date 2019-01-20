using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Map_Bul_App.Annotations;
using Map_Bul_App.Models.Tables;
using Map_Bul_App.Settings;
using Xamarin.Forms;

namespace Map_Bul_App.Models
{
    public class ArticleEventItem : ArticleEvent, INotifyPropertyChanged
    {
        public event EventHandler DeleteItem;
        public IEnumerable<string> Tags { get; set; }
        public AuthorName AuthorName { get; set; }
        public string FormatedText => "    " + Text;
        public ImageSource TitlePhotoSource => ImageSource.FromUri(new Uri(TitlePhoto));

        public ImageSource PhotoSource => !string.IsNullOrEmpty(Photo) ? ImageSource.FromUri(new Uri(Photo)) : null;

        public new string TagsString => Tags.ToTagsString();

        public string StartTimeString
            => StartTime.HasValue ? new DateTime(StartTime.Value.Ticks).ToString("HH:mm") : " ";
        public bool IsShowed => ApplicationSettings.DataBase.ShowedArticleEvents.Any(i => i.ServerId == ServerId);
        public string EventDateString 
        {
            get
            {
                if (StartDate == null) return string.Empty;
                var result = StartDate.Value.ToString("dd.MM.yy");
                if (StopDate != null)
                    result += " - " + StopDate.Value.ToString("dd.MM.yy");
                return result;
            }
        }
        public string PublishDateString => PublishedDate.ToString("dd.MM.yy");
        public ICommand DeleteCommand=> new Command (OnDeleteItem);
        public ArticleEventItem(ArticleEvent item)
        {
            ServerId = item.ServerId;
            Id = item.Id;
            Title = item.Title;
            TitlePhoto = item.TitlePhoto;
            Photo = item.Photo;
            Description = item.Description;
            Text = item.Text;
            AuthorId = item.AuthorId;
            EditorId = item.EditorId;
            AddedDate = item.AddedDate;
            PublishedDate = item.PublishedDate;
            MarkerId = item.MarkerId;
            StartDate = item.StartDate;
            StopDate = item.StopDate;
            StartTime = item.StartTime;
            Adress = item.Adress;
            StatusId = item.StatusId;
            BaseCategoryId = item.BaseCategoryId;
            IsFavorite = item.IsFavorite;
            AuthorName = new AuthorName
            {
                FirstName = item.FirstName,
                MiddleName = item.MiddleName,
                LastName = item.LastName
            };
            Tags= item.TagsString.Split(',').Where(s=>s!="");
            SourceUrl = item.SourceUrl;
            SourcePhoto = item.SourcePhoto;

            AddressName = item.AddressName;
        }

        public ArticleEventItem(DeserializeGetArticlesData item)
        {
            ServerId = item.Id;
            AddedDate = item.AddedDate;
            PublishedDate = item.PublishedDate ?? DateTime.MinValue;
            StartDate = item.StartDate;
            StopDate = item.StopDate;
            StartTime = item.StartTime;
            Photo = item.Photo;
            Title = item.Title;
            TitlePhoto = item.TitlePhoto;
            AuthorName = item.AuthorName;
            Text = item.Text;
            Adress = item.MarkerAddress;
            Tags = item.Subcategories;
            SourceUrl = item.SourceUrl;
            SourcePhoto = item.SourcePhoto;
            IsVisible = true;
            MarkerId = item.MarkerId;
            AddressName = item.AddressName;
        }

        public virtual void OnDeleteItem()
        {
            DeleteItem?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AuthorName
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + MiddleName + " " + LastName;
    }
}
