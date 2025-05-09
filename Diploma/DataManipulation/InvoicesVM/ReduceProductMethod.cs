using Diploma.Database;
namespace Diploma.DataManipulation.InvoicesVM
{
    public class ReduceProductMethod
    {
        public bool Reduce(int productId, int requestedQuantity)
        {
            bool isGood = false;
            try
            {
                using (var context = new AppDbContext())
                {
                    var contextProduct = context.Products.FirstOrDefault(p => p.Id == productId);

                    int actualProductQuantity = contextProduct.Quantity;

                    if (requestedQuantity > actualProductQuantity)
                    {
                        ShowMessages showMessages = new();
                        showMessages.ShowErrorMessage("Запрашиваемое количество продукции больше имеющейся на складе.");
                        isGood = false;
                    }
                    else
                    {
                        contextProduct.Quantity -= requestedQuantity;
                        context.SaveChanges();
                        isGood = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new ShowMessages().ShowError(ex);
            }
            return isGood;
        }
    }
}
