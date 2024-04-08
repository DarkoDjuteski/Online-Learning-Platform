using OnlineLearningPlatform.Frontend.Components;
using OnlineLearningPlatform.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<CourseServiceClient>(client => client.BaseAddress = new("http://gatewayapiservice"));
builder.Services.AddHttpClient<BasketServiceClient>(client => client.BaseAddress = new("http://gatewayapiservice"));

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
