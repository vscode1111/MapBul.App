using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Toasts;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Media;
using XLabs.Serialization;

namespace Map_Bul_App.Droid
{
    [Activity(Label = "Map_Bul_App", Icon = "@drawable/icon", MainLauncher = false, Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Forms.SetTitleBarVisibility(AndroidTitleBarVisibility.Never);//убираем ActionBar

            /*
                        var resolverContainer = new XLabs.Ioc.SimpleContainer();

                        resolverContainer.Register(t => AndroidDevice.CurrentDevice)
                            .Register(t => t.Resolve<IDevice>())
                            .Register<XLabs.Ioc.IDependencyContainer>(t => resolverContainer)
                            .Register<IMediaPicker, MediaPicker>()
                            .Register<IJsonSerializer, IJsonSerializer>();
            */
            var container = new SimpleContainer();
            container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
            container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            container.Register<INetwork>(t => t.Resolve<IDevice>().Network);
            Resolver.ResetResolver(container.GetResolver());
           // Resolver.SetResolver();
            DependencyService.Register<ToastNotificatorImplementation>();
            ToastNotificatorImplementation.Init(this);
            Xamarin.FormsMaps.Init(this, bundle);
            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;
           // App.ScreenWidth = (width - 0.5f) / density;
          //  App.ScreenHeight = (height - 0.5f) / density;
            LoadApplication(new App());
        }
    }
}

