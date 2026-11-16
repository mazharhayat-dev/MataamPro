# 02-restore-build-validation: Restore and build the upgraded solution

## Research Findings

- The solution contains one SDK-style ASP.NET Core project targeting `net10.0`; `dotnet build` is the appropriate tool.
- The installed active SDK is `10.0.402`; compatible SDKs `10.0.302` and `10.0.400` are also installed, and no `global.json` pins an incompatible version.
- Final project evaluation reports `TargetFramework=net10.0` and the requested direct package versions: EF Core packages `10.0.13` and `System.Security.Cryptography.Pkcs` `10.0.0`.
- The publish profile reports `TargetFramework=net10.0`.
- No test project was discovered. No connection-string file is modified by the upgrade; `git diff` for `appsettings.json` and `appsettings.Development.json` is empty.
- The only tracked code/project diff from the upgrade is `MataamPro.csproj`; the requested publish profile is ignored by the repository's `*.pubxml` rule but was verified locally. Workflow artifacts are expected scenario outputs.

## Validation Scope

Run `dotnet restore` and `dotnet build` against `MataamPro.slnx`, inspect warnings/errors, verify the `net10.0` output assembly, and confirm the final change scope.

## Done When

`dotnet restore` and `dotnet build` complete successfully with no relevant errors, the SDK requirement is clearly reported, and final validation confirms no connection-string changes or unrelated edits.
