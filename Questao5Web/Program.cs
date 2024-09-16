using Microsoft.EntityFrameworkCore;
using Questao5Web.DataContext;
using MediatR;
using Questao5Web.Application.Handler;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Configurar o DbContext para usar SQLite com a string de conexão do appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do MediatR
builder.Services.AddMediatR(typeof(CreateMovimentoCommandHandler).Assembly);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
    {

        swagger.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Api Conta Corrente.",
            Description = "Api para Movimentar a Conta dos nossos clientes.",
            Contact = new OpenApiContact()
            {
                Name = "Simeão Almeida",
                Email = "simer27@hotmail.com"
            }
        });
    
        var arquivoSwaggerXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var diretorioArquivoXml= Path.Combine(AppContext.BaseDirectory, arquivoSwaggerXml);
        swagger.IncludeXmlComments(diretorioArquivoXml);
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
