namespace LibrairieStock.Models
{
    using System.Collections.Generic;

    public class StockObject
    {
        public List<Category> Category { get; set; }
        public List<Catalog> Catalog { get; set; }
    }
}
