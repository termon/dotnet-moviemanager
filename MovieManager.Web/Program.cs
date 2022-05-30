
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieManager.Data.Services;
using MovieManager.Web;

var builder = WebApplication.CreateBuilder(args);

// ** Enable Cookie Authentication via extension method **
builder.Services.AddCookieAuthentication();

// ** Add Cookie and Jwt Authentication via extension method **
// builder.Services.AddCookieAndJwtAuthentication(builder.Configuration);
// ** Enable Cors for and webapi endpoints provided **
// builder.Services.AddCors();

// Add Services to DI         
builder.Services.AddTransient<IUserService,UserServiceDb>();
builder.Services.AddTransient<IMovieService,MovieServiceDb>();
builder.Services.AddTransient<IEmailService,EmailServiceMailTrap>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// ** Required to enable asp-authorize Taghelper **            
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// configure mvc
builder.Services.AddControllersWithViews();

var app = builder.Build();

// now run the application

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    Seeder.Seed(
        app.Services.GetService<IUserService>(),
        app.Services.GetService<IMovieService>()
    );
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ** configure cors to allow full cross origin access to any webapi end points **
//app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

/* Enable site Authentication/Authorization */
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();

