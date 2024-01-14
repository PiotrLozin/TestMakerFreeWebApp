using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
