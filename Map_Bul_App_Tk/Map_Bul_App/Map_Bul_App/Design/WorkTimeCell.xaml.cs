using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class WorkTimeCell 
    {
        public WorkTimeCell()
        {
            InitializeComponent();
            if (Device.OS == TargetPlatform.iOS)
            {
                PickersStackLayout.Padding = new Thickness(0, 0, 0, 3);
            }
        }
    }
}
