using Contracts.DAL.App;
using DAL.App.DTO.MappingProfiles;
using DAL.App.EF;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            o => { o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); })
        .ConfigureWarnings(w =>
            w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
);


builder.Services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile),
    typeof(AutoMapperProfile));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsAllowAll", builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        });
    }
);

// add support for api versioning
builder.Services.AddApiVersioning(options => { options.ReportApiVersions = true; });
// add support for m2m api documentation
builder.Services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });



var app = builder.Build();

using var serviceScope =
    app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
using var ctx = serviceScope.ServiceProvider.GetService<AppDbContext>();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsAllowAll");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");
});
app.MapControllers();
app.Run();