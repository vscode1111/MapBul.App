using System.Windows.Input;
using Map_Bul_App.Settings;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class HeaderStack 
    {
        public HeaderStack()
        {
            InitializeComponent();
        }

        private void HeaderStack_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName ==nameof(RightImageBackground))
            {
                RightImage.BackgroundColor = RightImageBackground;
            }
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create<HeaderStack, string>(p => p.Title, default(string));

        public static readonly BindableProperty LeftImageSourceProperty =
            BindableProperty.Create<HeaderStack, string>(p => p.LeftImageSource, default(string));

        public static readonly BindableProperty LeftImageCommandProperty =
            BindableProperty.Create<HeaderStack, ICommand>(p => p.LeftImageCommand, default(Command));
        
        public static readonly BindableProperty RightImageCommandProperty =
            BindableProperty.Create<HeaderStack, ICommand>(p => p.RightImageCommand, default(Command));

        public static readonly BindableProperty RightImageSourceProperty =
            BindableProperty.Create<HeaderStack, string>(p => p.RightImageSource, default(string));

        

        public static readonly BindableProperty NoGuestProperty =
            BindableProperty.Create<HeaderStack, bool>(p => p.NoGuest, false);

        public static readonly BindableProperty RightImageBackgroundProperty =
            BindableProperty.Create<HeaderStack, Color>(p => p.RightImageBackground, Color.White);

        public static readonly BindableProperty CustomTitleStackProperty =
            BindableProperty.Create<HeaderStack, StackLayout>(p => p.CustomTitleStack, default(StackLayout));

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null) return;
            LeftImage.Source = LeftImageSource;
            LeftImageStack.GestureRecognizers.Clear();
            LeftImageStack.GestureRecognizers.Add(new TapGestureRecognizer {Command = LeftImageCommand});
            TitleLabel.Text = Title;
            if (string.IsNullOrEmpty(RightImageSource)) return;
            RightImageStack.GestureRecognizers.Clear();
            RightImageStack.GestureRecognizers.Add(new TapGestureRecognizer { Command = RightImageCommand });
            RightImage.Source = RightImageSource;
            RightImage.BackgroundColor = RightImageBackground;
            PropertyChanged += HeaderStack_PropertyChanged;
            if(NoGuest)
              RightImage.IsVisible = RightImage.IsEnabled = ApplicationSettings.CurrentUser.UserType!=UserTypesMobile.Guest;
        }
        public string LeftImageSource
        {
            get { return (string)GetValue(LeftImageSourceProperty); }
            set { SetValue(LeftImageSourceProperty, value); }
        }

        public ICommand LeftImageCommand
        {
            get { return (ICommand)GetValue(LeftImageCommandProperty); }
            set { SetValue(LeftImageCommandProperty, value); }
        }

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string RightImageSource
        {
            get { return (string)GetValue(RightImageSourceProperty); }
            set { SetValue(RightImageSourceProperty, value); }
        }
        public ICommand RightImageCommand
        {
            get { return (ICommand)GetValue(RightImageCommandProperty); }
            set { SetValue(RightImageCommandProperty, value); }
        }

        public bool NoGuest
        {
            get { return (bool)GetValue(NoGuestProperty); }
            set { SetValue(NoGuestProperty, value); }
        }
        
        public StackLayout CustomTitleStack
        {
            get { return (StackLayout)GetValue(CustomTitleStackProperty); }
            set { SetValue(CustomTitleStackProperty, value); }
        }

        public Color RightImageBackground
        {
            get { return (Color)GetValue(RightImageBackgroundProperty); }
            set { SetValue(RightImageBackgroundProperty, value); }
        }

    }
}
