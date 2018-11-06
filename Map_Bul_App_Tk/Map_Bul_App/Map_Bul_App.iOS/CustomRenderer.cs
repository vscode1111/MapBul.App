using System;
using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Map_Bul_App.Design;
using Map_Bul_App.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(LabelForStackLayoutButton), typeof(LabelForStackLayoutButtonRenderer))]
[assembly: ExportRenderer(typeof (MainLabel), typeof (MainLabelRenderer))]
[assembly: ExportRenderer(typeof(MainEntry), typeof(UnderlinedEntryRenderer))]
[assembly: ExportRenderer(typeof(RegistrationEntry), typeof(UnderlinedEntryRenderer))]
[assembly: ExportRenderer(typeof (ActionBarEntry), typeof (UnderlinedEntryRenderer))]
[assembly: ExportRenderer(typeof (ActionBarLabel), typeof (ActionBarLabelRenderer))]
[assembly: ExportRenderer(typeof (ItemWhiteLabel), typeof (ItemWhiteLabelRenderer))]
[assembly: ExportRenderer(typeof (ItemMediumLabel), typeof (ItemMediumLabelRenderer))]
[assembly: ExportRenderer(typeof (TagsLabel), typeof (ItemBoldLabelRenderer))]
[assembly: ExportRenderer(typeof(CellTitleLabel), typeof(CellTitleLabelRenderer))]
[assembly: ExportRenderer(typeof(CellDateLabel), typeof(CellDateLabelRender))]
[assembly: ExportRenderer(typeof(AuthorNameLabel), typeof(AuthorNameLabelRender))]
[assembly: ExportRenderer(typeof(EventDateLabel), typeof(EventDateLabelRender))]
[assembly: ExportRenderer(typeof(EventDateLabelText), typeof(EventDateLabelTextRender))]
[assembly: ExportRenderer(typeof(DisabledScrollView), typeof(BannerScrollViewRenderer))]
[assembly: ExportRenderer(typeof(CustomTimePicker), typeof(UnderlinedTimePickerRender))]
[assembly: ExportRenderer(typeof(EventDatePicker), typeof(EventDatePickerRender))]
[assembly: ExportRenderer(typeof(RegistrationBirthDatePicker), typeof(EventDatePickerRender))]
[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRender))]
[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRender))]
[assembly: ExportRenderer(typeof(EventSmallDatePicker), typeof(EventSmallDatePickerRender))]

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]



/*[assembly: ExportRenderer(typeof(BorderFrame), typeof(BorderFrameRenderer))]*/
[assembly: ExportRenderer(typeof(InfoTitleLabel), typeof(InfoTitleLabelRenderer))]
[assembly: ExportRenderer(typeof(TypeLabel), typeof(TypeLabelRenderer))]
[assembly: ExportRenderer(typeof(SubTypeLabel), typeof(TypeLabelRenderer))]
[assembly: ExportRenderer(typeof(TimeLabel), typeof(TimeLabelRenderer))]




namespace Map_Bul_App.iOS
{

