using Map_Bul_App.Views.GeneralViews;
using Xamarin.Forms;

namespace Map_Bul_App.Settings
{
    public class MyMasterDetailPage : MasterDetailPage
    {
        public MyMasterDetailPage()
        {
            MasterBehavior = MasterBehavior.Popover;
            Master = new Menu();
            Detail = new NavPage();
            ApplicationSettings.CurrentUser.PropertyChanged += CurrentUser_PropertyChanged;
        }

         void CurrentUser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLogined")
            {
                Master= new Menu();
            }
        }
    }
}
