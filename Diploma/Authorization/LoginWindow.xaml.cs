using Diploma.Database;
using System.Windows;
namespace Diploma.Authorization
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (
                Properties.Settings.Default.SavedLogin != "null" ||
                Properties.Settings.Default.SavedPassword != "null"
                )
            {
                LoginInputBox.Text = Properties.Settings.Default.SavedLogin;
                PasswordInputBox.Password = Properties.Settings.Default.SavedPassword;

                Login();
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        void Login()
        {
            using (var context = new AppDbContext())
            {
                var worker = context.Workers.FirstOrDefault(w => w.Login == LoginInputBox.Text && w.Password == PasswordInputBox.Password);

                if (worker != null)
                {
                    AuthManager.CurrentUser = worker;

                    Properties.Settings.Default.SavedLogin = LoginInputBox.Text;
                    Properties.Settings.Default.SavedPassword = PasswordInputBox.Password;
                    Properties.Settings.Default.SavedId = worker.Id;
                    Properties.Settings.Default.Save();

                    new MainWindow().Show();
                    Close();
                }
                else
                {
                    new ShowMessages().ShowErrorMessage("Неверный логин или пароль.");
                }
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            Close();
        }
    }
}
