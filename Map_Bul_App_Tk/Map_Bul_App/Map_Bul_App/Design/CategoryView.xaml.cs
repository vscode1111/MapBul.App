using System;
using System.ComponentModel;
using Map_Bul_App.Converters;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class CategoryView
    {
        public CategoryView()
        {
            InitializeComponent();
            PropertyChanged += CategoryCellVew_PropertyChanged;
            _converter = new BoolToOpacityConverter();
            CellGestureRecognizer.Command = new Command(() =>
            {
                IsSelected = !IsSelected;
            });
        }

        private readonly BoolToOpacityConverter _converter;
        public event EventHandler<bool> ItemSelectionChanged;

        public void FireItemSelection()
        {
            var handler = ItemSelectionChanged;
            handler?.Invoke(this,IsSelected);
        }

        private void CategoryCellVew_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsSelected)) return;
            SeparatorStack.Opacity = Opacity = (double) _converter.Convert(IsSelected, Opacity.GetType(), null, null);
            SelectImage.IsVisible = IsSelected;
            FireItemSelection();
        }

        public static readonly BindableProperty ItemColorProperty =
            BindableProperty.Create<CategoryView, Color>(p => p.ItemColor, default(Color));

        public static readonly BindableProperty CategoryIdProperty =
            BindableProperty.Create<CategoryView, int>(p => p.CategoryId, default(int));

        public static readonly BindableProperty IconImageSourceProperty =
            BindableProperty.Create<CategoryView, ImageSource>(p => p.IconImageSource, "ico_museum.png");


        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create<CategoryView, bool>(p => p.IsSelected, default(bool));


        public static readonly BindableProperty ItemNameProperty =
            BindableProperty.Create<CategoryView, string>(p => p.ItemName, default(string));

        public static readonly BindableProperty SeparatorVisibleProperty =
    BindableProperty.Create<CategoryView, bool>(p => p.SeparatorVisible, true);

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null) return;
            SeparatorStack.IsVisible = SeparatorStack.IsEnabled = SeparatorVisible;
            IconImage.Source = IconImageSource;
            NameLabel.TextColor = SeparatorStack.BackgroundColor= SelectImage.BackgroundColor = ItemColor;
            SelectImage.IsVisible = IsSelected;
           // SelectImage.BackgroundColor = Color.White;
            NameLabel.Text = ItemName;
            SeparatorStack.Opacity = Opacity = (double) _converter.Convert(IsSelected, Opacity.GetType(), null, null);
        }

        public ImageSource IconImageSource
        {
            get => (ImageSource) GetValue(IconImageSourceProperty);
            set => SetValue(IconImageSourceProperty, value);
        }

        public Color ItemColor
        {
            get => (Color) GetValue(ItemColorProperty);
            set => SetValue(ItemColorProperty, value);
        }

        public string ItemName
        {
            get => (string) GetValue(ItemNameProperty);
            set => SetValue(ItemNameProperty, value);
        }

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        public int CategoryId
        {
            get => (int)GetValue(CategoryIdProperty);
            set => SetValue(CategoryIdProperty, value);
        }
        public bool SeparatorVisible
        {
            get => (bool)GetValue(SeparatorVisibleProperty);
            set => SetValue(SeparatorVisibleProperty, value);
        }
    }
}
