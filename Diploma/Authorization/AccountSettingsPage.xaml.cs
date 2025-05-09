using Diploma.Authorization;
using Diploma.Database;
using Diploma.Models;
using Diploma.ViewModels.WorkersVM;
using System.Windows;
using System.Windows.Controls;

namespace Diploma.Pages
{
    public partial class AccountSettingsPage : Page
    {
        Workers _worker = new();
        private int _id =  AuthManager.CurrentUser.Id;
        private string _login =  AuthManager.CurrentUser.Login;

        public List<Workers> WorkersList {  get; set; }

        public AccountSettingsPage()
        {
            InitializeComponent();
            LoadAccountData();
        }

        void LoadAccountData()
        {
            SurnameBox.Text =       AuthManager.CurrentUser.Surname;
            NameBox.Text =          AuthManager.CurrentUser.Name;
            PatronymicBox.Text =    AuthManager.CurrentUser.Patronymic;
            LoginBox.Text =         AuthManager.CurrentUser.Login;
            EmailBox.Text =         AuthManager.CurrentUser.Email;
            NumberBox.Text =        AuthManager.CurrentUser.PhoneNumber;
            RegDateBox.Text =       AuthManager.CurrentUser.RegistrationDate.ToString();

            if (AuthManager.CurrentUser.IsAdmin == true)
                AccountTYpeBox.Text = "Администратор";
            else
                AccountTYpeBox.Text = "Базовый";
        }

        // изменение
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditInfo();
            LoadAccountData();
        }
        void EditInfo()
        {
            new WorkerEditWindow(_login).ShowDialog();
            LoadAccountData();
        }

        // выход
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = new ShowMessages().ShowQuestion("Вы уверены, что хотите выйти из профиля?");

            if (res == MessageBoxResult.Yes)
            {
                EscapeFromAccount();
                OpenLogin();
            }
        }
        void EscapeFromAccount()
        {
            AuthManager.CurrentUser = null;
            Properties.Settings.Default.SavedLogin = "null";
            Properties.Settings.Default.SavedPassword = "null";
            Properties.Settings.Default.SavedId= 0;
            Properties.Settings.Default.Save();
        }

        //удаление
        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = new ShowMessages().ShowQuestion("Вы уверены, что хотите удалить профиль?");

            if (res == MessageBoxResult.Yes)
            {
                using (var context= new AppDbContext())
                {
                    var curWorker = context.Workers.FirstOrDefault(w => w.Login == _login);
                    context.Workers.Remove(curWorker);
                }
                EscapeFromAccount();
                RemoveUserData();
                OpenLogin();
            }
        }
        void RemoveUserData()
        {
            using (var context = new AppDbContext())
            {
                _worker = context.Workers.FirstOrDefault(w => w.Login == _login);

                context.Workers.Remove(_worker);
                context.SaveChanges();
            }
        }

        // переход к окну входа
        void OpenLogin()
        {
            new LoginWindow().Show();
            Window.GetWindow(this).Close();
        }
    }
}
