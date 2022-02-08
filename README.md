# mssql-cosmosdb-migrator-blazorapp-dbrowser-ci-deployment
Console app to migrate MSSQL export into Azure Cosmos DB and containerized Blazor server app to browse the new database and create excel exports. The apps are automatically deployed via Azure DevOps Pipeline CI.

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

1. Setup the DevOps project by running the __setdevopsproj.sh__ script.  Update the script variables to fit your environment.  This script requires the extension azure-devops, will run completely unattended if you set the __AZURE_DEVOPS_EXT_GITHUB_PAT__ variable before running the script or by entering the GitHub Pat in the script and uncommenting this line:

&nbsp; &nbsp; &nbsp; &nbsp;`#export AZURE_DEVOPS_EXT_GITHUB_PAT=enter-github-pat-here`


&nbsp; &nbsp; &nbsp; &nbsp;The __setdevopsproj.sh__ script creates a DevOps project, two Service Endpoints (one for Azure, another for GitHub) and a Pipeline.


2. Clone your copy of the repository onto a computer running Microsoft SQL Server.  

3. Execute the PowerShell __SQLRestoreExport.ps1__ script from the __.\dbmigrate\Files__ directory. This script does the following:
   * Downloads the __WideWorldImporters__ from https://github.com/Microsoft/sql-server-samples,
   * Restores the database into the local MSSQL server, and
   * Exports 5 __StockItem__-related tables using the `bcp` command.

4. Commit the new report files.

Committing the report files will trigger the execution of the pipeline.  

The pipeline will:

1.	Make sure all the Azure resources are in place, 
2.	compile the __dbmigrate__ console program and import the MSSQL reports into CosmosDB,
3.	compile the __cosmosdbviewer__ Blazor application,
4.	upload its container to the Container Registry, and
5.	create a container instance from the image.

This blog details each step and some of the features of dbmigrate and comsosdbviewer programs, including how to retrieve the cosmos db connection string from an Azure KeyVault while authenticating using the managed identity of the application container instance.

https://bcc.bz/post/mssql-to-cosmos-db-migrator-and-containerized-blazor-app-azure-devops-ci-deployment

There is also a YouTube 8 minute video that demonstrates an entire run of the process.

https://youtu.be/1lPo-VdEB1o


Enjoy

:smiley:
