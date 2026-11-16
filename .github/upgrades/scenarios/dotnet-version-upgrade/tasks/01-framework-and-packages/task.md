# 01-framework-and-packages: Upgrade project framework and dependencies

## Objective

Update the single MataamPro project to target `net10.0`, update the requested direct package references, and align the Azure Web Deploy publish profile without changing connection strings or unrelated repository content.

## Research Findings

- **Affected project**: `MataamPro.csproj` only; the solution contains one SDK-style ASP.NET Core project.
- **Target framework location**: `MataamPro.csproj` defines a singular `<TargetFramework>net8.0</TargetFramework>`; no `global.json`, `Directory.Build.props`, or `Directory.Packages.props` was found, so the property is project-local and should be replaced in place.
- **Direct package references**: `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.InMemory`, and `Microsoft.EntityFrameworkCore.SqlServer` are all `8.0.11`; `System.Security.Cryptography.Pkcs` is `8.0.0`. `get_project_dependencies` confirmed standard (non-centralized) package management and that all versions are defined directly in `MataamPro.csproj`.
- **Requested package scope**: Set the three direct Microsoft EF Core references to `10.0.13`; no direct `Microsoft.AspNetCore` package reference exists to update. Set `System.Security.Cryptography.Pkcs` to `10.0.0`. Transitive Microsoft packages are resolved by restore and are not direct edits unless required by validation.
- **Publish profile**: `Properties/PublishProfiles/mazharhayatdev - Web Deploy.pubxml` defines `<TargetFramework>net8.0</TargetFramework>`; only that property is in scope. Runtime, platform, deployment URL, and other publish settings are not to be changed.
- **DI/configuration pattern**: `Program.cs` currently uses `builder.Services.AddDbContext<MataamDbContext>(...)` directly with the existing connection string. `Models/MataamDbContext.cs` is a standard EF Core `DbContext`; no migration API change is indicated by assessment. Preserve this pattern and the repository instruction about an AddDbContext extension method; do not modify connection strings.
- **Connection-string baseline**: `appsettings.json` contains the existing `DefaultConnection`; `appsettings.Development.json` does not define a connection string. Neither file is part of the planned edits.
- **SDK/tooling**: Compatible .NET 10 SDKs are installed (`10.0.302`, `10.0.400`); no `global.json` pins an incompatible SDK. `validate_dotnet_sdk_installation` reports a compatible SDK.
- **Assessment signals**: One project, 57 resolved packages, no incompatible packages, and three low-impact behavioral-change findings. No source changes are expected beyond the project and publish-profile metadata unless restore/build exposes a relevant issue.

## Execution Scope

Modify only `MataamPro.csproj` and `Properties/PublishProfiles/mazharhayatdev - Web Deploy.pubxml` for this task. Validate the exact requested values and confirm connection-string files remain unchanged before completion.

## Done When

The project and publish profile target `net10.0`, direct requested packages have the specified versions, connection strings are unchanged, and the source changes remain limited to the requested upgrade scope.
