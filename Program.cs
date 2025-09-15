using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AgriEnergyConnect.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Denied";
    });

// Configuração híbrida (SQL + Mongo + Mode)
builder.Services.AddSingleton<DbConfig>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var sqlCs = config.GetConnectionString("DefaultConnection") ?? "";
    var mongoCs = config.GetConnectionString("MongoConnection") ?? "mongodb://localhost:27017";
    var mongoDb = config.GetConnectionString("MongoDatabase") ?? "AgriEnergyConnect";

    var modeStr = config["Persistence:Mode"] ?? "Sql";
    var mode = Enum.TryParse<PersistenceMode>(modeStr, out var parsedMode) ? parsedMode : PersistenceMode.Sql;

    return new DbConfig(sqlCs, mongoCs, mongoDb, mode);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
