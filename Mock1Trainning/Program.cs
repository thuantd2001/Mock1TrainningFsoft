using Microsoft.EntityFrameworkCore;
using Mock1Trainning.Common;
using Mock1Trainning.Data;
using Mock1Trainning.Logging;
using Mock1Trainning.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("log/villalogs.txt", rollingInterval: RollingInterval.Day).CreateBootstrapLogger();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnetion"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, Logging>();
builder.Services.AddSingleton<IVillaRepository, VillaRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
