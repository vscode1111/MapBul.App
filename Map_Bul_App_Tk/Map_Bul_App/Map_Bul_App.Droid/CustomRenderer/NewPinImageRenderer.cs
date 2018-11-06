using System.ComponentModel;
using System.Threading.Tasks;
using Android.Graphics;
using Map_Bul_App.Design;
using Map_Bul_App.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(NewPinImage), typeof(NewPinImageRenderer))]
namespace Map_Bul_App.Droid.CustomRenderer
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
                Control.Click += Control_Click;
                Control.LongClick += Control_LongClick;
            }
        }

        private async void _portableControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ImagePath")
            {
                if (!string.IsNullOrEmpty(_portableControl.ImagePath))
                {
                    BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync(_portableControl.ImagePath);
                    Bitmap bitmapToDisplay =
                        await
                            LoadScaledDownBitmapForDisplayAsync(_portableControl.ImagePath, options, Control.Width,
                                Control.Height);
                    Control.SetImageBitmap(bitmapToDisplay);
                }
                else
                {
                    Control.SetImageBitmap(null);
                }
            }
        }
        
        private void Control_Click(object sender, System.EventArgs e)
        {
            _portableControl?.OnClick();
        }

        private void Control_LongClick(object sender, View.LongClickEventArgs e)
        {
            _portableControl?.OnLongClick();
        }

        private async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(string path)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result = await BitmapFactory.DecodeFileAsync(path,options);

            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;
            System.Diagnostics.Debug.WriteLine($"Original Size= {imageWidth}x{imageHeight}");

            return options;
        }

        public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(string path, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeFileAsync(path, options);
        }

        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }
    }
}