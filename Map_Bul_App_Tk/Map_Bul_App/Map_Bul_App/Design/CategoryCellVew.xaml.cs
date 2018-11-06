using Map_Bul_App.Models;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class CategoryCellVew 
    {
        public CategoryCellVew()
        {
            InitializeComponent();
            CellGestureRecognizer.Command= new Command(() =>
            {
                CurrViewModel.OnItemTapped();
            });
        }
        private PinCategory CurrViewModel => BindingContext as PinCategory;
    }
}
