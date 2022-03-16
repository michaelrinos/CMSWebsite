using CMSWebsite;
using CMSWebsite.Models;
using CMSWebsite.Services;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
{
    options.FileProviders.Add(new DatabaseFileProvider(builder.Configuration.GetValue<string>("CMSConnection"), new CMSService())); ;
});

// one per request
builder.Services.AddScoped<ISqlDataProvider, SqlDataProvider>();
builder.Services.AddScoped<ISqlDataProviderCMS, SqlDataProviderCMS>();


builder.Services.AddTransient<ICMSService, CMSService>();

// AddDbCotext allows you to configure it at the same time.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:DefaultConnection"]
        )
);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

SeedData.EnsurePopulated(app, builder.Configuration.GetConnectionString("CMSConnection"), new CMSService());

app.Run();
