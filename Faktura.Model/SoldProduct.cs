namespace Faktura.Model
{
    public class SoldProduct
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public Invoice Invoice { get; set; }
        public int InvoiceID { get; set; }
        public Product Product { get; set; }
        public int ProductID { get; set; }

    }
}
