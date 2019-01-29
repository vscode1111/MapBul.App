using System.Threading.Tasks;
using System.Windows.Input;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using Map_Bul_App.ViewModel;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Map_Bul_App.Views
{
    public partial class RecoverPasswordView
    {
        public RecoverPasswordView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);//скрыть ActionBar
         }


        private RecoverPasswordViewModel CurrentViewModel => BindingContext as RecoverPasswordViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CurrentViewModel.Initialize();
        }
    }


    public class RecoverPasswordViewModel : BaseViewModel
    {
        public override void InitilizeFunc(object obj = null)
        {
            OnPropertyChanged(nameof(ButtonEnabled));
        }

        public bool ButtonEnabled => !string.IsNullOrEmpty(Mail);

        private string _mail;
        public string Mail
        {
            get => _mail;
            set
            {
                if(_mail==value) return;
                _mail = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }

        public ICommand RecoverCommand => new Command(() =>
        {
            if (string.IsNullOrEmpty(Mail))
            {

                ApplicationSettings.CreateToast(ToastNotificationType.Warning, TextResource.EnterEmailAddressToast);
                return;
            }
            StartLoading();
            Task.Run(() =>
            {
                var result = ApplicationSettings.Service.RecoverPassord(Mail);

                Device.BeginInvokeOnMainThread(() =>
                {
                    StopLoading();
                    if (result == null)
                        return;
                    if (result.Success)
                    {
                        ApplicationSettings.CreateToast(ToastNotificationType.Success, TextResource.PasswordSentToast);
                        GoBack();
                    }
                    else
                    {
                        ApplicationSettings.CreateToast(ToastNotificationType.Error, result.ErrorReason);
                    }
                });
            });
        });
    }
}
