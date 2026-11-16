# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade the MataamPro ASP.NET Core application from .NET 8 to .NET 10, update the requested package references, and align the Azure Web Deploy profile.
**Scope**: 1 SDK-style web project, 79 code files, and the specified project/publish-profile files; minimal in-place change.

## Upgrade Options

| Option | Selected | Why |
|--------|----------|-----|
| Upgrade Strategy | All-at-Once | One modern SDK-style project with a straightforward target framework and package upgrade. |

## Tasks

### 01-framework-and-packages: Upgrade project framework and dependencies

Update the single MataamPro project to target `net10.0`, verify the installed SDK/global.json compatibility, and update all direct Microsoft package references present in the project to `10.0.13`, including Entity Framework Core packages and any Microsoft.AspNetCore packages. Update `System.Security.Cryptography.Pkcs` to `10.0.0`, and change the requested Azure Web Deploy publish profile target framework to `net10.0`. Preserve connection strings, the existing `AddDbContext` extension method pattern, IDE0305 guidance, and unrelated working-tree content.

The assessment found one SDK-style ASP.NET Core project, no incompatible packages, and three low-impact behavioral-change findings. Research the actual project/package references and existing DI/configuration patterns before editing; do not broaden the change to transitive packages or unrelated deployment settings unless restore/build requires it.

**Done when**: The project and publish profile target `net10.0`, direct requested packages have the specified versions, connection strings are unchanged, and the source changes remain limited to the requested upgrade scope.

---

### 02-restore-build-validation: Restore and build the upgraded solution

Restore the upgraded solution and build it with the .NET 10 toolchain. Address only relevant restore or compilation errors introduced or exposed by the framework/package upgrade, while treating warnings as actionable and preserving the repository's existing coding constraints. Confirm whether the .NET 10 SDK is installed and document the result.

**Done when**: `dotnet restore` and `dotnet build` complete successfully with no relevant errors, the SDK requirement is clearly reported, and final validation confirms no connection-string changes or unrelated edits.
