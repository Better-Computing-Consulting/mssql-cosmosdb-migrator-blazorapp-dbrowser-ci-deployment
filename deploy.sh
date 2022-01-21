#!/bin/bash
azloc=westus
rgName=SQLWestRG
echo "Assert resource group $rgName"
rgID=$(az group create -n $rgName -l $azloc --query id --output tsv)
echo $rgID
echo
acrName=bccDevContainerRegistry1
echo "Assert ACR  $acrName"
if [[ $(az resource list --query "[?name=='$acrName'] | length(@)") > 0 ]]; then
  echo "ACR exists"
  acrLoginSrv=$(az acr show --n $acrName --query loginServer -o tsv)
else
  echo "ACR doesn't exist, deploy it"
  acrLoginSrv=$(az acr create --n $acrName -g $rgName -l $azloc --sku Basic --query loginServer --output tsv)
fi
echo $acrLoginSrv
echo
dbAccountName=bccdevcosmosdb1
echo "Assert CosmosDB account name $dbAccountName"
if ! [[ $(az resource list --query "[?name=='$dbAccountName'] | length(@)") > 0 ]]; then
  echo "DB doesn't exist, deploy it"
  az cosmosdb create -n $dbAccountName -g $rgName --kind GlobalDocumentDB --locations regionName=westcentralus failoverPriority=0 isZoneRedundant=false --enable-free-tier true --query name --output tsv
fi
echo
dbConnStr=$(az cosmosdb keys list -n $dbAccountName -g $rgName --type connection-strings --query connectionStrings[0].connectionString -o tsv)
kvName=bccDevKeyVault1
kvSecretName=CosmosDB1ConnectionString
echo "Assert KeyVault name  $kvName and secret name $kvSecretName"
if [[ $(az resource list --query "[?name=='$kvName'] | length(@)") > 0 ]]; then
  echo "KeyVault exist, check secret exists"
  if [[ $(az keyvault secret list --vault-name $kvName --query "[?name=='$kvSecretName'] | length(@)") > 0 ]]; then
    echo "KeyVault Secret exists, check value"
    if [[ $(az keyvault secret show --vault-name $kvName --name $kvSecretName --query value -o tsv) != $dbConnStr ]]
    then
      echo "Secret value is not the same as current connectionString, set it"
      az keyvault secret set --vault-name $kvName --name $kvSecretName --value $dbConnStr --query name --output tsv
    fi
  else
    echo "KeyVault Secret doesn't exist, set it"
    az keyvault secret set --vault-name $kvName --name $kvSecretName --value $dbConnStr --query name --output tsv
  fi
else
  echo "KeyVault doesn't exist, deploy and set secret"
  az keyvault create -n $kvName -g $rgName -l $azloc --query name --output tsv
  az keyvault secret set --vault-name $kvName --name $kvSecretName --value $dbConnStr --query name --output tsv
fi
echo
kvUri=$(az keyvault show -n $kvName --query properties.vaultUri -o tsv)
databaseid=WideWorldImporters
containerid=StockItems
partitionkey=/StockItemID
echo "Assert Cosmos DB Database name  $databaseid"
if [[ $(az cosmosdb sql database exists -a $dbAccountName -n $databaseid -g $rgName) == false ]]; then
  echo "DB $databaseid not found, install dotnet-sdk-6.0, run dbmigrate program"
  sudo apt-get install -y dotnet-sdk-6.0
  dotnet run -c Release --project dbmigrate/dbmigrate.csproj $dbConnStr $databaseid $containerid $partitionkey
fi
echo
echo "Build cosmosdbviewer"
docker build ./ -f cosmosdbviewer/Dockerfile -t cosmosdbviewer
echo
echo "Tag cosmosdbviewer"
docker tag cosmosdbviewer $acrLoginSrv/cosmosdbviewer:v1
echo
echo "Push cosmosdbviewer to $acrName"
docker login $acrLoginSrv --username ${servicePrincipalId} --password ${servicePrincipalKey}
docker push $acrLoginSrv/cosmosdbviewer:v1
echo
echo "Check if container instance exists"
if [[ $(az resource list --query "[?name=='cosmosdbviewer'] | length(@)") > 0 ]]; then
  echo "Container already deployed, restart it."
   az container restart -n cosmosdbviewer -g $rgName
else
  echo "Container does not exits, deploy it"
  spID=$(az container create -g $rgName -n cosmosdbviewer --image $acrLoginSrv/cosmosdbviewer:v1 --cpu 1 --memory 1 \
  --registry-login-server $acrLoginSrv --registry-username ${servicePrincipalId} --registry-password ${servicePrincipalKey} \
  --assign-identity --scope $rgID --environment-variables VaultUri=$kvUri 'databaseid=WideWorldImporters' 'containerid=StockItems' 'partitionkey=/StockItemID' \
  --dns-name-label cosmosdbviewer --ports 80 --query identity.principalId --out tsv)
  echo
  echo "Grant container's identity permissions to $kvName"
  az keyvault set-policy --name $kvName -g $rgName --object-id $spID --secret-permissions get list --query provisioningState -o tsv
fi
echo
echo "Get container's fqdn"
echo http://$(az container show -g $rgName --n cosmosdbviewer --query ipAddress.fqdn --out tsv)
echo
echo "done deploying resources"