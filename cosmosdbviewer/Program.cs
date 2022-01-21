using cosmosdbviewer.Services;
using Azure.Identity;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
//var vaultUri = builder.Configuration["VaultUri"];
Uri vaultUri = new(builder.Configuration["VaultUri"]);
builder.Configuration.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(builder.Configuration).GetAwaiter().GetResult());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();

static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfiguration configuration)
{
    string databaseId = configuration["databaseid"];
    string containerId = configuration["containerid"];
    string partitionKey = configuration["partitionkey"];
    string connStr = configuration["CosmosDB1ConnectionString"];
    CosmosClient client = new(connStr, new CosmosClientOptions());
    CosmosDbService cosmosDbService = new(client, databaseId, containerId);
    DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseId);
    await database.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(containerId, partitionKey), ThroughputProperties.CreateManualThroughput(1000));
    return cosmosDbService;
}