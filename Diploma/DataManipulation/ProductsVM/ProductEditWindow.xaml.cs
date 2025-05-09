using Diploma.Database;
using Diploma.Database.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;

namespace Diploma.ViewModels.ProductsVM
{
    public partial class ProductEditWindow : Window
    {
        readonly ShowMessages showMessages = new();
        Products _product = new();
        int _elementId;

        public ProductEditWindow(int elementId)
        {
            InitializeComponent();

            _elementId = elementId;

            LoadData();
        }
        void LoadData()
        {
            using (var context = new AppDbContext())
            {
                _product = context.Products.FirstOrDefault(m => m.Id == _elementId);

                NameBox.Text           =    _product.Name;
                ArticleBox.Text        =    Convert.ToString(_product.Article);
                TypeBox.Text           =    _product.Type;
                PriceBox.Text          =    Convert.ToString(_product.Price);
                TotalQuantityBox.Text  =    Convert.ToString(_product.Quantity);
            }
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var contextManufacturer = context.Products.FirstOrDefault(m => m.Id == _elementId);

                    contextManufacturer.Name                =   NameBox.Text;
                    contextManufacturer.Article             =   int.Parse(ArticleBox.Text);
                    contextManufacturer.Type                =   TypeBox.Text;
                    contextManufacturer.Price               =   int.Parse(PriceBox.Text);
                    contextManufacturer.Quantity            =   int.Parse(TotalQuantityBox.Text);

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

        static bool CheckParameters(Products newElement)
        {
            bool isGood;

            if
            (
                newElement.Name.IsNullOrEmpty() ||
                newElement.Type.IsNullOrEmpty() ||

                newElement.Article == null      ||
                newElement.Price == null        ||
                newElement.Quantity == null
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }


        private void EmailBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
