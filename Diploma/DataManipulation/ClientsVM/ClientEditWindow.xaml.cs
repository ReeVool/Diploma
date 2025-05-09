using Diploma.Database;
using Diploma.Database.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;

namespace Diploma.ViewModels.ClientsVM
{
    public partial class ClientEditWindow : Window
    {
        readonly ShowMessages showMessages = new();
        Clients _client = new();
        int _elementId;
        public List<Products> ProductsList { get; set; }

        public ClientEditWindow(int elementId)
        {
            InitializeComponent();

            _elementId = elementId;

            LoadData();
        }

        void LoadData()
        {
            using (var context = new AppDbContext())
            {
                _client = context.Clients.FirstOrDefault(m => m.Id == _elementId);

                NameBox.Text        =   _client.Name;
                TypeBox.Text        =   _client.Type;
                AddressBox.Text     =   _client.Address;
                PhoneBox.Text       =   _client.PhoneNumber;
                PersonBox.Text      =   _client.ContactPerson;
                EmailBox.Text       =   _client.Email;
                InnBox.Text         =   _client.INN;
            }
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var contextClient = context.Clients.FirstOrDefault(m => m.Id == _elementId);

                    contextClient.Name              =   NameBox.Text;
                    contextClient.Type              =   TypeBox.Text;
                    contextClient.Address           =   AddressBox.Text;
                    contextClient.PhoneNumber       =   PhoneBox.Text;
                    contextClient.ContactPerson     =   PersonBox.Text;
                    contextClient.Email             =   EmailBox.Text;
                    contextClient.INN               =   InnBox.Text;

                    if (!CheckParameters(contextClient))
                    {
                        showMessages.ShowErrorMessage("Заполните все поля, либо введите корректные данные.");
                        return;
                    }
                    
                    context.SaveChanges();
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

        private void InnBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
