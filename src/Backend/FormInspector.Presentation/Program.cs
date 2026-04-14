using FormInspector.Application.UseCases.GetLatestSnapshot;
using FormInspector.Application.UseCases.ReceiveSnapshot;
using FormInspector.Infrastructure.DependencyInjection;
using FormInspector.Infrastructure.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddSignalR();

// Add Infrastructure layer services
builder.Services.AddInfrastructure();

// Add Application layer use cases
builder.Services.AddTransient<ReceiveSnapshotHandler>();
builder.Services.AddTransient<GetLatestSnapshotHandler>();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS for Blazor viewer
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "https://localhost",
                "http://localhost",
                "https://localhost:5001",
                "http://localhost:5000",
                "https://localhost:7001",
                "http://localhost:7000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();

// Register global exception filter
app.UseMiddleware<ApiExceptionMiddleware>();

app.MapControllers();
app.MapHub<SnapshotHub>("/snapshothub");

app.Run();

/// <summary>
/// Middleware wrapper for the ApiExceptionFilter.
/// </summary>
public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var errorResponse = new
            {
                Error = ex.GetType().Name,
                Detail = ex.Message,
                Timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = ex switch
            {
                InvalidOperationException => 400,
                ArgumentNullException => 400,
                ArgumentException => 400,
                _ => 500
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
