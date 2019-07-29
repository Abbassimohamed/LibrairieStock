namespace LibrairieStock.Intarfeces
{
    public interface IStoreRepository
    {
        void ImportData(string catalogAsJson);
        string GetJsonData();

    }
}
