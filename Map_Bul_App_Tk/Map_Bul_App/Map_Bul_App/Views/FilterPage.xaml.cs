using System.Linq;
using Map_Bul_App.Design;
using Map_Bul_App.Models.Tables;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.Views
{
    public partial class FilterPage : ContentPage
    {
        public FilterPage(MapSpan region)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
            if (region == null) return;
            CurrentViewModel.MapRegion = region;
            ReloadPins(region);
        }



        public FilterPage(FilterSettings settings)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
            CurrentViewModel.LoadFromSettings(settings);
        }
        private bool CategoriesLoaded { get; set; }
        private bool PinsLoaded { get; set; }
        public  FilterViewModel CurrentViewModel => BindingContext as FilterViewModel;

        protected override void OnAppearing()
        {
            CurrentViewModel.Initialize();
            base.OnAppearing();
            Init();
        }

        private void Init()
        {
          if (CategoriesLoaded || !CurrentViewModel.PinCategories.Any()) return;
            foreach (var category in CurrentViewModel.PinCategories)
            {
                var itemView = new CategoryView();
                itemView.SetBinding(CategoryView.IsSelectedProperty,nameof(category.ItemSelected),BindingMode.TwoWay);
                itemView.SetBinding(CategoryView.ItemColorProperty, nameof(category.Color));
                itemView.SetBinding(CategoryView.ItemNameProperty, nameof(category.Name));
                itemView.SetBinding(CategoryView.IconImageSourceProperty, nameof(category.Icon));
                itemView.ItemSelectionChanged += CategorySelectionChanged;
                itemView.BindingContext = category;
                CategoriesStack.Children.Add(itemView);
            }
            foreach (var category in CurrentViewModel.PinSubCategories)
            {
                var itemView = new CategoryView();
                itemView.SetBinding(CategoryView.IsSelectedProperty, nameof(category.ItemSelected), BindingMode.TwoWay);
                itemView.SetBinding(CategoryView.ItemColorProperty, nameof(category.Color));
                itemView.SetBinding(CategoryView.ItemNameProperty, nameof(category.Name));
                itemView.SetBinding(CategoryView.IconImageSourceProperty, nameof(category.Icon));
                itemView.SetBinding(IsVisibleProperty, nameof(category.IsVisible));
                itemView.ItemSelectionChanged += SubCategorySelectionChanged;
                itemView.BindingContext = category;
                SubCategoriesStack.Children.Add(itemView);
            }
            CategoriesLoaded = true;
        }

        public void ReloadPins(MapSpan region)
        {
            if(region != null)
            CurrentViewModel.LoadPins(region);
        }

        private void CategorySelectionChanged(object sender, bool e)
        {
            CurrentViewModel.UpdateSubcatgoriesVisible();
        }

        private void SubCategorySelectionChanged(object sender, bool e)
        {
            CurrentViewModel.UpdatePinsVisible();
        }
    }
}
