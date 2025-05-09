using Diploma.Database;
using Diploma.Models;
using Microsoft.IdentityModel.Tokens;
using System.Windows;

namespace Diploma.Authorization
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _appDbContext = new AppDbContext())
                {
                    if (_appDbContext.Workers.Any(w => w.Login == LoginBox.Text))
                    {
                        new ShowMessages().ShowErrorMessage("Пользователь с таким логином уже существует.");
                        return;
                    }

                    var newWorker = new Workers
                    {
                        Surname = SurnameBox.Text,
                        Name = NameBox.Text,
                        Patronymic = PatronymicBox.Text,
                        Login = LoginBox.Text,
                        Password = PasswordBox.Text,
                        Email = EmailBox.Text,
                        PhoneNumber = PhoneNumberBox.Text,
                        RegistrationDate = DateOnly.FromDateTime(DateTime.Now.Date),
                        IsAdmin = false
                    };

                    if (CheckParameters(newWorker) == false)
                    {
                        new ShowMessages().ShowErrorMessage("Заполните все поля, либо введите корректные данные.");
                        return;
                    }

                    _appDbContext.Add(newWorker);
                    _appDbContext.SaveChanges();

                    AuthManager.CurrentUser = newWorker;

                    Properties.Settings.Default.SavedLogin = LoginBox.Text;
                    Properties.Settings.Default.SavedPassword = PasswordBox.Text;
                    Properties.Settings.Default.SavedId = newWorker.Id;
                    Properties.Settings.Default.Save();

                    new MainWindow().Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                new ShowMessages().ShowError(ex);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        static bool CheckParameters(Workers newElement)
        {
            bool isGood;

            if
            (
                newElement.Surname.IsNullOrEmpty()      ||
                newElement.Name.IsNullOrEmpty()         ||
                newElement.Patronymic.IsNullOrEmpty()   ||
                newElement.Login.IsNullOrEmpty()        ||
                newElement.Password.IsNullOrEmpty()     ||
                newElement.Email.IsNullOrEmpty()        ||
                newElement.PhoneNumber.IsNullOrEmpty()
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }

    }
}
