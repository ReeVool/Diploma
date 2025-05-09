using Diploma.Database;
using Diploma.Database.Models;
using Diploma.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Diploma.ViewModels.PharmacyVM
{
    public partial class PharmacyAddWindow : Window
    {
        readonly ShowMessages showMessages = new();

        public PharmacyAddWindow()
        {
            InitializeComponent();
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newElement = new Pharmacy
                {
                    City = CityBox.Text,
                    Address = AddressBox.Text,
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

        static bool CheckParameters(Pharmacy newElement)
        {
            bool isGood;

            if
            (
                newElement.City.IsNullOrEmpty() ||
                newElement.Address.IsNullOrEmpty() 
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }
    }
}
