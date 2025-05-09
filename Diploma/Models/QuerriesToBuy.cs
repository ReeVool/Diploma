namespace Diploma.Models
{
    public class QuerriesToBuy
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int IdProduct { get; set; }
        public int Quantity { get; set; }
        public int IdPharmacy { get; set; }
        public bool IsDone { get; set; }
    }
}
