# framework

Create nuget file:

```dotnet pack --configuration Release```

Create nuget file with version

```dotnet pack --configuration Release -p:PackageVersion=2.1.0```

Pulish to Github nuget packages:

```dotnet nuget push "bin/Release/GoGo.Infrastructure.Repository.1.1.0.nupkg"  --api-key replace_with_PAT --source "https://nuget.pkg.github.com/travelgogo/index.json"```