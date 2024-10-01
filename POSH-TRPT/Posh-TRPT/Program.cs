using AutoMapper;
using Posh_TRPT.Extensions;
using Posh_TRPT_Services.Mapping;
using Serilog;
using Serilog.Exceptions;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();


builder.Host.UseSerilog((hostContext, configuration) =>
{
    configuration.ReadFrom.Configuration(hostContext.Configuration).Enrich.FromLogContext().Enrich.WithExceptionDetails().Enrich.FromLogContext();
});
//Add services to the container.


builder.Services.AddDatabase(builder.Configuration)
                  .AddRepositories()
                  .AddServices(); 

builder.Services.AddAutoMapper(typeof(AutoMapperProfile), typeof(VehicleDetailMapProfile), typeof(GeneralAddressMapProfile), typeof(DriverDocumentsMapProfile), typeof(ApplicationUserMapProfile), typeof(DriverRegisterMapProfile), typeof(TokenInfoUserMapProfile), typeof(TokenResponseMapProfile), typeof(CountryResponseMapProfile),
    typeof(CityResponseMapProfile), typeof(StateResponseMapProfile), typeof(MenuMasterMapProfile));
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
    cfg.AddProfile(new ApplicationUserMapProfile());
    cfg.AddProfile(new TokenInfoUserMapProfile());
    cfg.AddProfile(new TokenResponseMapProfile());
    cfg.AddProfile(new CountryResponseMapProfile());
    cfg.AddProfile(new CityResponseMapProfile());
    cfg.AddProfile(new StateResponseMapProfile());
    cfg.AddProfile(new DriverRegisterMapProfile());
    cfg.AddProfile(new DriverDocumentsMapProfile());
    cfg.AddProfile(new MenuMasterMapProfile());
    cfg.AddProfile(new GeneralAddressMapProfile());
    cfg.AddProfile(new VehicleDetailMapProfile());
});
builder.Services.AddSingleton(config.CreateMapper());

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseSession();
app.Use(async (context, next) =>
{
    var JWToken = context.Session.GetString("Token");
    if (!string.IsNullOrEmpty(JWToken))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    }
    await next();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=SignIn}/{id?}");

app.Run();
