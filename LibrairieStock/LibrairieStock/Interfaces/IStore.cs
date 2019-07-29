namespace LibrairieStock.Intarfeces
{
    using LibrairieStock.CustomExceptions;
    using LibrairieStock.Models;
    using System.Collections.Generic;

    public interface IStore
    {
        void Import(string catalogAsJson);
        int Quantity(string name);
        double Buy(params string[] basketByNames);

        NotEnoughInventoryException ValidateBasket(List<Basket> basketToValidate, IEnumerable<Catalog> catalogData);
    }
}
