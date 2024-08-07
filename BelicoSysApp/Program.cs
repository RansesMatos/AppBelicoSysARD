using BelicoSysApp.Services;
using DinkToPdf.Contracts;
using DinkToPdf;
using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IApiServiceArma, ApiServiceArma>();
builder.Services.AddScoped<IApiServicePertrecho, ApiServicePertrecho>();
builder.Services.AddScoped<IApiServiceAsignacion, ApiServiceAsingacion>();
builder.Services.AddSingleton<IConverter, SynchronizedConverter>(sp =>
new SynchronizedConverter(new PdfTools()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Register the DinkToPdf services


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
