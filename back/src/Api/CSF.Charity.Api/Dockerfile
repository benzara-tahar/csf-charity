#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used for VS debugging on Docker
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt install -y nodejs
WORKDIR /src
COPY ["src/CSF.Charity.Api/CSF.Charity.Api.csproj", "src/CSF.Charity.Api/"]
COPY ["src/CSF.Charity.Application/CSF.Charity.Application.csproj", "src/CSF.Charity.Application/"]
COPY ["src/CSF.Charity.Domain/CSF.Charity.Domain.csproj", "src/Domain/"]
COPY ["src/CSF.Charity.Infrastructure/CSF.Charity.Infrastructure.csproj", "src/CSF.Charity.Infrastructure/"]
RUN dotnet restore "src/CSF.Charity.Api/CSF.Charity.Api.csproj"
COPY . .
WORKDIR "/src/src/CSF.Charity.Api"
RUN dotnet build "CSF.Charity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSF.Charity.Api.dll"]