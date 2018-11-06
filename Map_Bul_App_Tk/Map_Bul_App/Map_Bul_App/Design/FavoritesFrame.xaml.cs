using System;
using Map_Bul_App.Settings;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class FavoritesFrame
    {
        public FavoritesFrame()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            ApplicationSettings.CreateToast(ToastNotificationType.Success, "Тап прошел.");
        }
    }
}
