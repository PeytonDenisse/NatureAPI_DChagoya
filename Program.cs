using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== CORS =====
const string CorsPolicy = "AllowAngular";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, p =>
        p.WithOrigins(
                "http://localhost:4200",
                "http://18.188.37.201"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

// ===== Controllers + JSON =====
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// ===== DB Context =====
var connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<NatureDbContext>(o =>
    o.UseSqlServer(connectionString));

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Middleware =====

// CORS antes de controllers
app.UseCors(CorsPolicy);

// Swagger (lo dejamos siempre activo)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nature API v1");
    c.RoutePrefix = "swagger"; // => http://host:puerto/swagger
});

// ⚠️ Si tu contenedor NO expone HTTPS, comenta esto
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();