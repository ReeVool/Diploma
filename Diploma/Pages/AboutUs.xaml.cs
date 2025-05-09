using System.Windows;
using System.Windows.Controls;

namespace Diploma.Pages
{
    public partial class AboutUs : Page
    {
        public AboutUs()
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
    }
}
