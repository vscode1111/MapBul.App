using System;
using Map_Bul_App.ResX;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace Map_Bul_App.Design
{
    public static class CustomColors
    {
        public static Color Pink = Color.FromHex("7f7f7f");
        public static Color Orange = Color.FromHex("ffb33b");
        public static Color Red = Color.FromHex("ff3b4d");
        public static Color Blue = Color.FromHex("3c8dff");
        public static Color Gray = Color.FromHex("7f7f7f");
        public static Color HeavyBlurColor = Color.FromHex("CDFFFFFF");
        public static Color DefBlurColor = Color.FromHex("4CFFFFFF");
    }

    public static class Blur
    {
        public const double DefOpacity = 0.3d;
        public const double StrongOpacity = 0.5d;
        public const double HeavyOpacity = 0.8d;
    }
    #region [ MainElement ]

    public class CustomImageView : Image
    {
        public CustomImageView()
        {
            PropertyChanged += CustomImageView_PropertyChanged;
        }

        private void CustomImageView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == WidthProperty.PropertyName && Height > Width/3)
            {
                HeightRequest = Width/3;
                PropertyChanged -= CustomImageView_PropertyChanged;
            }
            else if (e.PropertyName == HeightProperty.PropertyName && Height > Width/3)
            {
                HeightRequest = Width / 3;
                PropertyChanged -= CustomImageView_PropertyChanged;
            }
        }

        private string _webImagePath;
        public string WebImagePath
        {
            get => _webImagePath;
            set
            {
                if (value != _webImagePath)
                {
                    _webImagePath = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public class PickerViewLeft : View
    {
        
    }

    public class StackLayoutButton : StackLayout
    {
        public StackLayoutButton()
        {
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async (o) =>
                {
                    await this.ScaleTo(0.95, 50, Easing.CubicOut);
                    await this.ScaleTo(1, 50, Easing.CubicIn);
                })
            });
        }
    }

    public class LabelForStackLayoutButton : ExtendedLabel
    {
        public LabelForStackLayoutButton()
        {
            
        }
    }

    public class MainLabel : ExtendedLabel
    {
        public MainLabel()
        {
            TextColor = Color.Black;
        }
    }

    public class MainEntry : ExtendedEntry
    {
        public MainEntry()
        {
            TextColor = Color.Black;
            HasBorder = false;
            
        }
    }

    public class MainPicker : ExtendedPicker
    {
    }

    public class MainButton : ExtendedButton
    {
        public MainButton()
        {
            TextColor = Color.Black;
            BackgroundColor = Color.White;
        }
    }



    #endregion [ MainElement ]

    #region [ Auth ]

    public class AuthLabel : MainLabel
    {
        public AuthLabel()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
        }
    }

    public class AuthLabelRegistr : MainLabel
    {
        public AuthLabelRegistr()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            IsUnderline = true;
        }
    }

    public class AuthEntry : MainEntry
    {
        public AuthEntry()
        {
            HeightRequest = 40;
        }
    }

    public class AuthButton : MainButton
    {
    }

    #endregion [ Auth ]

    #region [ Registration ]

    public class RegistrationLabel : MainLabel
    {

    }

    public class FormattedRegistrationLabel : MainLabel
    {

    }
    public class RegistrationEntry : MainEntry
    {

    }

    public class RegistrationPicker : MainPicker
    {
    }

    public class RegistrationButton : MainButton
    {

    }


    public class RegistrationBirthDatePicker : EventDatePicker
    {
        public RegistrationBirthDatePicker()
        {
            MinimumDate = DateTime.Now.AddYears(-100);
            MaximumDate = DateTime.Now;
            Format = "dd.MM.yyyy";
        }
    }
    #endregion [ Registration ]
    #region [ Navigation ]

    public class NavLabel : ExtendedLabel
    {
        public NavLabel()
        {
        }
    }

    public class NavMainLabel : ExtendedLabel
    {
        public NavMainLabel()
        {
            TextColor = Color.Black;
        }
    }

    public class BlurStack : StackLayout
    {
        public BlurStack()
        {
            Opacity = Blur.DefOpacity;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.White;
        }
    }

    public class BlurSeparator : StackLayout
    {
        public BlurSeparator()
        {
            BackgroundColor = Color.White;
            Opacity = Blur.StrongOpacity;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Fill;
            HeightRequest = 1;
        }
    }





    #endregion [ Navigation ]



    #region ActionBar
    public class ActionBarEntry : MainEntry
    {
        public ActionBarEntry()
        {
            PlaceholderColor = CustomColors.Gray;
            Placeholder = TextResource.ActionBarFindPlaceholder;
            if (Device.OS == TargetPlatform.Android)
            {
                FontAttributes = FontAttributes.Italic;
            }
        }
    }


    public class ActionBarLabel : MainLabel
    {
        public ActionBarLabel()
        {
            TextColor = Color.Black;
        }
    }
    public class SmallActionBarLabel : ActionBarLabel
    {
        public SmallActionBarLabel()
        {
          //  TextColor = Color.Black;
        }
    }
    #endregion


    #region [Filter]

    public class Separator : Image
    {
        public Separator()
        {
            Aspect = Aspect.Fill;
            BackgroundColor = CustomColors.Gray;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Fill;
            HeightRequest = 1;
        }
    }

    public class VerticalSeparator : Image
    {
        public VerticalSeparator()
        {
            Aspect = Aspect.Fill;
            BackgroundColor = CustomColors.Gray;
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.FillAndExpand;
            WidthRequest = 1;
        }
    }
    #endregion

    #region [Favorites]
    public class DisabledScrollView : ExtendedScrollView
    {
 
    }

    public class FavoritesBarLabel : ActionBarLabel
    {
        public FavoritesBarLabel()
        {
            VerticalOptions = LayoutOptions.CenterAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalTextAlignment = TextAlignment.Center;
            HorizontalTextAlignment = TextAlignment.Center;
        }
    }
    #endregion


    #region [Item]

    public class ItemWhiteLabel : MainLabel
    {
        public ItemWhiteLabel()
        {
            TextColor = Color.White;
        }
    }

    public class ItemMediumLabel : MainLabel
    {
        public ItemMediumLabel()
        {

        }
    }


    public class TagsLabel : MainLabel
    {
        public TagsLabel()
        {
            TextColor = CustomColors.Gray;
        }
    }
    #endregion

    #region [Add new pin]

    public class CustomEditor : ExtendedEditor
    {
        public CustomEditor()
        {

        }
    }
    #endregion




    #region [Articles]

    public class CachingListView : ListView
    {
        // public CachingListView():base(ListViewCachingStrategy.RecycleElement)
        public CachingListView()
        {

        }
    }
    public class CellDateLabel : ExtendedLabel
    {
        public CellDateLabel()
        {
            TextColor = CustomColors.Gray;
        }

    }
    public class CellTitleLabel : MainLabel
    {

    }

    public class AuthorNameLabel : ExtendedLabel
    {
        public AuthorNameLabel()
        {
            TextColor = CustomColors.Gray;
        }
    }

    public class EventLabelStack : ExtendedLabel
    {

    }



    #endregion

    #region [Events]

    public class EventDateLabel : ExtendedLabel
    {

    }
    public class EventDateLabelText : ExtendedLabel
    {

    }

    public class EventDatePicker : ExtendedDatePicker
    {
        public EventDatePicker()
        {
            HorizontalOptions=LayoutOptions.Fill;
        }
    }

    public class EventSmallDatePicker : EventDatePicker
    {
        public EventSmallDatePicker()
        {

        }
    }
    #endregion


    #region [Add Pin]

    public class CustomTimePicker : ExtendedTimePicker
    {
        public CustomTimePicker()
        {
            HorizontalOptions = LayoutOptions.Fill;
          
        }
    }

    public class CustomPicker : Picker
    {
        public CustomPicker()
        {

        }
    }
    #endregion


    #region [PinPreview]

    public class BorderFrame : Frame
    {
        public BorderFrame()
        {
            this.HasShadow = true;
            Padding = new Thickness(0);
        }
    }
    public class InfoTitleLabel : ActionBarLabel
    {

    }
    public class TypeLabel : ActionBarLabel
    {

    }
    public class SubTypeLabel : ActionBarLabel
    {

    }

    public class TimeLabel : ActionBarLabel
    {

    }


    #endregion

    public class HtmlLabel : Label
    {
        public HtmlLabel()
        {
            TextColor = Color.Black;
        }
    }
}
