FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY . .
ENTRYPOINT ["dotnet", "Application.Web.dll"]