FROM mcr.microsoft.com/dotnet/core/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS build
WORKDIR /app
COPY . .
RUN dotnet build RecipeFinderWebApi/RecipeFinderWebApi.UI.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish RecipeFinderWebApi/RecipeFinderWebApi.UI.csproj -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "RecipeFinderWebApi.UI.dll" ]