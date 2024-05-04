namespace CRMSMVCAPP.Models
{
    public class Cart
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        
        public int Amount { get; set; }
        public string Status { get; set; }
        public DateTime AdditionDay { get; set; }
        public string ProductName { get; set; }
        public double TotalPrice { get; set; }
        List<Product> lis = new List<Product>();

    }
}
