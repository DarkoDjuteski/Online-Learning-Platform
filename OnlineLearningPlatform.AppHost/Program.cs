var builder = DistributedApplication.CreateBuilder();

// To have a persistent volume across container instances, it must be named.
var onlineLearningPlatformDb = builder.AddSqlServer("OnlineLearningPlatform", password: "AdminAdmin1!", 51234)
    .WithVolumeMount("VolumeMount.sqlserver.data", "/var/opt/mssql")
    .AddDatabase("OnlineLearningPlatformDB");

var cache = builder.AddRedis("cache").WithRedisCommander();

var gatewayService = builder.AddProject<Projects.OnlineLearningPlatform_GatewayApiService>("gatewayapiservice");

builder.AddProject<Projects.OnlineLearningPlatform_Frontend>("onlinelearningplatform-frontend")
    .WithReference(cache)
    .WithReference(gatewayService);

builder.AddProject<Projects.OnlineLearningPlatform_OrderApiService>("orderapiservice")
    .WithReference(onlineLearningPlatformDb)
    .WithReference(cache);

builder.AddProject<Projects.OnlineLearningPlatform_PaymentApiService>("paymentapiservice");

builder.AddProject<Projects.OnlineLearningPlatform_CourseApiService>("courseapiservice")
    .WithReference(onlineLearningPlatformDb)
    .WithReference(cache);

builder.Build().Run();
