using System.IO;
using Map_Bul_App.Droid;
using Map_Bul_App.Settings;
using SQLite.Net;
using Xamarin.Forms;
[assembly: Dependency(typeof(SqLiteAndroid))]
namespace Map_Bul_App.Droid
{
    public class SqLiteAndroid : ISqLite
    {
        public SQLiteConnection GetConnection(string sqliteFilename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            // создаем подключение
            var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var conn = new SQLiteConnection(plat, path);

            return conn;
        }
    }
}