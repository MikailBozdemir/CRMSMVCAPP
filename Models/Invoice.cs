namespace CRMSMVCAPP.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; } 
        public int UserId { get; set; } 
        public int ProductId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalPrice { get; set; }
        public string Adress { get; set; }
        public string UserName { get; set; }
        public int Amount { get; set; }
        public string ProductName { get; set; }
        public double TotalPriceByProduct { get; set; }







    }
}
