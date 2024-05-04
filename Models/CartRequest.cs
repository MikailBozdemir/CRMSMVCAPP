namespace CRMSMVCAPP.Models
{
    public class CartRequest
    {
        public int ProductId { get; set; }
        public int CutomerId { get; set; }
        public int Amount { get; set; }
    }
}
