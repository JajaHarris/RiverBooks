var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server as a container
var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume("riverbooks-sqldata");

// Add the RiverBooks database
var riverbooksDb = sqlServer.AddDatabase("RiverBooksDb");

// Add the main web application
var web = builder.AddProject<Projects.RiverBooks_Web>("riverbooks-web")
    .WithExternalHttpEndpoints()
    .WithReference(riverbooksDb)
    .WaitFor(riverbooksDb);

builder.Build().Run();
