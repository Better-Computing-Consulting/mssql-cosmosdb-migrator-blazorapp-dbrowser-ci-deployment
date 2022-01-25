# mssql-cosmosdb-migrator-blazorapp-dbrowser-ci-deployment
Console app to migrate MSSQL export into Azure CosmosDB and containerized Blazor server app to browse the new database and create excel exports. The apps are automatically deployed via Azure DevOps Pipeline CI.

__Technologies used in this project:__

* ASP.NET Core 6.0, C# 10, Blazor Application using minimal hosting model
* .NET to JavaScript streaming via DotNetStreamReference
* .Net 6 Core Console Application

__Azure Resources:__
* Cosmos DB
* Container Registry
* Container Instance with environment variables and managed identity.
* KeyVault

__NuGet Packages:__
* Microsoft.Azure.Cosmos
* Azure.Identity
* Azure.Extensions.AspNetCore.Configuration.Secrets
* NPOI

__Automatic deployment of Azure DevOps Project, Service Endpoints, and Pipeline.__


__Steps to reproduce:__

1. Setup the DevOps project by running the __setdevopsproj.sh__ script.  Update the script variables to fit your environment.  This script requires the extension azure-devops, will run completely unattended if you set the __AZURE_DEVOPS_EXT_GITHUB_PAT__ variable before running the script or by entering the you GitHub Pat in the script and uncommenting this line:

```Shell
#export AZURE_DEVOPS_EXT_GITHUB_PAT=enter-github-pat-here
```

2. Clone your copy of the repository onto a computer running Microsoft SQL Server.  

3. Execute the PowerShell __SQLRestoreExport.ps1__ script from the .\dbmigrate\Files directory.

4. Commit the new report files.

Committing the report files will trigger the execution of the pipeline.  

The pipeline will:

1.	make sure all the Azure resources are in place, 
2.	compile the dbmigrate program and import the MSSQL reports into CosmosDB,
3.	compile the cosmosdbviewer program,
4.	upload its container to the Container Registry, and
5.	create a container instance from the image.

Enjoy
