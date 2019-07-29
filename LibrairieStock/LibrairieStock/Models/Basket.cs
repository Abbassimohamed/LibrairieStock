namespace LibrairieStock.Models
{
    public class Basket
    {
        public string Name;
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool IsReduced { get; set; }
    }
}
