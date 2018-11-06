using SQLite.Net.Attributes;

namespace Map_Bul_App.Models.Tables
{
    [Table("Session")]
    public class Session
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Token { get; set; }
    }
}
