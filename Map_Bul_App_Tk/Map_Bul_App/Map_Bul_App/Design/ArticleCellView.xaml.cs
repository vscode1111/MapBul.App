using System;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class ArticleCellView 
    {
        public ArticleCellView()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "IsShowed")
            {
                MainStackLayout.BackgroundColor = IsShowed ? Color.FromHex("#e6e6e6") : Color.White;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext == null) return;

            if (ImageSource != null)
            {
                var imageSource = new UriImageSource
                {
                    CachingEnabled = true,
                    CacheValidity = new TimeSpan(0, 5, 0, 0, 0),
                    Uri = new Uri(ImageSource)
                };

                PreviewImage.Source = imageSource;
            }

            TitleLabel.Text = Title;
            TagsLabel.Text = Tags;
            if (StartDate.HasValue)
            {
                EventDateDate.Text = EventDateString;
                EventDateDate.IsVisible = true;
                EventDateLabel.IsVisible = true;
            }
            else
            {
                DateLabel.Text = Date.ToString("dd.MM.yy");
                DateLabel.IsVisible = true;
            }

            if (StartTime.HasValue /* && StartTime.Value != TimeSpan.Zero*/)
            {
                StartTimeLabel.Text = new DateTime(StartTime.Value.Ticks).ToString("HH:mm");
            }
            else
            {
                StartTimeStackLayout.IsVisible = false;
            }
        }

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create<ArticleCellView, string>(p => p.ImageSource, default(string));

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create<ArticleCellView, DateTime>(p => p.Date, default(DateTime));

        public static readonly BindableProperty StartDateProperty =
            BindableProperty.Create<ArticleCellView, DateTime?>(p => p.StartDate, null);

        public static readonly BindableProperty StartTimeProperty =
            BindableProperty.Create<ArticleCellView, TimeSpan?>(p => p.StartTime, null);

        public static readonly BindableProperty StopDateProperty =
            BindableProperty.Create<ArticleCellView, DateTime?>(p => p.StopDate, null);

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create<ArticleCellView, string>(p => p.Title, default(string));

        public static readonly BindableProperty TagsProperty =
           BindableProperty.Create<ArticleCellView, string>(p => p.Tags, default(string));

        public static readonly BindableProperty IsShowedProperty =
           BindableProperty.Create<ArticleCellView, bool>(p => p.IsShowed, default(bool));

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }
        public DateTime? StartDate
        {
            get => (DateTime?)GetValue(StartDateProperty);
            set => SetValue(StartDateProperty, value);
        }
        public TimeSpan? StartTime
        {
            get => (TimeSpan?)GetValue(StartTimeProperty);
            set => SetValue(StartTimeProperty, value);
        }

        public DateTime? StopDate
        {
            get => (DateTime?)GetValue(StopDateProperty);
            set => SetValue(StopDateProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Tags
        {
            get => (string)GetValue(TagsProperty);
            set => SetValue(TagsProperty, value);
        }

        public bool IsShowed
        {
            get => (bool)GetValue(IsShowedProperty);
            set => SetValue(IsShowedProperty, value);
        }

        public string EventDateString
        {
            get
            {
                if (StartDate == null) return string.Empty;
                var result = StartDate.Value.ToString("dd.MM.yy");
                if (StopDate != null)
                    result += " - " + StopDate.Value.ToString("dd.MM.yy");
                return result;
            }
        }
    }
}
