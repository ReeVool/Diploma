using Diploma.Database;
using Diploma.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Diploma.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            FocusGrid();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FocusGrid();
        }

        void FocusGrid()
        {
            ContentContainer.Focus();
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            AboutUs aboutUs = new();
            NavigationService.Navigate(aboutUs);
        }

        private void Manufacturers_Click(object sender, RoutedEventArgs e)
        {
            ManufacturersPage page = new();
            NavigationService.Navigate(page);
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            ProductsPage page = new();
            NavigationService.Navigate(page);
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            ClientsPage page = new();
            NavigationService.Navigate(page);
        }

        private void Invoices_Click(object sender, RoutedEventArgs e)
        {
            InvoicesPage page = new();
            NavigationService.Navigate(page);
        }

        private void Queries_Click(object sender, RoutedEventArgs e)
        {
            QueriesPage page = new();
            NavigationService.Navigate(page);
        }

        private void Pharmacy_Click(object sender, RoutedEventArgs e)
        {
            PharmacyPage page = new();
            NavigationService.Navigate(page);
        }

        private void Workers_Click(object sender, RoutedEventArgs e)
        {
            WorkersPage page = new();
            NavigationService.Navigate(page);
        }

        private void AccountSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountSettingsPage());
        }
    }
}
