using Newtonsoft.Json;

namespace dbmigrate
{
    internal class StockItem
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("StockItemID")]
        public int StockItemID { get; set; }
        [JsonProperty("StockItemName")]
        public string? StockItemName { get; set; }
        [JsonProperty("Supplier")]
        public Supplier? Supplier { get; set; }
        [JsonProperty("Color")]
        public Color? Color { get; set; }
        [JsonProperty("UnitPackage")]
        public PackageType? UnitPackage { get; set; }
        [JsonProperty("OuterPackage")]
        public PackageType? OuterPackage { get; set; }
        [JsonProperty("Brand")]
        public string? Brand { get; set; }
        [JsonProperty("Size")]
        public string? Size { get; set; }
        [JsonProperty("LeadTimeDays")]
        public int LeadTimeDays { get; set; }
        [JsonProperty("QuantityPerOuter")]
        public int QuantityPerOuter { get; set; }
        [JsonProperty("IsChillerStock")]
        public bool IsChillerStock { get; set; }
        [JsonProperty("StockHolding")]
        public StockItemHolding? StockHolding { get; set; }
        [JsonProperty("Barcode")]
        public string? Barcode { get; set; }
        [JsonProperty("TaxRate")]
        public decimal TaxRate { get; set; }
        [JsonProperty("UnitPrice")]
        public decimal UnitPrice { get; set; }
        [JsonProperty("RecommendedRetailPrice")]
        public decimal RecommendedRetailPrice { get; set; }
        [JsonProperty("TypicalWeightPerUnit")]
        public decimal TypicalWeightPerUnit { get; set; }
        [JsonProperty("MarketingComments")]
        public string? MarketingComments { get; set; }
        [JsonProperty("InternalComments")]
        public string? InternalComments { get; set; }
        [JsonProperty("Photo")]
        public string? Photo { get; set; }
        [JsonProperty("CustomFields")]
        public string? CustomFields { get; set; }
        [JsonProperty("Tags")]
        public string? Tags { get; set; }
        [JsonProperty("SearchDetails")]
        public string? SearchDetails { get; set; }
        [JsonProperty("LastEditedBy")]
        public int LastEditedBy { get; set; }
        [JsonProperty("ValidFrom")]
        public DateTime ValidFrom { get; set; }
        [JsonProperty("ValidTo")]
        public DateTime ValidTo { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    internal class StockItemHolding
    {
        public int StockItemID { get; set; }
        public int QuantityOnHand { get; set; }
        public string? BinLocation { get; set; }
        public int LastStocktakeQuantity { get; set; }
        public decimal LastCostPrice { get; set; }
        public int ReorderLevel { get; set; }
        public int TargetStockLevel { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime LastEditedWhen { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    internal class Color
    {
        public int ColorID { get; set; }
        public string? ColorName { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Color p = (Color)obj;
                return (ColorName == p.ColorName);
            }
        }
        public override int GetHashCode() { return ColorID.GetHashCode(); }
    }
    internal class Supplier
    {
        public int SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public int SupplierCategoryID { get; set; }
        public int PrimaryContactPersonID { get; set; }
        public int AlternateContactPersonID { get; set; }
        public int DeliveryMethodID { get; set; }
        public int DeliveryCityID { get; set; }
        public int PostalCityID { get; set; }
        public string? SupplierReference { get; set; }
        public string? BankAccountName { get; set; }
        public string? BankAccountBranch { get; set; }
        public string? BankAccountCode { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankInternationalCode { get; set; }
        public int PaymentDays { get; set; }
        public string? InternalComments { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? WebsiteURL { get; set; }
        public string? DeliveryAddressLine1 { get; set; }
        public string? DeliveryAddressLine2 { get; set; }
        public string? DeliveryPostalCode { get; set; }
        public string? DeliveryLocation { get; set; }
        public string? PostalAddressLine1 { get; set; }
        public string? PostalAddressLine2 { get; set; }
        public string? PostalPostalCode { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    internal class PackageType
    {
        public int PackageTypeID { get; set; }
        public string? PackageTypeName { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
