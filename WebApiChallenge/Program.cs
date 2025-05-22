using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Stripe;
using WebApiChallenge.Context;
using WebApiChallenge.Interfaces;
using WebApiChallenge.Models;
using WebApiChallenge.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDentistaRepository, DentistaRepository>();
builder.Services.AddScoped<IAtendimentoUsuarioRepository, AtendimentoUsuarioRepository>();
builder.Services.AddScoped<IContatoUsuarioRepository, ContatoUsuarioRepository>();
builder.Services.AddScoped<IPrevisaoRepository, PrevisaoRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext string connection
builder.Services.AddDbContext<WebApiDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddPredictionEnginePool<SentimentModelInput, SentimentModelOutput>()
    .FromFile(modelName: "SentimentAnalysisModel", filePath: "sentiment_model.zip", watchForChanges: true);

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddHttpClient();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddSingleton<IStripeClient, StripeClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<StripeSettings>>().Value;
    return new StripeClient(config.SecretKey);
});

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
