using AddresBookEL.IdentityEntities;
using AddresBookEL.Mappings;
using AddressBookBL.EmailSenderBusiness;
using AddressBookBL.Implementations;
using AddressBookBL.Interfaces;
using AddressBookDAL.ContextInfo;
using AddressBookDL.ImplementationsOfRepo;
using AddressBookDL.InterfacesOfRepo;
using AddressBookPL.CreateDefaultDatas;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//contexti ayarlayalým
builder.Services.AddDbContext<AddressBookContext>(options=> {
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"));

});

//identity ayarý eklenmeli
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 7;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.User.AllowedUserNameCharacters = "abcdefghýijklmnopqrstuvwxyzABCDEFGHIÝJKLMNOPQRSTUVWXYZ0123456789&_.-@*+~!?";
}).AddDefaultTokenProviders().
AddEntityFrameworkStores<AddressBookContext>();

//Automapper ayarý

builder.Services.AddAutoMapper(options=>
{
    options.AddExpressionMapping();
    options.AddProfile(typeof(Maps));
});

//servislerin DI yaþam döngüleri
builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddScoped<ICityManager, CityManager>();

builder.Services.AddScoped<IDistrictRepo, DistrictRepo>();
builder.Services.AddScoped<IDistrictManager, DistrictManager>();

builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<IAddressManager, AddressManager>();
builder.Services.AddScoped<IEmailSenderManager, EmailSenderManager>();

builder.Services.AddScoped<INeighborhoodRepo, NeighborhoodRepo>();
builder.Services.AddScoped<INeighborhoodManager, NeighborhoodManager>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.PrepareData(); //extension metottur

app.Run();// uygulama çalýþýr
