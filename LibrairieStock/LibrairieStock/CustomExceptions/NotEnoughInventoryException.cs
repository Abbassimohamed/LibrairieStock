namespace LibrairieStock.CustomExceptions
{
    using LibrairieStock.Intarfeces;
    using System;
    using System.Collections.Generic;

    public class NotEnoughInventoryException : Exception
    {
        private List<INameQuantity> missing;
        public IEnumerable<INameQuantity> Missing
        {
            get
            {
                return this.missing;
            }
        }

        public void AddException(INameQuantity nameQuantity)
        {
            if (missing == null)
                missing = new List<INameQuantity>();
            missing.Add(nameQuantity);
        }
    }
}
