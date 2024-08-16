using Microsoft.EntityFrameworkCore;
using F1RestAPI.Data;
using F1RestAPI.Services.Drivers;
using F1RestAPI.Services.Constructors;
using F1RestAPI.Services.DriversConstructors;
using Prometheus;
using F1RestAPI.Services.Backgrounds;
using F1RestAPI.Middlewares;
using F1RestAPI.Services.IpConnectionCounts;
using F1RestAPI.Services.IpCountryStateServices;
using F1RestAPI.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using F1RestAPI.Services.Users;
using Microsoft.OpenApi.Models;
using F1RestAPI.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IConstructorService, ConstructorService>();
builder.Services.AddScoped<IDriverConstructorService, DriverConstructorService>();
builder.Services.AddScoped<IIpConnectionCount, IpConnectionCountService>();
builder.Services.AddScoped<IIpCountryStateService, IpCountryStateService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHostedService<MetricUpdateService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "F1 Rest API", Version = "v1" });

    c.DocumentFilter<HideNonGetEndpointsDocumentFilter>();
    c.DocumentFilter<HideSchemasDocumentFilter>();
});

var secretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMetricServer();
app.UseHttpMetrics();

app.Use(async (context, next) =>
{
    var ipConnectionCountService = context.RequestServices.GetRequiredService<IIpConnectionCount>();
    var middleware = new IPLoggingMiddleware(next, ipConnectionCountService);
    await middleware.InvokeAsync(context);
});

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
