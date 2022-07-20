##################################################################
# Stage 0: Set the Base Primary Image of the App
##################################################################
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

EXPOSE 80
EXPOSE 443

##################################################################
# Stage 1: Set the Base Development Image of the App
##################################################################
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy all csproj files as separate layers
COPY *.csproj ./

# Restore packages for each project
WORKDIR /app
RUN dotnet restore

# Copy everything else and build
WORKDIR /app

COPY ./ ./
WORKDIR /app

RUN dotnet publish -c Release -o out

##################################################################
# Stage 2: Copy Into Base Runtime image
##################################################################
FROM base as final
ENV dll_name="app-service-demo.dll"

COPY --from=build /app/out .

ENTRYPOINT ["/bin/sh", "-c", "dotnet app-service-demo.dll"]