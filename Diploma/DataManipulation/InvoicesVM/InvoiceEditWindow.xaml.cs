using Diploma.Database;
using Diploma.Database.Models;
using Diploma.DataManipulation.InvoicesVM;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Diploma.Views
{
    public partial class InvoiceEditWindow : Window
    {
        readonly ShowMessages showMessages = new();
        Invoices _invoices = new();
        private int _elementId;
        private int _price;
        int _oldRequestedProductQuantity;

        public List<Clients> ClientsList { get; set; }
        public List<Products> ProductsList { get; set; }
        public List<Manufacturers> ManufacturersList { get; set; }

        public InvoiceEditWindow(int elementId)
        {
            InitializeComponent();

            _elementId = elementId;

            LoadComboBoxData();
            InitializeBindings();
            LoadData();
        }

        void LoadData()
        {
            using (var context = new AppDbContext())
            {
                _invoices = context.Invoices.FirstOrDefault(m => m.Id == _elementId);

                ClientIdBox.Text            =   Convert.ToString(_invoices.ClientId);
                ClientNameBox.Text          =   _invoices.ClientName;
                ClientInnBox.Text           =   _invoices.ClientINN;
                ClientAddressBox.Text       =   _invoices.ClientAddress;
                ProductNameBox.Text         =   _invoices.ProductName;
                ProductIdBox.Text           =   Convert.ToString(_invoices.ProductId);
                QuantityBox.Text            =   Convert.ToString(_invoices.Quantity);
                PriceBox.Text               =   Convert.ToString(_invoices.TotalPrice);
                DateBox.Text                =   Convert.ToString(_invoices.Date);
                ManufacturerIdBox.Text      =   Convert.ToString(_invoices.ManufacturerId);
                ManufacturerNameBox.Text    =   _invoices.ManufacturerName;
                ManufacturerInnBox.Text     =   _invoices.ManufacturerINN;
                ManufacturerAddressBox.Text =   _invoices.ManufacturerAddress;
            }
            _oldRequestedProductQuantity = int.Parse(QuantityBox.Text);
        }
        void LoadComboBoxData()
        {
            using (var context = new AppDbContext())
            {
                ClientsList = context.Clients.ToList();
                ProductsList = context.Products.ToList();
                ManufacturersList = context.Manufacturers.ToList();
            }
        }

        void InitializeBindings()
        {
            //Клиенты
            ClientNameBox.ItemsSource = ClientsList;
            ClientNameBox.DisplayMemberPath = "Name";

            // Продукты
            ProductNameBox.ItemsSource = ProductsList;
            ProductNameBox.DisplayMemberPath = "Name";

            // Производители
            ManufacturerNameBox.ItemsSource = ManufacturersList;
            ManufacturerNameBox.DisplayMemberPath = "Name";
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            if (ReduceMethod() != true)
                return;
            try
            {
                using (var context = new AppDbContext())
                {
                    var contextInvoice = context.Invoices.FirstOrDefault(m => m.Id == _elementId);

                    contextInvoice.ClientId             =   int.Parse(ClientIdBox.Text);
                    contextInvoice.ClientName           =   ClientNameBox.Text;
                    contextInvoice.ClientINN            =   ClientInnBox.Text;
                    contextInvoice.ClientAddress        =   ClientAddressBox.Text;
                    contextInvoice.ProductId            =   int.Parse(ProductIdBox.Text);
                    contextInvoice.ProductName          =   ProductNameBox.Text;
                    contextInvoice.Quantity             =   int.Parse(QuantityBox.Text);
                    contextInvoice.Date                 =   DateOnly.Parse(DateBox.Text);
                    contextInvoice.TotalPrice           =   int.Parse(PriceBox.Text);
                    contextInvoice.ManufacturerId       =   int.Parse(ManufacturerIdBox.Text);
                    contextInvoice.ManufacturerName     =   ManufacturerNameBox.Text;
                    contextInvoice.ManufacturerINN      =   ManufacturerInnBox.Text;
                    contextInvoice.ManufacturerAddress  =   ManufacturerAddressBox.Text;

                    if (!CheckParameters(contextInvoice))
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

        bool ReduceMethod()
        {
            bool isGood = false;
            try
            {
                int oldRequestedProductQuantity = _oldRequestedProductQuantity;

                int productId = int.Parse(ProductIdBox.Text);
                int requestedQuantity = int.Parse(QuantityBox.Text) - oldRequestedProductQuantity;

                ReduceProductMethod method = new();

                if (method.Reduce(productId, requestedQuantity) == true)
                    isGood = true;
                else
                    isGood = false;
            }
            catch { }
            return isGood;
        }

        private void CancelBut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        static bool CheckParameters(Invoices newElement)
        {
            bool isGood;

            if
            (
                newElement.ClientName.IsNullOrEmpty()           ||
                newElement.ClientINN.IsNullOrEmpty()            ||
                newElement.ClientAddress.IsNullOrEmpty()        ||
                newElement.ProductName.IsNullOrEmpty()          ||
                newElement.ManufacturerName.IsNullOrEmpty()     ||
                newElement.ManufacturerINN.IsNullOrEmpty()      ||
                newElement.ManufacturerAddress.IsNullOrEmpty()  ||

                newElement.ClientId         ==      null        ||
                newElement.ProductId        ==      null        ||
                newElement.ManufacturerId   ==      null        ||
                newElement.TotalPrice       ==      null        ||
                newElement.Date             ==      null        ||

                newElement.ClientINN.Length != 10               &&
                newElement.ClientINN.Length != 12               ||

                newElement.ManufacturerINN.Length != 10         &&
                newElement.ManufacturerINN.Length != 12
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }

        static int PriceCalculation(int quantity, int price)
        {
            int result;

            result = quantity * price;

            return result;
        }

        private void QuantityBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                int quantity = int.Parse(QuantityBox.Text);

                int prodId = int.Parse(ProductIdBox.Text);

                using (var context = new AppDbContext())
                {
                    _price = context.Products.Where(p => p.Id == prodId)
                        .Select(p => p.Price)
                        .FirstOrDefault();
                }


                int price = _price;

                int result = PriceCalculation(quantity, price);

                PriceBox.Text = Convert.ToString(result);
            }
            catch { }
        }

        private void QuantityBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
