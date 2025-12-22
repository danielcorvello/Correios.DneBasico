using MicroElements.NSwag.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Json;
using NSwag;
using System.Text.Json.Serialization;

GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();
GridifyGlobalConfiguration.CaseSensitiveMapper = false;
GridifyGlobalConfiguration.AllowNullSearch = true;
GridifyGlobalConfiguration.CaseInsensitiveFiltering = true;
GridifyGlobalConfiguration.IgnoreNotMappedFields = true;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        // Desabilita autenticação JWT no Swagger
        o.EnableJWTBearerAuth = false;
        // Exibe os nomes dos DTOs de forma curta no Swagger
        o.ShortSchemaNames = true;

        // Desativa o uso de segmentos de caminho automáticos
        o.AutoTagPathSegmentIndex = 0;

        // Remove esquemas de requisição vazios do Swagger
        o.RemoveEmptyRequestSchema = true;
    });

builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddDbContext<DneBasicoDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("eDNE")));

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseFastEndpoints(c =>
{
    // Usa os nomes curtos para os endpoints
    // Exemplo: "GetCep" ao invés de "Correios.DneBasico.Features.GetCep"
    // Isso torna a documentação mais limpa e fácil de ler
    c.Endpoints.ShortNames = true;
}).UseSwaggerGen(o =>
{
    o.Path = "/swagger/{documentName}/swagger.json";
    o.PostProcess = (document, request) =>
    {
        document.Info.Title = "Correios e-DNE API";
        document.Info.Version = "v1";
        document.Info.Contact = new OpenApiContact
        {
            Name = "CORVELLO",
            Email = "danielcorvello@gmail.com"
        };
    };
}, ui =>
{
    // Título da documentação no Swagger UI
    ui.DocumentTitle = "Correios e-DNE API";
    // Define a expansão dos documentos como "list"
    ui.DocExpansion = "list";
    // Desativa o botão "Try it out" como clicado por padrão
    ui.DeActivateTryItOut();
    // Exibe os IDs das operações no Swagger UI
    ui.ShowOperationIDs();
});

app.UseHttpsRedirection();

app.Run();