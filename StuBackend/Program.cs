using Microsoft.EntityFrameworkCore;
using StuBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar soporte para controladores
builder.Services.AddControllers();

// 2. Agregar generador de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=flota_stu.db"));

var app = builder.Build();

// 3. Habilitar la interfaz gráfica de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "STU Flota API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();