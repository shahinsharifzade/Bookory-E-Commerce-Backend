using Bookory.API.Extensions;
using Bookory.Business.Utilities.Email.Settings;
using Bookory.Business.Utilities.Mapper;
using Bookory.Business.Utilities.Validators.AuthorValidator;
using Bookory.Core.Models.Identity;
using Bookory.Core.Models.Stripe;
using Bookory.DataAccess.Persistance.Context.EfCore;
using FluentValidation;
using FluentValidation.AspNetCore;
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

//StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

//StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("Stripe:SecretKey");

//Stripe
//builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
//StripeConfiguration.ApiKey = "sk_test_51Nn1IUDU36UGP8uCPNtMxVpoJRXD0wo8lan1PIBHV9XH03OEPvKwsIjxOJQWIDFmg14GEeZN783pjjbaX3H1XZuL00CcQcr6JG";
//StripeConfiguration.SetApiKey(builder.Configuration.GetSection("Stripe")["SecretKey"]);

//var options = new CardCreateOptions
//{
//    Source = "tok_visa_debit", //https://stripe.com/docs/testing?testing-method=tokens
//};
//var service = new CardService();
//service.Create("YourStripeUserId", options);
//StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe").Get<StripeSettings>().SecretKey;

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;
    
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

builder.Services.AddRepositoriesService();
builder.Services.AddService();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings")); //Mail 
builder.Services.AddAutoMapper(typeof(AutoMapperProfile)); // Mapper
//builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly); // Mapper
builder.Services.AddValidatorsFromAssembly(typeof(AuthorPostDtoValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation(c => c.DisableDataAnnotationsValidation = true).AddFluentValidationClientsideAdapters();

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
