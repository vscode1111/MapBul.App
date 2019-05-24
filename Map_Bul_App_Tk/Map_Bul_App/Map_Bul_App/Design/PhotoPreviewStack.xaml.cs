using System.Windows.Input;
using Map_Bul_App.Settings;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class PhotoPreviewStack 
    {
        public PhotoPreviewStack()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create<PhotoPreviewStack, string>(p => p.Source, default(string));

        public static readonly BindableProperty CloseCommandProperty =
            BindableProperty.Create<PhotoPreviewStack, ICommand>(p => p.CloseCommand, default(Command));

        public static readonly BindableProperty BackCommandProperty =
            BindableProperty.Create<PhotoPreviewStack, ICommand>(p => p.BackCommand, default(Command));

        public static readonly BindableProperty BackVisibleProperty =
            BindableProperty.Create<PhotoPreviewStack, bool>(p => p.BackVisible, default(bool));

        public static readonly BindableProperty ForwardCommandProperty =
            BindableProperty.Create<PhotoPreviewStack, ICommand>(p => p.ForwardCommand, default(Command));

        public static readonly BindableProperty ForwardVisibleProperty =
            BindableProperty.Create<PhotoPreviewStack, bool>(p => p.ForwardVisible, default(bool));

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public ICommand BackCommand
        {
            get => (ICommand)GetValue(BackCommandProperty);
            set => SetValue(BackCommandProperty, value);
        }
        public bool BackVisible
        {
            get => (bool)GetValue(BackVisibleProperty);
            set => SetValue(BackVisibleProperty, value);
        }

        public ICommand ForwardCommand
        {
            get => (ICommand)GetValue(ForwardCommandProperty);
            set => SetValue(ForwardCommandProperty, value);
        }

        public bool ForwardVisible
        {
            get => (bool)GetValue(ForwardVisibleProperty);
            set => SetValue(ForwardVisibleProperty, value);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null) return;

            CenterImage.Source = Source;
            CloseImage.GestureRecognizers.Clear();
            CloseImage.GestureRecognizers.Add(new TapGestureRecognizer { Command = CloseCommand });

            BackImage.GestureRecognizers.Clear();
            BackImage.GestureRecognizers.Add(new TapGestureRecognizer { Command = BackCommand });
            BackImage.IsVisible = BackVisible;

            ForwardImage.GestureRecognizers.Clear();
            ForwardImage.GestureRecognizers.Add(new TapGestureRecognizer { Command = ForwardCommand });
            ForwardImage.IsVisible = ForwardVisible;

            PropertyChanged += HeaderStack_PropertyChanged;
        }

        private void HeaderStack_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Source):
                    CenterImage.Source = Source;
                    break;
                case nameof(BackVisible):
                    BackImage.IsVisible = BackVisible;
                    break;
                case nameof(ForwardVisible):
                    ForwardImage.IsVisible = ForwardVisible;
                    break;
            }
        }
    }
}
