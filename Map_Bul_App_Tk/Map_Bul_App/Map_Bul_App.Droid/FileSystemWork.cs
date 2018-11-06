using System;
using Map_Bul_App.Droid;
using Map_Bul_App.Settings;
using Xamarin.Forms;
[assembly: Dependency(typeof(FileSystemWork))]
namespace Map_Bul_App.Droid
{

   public class FileSystemWork: IFileSystemWork
    {
       public byte[] GetFile(string path)
       {
           try
           {
                var file = System.IO.File.ReadAllBytes(path);
                return file;
            }
           catch (Exception)
           {
               return null;
           }
           
       }
    }
}