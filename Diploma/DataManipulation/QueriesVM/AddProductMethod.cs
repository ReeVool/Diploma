using Diploma.Database;

namespace Diploma.DataManipulation.QueriesVM
{
    public class AddProductMethod
    {
        ShowMessages showMessages = new();
        public void AddProductQuantity(bool isChecked, int productQuantity, int productId)
        {
            if (isChecked == true)
            {
                try
                {
                    using (var context = new AppDbContext())
                    {
                        var contextProduct = context.Products.FirstOrDefault(p => p.Id == productId);

                        contextProduct.Quantity += productQuantity;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    showMessages.ShowError(ex);
                }
            }
        }
    }
}
