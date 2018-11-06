using System;
using System.IO;
using Map_Bul_App.iOS;
using Map_Bul_App.Settings;
using Xamarin.Forms;
[assembly: Dependency(typeof(FileSystemWork))]
namespace Map_Bul_App.iOS
{

   public class FileSystemWork: IFileSystemWork
    {
       public byte[] GetFile(string path)
        {
           try
           {
               var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
               var filePath = Path.Combine(documentsPath, path);
               //return System.IO.File.ReadAllText(filePath);
               var file = System.IO.File.ReadAllBytes(filePath);
               return file;
           }
           catch (Exception e)
           {
               return null;
           }
        }
    }
}