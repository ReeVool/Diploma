using Diploma.Database.Models;
using Diploma.Database;
using System.Windows;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using Diploma.DataManipulation.InvoicesVM;

namespace Diploma.Views
{
    public partial class InvoiceAddWindow : Window
    {
        ShowMessages showMessages = new();
        private int _price;

        public List<Clients> ClientsList { get; set; }
        public List<Products> ProductsList { get; set; }
        public List<Manufacturers> ManufacturersList { get; set; }

        public InvoiceAddWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
            InitializeBindings();
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
                var newElement = new Invoices
                {
                    ClientName          =   ClientNameBox.Text,
                    ClientId            =   int.Parse(ClientIdBox.Text),
                    ClientINN           =   ClientInnBox.Text,
                    ClientAddress       =   ClientAddressBox.Text,
                    ProductName         =   ProductNameBox.Text,
                    ProductId           =   int.Parse(ProductIdBox.Text),
                    Quantity            =   int.Parse(QuantityBox.Text),
                    ManufacturerName    =   ManufacturerNameBox.Text,
                    ManufacturerId      =   int.Parse(ManufacturerIdBox.Text),
                    ManufacturerINN     =   ManufacturerInnBox.Text,
                    ManufacturerAddress =   ManufacturerAddressBox.Text,
                    Date                =   DateOnly.Parse(DateBox.Text),
                    TotalPrice          =   int.Parse(PriceBox.Text),
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

        bool ReduceMethod()
        {
            bool isGood = false;
            try
            {
                int productId = int.Parse(ProductIdBox.Text);
                int requestedQuantity = int.Parse(QuantityBox.Text);

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

                newElement.ClientId             ==  null        ||
                newElement.ProductId            ==  null        ||
                newElement.ManufacturerId       ==  null        ||
                newElement.TotalPrice           ==  null        ||
                newElement.Date                 == null         ||

                newElement.ClientINN.Length     != 10           &&
                newElement.ClientINN.Length     != 12           ||

                newElement.ManufacturerINN.Length     != 10     &&
                newElement.ManufacturerINN.Length     != 12
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

        private void QuantityBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
