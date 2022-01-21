namespace cosmosdbviewer.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using cosmosdbviewer.Data;

    public interface ICosmosDbService
    {
        Task<List<StockItem>> GetItemsAsync(string query);
        Task<StockItem> GetItemAsync(string id);
        Task<StockItem> AddItemAsync(StockItem item);
        Task UpdateItemAsync(string id, StockItem item);
        Task DeleteItemAsync(string id);
    }
}