using CMSWebsite;
using CMSWebsite.Models;
using CMSWebsite.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
{
    options.FileProviders.Add(
        new DatabaseFileProvider(
            builder.Configuration.GetValue<string>("ConnectionStrings:CMSConnection")
            )
        );
});

// one per request
builder.Services.AddScoped<ISqlDataProvider, SqlDataProvider>();
builder.Services.AddScoped<ISqlDataProviderCMS, SqlDataProviderCMS>();


builder.Services.AddTransient<IViewRepository, EFViewsRepository>();
builder.Services.AddTransient<ICMSService, CMSService>();

builder.Services.Configure<AppSettings>(options =>
{
    builder.Configuration.GetSection("AppSettings").Bind(options);
});


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

//app.UseHttpsRedirection();

//app.UseRouting();

//app.UseAuthorization();
app.UseStaticFiles();

app.MapDefaultControllerRoute();


// Page 171 example showing how to add params into the url
app.MapControllerRoute("pagination",
    "Products/Page{productPage}",
    new { Controller = "Dynaic", action = "Index" }
    );


SeedData.EnsurePopulated(app);

app.Run();
