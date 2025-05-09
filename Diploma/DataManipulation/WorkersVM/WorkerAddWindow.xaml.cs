using Diploma.Database;
using System.Windows;
using Diploma.Models;
using Microsoft.IdentityModel.Tokens;

namespace Diploma.ViewModels.WorkersVM
{
    public partial class WorkerAddWindow : Window
    {
        readonly ShowMessages showMessages = new();

        public WorkerAddWindow()
        {
            InitializeComponent();
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newElement = new Workers
                {
                    Surname = SurnameBox.Text,
                    Name = NameBox.Text,
                    Patronymic = PatronymicBox.Text,
                    PhoneNumber = PhoneBox.Text,
                    Email = EmailBox.Text,
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Now.Date),
                    Login = LoginBox.Text,
                    Password = PasswordBox.Text,
                    IsAdmin = (bool)IsAdminBox.IsChecked,
                };

                if (CheckParameters(newElement) == false)
                    showMessages.ShowErrorMessage("Заполните все поля, либо введите корректные данные.");

                else
                {
                    using (var context = new AppDbContext())
                    {
                        context.Add(newElement);
                        context.SaveChanges();
                    }
                    Close();
                }

            }
            catch (Exception ex)
            {
                showMessages.ShowError(ex);
            }
        }

        private void CancelBut_Click(object sender, RoutedEventArgs e)
        {
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
                newElement.PhoneNumber.IsNullOrEmpty()  ||
                newElement.Email.IsNullOrEmpty()        ||
                newElement.Login.IsNullOrEmpty()        ||
                newElement.Password.IsNullOrEmpty()     ||
                newElement.RegistrationDate == null
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }
    }
}
