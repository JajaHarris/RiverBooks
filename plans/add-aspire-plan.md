# Plan: Add .NET Aspire to RiverBooks Solution

## Overview

This plan outlines the steps to add .NET Aspire orchestration to the existing RiverBooks solution using the Aspire CLI and .NET CLI tools.

## Prerequisites

Before starting, ensure you have:

- [x] .NET 10.0 SDK installed (required for Aspire 13.x)
- [ ] Docker Desktop or Podman installed and running
- [x] Aspire CLI installed (see Step 1) ✅ Installed v13.0.0

## Current Solution Structure

The RiverBooks solution (`src/RiverBooks.slnx`) includes:

- `RiverBooks.Web` - Main ASP.NET Core web application
- `RiverBooks.Books` - Books module
- `RiverBooks.Users` - Users module  
- `RiverBooks.OrderProcessing` - Order processing module
- `RiverBooks.EmailSending` - Email sending module
- `RiverBooks.Reporting` - Reporting module
- `RiverBooks.SharedKernel` - Shared kernel/common code
- Various `.Contracts` and `.Tests` projects

## Step 1: Install the Aspire CLI

The Aspire CLI is the recommended way to work with Aspire projects. Choose one of these installation methods:

### Option A: Install as Native Executable (Recommended)

**PowerShell (Windows):**

```powershell
# Download and run the install script
Invoke-Expression "& { $(Invoke-RestMethod https://aspire.dev/install.ps1) }"
```

**Bash (WSL/Linux/macOS):**

```bash
curl -sSL https://aspire.dev/install.sh | bash
```

### Option B: Install as .NET Global Tool

```bash
dotnet tool install -g Aspire.Cli --prerelease
```

### Verify Installation

```bash
aspire --version
```

Expected output: `13.0.0` (or later)

## Step 2: Install Aspire Project Templates ✅ Complete

Ensure the Aspire templates are installed:

```bash
# Check if templates are already installed
dotnet new list aspire

# Install templates if not present
dotnet new install Aspire.ProjectTemplates
```

**Status**: Templates already installed (aspire-apphost, aspire-servicedefaults, aspire-starter, etc.)

## Step 3: Create the AppHost Project ✅ Complete

The AppHost is the orchestrator project that manages all services in development.

```bash
# Navigate to the src folder
cd src

# Create the AppHost project using Aspire CLI (interactive)
aspire new aspire-apphost --name RiverBooks.AppHost --output ./RiverBooks.AppHost

# OR using dotnet CLI (non-interactive)
dotnet new aspire-apphost -o RiverBooks.AppHost
```

**Status**: Created `src/RiverBooks.AppHost/RiverBooks.AppHost.csproj`

## Step 4: Create the ServiceDefaults Project ✅ Complete

The ServiceDefaults project provides common configuration like OpenTelemetry, health checks, and service discovery.

```bash
# Create the ServiceDefaults project using Aspire CLI (interactive)
aspire new aspire-servicedefaults --name RiverBooks.ServiceDefaults --output ./RiverBooks.ServiceDefaults

# OR using dotnet CLI (non-interactive)
dotnet new aspire-servicedefaults -o RiverBooks.ServiceDefaults
```

**Status**: Created `src/RiverBooks.ServiceDefaults/RiverBooks.ServiceDefaults.csproj`

## Step 5: Add Projects to the Solution ✅ Complete

```bash
# Add new projects to the solution
dotnet sln RiverBooks.slnx add ./RiverBooks.AppHost/RiverBooks.AppHost.csproj
dotnet sln RiverBooks.slnx add ./RiverBooks.ServiceDefaults/RiverBooks.ServiceDefaults.csproj
```

**Status**: Both projects added to `RiverBooks.slnx`

## Step 6: Configure Project References ✅ Complete

### Add Web Project Reference to AppHost

```bash
# AppHost needs references to projects it will orchestrate
dotnet add ./RiverBooks.AppHost/RiverBooks.AppHost.csproj reference ./RiverBooks.Web/RiverBooks.Web.csproj
```

### Add ServiceDefaults Reference to Web Project

```bash
# Web project needs reference to ServiceDefaults
dotnet add ./RiverBooks.Web/RiverBooks.Web.csproj reference ./RiverBooks.ServiceDefaults/RiverBooks.ServiceDefaults.csproj
```

**Status**:

- Added `RiverBooks.Web` reference to `RiverBooks.AppHost`
- Added `RiverBooks.ServiceDefaults` reference to `RiverBooks.Web`

