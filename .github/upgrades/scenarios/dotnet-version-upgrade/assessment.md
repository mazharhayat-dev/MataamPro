# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
  - [Binding Redirect Configuration](#binding-redirect-configuration)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [MataamPro.csproj](#mataamprocsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 1 | All require upgrade |
| Total NuGet Packages | 57 | 4 need upgrade |
| Total Code Files | 79 |  |
| Total Code Files with Incidents | 3 |  |
| Total Lines of Code | 3185 |  |
| Total Number of Issues | 9 |  |
| Estimated LOC to modify | 3+ | at least 0.1% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Binding Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| [MataamPro.csproj](#mataamprocsproj) | net8.0 | 🟢 Low | 5 | 3 | 0 | 3+ | AspNetCore, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 53 | 93.0% |
| ⚠️ Incompatible | 0 | 0.0% |
| 🔄 Upgrade Recommended | 4 | 7.0% |
| ***Total NuGet Packages*** | ***57*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 3 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 15839 |  |
| ***Total APIs Analyzed*** | ***15842*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| Azure.Core | 1.38.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Azure.Identity | 1.11.4 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Bcl.AsyncInterfaces | 1.1.1 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.CSharp | 4.5.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Data.SqlClient | 5.1.6 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Data.SqlClient.SNI.runtime | 5.1.1 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.EntityFrameworkCore | 8.0.11 | 10.0.15 | [MataamPro.csproj](#mataamprocsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Abstractions | 8.0.11 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.EntityFrameworkCore.Analyzers | 8.0.11 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.EntityFrameworkCore.InMemory | 8.0.11 | 10.0.15 | [MataamPro.csproj](#mataamprocsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Relational | 8.0.11 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.11 | 10.0.15 | [MataamPro.csproj](#mataamprocsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Caching.Abstractions | 8.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Extensions.Caching.Memory | 8.0.1 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Extensions.Configuration.Abstractions | 8.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Extensions.DependencyInjection | 8.0.1 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Extensions.DependencyInjection.Abstractions | 8.0.2 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Extensions.Logging | 8.0.1 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Extensions.Logging.Abstractions | 8.0.2 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Extensions.Options | 8.0.2 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Extensions.Primitives | 8.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Identity.Client | 4.61.3 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Identity.Client.Extensions.Msal | 4.61.3 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.IdentityModel.Abstractions | 6.35.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.IdentityModel.JsonWebTokens | 6.35.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.IdentityModel.Logging | 6.35.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.IdentityModel.Protocols | 6.35.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.IdentityModel.Protocols.OpenIdConnect | 6.35.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.IdentityModel.Tokens | 6.35.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.NETCore.Platforms | 1.1.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.NETCore.Targets | 1.1.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.SqlServer.Server | 1.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| Microsoft.Win32.SystemEvents | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.ClientModel | 1.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Configuration.ConfigurationManager | 6.0.1 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Diagnostics.DiagnosticSource | 6.0.1 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Drawing.Common | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Formats.Asn1 | 8.0.1 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.IdentityModel.Tokens.Jwt | 6.35.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Memory | 4.5.4 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Memory.Data | 1.0.2 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Numerics.Vectors | 4.5.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Runtime | 4.3.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Runtime.Caching | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Runtime.CompilerServices.Unsafe | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Security.AccessControl | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Security.Cryptography.Cng | 5.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Security.Cryptography.Pkcs | 8.0.0 | 10.0.15 | [MataamPro.csproj](#mataamprocsproj) | NuGet package upgrade is recommended |
| System.Security.Cryptography.ProtectedData | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Security.Permissions | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Security.Principal.Windows | 5.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Text.Encoding | 4.3.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Text.Encoding.CodePages | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Text.Encodings.Web | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Text.Json | 4.7.2 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Threading.Tasks.Extensions | 4.5.4 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |
| System.Windows.Extensions | 6.0.0 |  | [MataamPro.csproj](#mataamprocsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| T:System.Uri | 2 | 66.7% | Behavioral Change |
| M:System.Uri.TryCreate(System.String,System.UriKind,System.Uri@) | 1 | 33.3% | Behavioral Change |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;MataamPro.csproj</b><br/><small>net8.0</small>"]
    click P1 "#mataamprocsproj"

```

## Project Details

<a id="mataamprocsproj"></a>
### MataamPro.csproj

#### Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 0
- **Dependants**: 0
- **Number of Files**: 131
- **Number of Files with Incidents**: 3
- **Lines of Code**: 3185
- **Estimated LOC to modify**: 3+ (at least 0.1% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["MataamPro.csproj"]
        MAIN["<b>📦&nbsp;MataamPro.csproj</b><br/><small>net8.0</small>"]
        click MAIN "#mataamprocsproj"
    end

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 3 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 15839 |  |
| ***Total APIs Analyzed*** | ***15842*** |  |

