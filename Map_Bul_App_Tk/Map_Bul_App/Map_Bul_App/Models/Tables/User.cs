using System;
using SQLite.Net.Attributes;

namespace Map_Bul_App.Models.Tables
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public int ServerId { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public DateTime LastLogin { get; set; }

    }
}
