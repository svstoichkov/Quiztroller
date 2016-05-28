namespace Quiztroller.ViewModels
{
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using Properties;

    public class LoginViewModel : ViewModelBase
    {
        private int zIndex = 1000;
        private Visibility loginVisibility = Visibility.Visible;
        private string errorMessage;
        private string password = Settings.Default.Password;

        public LoginViewModel()
        {
#if DEBUG
            this.zIndex = 0;
#endif

            this.Login = new RelayCommand(this.HandleLogin, this.CanSave);
        }

        public ICommand Login { get; set; }

        public int ZIndex
        {
            get
            {
                return this.zIndex;
            }
            set
            {
                this.Set(() => this.ZIndex, ref this.zIndex, value);
            }
        }

        public Visibility LoginVisibility
        {
            get
            {
                return this.loginVisibility;
            }
            set
            {
                this.Set(() => this.LoginVisibility, ref this.loginVisibility, value);
            }
        }

        public string Email { get; set; } = Settings.Default.Email;

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.Set(() => this.Password, ref this.password, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            set
            {
                this.Set(() => this.ErrorMessage, ref this.errorMessage, value);
            }
        }
        
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(this.Email) && !string.IsNullOrWhiteSpace(this.Password);
        }

        private async void HandleLogin()
        {
            this.LoginVisibility = Visibility.Collapsed;
            var login = await Authenticator.Authenticate(this.Email, this.Password);
            this.LoginVisibility = Visibility.Visible;

            if (login == "Membership")
            {
                this.ZIndex = 0;

                Settings.Default.Email = this.Email;
                Settings.Default.Password = this.Password;
                Settings.Default.Save();
            }
            else
            {
                this.ErrorMessage = login;
            }
        }
    }
}
