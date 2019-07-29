namespace LibrairieStock.Services
{
    using LibrairieStock.CustomExceptions;
    using LibrairieStock.Intarfeces;
    using LibrairieStock.Models;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StoreService : IStore
    {
        private readonly IStoreRepository storeRepository;
        private readonly IMemoryCache memoryCache;

        public StoreService(IStoreRepository storeRepository, IMemoryCache memoryCache)
        {
            this.storeRepository = storeRepository;
            this.memoryCache = memoryCache;
        }

        public NotEnoughInventoryException ValidateBasket(List<Basket> basketToValidate, IEnumerable<Catalog> catalogData)
        {
            NotEnoughInventoryException exception = new NotEnoughInventoryException();
            foreach (var product in basketToValidate)
            {
                var stock = catalogData.Where(n => n.Name == product.Name).First().Quantity;
                if (product.Quantity > stock)
                {
                    NameQuantity missing = new NameQuantity(); ;
                    missing.createException(product.Name, product.Quantity);
                    exception.AddException(missing);
                }
            }
            if (exception.Missing != null)
                throw exception;
            else
                return null;
        }

        public double Buy(params string[] basketByNames)
        {
            var cacheEntry = this.memoryCache.Get<StockObject>("Stock");
            if (cacheEntry == null)
            {
                var jsonAsString = this.storeRepository.GetJsonData();
                this.Import(jsonAsString);
                cacheEntry = this.memoryCache.Get<StockObject>("Stock");
            }
            double sommePannier = 0;
            if (basketByNames.Count() == 1)
            {
                sommePannier = cacheEntry.Catalog.Where(c => c.Name == basketByNames[0]).FirstOrDefault().Price;

            }
            else
            {
                var catalogData = cacheEntry.Catalog.Where(v => basketByNames.Contains(v.Name));
                var groupedProduct = catalogData.GroupBy(c => c.Category)
                                                .Select(grp =>
                                                    new GroupedProduct
                                                    {
                                                        Category = grp.Key,
                                                        Occurence = grp.Count()
                                                    }
                                                );

                if (groupedProduct.Count() != 0)
                {
                    var groupedBasket = basketByNames
                            .GroupBy(s => s)
                            .Select(g => new { name = g.Key, Count = g.Count() });

                    List<Basket> baskets = new List<Basket>();
                    List<Basket> reducedBaskets = new List<Basket>();
                    foreach (var c in groupedBasket)
                    {
                        Basket bas = new Basket();
                        bas.Name = c.name;
                        bas.Quantity = c.Count;
                        bas.Price = catalogData.Where(n => n.Name == c.name).First().Price;
                        var categoryOdProduct = catalogData.Where(n => n.Name == c.name).First().Category;

                        bas.IsReduced = groupedProduct.Where(v => v.Category == categoryOdProduct).First().Occurence > 1 ? true : false;
                        baskets.Add(bas);
                    }
                    double pourcentRemise = 0.9;
                    foreach (var product in baskets)
                    {


                        pourcentRemise = (product.Price * 0.1);
                        if (product.IsReduced && product.Quantity == 1)
                        {
                            sommePannier += product.Price - pourcentRemise;
                        }
                        else if (product.IsReduced && product.Quantity > 1)
                        {
                            double firstArticleWithRemise = product.Price - pourcentRemise;
                            double RestArticleWithoutRemise = product.Price * (product.Quantity - 1);
                            double reductionMultiArticle = firstArticleWithRemise + RestArticleWithoutRemise;

                            sommePannier += reductionMultiArticle;
                        }
                        else
                        {
                            sommePannier += product.Price * product.Quantity;
                        }

                    }

                    ValidateBasket(baskets, catalogData);
                }
            }

            return Math.Round(sommePannier, 3);
        }

        public void Import(string catalogAsJson)
        {
            this.storeRepository.ImportData(catalogAsJson);
        }

        public int Quantity(string name)
        {
            var jsonAsString = this.storeRepository.GetJsonData();
            int quantite = -1;
            this.Import(jsonAsString);
            var cacheEntry = this.memoryCache.Get<StockObject>("Stock");
            if (cacheEntry != null)
                quantite = cacheEntry.Catalog.Where(c => c.Name == name).FirstOrDefault().Quantity;
            return quantite;
        }
    }
}
