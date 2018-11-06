using SQLite.Net.Attributes;

namespace Map_Bul_App.Models.Tables
{
    [Table("ShowedArticleEvent")]
    public class ShowedArticleEvent
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public int ServerId { get; set; }
        /// <summary>
        /// Указывает тип записи - событие(0) или статья(1)
        /// </summary>
        public int Type { get; set; }
    }
}
