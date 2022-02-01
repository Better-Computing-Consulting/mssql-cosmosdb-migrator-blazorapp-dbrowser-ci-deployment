using Microsoft.Azure.Cosmos;
using dbmigrate;
if (args.Length == 4)
{
    try
    {
        using CosmosClient client = new("" + args[0] + "", new CosmosClientOptions());
        string databaseId = args[1];
        string containerId = args[2];
        string partitionKey = args[3];
        Database? database = null;
        Container? container = null;
        Console.WriteLine("Creating database if it does not already exists");
        database = await client.CreateDatabaseIfNotExistsAsync(databaseId);
        Console.WriteLine("Deleting existing Container");
        using (await database.GetContainer(containerId).DeleteContainerStreamAsync()){ }
        Console.WriteLine("Creating new Container");
        container = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(containerId, partitionKey), ThroughputProperties.CreateManualThroughput(1000));
        List<StockItem>? allitems = GetStockItems();
        int c = 0;
        foreach (StockItem anitem in allitems)
        {
            ItemResponse<StockItem> response = await container.CreateItemAsync(anitem);
            StockItem resultItem = response;
            c++;
            if (c % 10 == 0) { Console.WriteLine(resultItem.StockItemID + " "); } else { Console.Write(resultItem.StockItemID + " "); }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
else
{
    Console.WriteLine("This app takes 4 arguments: ");
    Console.WriteLine("  1) the Connection String to the destination Cosmos DB Account,");
    Console.WriteLine("  2) the Database Id ");
    Console.WriteLine("  3) the Container Id, and");
    Console.WriteLine("  4) the Partition Key.");
}
Console.WriteLine(Environment.NewLine + "done creating db, container, and importing items");
static List<StockItem> GetStockItems()
{
    var items = new List<StockItem>();
    try
    {
        List<StockItemHolding> stockItemHoldings = GetStockItemHoldings();
        List<Color> colors = GetColors();
        List<PackageType> packageTypes = GetPackageTypes();
        List<Supplier> suppliers = GetSuppliers();

        using StreamReader sr = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "StockItems.txt"));
        while (sr.Peek() >= 0)
        {
            string[] vals = sr.ReadLine().Split('\t');
            int i = 0;
            StockItem itm = new()
            {
                Id = Guid.NewGuid().ToString(),
                StockItemID = Int32.Parse(vals[i++].Trim()),
                StockItemName = vals[i++].Trim(),
                Supplier = GetSupplier(Int32.Parse(vals[i++].Trim()), suppliers),
                Color = GetColor(ToNullableInt(vals[i++].Trim()), colors),
                UnitPackage = GetPakageType(Int32.Parse(vals[i++].Trim()), packageTypes),
                OuterPackage = GetPakageType(Int32.Parse(vals[i++].Trim()), packageTypes),
                Brand = vals[i++].Trim(),
                Size = vals[i++].Trim(),
                LeadTimeDays = Int32.Parse(vals[i++].Trim()),
                QuantityPerOuter = Int32.Parse(vals[i++].Trim()),
                IsChillerStock = Convert.ToBoolean(Int32.Parse(vals[i++].Trim())),
                StockHolding = GetStockItemHolding(Int32.Parse(vals[0].Trim()), stockItemHoldings),
                Barcode = vals[i++].Trim(),
                TaxRate = decimal.Parse(vals[i++].Trim()),
                UnitPrice = decimal.Parse(vals[i++].Trim()),
                RecommendedRetailPrice = decimal.Parse(vals[i++].Trim()),
                TypicalWeightPerUnit = decimal.Parse(vals[i++].Trim()),
                MarketingComments = vals[i++].Trim(),
                InternalComments = vals[i++].Trim(),
                Photo = vals[i++].Trim(),
                CustomFields = vals[i++].Trim(),
                Tags = vals[i++].Trim(),
                SearchDetails = vals[i++].Trim(),
                LastEditedBy = Int32.Parse(vals[i++].Trim()),
                ValidFrom = DateTime.Parse(vals[i++].Trim()),
                ValidTo = DateTime.Parse(vals[i++].Trim()),
            };
            items.Add(itm);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("StockItems file could not be read:");
        Console.WriteLine(ex.Message);
    }
    return items;
}
static List<StockItemHolding> GetStockItemHoldings()
{
    var stockItemHolding = new List<StockItemHolding>();
    try
    {
        using StreamReader sr = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "StockItemHoldings.txt"));
        {
            while (sr.Peek() >= 0)
            {
                int i = 0;
                string? line = sr.ReadLine();
                string[] vals = line.Split('\t');
                StockItemHolding s = new()
                {
                    StockItemID = Int32.Parse(vals[i++].Trim()),
                    QuantityOnHand = Int32.Parse(vals[i++].Trim()),
                    BinLocation = vals[i++].Trim(),
                    LastStocktakeQuantity = Int32.Parse(vals[i++].Trim()),
                    LastCostPrice = decimal.Parse(vals[i++].Trim()),
                    ReorderLevel = Int32.Parse(vals[i++].Trim()),
                    TargetStockLevel = Int32.Parse(vals[i++].Trim()),
                    LastEditedBy = Int32.Parse(vals[i++].Trim()),
                    LastEditedWhen = DateTime.Parse(vals[i++].Trim())
                };
                stockItemHolding.Add(s);
            }
        }
        return stockItemHolding;
    }
    catch (Exception ex)
    {
        Console.WriteLine("StockItemHoldings file could not be read:");
        Console.WriteLine(ex.Message);
    }
    return stockItemHolding;
}
static StockItemHolding GetStockItemHolding(int StockItemID, List<StockItemHolding> stockItemHoldings)
{
    StockItemHolding? stockItemHolding = null;
    foreach (StockItemHolding s in stockItemHoldings)
    {
        if (s.StockItemID == StockItemID) { return s; }
    }
    return stockItemHolding;
}
static List<PackageType> GetPackageTypes()
{
    var packageTypes = new List<PackageType>();
    try
    {
        using StreamReader sr = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "PackageTypes.txt"));
        {
            while (sr.Peek() >= 0)
            {
                int i = 0;
                string? line = sr.ReadLine();
                string[] vals = line.Split('\t');
                PackageType p = new()
                {
                    PackageTypeID = Int32.Parse(vals[i++].Trim()),
                    PackageTypeName = vals[i++].Trim(),
                    LastEditedBy = Int32.Parse(vals[i++].Trim()),
                    ValidFrom = DateTime.Parse(vals[i++].Trim()),
                    ValidTo = DateTime.Parse(vals[i++].Trim())
                };
                packageTypes.Add(p);
            }
        }
        return packageTypes;
    }
    catch (Exception ex)
    {
        Console.WriteLine("PackageTypes file could not be read:");
        Console.WriteLine(ex.Message);
    }
    return packageTypes;
}
static PackageType GetPakageType(int PackageTypeID, List<PackageType> packageTypes)
{
    PackageType? pakageType = null;
    foreach (PackageType p in packageTypes)
    {
        if (p.PackageTypeID == PackageTypeID) { return p; }
    }
    return pakageType;
}
static List<Supplier> GetSuppliers()
{
    var suppliers = new List<Supplier>();
    try
    {
        using StreamReader sr = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Suppliers.txt"));
        {
            while (sr.Peek() >= 0)
            {
                int i = 0;
                string? line = sr.ReadLine();
                string[]? vals = line.Split('\t');

                Supplier s = new()
                {
                    SupplierID = Int32.Parse(vals[i++].Trim()),
                    SupplierName = vals[i++].Trim(),
                    SupplierCategoryID = Int32.Parse(vals[i++].Trim()),
                    PrimaryContactPersonID = Int32.Parse(vals[i++].Trim()),
                    AlternateContactPersonID = Int32.Parse(vals[i++].Trim()),
                    DeliveryMethodID = ToNullableInt(vals[i++].Trim()),
                    DeliveryCityID = Int32.Parse(vals[i++].Trim()),
                    PostalCityID = Int32.Parse(vals[i++].Trim()),
                    SupplierReference = vals[i++].Trim(),
                    BankAccountName = vals[i++].Trim(),
                    BankAccountBranch = vals[i++].Trim(),
                    BankAccountCode = vals[i++].Trim(),
                    BankAccountNumber = vals[i++].Trim(),
                    BankInternationalCode = vals[i++].Trim(),
                    PaymentDays = Int32.Parse(vals[i++].Trim()),
                    InternalComments = vals[i++].Trim(),
                    PhoneNumber = vals[i++].Trim(),
                    FaxNumber = vals[i++].Trim(),
                    WebsiteURL = vals[i++].Trim(),
                    DeliveryAddressLine1 = vals[i++].Trim(),
                    DeliveryAddressLine2 = vals[i++].Trim(),
                    DeliveryPostalCode = vals[i++].Trim(),
                    DeliveryLocation = vals[i++].Trim(),
                    PostalAddressLine1 = vals[i++].Trim(),
                    PostalAddressLine2 = vals[i++].Trim(),
                    PostalPostalCode = vals[i++].Trim(),
                    LastEditedBy = Int32.Parse(vals[i++].Trim()),
                    ValidFrom = DateTime.Parse(vals[i++].Trim()),
                    ValidTo = DateTime.Parse(vals[i++].Trim())
                };
                suppliers.Add(s);
            }
            return suppliers;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Suppliers file could not be read:");
        Console.WriteLine(ex.Message);
    }
    return suppliers;
}
static Supplier GetSupplier(int SupplierID, List<Supplier> suppliers)
{
    Supplier? supplier = null;
    foreach (Supplier s in suppliers)
    {
        if (s.SupplierID == SupplierID) { return s; }
    }
    return supplier;
}
static List<Color> GetColors()
{
    var colors = new List<Color>();
    try
    {
        using StreamReader sr = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Colors.txt"));
        {
            while (sr.Peek() >= 0)
            {
                int i = 0;
                string? line = sr.ReadLine();
                string[] vals = line.Split('\t');
                Color c = new()
                {
                    ColorID = Int32.Parse(vals[i++].Trim()),
                    ColorName = vals[i++].Trim(),
                    LastEditedBy = Int32.Parse(vals[i++].Trim()),
                    ValidFrom = DateTime.Parse(vals[i++].Trim()),
                    ValidTo = DateTime.Parse(vals[i++].Trim())
                };
                colors.Add(c);
            }
        }
        return colors;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Colors file could not be read:");
        Console.WriteLine(ex.Message);
    }
    return colors;
}
static Color GetColor(int ColorID, List<Color> colors)
{
    Color? color = null;
    foreach (Color c in colors)
    {
        if (c.ColorID == ColorID) { return c; }
    }
    return color;
}
static int ToNullableInt(string s)
{
    if (int.TryParse(s, out int i)) return i;
    return 0;
}
