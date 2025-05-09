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
    public partial class PharmacyEditWindow : Window
    {
        readonly ShowMessages showMessages = new();
        Pharmacy _pharmacy= new();
        int _elementId;

        public PharmacyEditWindow(int elementId)
        {
            InitializeComponent();

            _elementId = elementId;

            LoadData();
        }

        void LoadData()
        {
            using (var context = new AppDbContext())
            {
                _pharmacy = context.Pharmacy.FirstOrDefault(m => m.Id == _elementId);

                CityBox.Text = _pharmacy.City;
                AddressBox.Text = _pharmacy.Address;
            }
        }
        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var contextPharmacy= context.Pharmacy.FirstOrDefault(m => m.Id == _elementId);

                    contextPharmacy.City = CityBox.Text;
                    contextPharmacy.Address = AddressBox.Text;

                    if (!CheckParameters(contextPharmacy))
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
