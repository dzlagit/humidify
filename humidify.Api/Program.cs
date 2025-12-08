using humidify.Api.Data;
using humidify.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// Load connection string from configuration (AWS env variable will override it)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Required for AWS Load Balancer HTTPS handling
app.Use((context, next) =>
{
    var proto = context.Request.Headers["X-Forwarded-Proto"].FirstOrDefault();
    if (!string.IsNullOrEmpty(proto))
        context.Request.Scheme = proto;

    return next();
});

// Development-only Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Required AWS health check endpoint
app.MapGet("/health", () => "OK");

app.Run();
