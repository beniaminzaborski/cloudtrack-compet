var builder = DistributedApplication.CreateBuilder(args);

var postgresUsername = builder.AddParameter("postgresUsername", "postgres", secret: true);
var postgresPassword = builder.AddParameter("postgresPassword", "postgres", secret: true);
var postgresDb = builder.AddPostgres("cloudtrack-postgres", postgresUsername, postgresPassword)
                        .WithPgAdmin()
                        .AddDatabase("cloudtrack-compet-db");

var rabbitmqUsername = builder.AddParameter("rabbitmqUsername", "guest", secret: true);
var rabbitmqPassword = builder.AddParameter("rabbitmqPassword", "guest", secret: true);
var rabbitmq = builder.AddRabbitMQ("cloudtrack-rabbitmq", rabbitmqUsername, rabbitmqPassword)
                      .WithManagementPlugin();

builder.AddProject<Projects.CloudTrack_Competitions_WebAPI>("cloudtrack-compet")
    .WithEnvironment("MessageBus__UseRabbitMq", "true")
    .WithEnvironment("OpenTelemetry__UseOtlpExporter", "true")
    .WithReference(postgresDb, "Postgres")
    .WaitFor(postgresDb)
    .WithReference(rabbitmq, "RabbitMQ")
    .WaitFor(rabbitmq);

builder.Build().Run();
