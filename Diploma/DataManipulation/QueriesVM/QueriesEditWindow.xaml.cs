using Diploma.Database;
using Diploma.Database.Models;
using Diploma.DataManipulation.QueriesVM;
using Diploma.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Diploma.ViewModels.QueriesVM
{
    public partial class QueriesEditWindow : Window
    {
        readonly ShowMessages showMessages = new();
        QuerriesToBuy _query = new();
        int _elementId;
        int _oldRequestedProductQuantity;
        bool _isQuerieDone;

        public List<Pharmacy> PharmacyList { get; set; }
        public List<Products> ProductsList { get; set; }

        public QueriesEditWindow(int elementId)
        {
            InitializeComponent();

            _elementId = elementId;

            LoadComboBoxData();
            InitializeBindings();
            LoadData();
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

        void LoadData()
        {
            using (var context = new AppDbContext())
            {
                _query = context.QuerriesToBuy.FirstOrDefault(m => m.Id == _elementId);

                ProductNameBox.Text     =   _query.ProductName;
                ProductIdBox.Text       =   Convert.ToString(_query.IdProduct);
                QuantityBox.Text        =   Convert.ToString(_query.Quantity);
                IdPharmacyBox.Text      =   Convert.ToString(_query.IdPharmacy);
                IsDoneBox.IsChecked     =   _query.IsDone;
            }
            _oldRequestedProductQuantity = int.Parse(QuantityBox.Text);
            _isQuerieDone = (bool)IsDoneBox.IsChecked;
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var contextQueri= context.QuerriesToBuy.FirstOrDefault(m => m.Id == _elementId);

                    contextQueri.ProductName    =   ProductNameBox.Text;
                    contextQueri.IdProduct      =   int.Parse(ProductIdBox.Text);
                    contextQueri.Quantity       =   int.Parse(QuantityBox.Text);
                    contextQueri.IdPharmacy     =   int.Parse(IdPharmacyBox.Text);
                    contextQueri.IsDone         =   (bool)IsDoneBox.IsChecked;

                    if (!CheckParameters(contextQueri))
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

            AddMethod();
        }

        void AddMethod()
        {
            try
            {
                int oldRequestedProductQuantity = _oldRequestedProductQuantity;

                bool isChecked = (bool)IsDoneBox.IsChecked;
                int productId = int.Parse(ProductIdBox.Text);
                int productQuantity =  int.Parse(QuantityBox.Text);

                if (_isQuerieDone == true)
                    productQuantity -= _oldRequestedProductQuantity;



                AddProductMethod method = new();
                method.AddProductQuantity(isChecked, productQuantity, productId);
            }
            catch { }
        }

        private void CancelBut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        bool CheckParameters(QuerriesToBuy newElement)
        {
            bool isGood = false;

            if
            (
                newElement.ProductName.IsNullOrEmpty()  ||
                newElement.IdProduct        ==  null    ||
                newElement.Quantity         ==  null    ||
                newElement.IdPharmacy       ==  null    ||
                newElement.IsDone           ==  null
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
