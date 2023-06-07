# Setup



## Setup Azure Event Hubs for receiving Graph Changes

- https://learn.microsoft.com/en-us/azure/event-hubs/


Source: https://learn.microsoft.com/en-us/graph/change-notifications-delivery-event-hubs?view=graph-rest-1.0&tabs=change-notifications-eventhubs-azure-cli%2Chttp
Note: This has been heavily modified to work with PowerShell and to work in general

``` PowerShell

# sets the name of the resource group
$resourcegroup = "rg-graphevents-dev"

# sets the location of the resources
$location='northeurope'

# sets the name of the Azure Event Hubs namespace
$evhamespacename = "pkb-evh-graphevents-dev"

# sets the name of the hub under the namespace
$evhhubname = "pkb-graphevents"

# sets the name of the access policy to the hub
$evhpolicyname = "pkb-grapheventspolicy"

# sets the name of the Azure KeyVault
$keyvaultname = "pkb-kv-graphevents"

# sets the name of the secret in Azure KeyVault that will contain the connection string to the hub
$keyvaultsecretname = "pkb-grapheventsconnectionstring"

# --------------
az group create --location $location --name $resourcegroup

az eventhubs namespace create --name $evhamespacename --resource-group $resourcegroup --sku Basic --location $location

az eventhubs eventhub create --name $evhhubname --namespace-name $evhamespacename --resource-group $resourcegroup --partition-count 2 --retention-time 24 --cleanup-policy delete

az eventhubs eventhub authorization-rule create --name $evhpolicyname --eventhub-name $evhhubname --namespace-name $evhamespacename --resource-group $resourcegroup --rights Send


$evhprimaryconnectionstring = az eventhubs eventhub authorization-rule keys list --name $evhpolicyname --eventhub-name $evhhubname --namespace-name $evhamespacename --resource-group $resourcegroup --query "primaryConnectionString" --output tsv

az keyvault create --name $keyvaultname --resource-group $resourcegroup --location $location --sku standard --retention-days 90

az keyvault secret set --name $keyvaultsecretname --value $evhprimaryconnectionstring --vault-name $keyvaultname --output none

$graphspn = az ad sp list --display-name 'Microsoft Graph Change Tracking' --query "[].appId" --output tsv

az keyvault set-policy --name $keyvaultname --resource-group $resourcegroup --secret-permissions get --spn $graphspn --output none

$keyvaulturi= az keyvault show --name $keyvaultname --resource-group $resourcegroup --query "properties.vaultUri" --output tsv

$loggedinuser = az ad signed-in-user show --query 'userPrincipalName'
$domainname = $loggedinuser.Split("@")[1]

$notificationUrl="EventHub:${keyvaulturi}secrets/${keyvaultsecretname}?tenantId=${domainname}"

Write-Host "Notification Url: ${notificationUrl}"

```

```powershell

# Purge the Secret - if needed (optional)

az keyvault secret list-deleted --vault-name pkb-kv-graphevents
az keyvault secret purge --id <id of above>

```


# Register Subscription in Microsoft Graph SDk

```powershell


Import-Module Microsoft.Graph.ChangeNotifications

$testUserId = "d47e12f9-99f3-40ea-8870-7b39d2be92f7"

# Expiration time cannot be greater than one hour from now
# Note: This will expire in 1 hour when you run it and kill your app - thus this is temporary

$params = @{
	changeType = "created"
	notificationUrl = $notificationUrl
	resource = "/users/${testUserId}/chats/getAllMessages"
	expirationDateTime = [System.DateTime]::Now.AddHours(1)
	clientState = "secretClientValue"
	latestSupportedTlsVersion = "v1_2"
}

Connect-MgGraph -TenantId $domainname -Scopes "Chat.Read,Chat.ReadWrite"
$result = New-MgSubscription -BodyParameter $params
$result

# Remove Subscription



```