    class HtmlLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }
            if (Control == null) return;
            Control.Font = Font.RobotoLight(15f);

            var attr = new NSAttributedStringDocumentAttributes();
            var nsError = new NSError();
            attr.DocumentType = NSDocumentType.HTML;

            var text = e.NewElement.Text;

            //I wrap the text here with the default font and size
            text = "<style>body{font-family: '" + this.Control.Font.Name + "'; font-size:" + this.Control.Font.PointSize + "px;}</style>" + text;

            var myHtmlData = NSData.FromString(text, NSStringEncoding.Unicode);
            Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Control == null)
            {
                return;
            }
            Control.Font = Font.RobotoLight(15f);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                var attr = new NSAttributedStringDocumentAttributes();
                var nsError = new NSError();
                attr.DocumentType = NSDocumentType.HTML;

                var myHtmlData = NSData.FromString(this.Control.Text, NSStringEncoding.Unicode);
                this.Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
            }
        }
    }

    public class LabelForStackLayoutButtonRenderer : ExtendedLabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.Font = Font.RobotoLight(18f);
        }
    }

    #region [Main]

    public class MainLabelRenderer : ExtendedLabelRenderer
    {
            protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
            {
                base.OnElementChanged(e);
                if (Control == null) return;
                Control.Font = Font.RobotoLight(15f);
            }
        }

        #endregion

        #region [ActionBar]

        public class UnderlinedEntryRenderer : ExtendedEntryRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
            {
                base.OnElementChanged(e);
                if (Control == null) return;
                Control.Font = Font.RobotoLightItalic(16f);
                Control.DrawBorder();
                Control.BorderStyle = UITextBorderStyle.None;
        }
        private bool _focus;
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            if (e.PropertyName.Equals("IsFocused"))
            {
                _focus = !_focus;
                Control.DrawBorder(_focus);
            }
            if (e.PropertyName.Equals("Width") || e.PropertyName.Equals("Height"))
            {
                var width = ((View) sender).Width;
                var height = ((View) sender).Height;
                if (height > 1)
                    Control.DrawBorder(_focus, new nfloat(width), new nfloat(height));
            }
        }
    }

        public class ActionBarLabelRenderer : ExtendedLabelRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
            {
                base.OnElementChanged(e);
                if (Control == null) return;
                Control.Font = Font.RobotoLight(19f);
            }

            protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoLight(20f);

        }
        }

        #endregion

        #region [Item]

        public class ItemWhiteLabelRenderer : ExtendedLabelRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
            {
                base.OnElementChanged(e);
                if (Control == null) return;
                Control.Font = Font.RobotoMedium(18f);
            }
        }

        public class ItemMediumLabelRenderer : ExtendedLabelRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
            {
                base.OnElementChanged(e);
                if (Control == null) return;
                Control.Font = Font.RobotoMedium(15f);
            }
        }


        public class ItemBoldLabelRenderer : ExtendedLabelRenderer
        {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoBold(12f);
        }


    }

    #endregion
    #region [Auth]



    #endregion

    #region [Articles]
    public class CellDateLabelRender: ExtendedLabelRenderer
    {
/*        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.Font = Font.RobotoLight(14f);
        }*/


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoLight(14f);
        }
    }



    public class CellTitleLabelRenderer: ExtendedLabelRenderer
    {
/*        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.Font = Font.RobotoBold(14f);
        }*/


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoBold(14f);
        }
    }

    public class AuthorNameLabelRender : ExtendedLabelRenderer
    {
/*        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.Font = Font.RobotoLightItalic(16f);
        }*/


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoLightItalic(16f);
        }
    }
    #endregion


    #region [Events]
    public class EventDateLabelRender : ExtendedLabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.Font = Font.RobotoLight(13f);
        }
    }


    public class EventDateLabelTextRender : ExtendedLabelRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoMedium(13f);
        }
    }


    public class EventDatePickerRender : DatePickerRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            if (e.PropertyName.Equals("IsFocused"))
            {
                _focus = !_focus;
                Control.DrawPickerBorder(_focus);
            }
            if (e.PropertyName.Equals("Width") || e.PropertyName.Equals("Height"))
            {
                var width = ((DatePicker)sender).Width;
                var height = ((DatePicker)sender).Height;
                Control.DrawPickerBorder(_focus, new nfloat(width), new nfloat(height));
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.BorderStyle = UITextBorderStyle.None;
            Control.Font = Font.RobotoLight(16f);
            Control.DrawPickerBorder();
        }

        private bool _focus;
    }
    public class EventSmallDatePickerRender : EventDatePickerRender
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.BorderStyle = UITextBorderStyle.None;
            Control.Font = Font.RobotoLightItalic(14f);
            Control.DrawPickerBorder();
        }

    }
    public class CustomPickerRender:PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.BorderStyle = UITextBorderStyle.None;
            Control.Font = Font.RobotoLightItalic(16f);
            Control.DrawPickerBorder();
        }
        private bool _focus;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            if (e.PropertyName.Equals("IsFocused"))
            {
                _focus = !_focus;
                Control.DrawPickerBorder(_focus);
            }
            if (e.PropertyName.Equals("Width") || e.PropertyName.Equals("Height"))
            {
                var width = ((Picker)sender).Width;
                var height = ((Picker)sender).Height;
                Control.DrawPickerBorder(_focus, new nfloat(width), new nfloat(height));
            }
        }
    }
    #endregion
    






    #region Favorites
    public class BannerScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            ScrollEnabled = false;
        }
    }
    #endregion
    #region [Add Pin]


    public class UnderlinedTimePickerRender : ExtendedTimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedTimePicker> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.Font = Font.RobotoLightItalic(16f);
            Control.DrawPickerBorder();
            Control.BorderStyle = UITextBorderStyle.None;
            //var timePicker = (UIDatePicker)Control.InputView;
            //timePicker.Locale = new NSLocale("no_nb");
        }
        private bool _focus;
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            if (e.PropertyName.Equals("IsFocused"))
            {
                _focus = !_focus;
                Control.DrawPickerBorder(_focus);
            }
            if (e.PropertyName.Equals("Width") || e.PropertyName.Equals("Height"))
            {
                var width = ((TimePicker)sender).Width;
                var height = ((TimePicker)sender).Height;
                Control.DrawPickerBorder(_focus, new nfloat(width), new nfloat(height));
            }
        }
    }


    public class CustomEditorRender : ExtendedEditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.Font = Font.RobotoLight(16f);
            Control.DrawBorder();

        }
        private bool _focus;
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            if (e.PropertyName.Equals("IsFocused"))
            {
                _focus = !_focus;
                Control.DrawBorder(_focus);
            }
            if (e.PropertyName.Equals("Width") || e.PropertyName.Equals("Height"))
            {
                var width = ((View)sender).Width;
                var height = ((View)sender).Height;
                Control.DrawBorder(_focus, new nfloat(width), new nfloat(height));
            }

        }

    }

    public static class Borders
    {

        public static void DrawBorder(this UIView control, bool focus = false, nfloat width = default(nfloat), nfloat height = default(nfloat))
        {
            if (width == default(nfloat))
                width = control.Frame.Width;
            if (height == default(nfloat))
                height = control.Frame.Height;
            if (width == 0 && height == 0) return;
            var borderLayer = new CALayer
            {
                MasksToBounds = true,
                Frame = new CGRect(0f, height - 11f, width, 1.0f),
                BorderColor = focus ? Color.Black.ToCGColor() : CustomColors.Gray.ToCGColor(),
                BorderWidth = 1.0f
            };
            control.Layer.AddSublayer(borderLayer);
        }

        public static void DrawPickerBorder(this UIView control, bool focus = false, nfloat width = default(nfloat), nfloat height = default(nfloat))
        {
            if (width == default(nfloat))
                width = control.Frame.Width;
            if (height == default(nfloat))
                height = control.Frame.Height;
            if ((width == 0 && height == 0) || width < 0 || height < 0) return;
            var b = 0;
            if (width < 200)
                b = 5;
            var a = 2.0f;
            var borderLayer = new CALayer
            {
                MasksToBounds = true,
                Frame = new CGRect(0f, height - a, width-b, 1.0f),
                BorderColor = focus ? Color.Black.ToCGColor() : CustomColors.Gray.ToCGColor(),
                BorderWidth = 1.0f
            };
            control.Layer.AddSublayer(borderLayer);
        }
    }
    #endregion




    #region [PinPreview]


    public class InfoTitleLabelRenderer : ExtendedLabelRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoLight(18f);
        }
    }
    public class TypeLabelRenderer : ExtendedLabelRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoLight(16f);
        }
    }

    public class TimeLabelRenderer : ExtendedLabelRenderer
    {

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null) return;
            Control.Font = Font.RobotoBold(16f);
        }
    }


/*    public class BorderFrameRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (NativeView != null)
                {
                  
                    NativeView.Layer.ShadowColor = e.NewElement?.OutlineColor.ToCGColor();
                    NativeView.Layer.ShadowRadius = 0;
                    NativeView.Layer.BorderWidth = 4;

                }
            }
        }
    }*/

    #endregion
    }

