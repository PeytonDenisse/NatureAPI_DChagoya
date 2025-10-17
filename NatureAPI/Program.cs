using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;

var builder = WebApplication.CreateBuilder(args);


const string CorsPolicy = "AllowAngular";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, p =>
        p.WithOrigins("http://localhost:4200") 
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// ===== JSON y Controllers =====
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
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Habilitar CORS antes de MapControllers
app.UseCors(CorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();