using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using TheEstate.Api;
using TheEstate.Data;
using static TheEstate.Data.Constants;

var builder = WebApplication.CreateBuilder(args);


#region Swaggar
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "miCASA Api",
        Version = "v1",
        Description = "Api Documentation for miCASA"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
#endregion


#region Cors Policy
string Cors = "Cors";
builder.Services.AddCors(c =>
{
    c.AddPolicy(Cors, cors =>
    {
        cors.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});
#endregion


#region JWT Authentication
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.SaveToken = true;
    o.RequireHttpsMetadata = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
    };
});
builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("AdminOnly", p =>
    //{
    //    p.RequireRole("ADMIN", "HR");
    //});

    options.AddPolicy(JWTClaimTypes.ResidentCategory, p =>
    {
        p.RequireClaim(JWTClaimTypes.ResidentCategory, ResidentCategories.PRIMARY.ToString());
    });

    //options.AddPolicy("PRIMARY_RESIDENT", p => {
    //    p.RequireAssertion(a => a.User.HasClaim(claim => claim.Type.Equals(ClaimTypes.Role) && claim.Value.Equals("PRIMARY")));
    //});
});
#endregion


#region Serilog
string dirPath = Path.Combine(AppContext.BaseDirectory, "logs\\");

bool exists = Directory.Exists(dirPath);
if (!exists) Directory.CreateDirectory(dirPath);

try
{
    Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(path: dirPath, rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Verbose()
                    .CreateLogger();
}
catch (Exception)
{
    Log.CloseAndFlush();
}
#endregion



var app = builder.Build();

app.RegisterAuthApi();
app.RegisterProfileApi();
app.RegisterEstateApi();
app.RegisterResidentApi();
app.RegisterPropertyApi();
app.RegisterHouseholdApi();
app.RegisterVisitorApi();
app.RegisterBillingApi();
app.RegisterZoneApi();
app.RegisterStreetApi();
app.RegisterInvoiceApi();
app.RegisterBillingElementApi();
app.RegisterCfgApi();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateTime.Now.AddDays(index),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(Cors);
app.UseAuthentication();
//app.UseMiddleware<Apikey>();
app.UseAuthorization();
app.Run();

//internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}