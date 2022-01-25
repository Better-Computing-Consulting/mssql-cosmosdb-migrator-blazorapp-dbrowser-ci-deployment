spname=sqldemosp
subscription_id=$(az account show --query id -o tsv)
sed -i "s/enter-subscription-id-here/$subscription_id/g" CustomRole.json
az role definition create --role-definition @CustomRole.json --only-show-errors --query "{roleType: roleType, roleName:roleName}" -o table
sed -i "s/$subscription_id/enter-subscription-id-here/g" CustomRole.json
spkey=$(az ad sp create-for-rbac --name $spname --role "Custom SQL Demo Contributor Role" --only-show-errors --query password -o tsv)
tenant=$(az account show --query tenantId -o tsv)
client_id=$(az ad sp list --display-name $spname --query '{clientId:[0].appId}' -o tsv)
subscrname="Microsoft Partner Network"
orgurl=https://dev.azure.com/Better-Computing-Consulting
prjname=bccSQLDemo
githubsvc=bccGitHub
pipelinename=bccBuildDeploy
pipelinerepo=https://github.com/Better-Computing-Consulting/mssql-cosmosdb-migrator-blazorapp-dbrowser-ci-deployment

#az login
#export AZURE_DEVOPS_EXT_GITHUB_PAT=enter-github-pat-here
export AZURE_DEVOPS_EXT_AZURE_RM_SERVICE_PRINCIPAL_KEY=$spkey
az devops project create --name $prjname --org $orgurl --query "{name: name, state: state}" -o table
azrmsvcid=$(az devops service-endpoint azurerm create --azure-rm-service-principal-id $client_id --azure-rm-subscription-id $subscription_id --azure-rm-subscription-name "$subscrname" --azure-rm-tenant-id $tenant --name AzureServiceConnection --org $orgurl --project $prjname --query  id -o tsv)
az devops service-endpoint update --id $azrmsvcid --enable-for-all true --org $orgurl --project $prjname --query "{Type: type, name: name, isReady: isReady}" -o table
githubsvcid=$(az devops service-endpoint github create --github-url https://github.com/ --name $githubsvc --org $orgurl --project $prjname --query id -o tsv)
az devops service-endpoint update --id $githubsvcid --enable-for-all true --org $orgurl --project $prjname --query "{Type: type, name: name, isReady: isReady}" -o table
pipelineid=$(az pipelines create --name $pipelinename --project $prjname --org $orgurl --repository $pipelinerepo --branch master --yml-path azure-pipelines.yml --skip-first-run true --service-connection $githubsvcid --only-show-errors --query id -o tsv)
echo "$orgurl/$prjname/_build?definitionId=$pipelineid"

