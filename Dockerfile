# STAGE 1: Build
# Uses the full .NET SDK image to compile and publish the application.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files (.csproj) individually to leverage Docker layer caching.
# By doing this first, 'dotnet restore' only re-runs if your dependencies change, 
# significantly speeding up subsequent builds when only source code is modified.
COPY ["src/FishSpotApi.Core/FishSpotApi.Core.csproj", "src/FishSpotApi.Core/"]
COPY ["src/FishSpotApi.Application/FishSpotApi.Application.csproj", "src/FishSpotApi.Application/"]
COPY ["src/FishSpotApi.Data/FishSpotApi.Data.csproj", "src/FishSpotApi.Data/"]
COPY ["src/FishSpotApi.Domain/FishSpotApi.Domain.csproj", "src/FishSpotApi.Domain/"]
COPY ["src/FishSpotApi.Logger/FishSpotApi.Logger.csproj", "src/FishSpotApi.Logger/"]

# Restore NuGet packages based on the project files copied above.
RUN dotnet restore "src/FishSpotApi.Application/FishSpotApi.Application.csproj"

# Copy the rest of the source code into the container.
COPY . .

# Set the working directory to the entry point project and publish the binaries.
# -c Release: Optimizes the code for production.
# -o /app/publish: Directs the output to a specific folder.
# /p:UseAppHost=false: Ensures we use the generic dotnet host rather than a platform-specific executable.
WORKDIR "/src/src/FishSpotApi.Application"
RUN dotnet publish "FishSpotApi.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

# STAGE 2: Runtime
# Uses a lightweight ASP.NET runtime image, which is much smaller than the SDK image.
# This keeps the final production image secure and efficient.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy only the compiled output from the 'build' stage. 
# This discards the source code and SDK tools, minimizing the image size.
COPY --from=build /app/publish .

# STAGE 3: Configuration & Environment
# Inform Docker that the container listens on port 8080 at runtime.
EXPOSE 8080

# Environment variables to configure the ASP.NET Core hosting environment.
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_HTTPS_PORT=

# Defines the command to execute when the container starts.
ENTRYPOINT ["dotnet", "FishSpotApi.Application.dll"]