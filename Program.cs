using Microsoft.EntityFrameworkCore;
using BibliotecaApi.DbModels;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Asp.Versioning;
using BibliotecaApi.Filters;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme 
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(CustomExceptionFilter)); //Adding a global filter
}).AddJsonOptions(x=>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles //to ignore circular references
).AddNewtonsoftJson();

// Configuración de servicios
builder.Services.AddAllService();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"])),
        ClockSkew = TimeSpan.Zero
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BibliotecaDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(x =>
    {
         options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });
});

// DB en memoria
builder.Services.AddDbContext<BibliotecaDbContext>(options => 
{
    options.UseInMemoryDatabase("AuthorDb");
}, ServiceLifetime.Singleton);

//versionamiento
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Datos Semilla
var context = app.Services.GetRequiredService<BibliotecaDbContext>();
DBSeeder.Seed(context);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

