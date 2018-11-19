using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Text;
using Android.Views.InputMethods;
using Android.Widget;
using Map_Bul_App.Design;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using Map_Bul_App.Droid.CustomRenderer;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]


//[assembly: ExportRenderer(typeof(CustomImageView), typeof(CustomImageViewRenderer))]
[assembly: ExportRenderer(typeof(LabelForStackLayoutButton), typeof(LabelForStackLayoutButtonRenderer))]
[assembly: ExportRenderer(typeof(MainLabel), typeof(MainLabelRenderer))]
[assembly: ExportRenderer(typeof(ActionBarEntry), typeof(ActionBarEntryRenderer))]
[assembly: ExportRenderer(typeof(ActionBarLabel), typeof(ActionBarLabelRenderer))]
[assembly: ExportRenderer(typeof(ItemWhiteLabel), typeof(ItemWhiteLabelRenderer))]
[assembly: ExportRenderer(typeof(ItemMediumLabel), typeof(ItemMediumLabelRenderer))]
[assembly: ExportRenderer(typeof(TagsLabel), typeof(ItemBoldLabelRenderer))]
[assembly: ExportRenderer(typeof(MainButton), typeof(MainButtonRenderer))]
[assembly: ExportRenderer(typeof(AuthButton), typeof(AuthButtonnRenderer))]
[assembly: ExportRenderer(typeof(AuthLabel), typeof(AuthLabelRenderer))]
[assembly: ExportRenderer(typeof(CellTitleLabel), typeof(CellTitleLabelRenderer))]
[assembly: ExportRenderer(typeof(CellDateLabel), typeof(CellDateLabelRender))]
[assembly: ExportRenderer(typeof(AuthorNameLabel), typeof(AuthorNameLabelRender))]
[assembly: ExportRenderer(typeof(EventDatePicker), typeof(EventDatePickerRender))]
[assembly: ExportRenderer(typeof(DisabledScrollView), typeof(DisabledScrollViewRenderer))]
[assembly: ExportRenderer(typeof(CustomTimePicker), typeof(CustomTimePickerRenderer))]
[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
[assembly: ExportRenderer(typeof(MainEntry), typeof(MainEntryRenderer))]
[assembly: ExportRenderer(typeof(AuthEntry), typeof(AuthEntryRenderer))]
[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRender))]


[assembly: ExportRenderer(typeof(BorderFrame), typeof(BorderedFrameRenderer))]
[assembly: ExportRenderer(typeof(InfoTitleLabel), typeof(InfoTitleLabelRenderer))]
[assembly: ExportRenderer(typeof(TypeLabel), typeof(TypeLabelRenderer))]
[assembly: ExportRenderer(typeof(SubTypeLabel), typeof(TypeLabelRenderer))]
[assembly: ExportRenderer(typeof(TimeLabel), typeof(TimeLabelRenderer))]

namespace Map_Bul_App.Droid.CustomRenderer
{

    #region [Main]

    public class CustomImageViewRenderer : ImageRenderer
    {
        private CustomImageView _portableControl;
        private byte[] _imageByteArray;
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (Control != null && e.NewElement != null && e.OldElement == null)
            {
                _portableControl = e.NewElement as CustomImageView;
                _portableControl.PropertyChanged += _portableControl_PropertyChanged;
            }
        }

