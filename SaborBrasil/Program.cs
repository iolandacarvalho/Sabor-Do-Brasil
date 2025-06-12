using Microsoft.EntityFrameworkCore;
using SaborBrasil.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 13))));

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles(); // Permite servir arquivos da pasta wwwroot

// Redireciona a rota raiz para index.html
app.MapGet("/", context =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});


app.MapDefaultControllerRoute(); // Configura as rotas padrão para os controladores

app.Run();
