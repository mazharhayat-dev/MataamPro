@description('Azure region for the App Service resources.')
param location string = resourceGroup().location

@description('Globally unique App Service name. This becomes the default Azure hostname.')
param webAppName string

@description('App Service plan name.')
param appServicePlanName string = '${webAppName}-plan'

@description('App Service plan SKU.')
@allowed([
  'B1'
  'B2'
  'S1'
  'P1v3'
])
param skuName string = 'B1'

@description('Production SQL Server connection string. Supply this at deployment time; do not commit it.')
@secure()
param sqlConnectionString string

var appServicePlan = resource appServicePlanResource 'Microsoft.Web/serverfarms@2024-11-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  sku: {
	name: skuName
	tier: contains(skuName, 'P') ? 'PremiumV3' : contains(skuName, 'S') ? 'Standard' : 'Basic'
	size: skuName
	capacity: 1
  }
  properties: {
	reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2024-11-01' = {
  name: webAppName
  location: location
  kind: 'app,linux'
  identity: {
	type: 'SystemAssigned'
  }
  properties: {
	serverFarmId: appServicePlanResource.id
	httpsOnly: true
	siteConfig: {
		linuxFxVersion: 'DOTNETCORE|8.0'
	  alwaysOn: true
	  ftpsState: 'Disabled'
	  minTlsVersion: '1.2'
	  http20Enabled: true
	  appSettings: [
		{
		  name: 'ASPNETCORE_ENVIRONMENT'
		  value: 'Production'
		}
		{
		  name: 'DatabaseProvider'
		  value: 'SqlServer'
		}
		{
		  name: 'WEBSITE_RUN_FROM_PACKAGE'
		  value: '1'
		}
	  ]
	}
  }
}

resource webAppConnectionStrings 'Microsoft.Web/sites/config@2024-11-01' = {
  parent: webApp
  name: 'connectionstrings'
  properties: {
	DefaultConnection: {
	  value: sqlConnectionString
	  type: 'SQLAzure'
	}
  }
}

output webAppName string = webApp.name
output webAppUrl string = 'https://${webApp.properties.defaultHostName}'
output webAppPrincipalId string = webApp.identity.principalId
