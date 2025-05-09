using Diploma.Database;
using Diploma.Database.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Diploma.ViewModels.ClientsVM
{
    public partial class ClientAddWindow : Window
    {
        readonly ShowMessages showMessages = new();

        public ClientAddWindow()
        {
            InitializeComponent();
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newElement = new Clients
                {
                    Name = NameBox.Text,
                    Type = TypeBox.Text,
                    Address = AddressBox.Text,
                    ContactPerson = PersonBox.Text,
                    PhoneNumber = PhoneBox.Text,
                    Email = EmailBox.Text,
                    INN = InnBox.Text,
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

        static bool CheckParameters(Clients newElement)
        {
            bool isGood;

            if
            (
                newElement.Name.IsNullOrEmpty()             ||
                newElement.Type.IsNullOrEmpty()             ||
                newElement.Address.IsNullOrEmpty()          ||
                newElement.ContactPerson.IsNullOrEmpty()    ||
                newElement.PhoneNumber.IsNullOrEmpty()      ||
                newElement.Email.IsNullOrEmpty()            ||
                newElement.INN.IsNullOrEmpty()              ||

                newElement.INN.Length != 10                 &&
                newElement.INN.Length != 12
                
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }

        private void InnBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
