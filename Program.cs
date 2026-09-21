using Microsoft.EntityFrameworkCore;
using APIRESTSTATE.Models.EntityFramework;
using System.Reflection; // Indispensable pour que Assembly fonctionne

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SeriesDbContext") ?? throw new InvalidOperationException("Connection string 'SerieDbContext' not found.");

builder.Services.AddDbContext<SerieDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddSwaggerGen(options =>
{
    // Permet à Swagger de trouver et lire les commentaires XML écrits
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });

}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
