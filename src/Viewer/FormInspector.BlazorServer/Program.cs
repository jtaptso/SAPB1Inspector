using FormInspector.BlazorServer.Components;
using FormInspector.BlazorServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add HTTP client for backend API communication
builder.Services.AddHttpClient<SnapshotApiClient>(client =>
{
    var baseUrl = builder.Configuration["BackendApi:Url"] ?? "http://localhost:5000";
    client.BaseAddress = new Uri(baseUrl);
});

// Add SignalR client service for real-time updates
builder.Services.AddSingleton<SignalRClient>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
