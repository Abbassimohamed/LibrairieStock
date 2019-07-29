namespace LibrairieStock.Repositories
{
    using LibrairieStock.Intarfeces;
    using LibrairieStock.Models;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
    using System.IO;

    public class StoreRepository : IStoreRepository
    {
        private IMemoryCache cache;
        public StoreRepository(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void ImportData(string catalogAsJson)
        {
            var result = JsonConvert.DeserializeObject<StockObject>(catalogAsJson);
            cache.Set("Stock", result);
        }

        public string GetJsonData()
        {
            string pathFile = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string pathJson = pathFile + @"\LibrairieStock\LibrairieStock\Data\Stock.json";
            using (StreamReader r = new StreamReader(pathJson))
            {
                string json = r.ReadToEnd();
                return json;
            }
        }
    }
}
