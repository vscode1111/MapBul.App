using System.ComponentModel;
using Map_Bul_App.Design;
using Map_Bul_App.iOS.Renderer;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(NewPinImage), typeof(NewPinImageRenderer))]
namespace Map_Bul_App.iOS.Renderer
{
    public class NewPinImageRenderer : ImageRenderer
    {
        private NewPinImage _portableControl;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (Control != null && e.NewElement != null && e.OldElement == null)
            {
                _portableControl = e.NewElement as NewPinImage;
                _portableControl.PropertyChanged += _portableControl_PropertyChanged;

                AddGestureRecognizer(new UILongPressGestureRecognizer((longPress) =>
                {
                    if (longPress.State == UIGestureRecognizerState.Began)
                    {
                        _portableControl.OnLongClick();
                    }
                }));
                AddGestureRecognizer(new UITapGestureRecognizer((press) =>
                {
                    _portableControl.OnClick();
                }));
            }
        }

        private void _portableControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ImagePath")
            {
                if (!string.IsNullOrEmpty(_portableControl.ImagePath))
                {
                    Control.Image=new UIImage(_portableControl.ImagePath);
                }
                else
                {
                    Control.Image = null;
                }
            }
        }
    }
}
