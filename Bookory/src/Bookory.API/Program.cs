using Bookory.API.Extensions;
using Bookory.Business.ConfiguratoinService;
using Bookory.DataAccess.ConfigurationService;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and configure JSON serialization settings
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add services related to database, Stripe, user identity, JWT authentication, and other business services
builder.Services.AddDatabaseSevice(builder.Configuration);
builder.Services.AddStripeServices(builder.Configuration);
builder.Services.AddCorsService();
builder.Services.AddUserIdentityService();
builder.Services.AddJwtAuthenticationService(builder.Configuration);
builder.Services.AddBusinessServices();
builder.Services.AddRepositoriesService();
builder.Services.AddCustomServices(builder.Configuration);

// Add API documentation services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenService(); // Add the custom Swagger service

var app = builder.Build();

// Enable Swagger UI in both development and production environments
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Define the Swagger endpoint for the API documentation
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bookory API v1");
    options.RoutePrefix = string.Empty; // Swagger UI available at the root URL
});

// Initialize the database asynchronously
await app.InitDatabaseAsync();

// Use HTTPS redirection for secure communication
app.UseHttpsRedirection();
app.AddExceptionHandlerService();

// Enable CORS for cross-origin requests
app.UseCors();

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Serve static files (if necessary)
app.UseStaticFiles();

// Map controllers for routing
app.MapControllers();

// Run the application asynchronously
await app.RunAsync();


//using Bookory.API.Extensions;
//using Bookory.Business.ConfiguratoinService;
//using Bookory.DataAccess.ConfigurationService;
//using Bookory.DataAccess.Initalizers;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


//builder.Services.AddDatabaseSevice(builder.Configuration);

//builder.Services.AddStripeServices(builder.Configuration);

//builder.Services.AddCorsService();

//builder.Services.AddUserIdentityService();
//builder.Services.AddJwtAuthenticationService(builder.Configuration);

//builder.Services.AddBusinessServices();
//builder.Services.AddRepositoriesService();
//builder.Services.AddCustomServices(builder.Configuration);

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGenService();

//var app = builder.Build();
//app.UseCors();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//await app.InitDatabaseAsync();

//app.UseHttpsRedirection();
//app.AddExceptionHandlerService();


//app.UseAuthentication();
//app.UseAuthorization();

//app.UseStaticFiles();
//app.MapControllers();
//await app.RunAsync();