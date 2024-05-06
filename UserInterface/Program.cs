using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using UserInterface.Data;
using UserInterface.Data.Authentication;
using UserInterface.Data.HttpHeader;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddAuthentication();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy => policy.RequireAuthenticatedUser());
    //options.AddPolicy("SuperadminPolicy", policy => policy.RequireRole("Superadmin"));
});
builder.Services.AddScoped<HttpClient>(sp => new HttpClient(new AddHeadersDelegatingHandler(config)) 
{
    BaseAddress = new Uri("https://airtime-backend.azurewebsites.net/") 
});
builder.Services.AddScoped<AirtimeAuthenticationProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AirtimeAuthenticationProvider>();
builder.Services.AddScoped<ILoginService, AirtimeAuthenticationProvider>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();