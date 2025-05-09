using Diploma.Database;
using Diploma.Database.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Diploma.ViewModels.ProductsVM
{
    public partial class ProdutsAddWindow : Window
    {
        readonly ShowMessages showMessages = new();

        public ProdutsAddWindow()
        {
            InitializeComponent();
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newElement = new Products
                {
                    Name                    =   NameBox.Text,
                    Article                 =   int.Parse(ArticleBox.Text),
                    Type                    =   TypeBox.Text,
                    Price                   =   int.Parse(PriceBox.Text),
                    Quantity                =   int.Parse(TotalQuantityBox.Text),
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

        static bool CheckParameters(Products newElement)
        {
            bool isGood;

            if
            (
                newElement.Name.IsNullOrEmpty() ||
                newElement.Type.IsNullOrEmpty() ||

                newElement.Article             == null ||
                newElement.Price               == null ||
                newElement.Quantity            == null 
            )
                isGood = false;

            else
                isGood = true;

            return isGood;
        }

        private void EmailBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
