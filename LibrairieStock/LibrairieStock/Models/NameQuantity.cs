namespace LibrairieStock.Models
{
    using LibrairieStock.Intarfeces;

    public class NameQuantity : INameQuantity
    {
        private string name;
        private int quantity;
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public NameQuantity createException(string name, int quantite)
        {
            NameQuantity nameQuantity = new NameQuantity();
            this.name = name;
            this.quantity = quantite;
            return this;
        }

        public int Quantity
        {
            get
            {
                return this.quantity;
            }
        }
    }
}
