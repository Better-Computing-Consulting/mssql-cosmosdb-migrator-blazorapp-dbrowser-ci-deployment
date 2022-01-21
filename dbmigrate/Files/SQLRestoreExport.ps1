echo "Assert SQL DB Data folder exists"
$folder = 'C:\temp\DATA\'
if (-not (Test-Path $folder)){
     New-Item -ItemType Directory -Force -Path $folder
     if (-not $?){
        "Failed to create the SQL restore command data destination folder $folder"
        exit
     }
}
echo "Download WideWorldImporters database from GitHub"
Invoke-WebRequest https://github.com/Microsoft/sql-server-samples/releases/download/wide-world-importers-v1.0/WideWorldImporters-Full.bak -OutFile C:\temp\WideWorldImporters-Full.bak
$sqlserver = hostname
echo "Restore downloaded database"
sqlcmd -E -S $sqlserver -i restore.sql
echo "Export tables"
bcp "SELECT * FROM WideWorldImporters.Warehouse.StockItems" queryout StockItems.txt -c -T
bcp "SELECT * FROM WideWorldImporters.Warehouse.PackageTypes" queryout PackageTypes.txt -c -T
bcp "SELECT * FROM WideWorldImporters.Warehouse.Colors" queryout Colors.txt -c -T
bcp "SELECT * FROM WideWorldImporters.Warehouse.StockItemHoldings" queryout StockItemHoldings.txt -c -T
bcp "SELECT * FROM WideWorldImporters.Purchasing.Suppliers" queryout Suppliers.txt -c -T
dir