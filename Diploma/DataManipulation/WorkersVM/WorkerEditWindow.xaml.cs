using Diploma.Authorization;
using Diploma.Database;
using Diploma.Models;
using Microsoft.IdentityModel.Tokens;
using System.Windows;

namespace Diploma.ViewModels.WorkersVM
{
    public partial class WorkerEditWindow : Window
    {
        readonly ShowMessages showMessages = new();
        Workers _worker = new();
        string _login;

        public WorkerEditWindow(string login)
        {
            InitializeComponent();

            _login = login;

            LoadData();
        }

        void LoadData()
        {
            using (var context = new AppDbContext())
            {
                _worker = context.Workers.FirstOrDefault(m => m.Login == _login);

                SurnameBox.Text         =   _worker.Surname;
                NameBox.Text            =   _worker.Name;
                PatronymicBox.Text      =   _worker.Patronymic;
                LoginBox.Text           =   _worker.Login;
                PasswordBox.Text        =   _worker.Password;
                PhoneBox.Text           =   _worker.PhoneNumber;
                EmailBox.Text           =   _worker.Email;
                IsAdminBox.IsChecked    =   _worker.IsAdmin;
            }
            if (AuthManager.CurrentUser.IsAdmin == false)
            {
                IsAdminBox.IsEnabled = false;
                
            }
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var contextWorker = context.Workers.FirstOrDefault(w => w.Login == _login);

                    contextWorker.Surname           =   SurnameBox.Text;
                    contextWorker.Name              =   NameBox.Text;
                    contextWorker.Patronymic        =   PatronymicBox.Text;
                    contextWorker.Login             =   LoginBox.Text;
                    contextWorker.Password          =   PasswordBox.Text;
                    contextWorker.Email             =   EmailBox.Text;
                    contextWorker.PhoneNumber       =   PhoneBox.Text;
                    contextWorker.IsAdmin           =   (bool)IsAdminBox.IsChecked;

                    if (!CheckParameters(contextWorker))
                    {
                        showMessages.ShowErrorMessage("Заполните все поля, либо введите корректные данные.");
                        return;
                    }

                    SaveMyData(contextWorker);

                    context.SaveChanges();
                }
                Close();
            }
            catch (Exception ex)
            {
                showMessages.ShowError(ex);
            }
        }

        void SaveMyData(Workers worker)
        {
            if (worker.Id != Properties.Settings.Default.SavedId)
                return;

            Properties.Settings.Default.SavedLogin = LoginBox.Text;
            Properties.Settings.Default.SavedPassword = PasswordBox.Text;
            Properties.Settings.Default.Save();

            AuthManager.CurrentUser = worker;
        }

        private void CancelBut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        bool CheckParameters(Workers newElement)
        {
            bool isGood = false;

            if
            (
                newElement.Surname.IsNullOrEmpty()      ||
                newElement.Name.IsNullOrEmpty()         ||
                newElement.Patronymic.IsNullOrEmpty()   ||
                newElement.PhoneNumber.IsNullOrEmpty()  ||
                newElement.Email.IsNullOrEmpty()        ||
                newElement.Login.IsNullOrEmpty()        ||
                newElement.Password.IsNullOrEmpty()
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }
    }
}
