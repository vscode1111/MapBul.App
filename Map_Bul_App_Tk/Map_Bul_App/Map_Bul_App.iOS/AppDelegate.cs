using CoreLocation;
using Foundation;
using Plugin.Toasts;
using TK.CustomMap.iOSUnified;
using UIKit;
using Xamarin.Forms;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using XLabs.Serialization;

namespace Map_Bul_App.iOS
{

    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var resolverContainer = new XLabs.Ioc.SimpleContainer();
            resolverContainer.Register(t => AppleDevice.CurrentDevice)
                .Register(t => t.Resolve<IDevice>().Display)
                .Register<IMediaPicker, MediaPicker>()
                .Register<XLabs.Ioc.IDependencyContainer>(t => resolverContainer)
                .Register<IJsonSerializer, IJsonSerializer>();
                
            XLabs.Ioc.Resolver.SetResolver(resolverContainer.GetResolver());

            Forms.Init();
            Xamarin.FormsMaps.Init();//карты
            var a = CLAuthorizationStatus.Authorized;           DependencyService.Register<ToastNotificatorImplementation>();
           ToastNotificatorImplementation.Init();
            
            var dummy = new TKCustomMapRenderer();
            var dummy2 = new NativePlacesApi();

            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}
