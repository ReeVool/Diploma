using Diploma.Database;
using Diploma.Database.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Diploma.ViewModels
{
    public partial class ManufacturerAddWindow : Window
    {
        readonly ShowMessages showMessages = new();

        public ManufacturerAddWindow()
        {
            InitializeComponent();
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newElement = new Manufacturers
                {
                    Name                =   NameBox.Text,
                    JuridicalAddress    =   JuridicalAddressBox.Text,
                    ActualAddress       =   ActualAddressBox.Text,
                    INN                 =   InnBox.Text,
                    PhoneNumber         =   PhoneBox.Text,
                    Email               =   EmailBox.Text,
                };

                if(CheckParameters(newElement) == false)
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

        static bool CheckParameters(Manufacturers newElement)
        {
            bool isGood;

            if
            (
                newElement.Name.IsNullOrEmpty()                 ||
                newElement.JuridicalAddress.IsNullOrEmpty()     ||
                newElement.ActualAddress.IsNullOrEmpty()        ||
                newElement.INN.IsNullOrEmpty()                  ||
                newElement.PhoneNumber.IsNullOrEmpty()          ||
                newElement.Email.IsNullOrEmpty()                ||

                newElement.INN.Length != 10                     &&
                newElement.INN.Length != 12
            )
                isGood = false;
            
            else
                isGood = true;

            return isGood;
        }

        private void EmailBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
