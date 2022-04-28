#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AsposeTestWork.Web/AsposeTestWork.Web.csproj", "AsposeTestWork.Web/"]
RUN dotnet restore "AsposeTestWork.Web/AsposeTestWork.Web.csproj"
COPY . .
WORKDIR "/src/AsposeTestWork.Web"
RUN dotnet build "AsposeTestWork.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AsposeTestWork.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AsposeTestWork.Web.dll"]
LABEL "vendor"="fominskiy"
LABEL version="1.0"
LABEL description="Test WebApp with Aspose.Word and Yandex.Translator"