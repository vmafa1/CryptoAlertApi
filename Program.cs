using CryptoAlertApi.Models;
using CryptoAlertApi.Services;
using Azure.Identity;



var builder = WebApplication.CreateBuilder(args);

// ✅ This is where services are configured


builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


// ✅ This line ensures environment variables are loaded
// ✅ Add environment variables to configuration
// ✅ Ensure environment variables are loaded
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // ← This is critical




// ✅ Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Add telemetry and HTTP client
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHttpClient();

// ✅ Register alert services
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<TelegramService>();

var app = builder.Build();

// ✅ Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();