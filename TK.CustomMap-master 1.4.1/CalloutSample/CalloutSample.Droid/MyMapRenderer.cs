using Android.Widget;
using TK.CustomMap.Droid;
using Xamarin.Forms;
using CalloutSample.Droid;
using CalloutSample;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]

namespace CalloutSample.Droid
{
    public class MyMapRenderer : TKCustomMapRenderer, Android.Gms.Maps.GoogleMap.IInfoWindowAdapter
    {
        public override void OnMapReady(Android.Gms.Maps.GoogleMap googleMap)
        {
            base.OnMapReady(googleMap);

            googleMap.SetInfoWindowAdapter(this);
        }

        Android.Views.View Android.Gms.Maps.GoogleMap.IInfoWindowAdapter.GetInfoContents(Android.Gms.Maps.Model.Marker marker)
        {
            var pin = this.GetPinByMarker(marker);
            if (pin == null) return null;

            var image = new ImageView(this.Context);
            image.SetImageResource(Resource.Drawable.icon);

            return image;
        }

        Android.Views.View Android.Gms.Maps.GoogleMap.IInfoWindowAdapter.GetInfoWindow(Android.Gms.Maps.Model.Marker marker)
        {
            return null;
        }
    }
}