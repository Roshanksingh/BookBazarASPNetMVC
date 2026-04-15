# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution + all project files first
COPY BookBazar.slnx ./
COPY BookBazar.Web/BookBazar.Web.csproj BookBazar.Web/
COPY BookBazar.DataAccess/BookBazar.DataAccess.csproj BookBazar.DataAccess/
COPY BookBazar.Model/BookBazar.Models.csproj BookBazar.Model/
COPY BookBazar.Utility/BookBazar.Utility.csproj BookBazar.Utility/

# Restore dependencies
RUN dotnet restore BookBazar.Web/BookBazar.Web.csproj

# Copy everything else
COPY . .

# Publish
WORKDIR /src/BookBazar.Web
RUN dotnet publish BookBazar.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BookBazar.Web.dll"]

