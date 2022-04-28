using System.Globalization;
using System.Resources;
using AsposeTestWork.Web;
using Microsoft.AspNetCore.Mvc.Razor;
using AspNetCore.Unobtrusive.Ajax;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");
builder.Services.AddUnobtrusiveAjax();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var supportedCulture = Helper.GetAvailableCultures().Select(n => n.Name).ToArray();
var localisationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCulture[0])
    .AddSupportedCultures(supportedCulture)
    .AddSupportedUICultures(supportedCulture);
app.UseRequestLocalization(localisationOptions);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseUnobtrusiveAjax();
app.Urls.Clear();
app.Urls.Add("http://*:" + Environment.GetEnvironmentVariable("PORT"));
app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

