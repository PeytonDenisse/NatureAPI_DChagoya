using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// conexión 
var connectionString = builder.Configuration.GetConnectionString("SqlServer");
                      
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddDbContext<NatureDbContext>(o =>
    o.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

//
app.MapControllers();

app.Run();