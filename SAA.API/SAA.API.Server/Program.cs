using Microsoft.EntityFrameworkCore;
using SAA.Application.Services;
using SAA.Infrastructure.Data;
using SAA.Infrastructure.Services;
using SAA.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// Configurar JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SecretKeyVerySecret1234567890";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name
        };
    });

builder.Services.AddAuthorization();

// Registrar SAADbContext con EF Core (InMemory para Producción, SQL Server para Desarrollo)
if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<SAADbContext>(options =>
        options.UseInMemoryDatabase("SAAProductionDb"));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");
    builder.Services.AddDbContext<SAADbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Register IApplicationDbContext mapping to SAADbContext
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SAADbContext>());

// Registrar servicios
builder.Services.AddScoped<SeedDataService>();
builder.Services.AddScoped<PostulanteService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MotorAdmisionService>();

// CORS policies si son necesarias
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Siembra de datos (Solo para desarrollo)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var seedService = services.GetRequiredService<SeedDataService>();
        await seedService.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al sembrar la base de datos.");
    }
}

app.Run();
