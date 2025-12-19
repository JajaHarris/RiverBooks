var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server as a container
var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume("riverbooks-sqldata");

// Add the RiverBooks database
var riverbooksDb = sqlServer.AddDatabase("RiverBooksDb");

// Add Papercut email server
var papercut = builder.AddContainer("papercut", "jijiechen/papercut", "latest")
    .WithHttpEndpoint(port: 37408, targetPort: 37408, name: "smtp-web")
    .WithEndpoint(port: 25, targetPort: 25, name: "smtp", scheme: "tcp");

// Add the main web application
var web = builder.AddProject<Projects.RiverBooks_Web>("riverbooks-web")
    .WithExternalHttpEndpoints()
    .WithReference(riverbooksDb)
    .WithEnvironment("Email__SmtpServer", "papercut")
    .WithEnvironment("Email__SmtpPort", "25")
    .WaitFor(riverbooksDb)
    .WaitFor(papercut);

builder.Build().Run();
