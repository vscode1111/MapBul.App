using System;
using Map_Bul_App.Settings;
using SQLite.Net.Attributes;

namespace Map_Bul_App.Models.Tables
{
    [Table("ArticleEvent")]
    public class ArticleEvent
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public int ServerId { get; set; }
        public string Title { get; set; }
        public string TitlePhoto { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string SourceUrl { get; set; }
        public string SourcePhoto { get; set; }
        public int AuthorId { get; set; }
        public int EditorId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public int? MarkerId { get; set; }


        public DateTime? StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public string Adress { get; set; }
        public string AddressName { get; set; }
        public int StatusId { get; set; }
        public int BaseCategoryId { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsVisible { get; set; } = false;
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string TagsString { get; set; }
        public ArticleType Type => StartDate == null ? ArticleType.Article : ArticleType.Event;
        public string OwnerServerId { get; set; }

        public ArticleEvent()
        {
        }

        public ArticleEvent(ArticleEventItem item)
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
            FirstName = item.AuthorName.FirstName;
            MiddleName = item.AuthorName.MiddleName;
            LastName = item.AuthorName.LastName;
            TagsString = item.Tags.ToTagsString(false);
            SourcePhoto = item.SourcePhoto;
            SourceUrl = item.SourceUrl;

            AddressName = item.AddressName;
        }
    }
}