        private async void _portableControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WebImagePath")
            {
                if (!string.IsNullOrEmpty(_portableControl.WebImagePath))
                {
                    _imageByteArray = GetImageByteArray(_portableControl.WebImagePath);
                    BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync();
                    Bitmap bitmapToDisplay =
                        await
                            LoadScaledDownBitmapForDisplayAsync( options, Control.Width,
                                Control.Height);
                    Control.SetImageBitmap(bitmapToDisplay);
                }
                else
                {
                    Control.SetImageBitmap(null);
                }
            }
        }

        private async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync()
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result =
                await BitmapFactory.DecodeByteArrayAsync(_imageByteArray, 0, _imageByteArray.Length, options);

            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;
            System.Diagnostics.Debug.WriteLine($"Original Size= {imageWidth}x{imageHeight}");

            return options;
        }

        public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, 250);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeByteArrayAsync(_imageByteArray,0, _imageByteArray.Length,options);
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

        public byte[] GetImageByteArray(string url)
        {
            try
            {
                byte[] _byteArray;
                using (var _wClient = new WebClient())
                {
                    _byteArray = _wClient.DownloadData(url);
                }
                return _byteArray;
            }
            catch (Exception )
            {
                //ignored
            }
            return null;
        }

    }

    public class LabelForStackLayoutButtonRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 18f;
        }
    }

    public class MainEntryRenderer : ExtendedEntryRenderer
    {

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            if (e.PropertyName == "IsFocused" && Control.IsFocused)
                Control.ShowKeyboard();
        }
    }

    public class MainLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 15f;
        }
    }

    public class MainButtonRenderer : ExtendedButtonRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 15f;
        }
    }

    public class AuthButtonnRenderer : ExtendedButtonRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 20f;
        }
    }


    #endregion

    #region [Login]


    public class AuthLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 25;
        }
    }

    public class AuthLabelRegistrnRenderer : AuthLabelRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.TextSize = 18;
        }
    }

    public class AuthEntryRenderer : MainEntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLightItalic;
            Control.TextSize = 16f;
        }
    }


    #endregion

    #region [ActionBar]

    public class ActionBarEntryRenderer : MainEntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLightItalic;
            Control.TextSize = 15f;
        }
    }

    public class ActionBarLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 19f;
        }
    }

    #endregion

    #region [Item]

    public class ItemWhiteLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoMedium;
            Control.TextSize = 18f;
        }
    }

    public class ItemMediumLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoMedium;
            Control.TextSize = 15f;
        }
    }



    #endregion

    #region [Articles]

    public class CellTitleLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoBold;
            Control.TextSize = 16f;
        }
    }

    public class CellDateLabelRender : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 16f;
        }
    }

    public class AuthorNameLabelRender : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLightItalic;
            Control.TextSize = 16f;
        }
    }

    public class EventDatePickerRender : DatePickerRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLightItalic;
            Control.TextSize = 16f;
        }
    }

    #endregion

    #region Favorites

    public class DisabledScrollViewRenderer : ExtendedScrollViewRenderer
    {

    }


    #endregion

    #region [Add Pin]

    public class CustomTimePickerRenderer : TimePickerRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 15f;
        }
    }

    public class CustomEditorRender : ExtendedEditorRenderer
    {

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Control.Typeface = Font.RobotoLightItalic;
            Control.TextSize = 16f;
            if (e.PropertyName == "IsFocused" && Control.IsFocused)
                Control.ShowKeyboard();
        }
    }

    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLightItalic;
            Control.TextSize = 15f;
        }
    }

    #endregion

    #region [PinPreview]

    public class ItemBoldLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoBold;
            Control.TextSize = 12f;
        }
    }

    public class InfoTitleLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 20;
        }
    }

    public class TypeLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
        }
    }

    public class TimeLabelRenderer : ExtendedLabelRender
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoBold;
        }
    }


    public class BorderedFrameRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e?.NewElement != null)
            {
                e.NewElement.HasShadow = false;
            }
        }
    }
    #endregion

    public static class Keyboard
    {
        public static void ShowKeyboard(this Android.Views.View control)
        {
            var a = control.RequestFocus();
            var inputMethodManager =
                control.Context.GetSystemService(Android.Content.Context.InputMethodService) as InputMethodManager;
            inputMethodManager?.ShowSoftInput(control, ShowFlags.Forced);
            inputMethodManager?.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }
    }

    public class HtmlLabelRenderer : LabelRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Typeface = Font.RobotoLight;
            Control.TextSize = 15f;

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                Control?.SetText(Html.FromHtml(Element.Text), TextView.BufferType.Spannable);
            }
            
        }
    }

}

