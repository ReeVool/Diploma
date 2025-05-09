using Diploma.Database;
using Diploma.Database.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Diploma.ViewModels
{
    public partial class ManufacturerEditWindow : Window
    {
        readonly ShowMessages showMessages = new();
        Manufacturers _manufacturer = new();
        int _elementId;

        public ManufacturerEditWindow(int elementId)
        {
            InitializeComponent();

            _elementId = elementId;

            LoadData();
        }

        void LoadData()
        {
            using (var context = new AppDbContext())
            {
                _manufacturer = context.Manufacturers.FirstOrDefault(m => m.Id == _elementId);

                NameBox.Text                =    _manufacturer.Name;
                JuridicalAddressBox.Text    =    _manufacturer.JuridicalAddress;
                ActualAddressBox.Text       =    _manufacturer.ActualAddress;
                InnBox.Text                 =    _manufacturer.INN;
                PhoneBox.Text               =    _manufacturer.PhoneNumber;
                EmailBox.Text               =    _manufacturer.Email;
            }
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var contextManufacturer = context.Manufacturers.FirstOrDefault(m => m.Id == _elementId);

                    contextManufacturer.Name                =   NameBox.Text;
                    contextManufacturer.JuridicalAddress    =   JuridicalAddressBox.Text;
                    contextManufacturer.ActualAddress       =   ActualAddressBox.Text;
                    contextManufacturer.INN                 =   InnBox.Text;
                    contextManufacturer.PhoneNumber         =   PhoneBox.Text;
                    contextManufacturer.Email               =   EmailBox.Text;

                    if (!CheckParameters(contextManufacturer))
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

        bool CheckParameters(Manufacturers newElement)
        {
            bool isGood = false;

            if
            (
                newElement.Name.IsNullOrEmpty()             ||
                newElement.JuridicalAddress.IsNullOrEmpty() ||
                newElement.ActualAddress.IsNullOrEmpty()    ||
                newElement.INN.IsNullOrEmpty()              ||
                newElement.PhoneNumber.IsNullOrEmpty()      ||
                newElement.Email.IsNullOrEmpty()            ||

                newElement.INN.Length != 10                 &&
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
