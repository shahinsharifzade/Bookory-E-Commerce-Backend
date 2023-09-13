using Bookory.API.Extensions;
using Bookory.Business.ConfiguratoinService;
using Bookory.Core.Models.Identity;
using Bookory.Core.Models.Stripe;
using Bookory.DataAccess.ConfigurationService;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Use database 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

#region Stripe
//old
//builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
//StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;

//new
builder.Services.AddStripeServices(builder.Configuration);
    
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
#endregion


// Auth 
TokenOption tokenOption = builder.Configuration.GetSection("TokenOptions").Get<TokenOption>();

builder.Services.AddUserIdentityService();
builder.Services.AddJwtAuthenticationService(tokenOption.Audience, tokenOption.Issuer, tokenOption.SecurityKey);

//old
//builder.Services.AddRepositoriesService();

//new
builder.Services.AddBusinessServices();
builder.Services.AddRepositoriesService();
builder.Services.AddCustomServices(builder.Configuration);

//old
//builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings")); //Mail 
//builder.Services.AddAutoMapper(typeof(AutoMapperProfile)); // Mapper
//builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly); // Mapper
//builder.Services.AddValidatorsFromAssembly(typeof(AuthorPostDtoValidator).Assembly);
//builder.Services.AddFluentValidationAutoValidation(c => c.DisableDataAnnotationsValidation = true).AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Exceptions
//app.AddExceptionHandlerService();

#region Cors
app.UseCors();
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();
app.Run();
