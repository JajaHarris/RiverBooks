---
title: "EF Migrations"
description: "Entity Framework Core migration commands for RiverBooks"
summary: "Complete reference for EF Core migration commands"
date: 2024-12-04
weight: 2
---

This page contains all the Entity Framework Core migration commands you'll need when working with RiverBooks.

## Prerequisites

Install or update the EF Core CLI tools:

```bash
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

Add the EF Core Design package to projects that need migrations:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

## Important Notes

> ⚠️ **Warning:** Watch out for `<InvariantGlobalization>true</InvariantGlobalization>` in your Web API project - this can cause issues with EF Core.

## Books Module

### Create Migration

```bash
dotnet ef migrations add Initial -c BookDbContext -p ../RiverBooks.Books/RiverBooks.Books.csproj -s RiverBooks.Web.csproj -o Data/Migrations
```

### Update Database

```bash
dotnet ef database update -c BookDbContext
```

With explicit project paths:

```bash
dotnet ef database update -c BookDbContext -p .\RiverBooks.Books.csproj --startup-project ..\RiverBooks.Web\RiverBooks.Web.csproj
```

For test environment:

```bash
dotnet ef database update -c BookDbContext -p .\RiverBooks.Books.csproj --startup-project ..\RiverBooks.Web\RiverBooks.Web.csproj -- --environment Testing
```

## Users Module

### Create Migration

From the `RiverBooks.Users` folder:

```bash
dotnet ef migrations add CartItemDescription -c UsersDbContext -p ..\RiverBooks.Users\RiverBooks.Users.csproj -s .\RiverBooks.Web.csproj -o Data/Migrations
```

### Update Database

```bash
dotnet ef database update -c UsersDbContext -p .\RiverBooks.Users.csproj -s ..\RiverBooks.Web\RiverBooks.Web.csproj
```

## Order Processing Module

### Create Migration

From the `RiverBooks.OrderProcessing` folder:

```bash
dotnet ef migrations add Initial_OrderProcessing -c OrderProcessingDbContext -p ..\RiverBooks.OrderProcessing\RiverBooks.OrderProcessing.csproj -s .\RiverBooks.Web.csproj -o Data/Migrations
```

### Update Database

```bash
dotnet ef database update -c OrderProcessingDbContext -p .\RiverBooks.OrderProcessing.csproj -s ..\RiverBooks.Web\RiverBooks.Web.csproj
```

## Common Commands Reference

| Command | Description |
|---------|-------------|
| `dotnet ef migrations add <Name>` | Create a new migration |
| `dotnet ef database update` | Apply pending migrations |
| `dotnet ef migrations list` | List all migrations |
| `dotnet ef migrations remove` | Remove the last migration |
| `dotnet ef database drop` | Drop the database |

## Parameters

| Parameter | Description |
|-----------|-------------|
| `-c` | DbContext class name |
| `-p` | Target project containing the DbContext |
| `-s` | Startup project (usually the Web project) |
| `-o` | Output directory for migrations |