## Step 7: Update the AppHost Program.cs ✅ Complete

Edit `RiverBooks.AppHost/AppHost.cs` (note: template creates `AppHost.cs` not `Program.cs`):

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Add the main web application
var web = builder.AddProject<Projects.RiverBooks_Web>("riverbooks-web")
    .WithExternalHttpEndpoints();

builder.Build().Run();
```

**Status**: Updated `src/RiverBooks.AppHost/AppHost.cs` to orchestrate the Web project

## Step 8: Update the Web Project Program.cs ✅ Complete

Add the ServiceDefaults to `RiverBooks.Web/Program.cs` right after the builder is created:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, health checks, etc.)
builder.AddServiceDefaults();

// ... rest of existing configuration
```

Also add the health check endpoints before `app.Run()`:

```csharp
// Map Aspire default endpoints (health checks)
app.MapDefaultEndpoints();

app.Run();
```

**Status**: Updated `src/RiverBooks.Web/Program.cs` with `AddServiceDefaults()` and `MapDefaultEndpoints()`

## Step 9: Add Aspire Integrations (Optional) ✅ Complete (SQL Server)

Use the Aspire CLI to add integrations for external services. For example, if RiverBooks uses SQL Server:

```bash
# Interactive mode - shows available integrations
aspire add

# Or specify the integration directly
aspire add Aspire.Hosting.SqlServer

# Or using dotnet CLI
dotnet add ./RiverBooks.AppHost/RiverBooks.AppHost.csproj package Aspire.Hosting.SqlServer
```

### Common Integrations to Consider

Based on the RiverBooks architecture, you might want to add:

- `Aspire.Hosting.SqlServer` - For SQL Server database ✅ Added
- `Aspire.Hosting.MongoDB` - For MongoDB (used by EmailSending)
- `Aspire.Hosting.Redis` - For caching
- `Aspire.Hosting.RabbitMQ` - For messaging

**Status**:

- Added `Aspire.Hosting.SqlServer` package (v13.0.2)
- Updated `AppHost.cs` to add SQL Server container with data volume and database

## Step 10: Run the Application ✅ Complete

### Using Aspire CLI (Recommended)

```bash
cd src
aspire run
```

This will:

1. Build all projects
2. Start the orchestrated services
3. Launch the Aspire Dashboard
4. Display endpoint URLs in the terminal

### Using Visual Studio / VS Code

Set `RiverBooks.AppHost` as the startup project and press F5.

**Status**: Successfully ran with `aspire run`. Dashboard available at `https://localhost:17121`

## Step 11: Verify the Setup ✅ Complete

1. The Aspire Dashboard should open automatically (usually at `https://localhost:15XXX`)
2. You should see the `riverbooks-web` project listed in the dashboard
3. Click on the endpoint to open the application
4. Check the Logs, Traces, and Metrics tabs in the dashboard

## Summary of Files Created/Modified

### New Files

- `src/RiverBooks.AppHost/RiverBooks.AppHost.csproj`
- `src/RiverBooks.AppHost/Program.cs`
- `src/RiverBooks.AppHost/appsettings.json`
- `src/RiverBooks.AppHost/appsettings.Development.json`
- `src/RiverBooks.ServiceDefaults/RiverBooks.ServiceDefaults.csproj`
- `src/RiverBooks.ServiceDefaults/Extensions.cs`

### Modified Files

- `src/RiverBooks.slnx` - Added new project references
- `src/RiverBooks.Web/RiverBooks.Web.csproj` - Added ServiceDefaults reference
- `src/RiverBooks.Web/Program.cs` - Added `AddServiceDefaults()` and `MapDefaultEndpoints()`

## Troubleshooting

### Docker/Podman Not Running

If you see container runtime errors, ensure Docker Desktop or Podman is running.

### Certificate Errors

Aspire manages its own development certificates. If you encounter issues:

```bash
dotnet dev-certs https --trust
```

### Port Conflicts

The Aspire Dashboard uses dynamic ports. If you need specific ports, configure them in the AppHost.

## References

- [Add Aspire to an existing .NET app](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/add-aspire-existing-app)
- [Aspire CLI Overview](https://learn.microsoft.com/en-us/dotnet/aspire/cli/overview)
- [Install Aspire CLI](https://learn.microsoft.com/en-us/dotnet/aspire/cli/install)
- [Aspire CLI Command Reference](https://learn.microsoft.com/en-us/dotnet/aspire/cli-reference/aspire)
- [Aspire Setup and Tooling](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling)
