
namespace cosmosdbviewer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using cosmosdbviewer.Data;
    using Microsoft.Azure.Cosmos;
    using Newtonsoft.Json;

    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;
        private static readonly JsonSerializer Serializer = new();

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<StockItem> AddItemAsync(StockItem item)
        {
            ItemResponse<StockItem> response = await this._container.CreateItemAsync<StockItem>(item, new PartitionKey(item.StockItemID));
            return response;
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<StockItem>(id, new PartitionKey(id));
        }

        public async Task<StockItem> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<StockItem> response = await this._container.ReadItemAsync<StockItem>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<List<StockItem>> GetItemsAsync(string queryString)
        {
            FeedIterator streamResultSet = this._container.GetItemQueryStreamIterator(new QueryDefinition(queryString));
            List<StockItem> results = new();
            while (streamResultSet.HasMoreResults)
            {
                ResponseMessage responseMessage = await streamResultSet.ReadNextAsync();
                if (responseMessage.IsSuccessStatusCode)
                {
                    dynamic streamResponse = FromStream<dynamic>(responseMessage.Content);
                    List<StockItem> StockItems = streamResponse.Documents.ToObject<List<StockItem>>();
                    results.AddRange(StockItems);
                }
            }
            return results;
        }

        public async Task UpdateItemAsync(string id, StockItem item)
        {
            await this._container.UpsertItemAsync<StockItem>(item, new PartitionKey(id));
        }

        private static T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)stream;
                }
                StreamReader sr = new(stream);
                JsonTextReader jsonTextReader = new(sr);
                return CosmosDbService.Serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
