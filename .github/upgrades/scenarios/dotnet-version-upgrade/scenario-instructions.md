# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0
- **Scope**: Upgrade MataamPro.csproj and align its Azure publish profile; preserve minimal scope.
- **Microsoft package versions**: Set all Microsoft package references present in the project to 10.0.13, including Entity Framework Core and Microsoft.AspNetCore packages.
- **System.Security.Cryptography.Pkcs**: Set to 10.0.0.
- **Connection strings**: Do not modify.
- **Validation**: Run dotnet restore and dotnet build; fix only relevant errors.
- **Coding constraints**: Preserve the AddDbContext extension method pattern for IServiceCollection and follow IDE0305 collection-initialization guidance.
- **Azure context**: The publish profile must target net10.0 to address the reported HTTP 500.30 deployment issue.

## Upgrade Options

### Strategy
- Upgrade Strategy: All-at-Once

## Strategy
**Selected**: All-At-Once
**Rationale**: One modern SDK-style project with a straightforward target framework and package upgrade.

### Execution Constraints
- Apply the target framework, package, and publish-profile updates as one atomic upgrade.
- Verify the .NET SDK and global.json compatibility before editing.
- Restore dependencies and build the solution after the upgrade.
- Fix only relevant upgrade errors and preserve existing connection strings and unrelated changes.
- Validate the final solution with zero build errors and document any tooling block.

## Source Control
- **Source Branch**: main
- **Working Branch**: upgrade-dotnet-10
- **Pending Changes**: Committed before scenario initialization as the pre-upgrade checkpoint.
- **Commit Strategy**: Single Commit at End
- **Branch Sync**: Auto (Merge)
