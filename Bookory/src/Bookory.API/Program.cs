using Bookory.API.Extensions;
using Bookory.Business.ConfiguratoinService;
using Bookory.DataAccess.ConfigurationService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddDatabaseSevice(builder.Configuration);

builder.Services.AddStripeServices(builder.Configuration);

builder.Services.AddCorsService();

builder.Services.AddUserIdentityService();
builder.Services.AddJwtAuthenticationService(builder.Configuration);

builder.Services.AddBusinessServices();
builder.Services.AddRepositoriesService();
builder.Services.AddCustomServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.AddExceptionHandlerService();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();
app.Run();
