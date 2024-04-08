using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using OnlineLearningPlatform.Infrastructure.Abstraction;
using OnlineLearningPlatform.Infrastructure.Sql;
using OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddDbContextPool<OnlineLearningPlatformContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineLearningPlatformDB"), sqlOptions =>
    {
        sqlOptions.ExecutionStrategy(c => new CustomRetryingExecutionStrategy(c));
    }));
builder.EnrichSqlServerDbContext<OnlineLearningPlatformContext>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.Retry = false);
builder.AddRedisDistributedCache("cache");

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();


builder.Services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
