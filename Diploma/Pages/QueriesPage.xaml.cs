using Diploma.Models;
using Diploma.ViewModels.QueriesVM;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diploma.Database
{
    public partial class QueriesPage : Page
    {
        ShowMessages showMessages = new();
        public QueriesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }


        void LoadData()
        {
            using (var context = new AppDbContext())
            {
                DataTable.ItemsSource = context.QuerriesToBuy.ToList();
            }

            foreach (var column in DataTable.Columns)
            {
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            DataTable.Focus();
        }

        // кнопка "поиск"
        [Obsolete]
        private void SearchBut_Click(object sender, RoutedEventArgs e)
        {
            SearchInfo();
        }
        void SearchInfo()
        {
            string searchedInfo = SearchPlace.Text;

            if (searchedInfo.IsNullOrEmpty())
                return;

            try
            {
                QuerriesSearch search = new();
                using var db = new AppDbContext();
                var results = search.Search(searchedInfo, db).ToList();

                DataTable.ItemsSource = results;
            }
            catch (Exception ex)
            {
                showMessages.ShowError(ex);
            }
        }

        // кнопка "отменить поиск"
        private void CancelSearchBut_Click(object sender, RoutedEventArgs e)
        {
            SearchPlace.Text = "";
            LoadData();
        }

        // кнопка "добавить"
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddInformation();
        }
        void AddInformation()
        {
            QueriesAddWindow window = new();
            window.ShowDialog();
            LoadData();
        }

        // кнопка "изменить"
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditInformation();
        }
        void EditInformation()
        {
            var selItem = DataTable.SelectedItem as QuerriesToBuy;

            if (selItem == null)
                return;

            QueriesEditWindow window = new(selItem.Id);
            window.ShowDialog();
            LoadData();
        }

        // кнопка "удалить"
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteInformation();
        }
        void DeleteInformation()
        {
            var selItem = DataTable.SelectedItem as QuerriesToBuy;

            if (selItem == null)
                return;

            MessageBoxResult res = showMessages.ShowQuestion("Вы уверены, что хотите удалить данные?");

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new AppDbContext())
                    {
                        context.QuerriesToBuy.Remove(selItem);
                        context.SaveChanges();
                    }
                    LoadData();
                }
                catch
                {
                    new ShowMessages().ShowErrorMessage("Удаление не возможно, так как выбранное поле связано с другими записями базы данных.");
                }
            }
        }


        #region Горячие клавиши
        private void Page_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AddInformation();
            }
            else if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                DeleteInformation();
            }
            else if (e.Key == Key.E && Keyboard.Modifiers == ModifierKeys.Control)
            {
                EditInformation();
            }
            else if (e.Key == Key.R && Keyboard.Modifiers == ModifierKeys.Control || e.Key == Key.F5)
            {
                SearchPlace.Text = "";
                LoadData();
            }
            else if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SearchPlace.Focus();
            }
            else if (SearchPlace.IsFocused == true && e.Key == Key.Enter)
            {
                SearchInfo();
            }
            else if (SearchPlace.IsFocused == true && e.Key == Key.Escape)
            {
                SearchPlace.Text = "";
                LoadData();
            }
        }
        #endregion

    }
}
