using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class PinCellView
    {
        public PinCellView()
        {
            InitializeComponent();
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null) return;
            PreviewImage.Source= PreviewImageSource;
            CategoryImage.Source= CategoryImageSource;
            NameLabel.Text = PinName;
            CategoryLabel.Text = CategoryName;
            CategoryLabel.TextColor = CategoryColor;
            TagsLabel.Text = Tags;
            SubcategoryLabel.Text = SubCategoryName;
        }


        public static readonly BindableProperty PreviewImageSourceProperty =
            BindableProperty.Create<PinCellView, string>(p => p.PreviewImageSource, default(string));


        public static readonly BindableProperty CategoryImageSourceProperty =
    BindableProperty.Create<PinCellView, string>(p => p.CategoryImageSource, default(string));

        public static readonly BindableProperty CategoryNameProperty =
   BindableProperty.Create<PinCellView, string>(p => p.CategoryName, default(string));
        public static readonly BindableProperty SubCategoryNameProperty =
   BindableProperty.Create<PinCellView, string>(p => p.SubCategoryName, default(string));


        public static readonly BindableProperty PinNameProperty =
            BindableProperty.Create<PinCellView, string>(p => p.PinName, default(string));

        public static readonly BindableProperty TagsProperty =
           BindableProperty.Create<PinCellView, string>(p => p.Tags, default(string));


        public static readonly BindableProperty CategoryColorProperty =
           BindableProperty.Create<PinCellView, Color>(p => p.CategoryColor, Color.White);

        public string PreviewImageSource
        {
            get => (string)GetValue(PreviewImageSourceProperty);
            set => SetValue(PreviewImageSourceProperty, value);
        }

        public string PinName
        {
            get => (string)GetValue(PinNameProperty);
            set => SetValue(PinNameProperty, value);
        }

        public string CategoryImageSource
        {
            get => (string)GetValue(CategoryImageSourceProperty);
            set => SetValue(CategoryImageSourceProperty, value);
        }

        public string CategoryName
        {
            get => (string)GetValue(CategoryNameProperty);
            set => SetValue(CategoryNameProperty, value);
        }

        public string SubCategoryName
        {
            get => (string)GetValue(SubCategoryNameProperty);
            set => SetValue(SubCategoryNameProperty, value);
        }

        public Color CategoryColor
        {
            get => (Color)GetValue(CategoryColorProperty);
            set => SetValue(CategoryColorProperty, value);
        }

        public string Tags
        {
            get => (string)GetValue(TagsProperty);
            set => SetValue(TagsProperty, value);
        }



    }
}
