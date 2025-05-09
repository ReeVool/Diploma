namespace Diploma.Database.Models
{
    public class Invoices
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientINN { get; set; }
        public string ClientAddress { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateOnly Date { get; set; }
        public int TotalPrice { get; set; }
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerINN { get; set; }
        public string? ManufacturerAddress { get; set; }
    }
}
