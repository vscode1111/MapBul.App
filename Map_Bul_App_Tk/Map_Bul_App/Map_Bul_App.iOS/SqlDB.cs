using System.IO;
using Map_Bul_App.iOS;
using Map_Bul_App.Settings;
using SQLite.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqLiteiOS))]
namespace Map_Bul_App.iOS
{
    // ReSharper disable once InconsistentNaming
    public class SqLiteiOS : ISqLite
    {
        public SQLiteConnection GetConnection(string sqliteFilename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            // создаем подключение
            var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var conn = new SQLiteConnection(plat, path);
            return conn;
        }
    }
}
