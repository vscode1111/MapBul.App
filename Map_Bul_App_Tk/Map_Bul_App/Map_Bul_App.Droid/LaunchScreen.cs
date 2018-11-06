using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Map_Bul_App.Droid
{
    [Activity(Label = "X-island", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash",
       ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LaunchScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}