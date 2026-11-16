# MataamPro Azure App Service deployment

This repository includes:

- `infra/main.bicep`: Linux Azure App Service infrastructure.
- `infra/main.parameters.example.json`: safe parameter template.
- `.github/workflows/deploy-mataampro.yml`: GitHub Actions deployment to App Service.

The workflow uses GitHub OIDC. It does not use a publish profile or a client secret.

## 1. Prerequisites

Install and sign in to the Azure CLI:

```powershell
az login
az account set --subscription '<SUBSCRIPTION_ID>'
```

Set values for the deployment. The App Service name must be globally unique and use only supported hostname characters.

```powershell
$subscriptionId = '<SUBSCRIPTION_ID>'
$resourceGroup = 'rg-mataampro-prod'
$location = 'eastus'
$webAppName = '<globally-unique-web-app-name>'
$planName = "$webAppName-plan"
$sqlConnectionString = '<production-sql-connection-string>'
```

For this repository, the intended production value is `mazharhayatdev`, so the
default hostname is `mazharhayatdev.azurewebsites.net`. `webAppName` is the
App Service resource name only; do not include `https://` or the domain suffix.
The name must already exist before the GitHub Actions deployment can succeed.

Create the resource group:

```powershell
az group create --name $resourceGroup --location $location
```

## 2. Provision App Service

Preview the deployment first:

```powershell
az deployment group what-if `
  --resource-group $resourceGroup `
  --template-file infra/main.bicep `
  --parameters location=$location webAppName=$webAppName appServicePlanName=$planName skuName=B1 sqlConnectionString=$sqlConnectionString
```

After reviewing the preview, deploy:

```powershell
az deployment group create `
  --resource-group $resourceGroup `
  --template-file infra/main.bicep `
  --parameters location=$location webAppName=$webAppName appServicePlanName=$planName skuName=B1 sqlConnectionString=$sqlConnectionString
```

The template configures:

- Linux App Service with the .NET 8 runtime.
- HTTPS-only traffic.
- TLS 1.2 minimum.
- FTP/FTPS disabled.
- Always On enabled.
- `ASPNETCORE_ENVIRONMENT=Production`.
- `DatabaseProvider=SqlServer`.
- SQL connection string as an App Service connection string.
- System-assigned managed identity for future Azure resource access.

Do not commit the real SQL connection string or a filled parameter file.

## 3. Configure GitHub OIDC

Create an Entra application and service principal once. Use a separate identity for CI/CD rather than a personal account.

```powershell
$app = az ad app create --display-name 'MataamPro-GitHub-Deploy' | ConvertFrom-Json
$appId = $app.appId
az ad sp create --id $appId | Out-Null
$tenantId = az account show --query tenantId --output tsv
```

Create a federated credential for the `production` GitHub environment. Run this from PowerShell after replacing the repository owner/name if needed:

```powershell
$federatedCredential = @{
  name = 'mataampro-production'
  issuer = 'https://token.actions.githubusercontent.com'
  subject = 'repo:mazharhayat-dev/MataamPro:environment:production'
  audiences = @('api://AzureADTokenExchange')
} | ConvertTo-Json -Compress

az ad app federated-credential create --id $appId --parameters $federatedCredential
```

Grant the service principal access only to the deployed web app. `Website Contributor` is sufficient for the workflow’s application deployment:

```powershell
$webAppId = az webapp show --resource-group $resourceGroup --name $webAppName --query id --output tsv
az role assignment create `
  --assignee $appId `
  --role 'Website Contributor' `
  --scope $webAppId
```

If the service principal will also provision infrastructure, grant that permission separately and only at the resource-group scope. The application deployment workflow itself does not need subscription-wide Contributor access.

## 4. Add GitHub environment secrets

In GitHub, open **Settings > Environments > New environment**, create an environment named `production`, and add these secrets:

| Secret | Value |
|---|---|
| `AZURE_CLIENT_ID` | `$appId` from the Entra app |
| `AZURE_TENANT_ID` | `$tenantId` |
| `AZURE_SUBSCRIPTION_ID` | `$subscriptionId` |
| `AZURE_WEBAPP_MataamPro` | `$webAppName` (`mazharhayatdev`) |

The workflow runs automatically for pushes to `main` and can also be started from the Actions tab with **Run workflow**.

## 5. Database requirements

The App Service must be able to reach the production SQL Server. For Azure SQL Database, configure the SQL server firewall to allow Azure services or use private networking. The deployed application uses the `DefaultConnection` connection string and `SqlServer` provider.

The application currently seeds data during startup. Ensure the production database schema exists and that the configured database principal has the required permissions before first launch. Prefer EF Core migrations in a controlled release process instead of granting broad permissions to the running web app.

## 6. Verify deployment

After the workflow completes, open:

```text
https://<web-app-name>.azurewebsites.net
```

An `NXDOMAIN` result is a DNS/resource-name problem, not an ASP.NET Core
startup problem. It means Azure does not currently publish that hostname. A
failed package deployment normally still leaves an existing App Service
hostname resolvable (it may return an application or platform error instead).

Run these checks using the subscription and resource group that were used for
the Bicep deployment:

```powershell
# Confirm the resource exists and obtain the authoritative hostname/state.
az webapp show --resource-group $resourceGroup --name mazharhayatdev `
  --query '{name:name,state:state,host:defaultHostName,kind:kind}' --output table

# Find the actual app if the expected name is uncertain.
az webapp list --query "[?contains(name, 'mazharhayat')].{name:name,resourceGroup:resourceGroup,host:defaultHostName,state:state}" --output table

# Check DNS independently of the browser.
Resolve-DnsName mazharhayatdev.azurewebsites.net
```

Interpret the results as follows:

- If `az webapp show` returns `ResourceNotFound`, provision the Bicep
  deployment first, or use the hostname returned by `az webapp list`.
- If the app exists under another name, set the GitHub **production
  environment** secret `AZURE_WEBAPP_MataamPro` to that exact name and use
  `https://<actual-name>.azurewebsites.net`.
- If the app exists with the name `mazharhayatdev` but DNS still returns
  `NXDOMAIN`, verify that the App Service is not being queried in the wrong
  subscription/tenant and retry after a short interval. Azure's default
  `azurewebsites.net` hostname normally becomes available immediately; this
  is not a custom-domain DNS propagation issue.
- If the app exists and resolves, inspect the GitHub Actions run. A failed
  `azure/login` indicates an OIDC secret/federated-credential problem; a
  failed `webapps-deploy` indicates the app name or deployment permissions.
  The application logs can then be checked with `az webapp log tail`.

Useful checks:

```powershell
az webapp show --resource-group $resourceGroup --name $webAppName --query defaultHostName --output tsv
az webapp log tail --resource-group $resourceGroup --name $webAppName
```

The GitHub workflow can deploy only when `az webapp show` succeeds. Confirm the
environment secret is exactly:

```text
AZURE_WEBAPP_MataamPro=mazharhayatdev
```

If the resource has not been created, run the Bicep deployment in section 2,
then rerun the workflow from **Actions > Deploy MataamPro to Azure App Service
> Run workflow**.
