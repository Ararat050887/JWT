# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./JWT_API_SECOND_EXPIRIENCE/JWT_API_SECOND_EXPIRIENCE.csproj" --disable-parallel

#COPY ["JWT_API_SECOND_EXPIRIENCE/.","JWT_API_SECOND_EXPIRIENCE/."]
COPY . .
##WORKDIR "./JWT_API_SECOND_EXPIRIENCE"
RUN dotnet build "./JWT_API_SECOND_EXPIRIENCE/JWT_API_SECOND_EXPIRIENCE.csproj" -c release -o /app --no-restore


FROM build AS publish
RUN dotnet publish "./JWT_API_SECOND_EXPIRIENCE/JWT_API_SECOND_EXPIRIENCE.csproj" -c release -o /app


# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
#COPY -from=publish /app/publish.


EXPOSE 80
EXPOSE 443

RUN apt-get update
RUN apt-get -y install gss-ntlmssp



ENTRYPOINT ["dotnet", "JWT_API_SECOND_EXPIRIENCE.dll"]