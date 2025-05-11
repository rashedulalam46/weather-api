# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files
COPY . .

# Restore dependencies
RUN dotnet restore "myfirstapi.csproj"

# Build the application
RUN dotnet build "myfirstapi.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "myfirstapi.csproj" -c Release -o /app/publish

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose the port your app runs on (adjust if needed)
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "myfirstapi.dll"]