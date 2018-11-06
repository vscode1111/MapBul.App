using Map_Bul_App.Models;
using Map_Bul_App.ViewModel;
using Map_Bul_App.ViewModel.Design;

namespace Map_Bul_App.Design
{
    public partial class DayCell 
    {
        public DayCell()
        {
            InitializeComponent();
        }
        private MyDayOfWeek CurrViewModel => BindingContext as MyDayOfWeek;

    }
}
