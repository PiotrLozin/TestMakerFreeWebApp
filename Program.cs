using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add db context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:44484")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

var dbContext = app.Services.CreateScope().ServiceProvider.GetService<ApplicationDbContext>();
dbContext.Database.Migrate();

DbSeeder.Seed(dbContext);

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = (context) =>
    {
        //Wy³¹czenie stosowania pamiêci podrêcznej dla wszystkich plików statycznych
        context.Context.Response.Headers["Cache-Control"] = builder.Configuration["StaticFiles:Headers:Cache-Control"];
        context.Context.Response.Headers["Pragma"] = builder.Configuration["StaticFiles:Headers:Pragma"]; ;
        context.Context.Response.Headers["Expires"] = builder.Configuration["StaticFiles:Headers:Expires"]; ;
    }
});

app.UseCors();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
