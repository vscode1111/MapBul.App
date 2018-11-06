using System;
using System.Collections.Generic;
using System.Linq;
using TK.CustomMap;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.Settings
{
    public static class Statics
    {
        #region [Map]

        public static bool Contains(this MapSpan firstRegion, MapSpan secondRegion)
        {
            if (firstRegion == null || secondRegion == null) return false;
            var firstLeftTop = new Position(firstRegion.Center.Latitude + firstRegion.LatitudeDegrees/2,
                firstRegion.Center.Longitude - firstRegion.LongitudeDegrees/2);
            var firstReghtBottom = new Position(firstRegion.Center.Latitude - firstRegion.LatitudeDegrees/2,
                firstRegion.Center.Longitude + firstRegion.LongitudeDegrees/2);

            var secondLeftTop = new Position(secondRegion.Center.Latitude + secondRegion.LatitudeDegrees/2,
                secondRegion.Center.Longitude - secondRegion.LongitudeDegrees/2);
            var secondReghtBottom = new Position(secondRegion.Center.Latitude - secondRegion.LatitudeDegrees/2,
                secondRegion.Center.Longitude + secondRegion.LongitudeDegrees/2);

            var leftTop = (firstLeftTop.Latitude > secondReghtBottom.Latitude) &&
                          firstLeftTop.Longitude < secondLeftTop.Longitude;
            var rightBottom = (firstReghtBottom.Latitude < secondReghtBottom.Latitude) &&
                              (firstReghtBottom.Longitude > secondReghtBottom.Longitude);
            return leftTop && rightBottom;
        }

        public static bool Contains(this MapSpan firstRegion, Position position)
        {
            if (firstRegion == null || position == null) return false;
            var leftTop = new Position(firstRegion.Center.Latitude + firstRegion.LatitudeDegrees/2,
                firstRegion.Center.Longitude - firstRegion.LongitudeDegrees);
            var rightBottom = new Position(firstRegion.Center.Latitude - firstRegion.LatitudeDegrees,
                firstRegion.Center.Longitude + firstRegion.LongitudeDegrees);
            var result = position.Latitude <= leftTop.Latitude && position.Latitude >= rightBottom.Latitude &&
                         position.Longitude >= leftTop.Longitude && position.Longitude <= rightBottom.Longitude;
            return result;
        }

        //public static Position LeftTop(this MapSpan region)
        //{
        //    return new Position(region.Center.Latitude + region.LatitudeDegrees,
        //        region.Center.Longitude - region.LongitudeDegrees);
        //}

        //public static Position RightBottom(this MapSpan region)
        //{
        //    return new Position(region.Center.Latitude - region.LatitudeDegrees,
        //        region.Center.Longitude + region.LongitudeDegrees);
        //}

        //public static MapSpan MapSpanByPositions(Position leftTop, Position rightBot)
        //{
        //    var mapCenter = new Position((leftTop.Latitude + rightBot.Latitude)/2,
        //        (leftTop.Longitude + rightBot.Longitude)/2);
        //    var lat = Math.Abs(Math.Abs(leftTop.Latitude) -
        //                       Math.Abs(rightBot.Latitude));
        //    var lng = Math.Abs(Math.Abs(leftTop.Longitude) -
        //                       Math.Abs(rightBot.Longitude));
        //    return new MapSpan(mapCenter, lat, lng);
        //}

        #endregion

        //public static List<int> ToList(this string integers)
        //{
        //    var fixedString = integers.Replace(" ", "");
        //    var listStrings = fixedString.Split(',');
        //    var result =
        //        listStrings.Where(item => !string.IsNullOrEmpty(item)).Select(item => Convert.ToInt32(item)).ToList();
        //    return result;
        //}

        public static string ToTagsString(this IEnumerable<string> self, bool enableHashTag = true)
        {
            var enumerable = self as string[] ?? self.ToArray();
            if (self == null || !enumerable.Any()) return string.Empty;

            var result = enableHashTag
                ? enumerable.Aggregate("", (current, item) => current + ("#" + item + " "))
                : enumerable.Aggregate("", (current, item) => current + (item + ", ")).TrimEnd(' ').Trim(',');
            return result.Replace("# ", "#");
        }


        public static int ToRussian(this DayOfWeek item)
        {
            switch (item)
            {
                case DayOfWeek.Monday:
                    return 1;

                case DayOfWeek.Tuesday:
                    return 2;

                case DayOfWeek.Wednesday:
                    return 3;

                case DayOfWeek.Thursday:
                    return 4;

                case DayOfWeek.Friday:
                    return 5;

                case DayOfWeek.Saturday:
                    return 6;

                case DayOfWeek.Sunday:
                    return 7;
            }
            return 0;
        }

        public static DayOfWeek ToDayOfWeek(this int item)
        {
            switch (item)
            {
                case (1):
                    return DayOfWeek.Monday;

                case (2):
                    return DayOfWeek.Tuesday;

                case (3):
                    return DayOfWeek.Wednesday;

                case (4):
                    return DayOfWeek.Thursday;

                case (5):
                    return DayOfWeek.Friday;

                case (6):
                    return DayOfWeek.Saturday;

                case (7):
                    return DayOfWeek.Sunday;
            }
            return default(DayOfWeek);
        }



       public class ComparePins : IEqualityComparer<TKCustomMapPin>
        {
            public bool Equals(TKCustomMapPin x, TKCustomMapPin y)
            {
               return x.Id == y.Id && x.IsVisible== y.IsVisible;
            }
            public int GetHashCode(TKCustomMapPin codeh)
            {
                return (codeh.Id+codeh.IsVisible.ToString()).GetHashCode();
            }
        }
    }
}
