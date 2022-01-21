using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace cosmosdbviewer.Data
{
    public class StockItem
    {
        [JsonProperty("id")]
        public string? id { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("StockItemID")]
        public int StockItemID { get; set; }
        [Required(ErrorMessage = "A unique StockItemName is required."), MinLength(4), MaxLength(50), ]
        [JsonProperty("StockItemName")]
        public string? StockItemName { get; set; }
        [Required]
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
        [Required]
        [JsonProperty("LeadTimeDays")]
        public int LeadTimeDays { get; set; }
        [Required]
        [JsonProperty("QuantityPerOuter")]
        public int QuantityPerOuter { get; set; }
        [Required]
        [JsonProperty("IsChillerStock")]
        public bool IsChillerStock { get; set; }
        [JsonProperty("StockHolding")]
        public StockItemHolding? StockHolding { get; set; }
        [JsonProperty("Barcode")]
        public string? Barcode { get; set; }
        [Required]
        [JsonProperty("TaxRate")]
        public decimal TaxRate { get; set; }
        [Required]
        [JsonProperty("UnitPrice")]
        public decimal UnitPrice { get; set; }
        [Required]
        [JsonProperty("RecommendedRetailPrice")]
        public decimal RecommendedRetailPrice { get; set; }
        [Required]
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
        [Required]
        [JsonProperty("SearchDetails")]
        public string? SearchDetails { get; set; }
        [JsonProperty("LastEditedBy")]
        public int LastEditedBy { get; set; } = 1;
        [JsonProperty("ValidFrom")]
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [JsonProperty("ValidTo")]
        public DateTime ValidTo { get; set; } = DateTime.MaxValue;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented);
        }
    }
    public class StockItemHolding
    {
        public int StockItemID { get; set; }
        public int QuantityOnHand { get; set; } = 1000;
        public string? BinLocation { get; set; } = "Default";
        public int LastStocktakeQuantity { get; set; } = 10;
        public decimal LastCostPrice { get; set; } = 1;
        public int ReorderLevel { get; set; } = 100;
        public int TargetStockLevel { get; set; } = 1000;
        public int LastEditedBy { get; set; } = 1;
        public DateTime LastEditedWhen { get; set; } = DateTime.Now;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class Color
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
    public class Supplier
    {
        public int SupplierID { get; set; }
        [Required(ErrorMessage = "A unique SupplierName is required."), MinLength(4), MaxLength(50),]
        public string? SupplierName { get; set; }
        public int SupplierCategoryID { get; set; } = 3;
        public int PrimaryContactPersonID { get; set; } = 23;
        public int AlternateContactPersonID { get; set; } = 36;
        public int DeliveryMethodID { get; set; } = 9;
        public int DeliveryCityID { get; set; } = 18557;
        public int PostalCityID { get; set; } = 18557;
        public string? SupplierReference { get; set; } = "Default";
        public string? BankAccountName { get; set; }
        public string? BankAccountBranch { get; set; }
        public string? BankAccountCode { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankInternationalCode { get; set; }
        [Required]
        public int PaymentDays { get; set; }
        public string? InternalComments { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; } = "333-444-5555";
        [Required]
        [Url]
        public string? WebsiteURL { get; set; }
        [Required]
        public string? DeliveryAddressLine1 { get; set; }
        public string? DeliveryAddressLine2 { get; set; }
        [Required]
        public string? DeliveryPostalCode { get; set; }
        public string? DeliveryLocation { get; set; }
        public string? PostalAddressLine1 { get; set; } = "PostalAddressLine1";
        public string? PostalAddressLine2 { get; set; }
        public string? PostalPostalCode { get; set; } = "PostalPostalCode";
        public int LastEditedBy { get; set; } = 1;
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime ValidTo { get; set; } = DateTime.MaxValue;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented);
        }
        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Supplier p = (Supplier)obj;
                return (SupplierName == p.SupplierName);
            }
        }
        public override int GetHashCode() { return SupplierID.GetHashCode(); }
    }
    public class PackageType
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
        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                PackageType p = (PackageType)obj;
                return (PackageTypeName == p.PackageTypeName);
            }
        }
        public override int GetHashCode() { return PackageTypeID.GetHashCode(); }
    }

}
