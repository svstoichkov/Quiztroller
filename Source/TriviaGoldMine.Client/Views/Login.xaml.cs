namespace Quiztroller.Views
{
    using System.Windows;

    using Properties;

    using ViewModels;

    public partial class Login
    {
        public Login()
        {
            this.InitializeComponent();
            this.txtEmail.Text = Settings.Default.Email;
            this.txtPassword.Password = Settings.Default.Password;

            this.txtPassword.PasswordChanged += this.OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel) this.DataContext).Password = this.txtPassword.Password;
        }
    }
}