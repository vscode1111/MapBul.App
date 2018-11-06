using System;
using SQLite.Net.Attributes;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.Models.Tables
{
    [Table("FilterSettings")]
    public class FilterSettings
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public double CenterLat { get; set; }
        public double CenterLng { get; set; }
        public double MRadius { get; set; }

        public bool MyMarkerSelected { get; set; } = true;
        public bool WifiSelected { get; set; }
        public bool NowOpenSelected { get; set; }
        public string Categories { get; set; }
        public string SubCategories { get; set; }
        public DateTime DateStamp { get; set; }
        public Position Center => new Position(CenterLat, CenterLng);
        public MapSpan MapRegion => MapSpan.FromCenterAndRadius(Center, new Distance(MRadius * 1000));
    }
}
