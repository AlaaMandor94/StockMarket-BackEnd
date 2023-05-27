namespace Market.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int StockId { get; set; }
        public string StockName { get; set; } = string.Empty;
    }


}
