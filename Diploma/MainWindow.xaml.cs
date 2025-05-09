using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Diploma.Authorization;
using Diploma.Database;
using Diploma.Pages;
using Diploma.Views;

namespace Diploma
{
    public partial class MainWindow : Window
    {
        private bool isPanelExpanded = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainPage page = new();
            PageContainer.Content = page;

            // Разворачивание панели
            Storyboard expandStoryboard = (Storyboard)FindResource("ExpandStoryboard");
            expandStoryboard.Begin();

            FocusGrid();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AuthManager.CurrentUser != null)
            {
                var result = new ShowMessages().ShowQuestion("Вы уверены, что хотите закрыть программу?");
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        void FocusGrid()
        {
            ContentContainer.Focus();
        }

        // кнопка "свернуть/развернуть боковую панель"
        private void TogglePanel_Click(object sender, RoutedEventArgs e)
        {
            Expand();
        }
        void Expand()
        {
            if (isPanelExpanded)
            {
                // Сворачивание панели
                Storyboard collapseStoryboard = (Storyboard)FindResource("CollapseStoryboard");
                collapseStoryboard.Begin();
            }
            else
            {
                // Разворачивание панели
                Storyboard expandStoryboard = (Storyboard)FindResource("ExpandStoryboard");
                expandStoryboard.Begin();
            }

            isPanelExpanded = !isPanelExpanded;
        }

        // кнопка "производители"
        private void ManufacturersBut_Click(object sender, RoutedEventArgs e)
        {
            ManufacturersPage page = new();
            PageContainer.Content = page;
        }

        // кнопка "продукция"
        private void ProductsBut_Click(object sender, RoutedEventArgs e)
        {
            ProductsPage page = new();
            PageContainer.Content = page;
        }

        // кнопка "клиенты"
        private void ClientsBut_Click(object sender, RoutedEventArgs e)
        {
            ClientsPage page = new();
            PageContainer.Content = page;
        }

        // кнопка "счет-фактуры"
        private void InvoiceBut_Click(object sender, RoutedEventArgs e)
        {
            InvoicesPage page = new();
            PageContainer.Content = page;
        }

        // кнопка "запросы на закупку"
        private void QuerriesToBuyBut_Click(object sender, RoutedEventArgs e)
        {
            QueriesPage page = new();
            PageContainer.Content = page;
        }

        // кнопка "аптеки"
        private void PharmacyBut_Click(object sender, RoutedEventArgs e)
        {
            PharmacyPage page = new();
            PageContainer.Content = page;
        }

        // кнопка "сотрудники"
        private void Workers_Click(object sender, RoutedEventArgs e)
        {
            WorkersPage page = new();
            PageContainer.Content = page;
        }

        // кнопка "личный кабинет"
        private void AccountSetingsBut_Click(object sender, RoutedEventArgs e)
        {
            PageContainer.Content = new AccountSettingsPage();
        }

        // кнопка "о нас"
        private void AboutUsBut_Click(object sender, RoutedEventArgs e)
        {
            AboutUs page = new();
            PageContainer.Content = page;
        }

        // кнопка "на главную"
        private void ToMainBut_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new();
            PageContainer.Content = page;
        }

        // кнопка "назад"
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PageContainer.GoBack();
            }
            catch { }
        }

        // кнопка "вперед"
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PageContainer.GoForward();
            }
            catch { }
        }

        #region Горячие клавиши
        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.B && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Expand();
            }
            else if (e.Key == Key.Left && Keyboard.Modifiers == ModifierKeys.Control ||
                     e.Key == Key.Up && Keyboard.Modifiers == ModifierKeys.Control)
            {
                try
                {
                    PageContainer.GoBack();
                }
                catch { }
            }
            else if (e.Key == Key.Right && Keyboard.Modifiers == ModifierKeys.Control ||
                     e.Key == Key.Down && Keyboard.Modifiers == ModifierKeys.Control)
            {
                try
                {
                    PageContainer.GoForward();
                }
                catch { }
            }
        }
        #endregion
    }
}