using Microsoft.EntityFrameworkCore;
using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Dados.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ScreenSoundContext>((options) =>
{
    options
        .UseSqlServer(builder.Configuration["ConnectionStrings:ScreenSoundDB"])
        .UseLazyLoadingProxies();
});

// Injetar mais um serviço
builder.Services
    .AddIdentityApiEndpoints<PessoaComAcesso>()
    .AddEntityFrameworkStores<ScreenSoundContext>();

// Injetar autorização
builder.Services.AddAuthorization();

builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();
builder.Services.AddTransient<DAL<Genero>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ScreenSoundWeb", policy =>
    {
        policy
            .WithOrigins("https://localhost:7236", "http://localhost:7236")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("ScreenSoundWeb");

app.UseStaticFiles();

app.UseAuthorization();

app.AddEndPointsArtistas();
app.AddEndPointsMusicas();
app.AddEndPointGeneros();

// Adicionar uma camada de autorização
app.MapGroup("auth").MapIdentityApi<PessoaComAcesso>()
    .WithTags("Autorização");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();