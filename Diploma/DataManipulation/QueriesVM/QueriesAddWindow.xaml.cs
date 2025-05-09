using Diploma.Database.Models;
using Diploma.Database;
using System.Windows;
using System.Windows.Input;
using Diploma.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using Diploma.DataManipulation.QueriesVM;

namespace Diploma.ViewModels.QueriesVM
{
    public partial class QueriesAddWindow : Window
    {
        readonly ShowMessages showMessages = new();
        public List<Pharmacy> PharmacyList { get; set; }
        public List<Products> ProductsList { get; set; }


        public QueriesAddWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
            InitializeBindings();
        }

        void LoadComboBoxData()
        {
            using (var context = new AppDbContext())
            {
                PharmacyList = context.Pharmacy.ToList();
                ProductsList = context.Products.ToList();
            }
        }

        void InitializeBindings()
        {
            IdPharmacyBox.ItemsSource = PharmacyList;
            IdPharmacyBox.DisplayMemberPath = "Id";

            ProductNameBox.ItemsSource = ProductsList;
            ProductNameBox.DisplayMemberPath = "Name";
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newElement = new QuerriesToBuy
                {
                    ProductName     =   ProductNameBox.Text,
                    IdProduct       =   int.Parse(ProductIdBox.Text),
                    Quantity        =   int.Parse(QuantityBox.Text),
                    IdPharmacy      =   int.Parse(IdPharmacyBox.Text),
                    IsDone          =   (bool)IsDoneBox.IsChecked,
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

            AddMethod();
        }

        void AddMethod()
        {
            try
            {
                bool isChecked = (bool)IsDoneBox.IsChecked;
                int productQuantity = int.Parse(QuantityBox.Text);
                int productId = int.Parse(ProductIdBox.Text);

                AddProductMethod method = new();
                method.AddProductQuantity(isChecked, productQuantity, productId);
            }
            catch { }
        }

        private void CancelBut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        static bool CheckParameters(QuerriesToBuy newElement)
        {
            bool isGood;

            if
            (
                newElement.ProductName.IsNullOrEmpty()      ||
                newElement.IdProduct        ==      null    ||
                newElement.Quantity         ==      null    ||
                newElement.IdPharmacy       ==      null    ||
                newElement.IsDone           ==      null
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }

        private void QuantityBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
