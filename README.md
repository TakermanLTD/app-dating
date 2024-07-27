# Takerman.Dating

## Migrations
On Takerman.Dating.Server:
dotnet ef migrations add [name] --project ../Takerman.Dating.Data/Takerman.Dating.Data.csproj
dotnet ef database update --project ../Takerman.Dating.Data/Takerman.Dating.Data.csproj
dotnet ef migrations remove

## E2E tests
npx cypress open
cypress run --browser chrome

## Upgrade NPM packages
ncu --upgrade
npm install