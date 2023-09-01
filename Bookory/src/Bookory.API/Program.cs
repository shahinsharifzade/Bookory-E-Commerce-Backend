using Bookory.Business.Utilities.Validators.AuthorValidator;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.Business.Utilities.Email.Settings;
using Bookory.Business.Utilities.Mapper;
using Microsoft.EntityFrameworkCore;
using Bookory.Core.Models.Identity;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Bookory.API.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Use database 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

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
builder.Services.AddFluentValidationAutoValidation(c=>c.DisableDataAnnotationsValidation = true).AddFluentValidationClientsideAdapters();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();
app.Run();
