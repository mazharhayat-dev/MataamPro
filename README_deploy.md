# MataamPro Azure App Service deployment

This project targets .NET 10, so the deployment workflow uses the .NET 10 SDK. It deploys with an Azure App Service publish profile and does not require Azure CLI, an Azure subscription ID, a resource group, or OIDC credentials.

## Target App Service

- App Service name: `mazharhayatdev`
- URL: <https://mazharhayatdev.azurewebsites.net>
- GitHub Actions environment: `production`
- Workflow: `.github/workflows/deploy-mataampro.yml`

## Configure the publish profile

1. Sign in to the [Azure portal](https://portal.azure.com).
2. Open **App Services** and select `mazharhayatdev`.
3. On the **Overview** page, select **Get publish profile**.
4. Save the downloaded `.PublishSettings` file temporarily. Treat it as a password and do not commit it.
5. Open the GitHub repository `mazharhayat-dev/MataamPro`.
6. Go to **Settings > Environments** and select or create the `production` environment.
7. Under **Environment secrets**, select **New environment secret**.
8. Set the name to `AZURE_WEBAPP_PUBLISH_PROFILE`.
9. Open the downloaded publish-settings file in a text editor, copy its complete contents, and paste them into the secret value.
10. Save the secret.

The existing app-name secret must also be present in the same `production` environment:

```text
AZURE_WEBAPP_MataamPro=mazharhayatdev
```

Do not add the publish profile, SQL connection string, or any other secret to the repository.

## Configure the production database

In the App Service, open **Settings > Environment variables** (or **Configuration** in the classic portal), add a connection string named `DefaultConnection`, and set its value to the production SQL connection string. Do not put that value in source control or workflow YAML.

The application uses the `SqlServer` provider unless `DatabaseProvider` is explicitly set to `InMemory`.

## Deploy

The workflow runs automatically when changes are pushed to `main`. It can also be started manually:

1. Open the repository's **Actions** tab.
2. Select **Deploy MataamPro to Azure App Service**.
3. Select **Run workflow**, choose `main`, and start it.

The workflow restores, builds, publishes to `./publish`, and deploys that folder with `azure/webapps-deploy@v3`.

## App Service settings

Use a Linux App Service with the .NET 10 runtime. No `web.config` is required for this Linux deployment. `Program.cs` uses `app.Run()`, allowing App Service to provide the listening address and port through the hosting environment; the application does not hardcode a port.

After a successful deployment, open <https://mazharhayatdev.azurewebsites.net>.
