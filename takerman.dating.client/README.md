Executing on Takerman.Dating.Server project
------
dotnet ef migrations add UserSavedSpots --project ../Takerman.Dating.Data/Takerman.Dating.Data.csproj
dotnet ef database update --project ../Takerman.Dating.Data/Takerman.Dating.Data.csproj
dotnet ef migrations remove
npx cypress open
cypress run --browser chrome