using humidify.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
.AddInteractiveServerComponents();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("http://humidify-api.eu-west-1.elasticbeanstalk.com");
});

// CRITICAL FIX: Register the Sensor Data Service for Dependency Injection
// This tells Blazor: "When Home.razor asks for ISensorDataService, give it a SensorDataService."
builder.Services.AddScoped<ISensorDataService, SensorDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<humidify.Web.Components.App>()
.AddInteractiveServerRenderMode();

app.Run();