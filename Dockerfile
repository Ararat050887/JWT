# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./JWT_API_SECOND_EXPIRIENCE/JWT_API_SECOND_EXPIRIENCE.csproj" --disable-parallel
RUN dotnet publish "./JWT_API_SECOND_EXPIRIENCE/JWT_API_SECOND_EXPIRIENCE.csproj" -c release -o /app --no-restore


# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000:80

ENTRYPOINT ["dotnet", "JWT_API_SECOND_EXPIRIENCE.dll"]