# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY *.slnx .
COPY BookBazar.Web/*.csproj ./BookBazar.Web/
RUN dotnet restore

COPY . .
WORKDIR /src/BookBazar.Web
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BookBazar.Web.dll"]