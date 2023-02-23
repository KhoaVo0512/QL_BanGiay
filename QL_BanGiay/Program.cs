global using CodesByAniz.Tools;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using QL_BanGiay.Interface;
using QL_BanGiay.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    PreventDuplicates = true,
    CloseButton = true
});
builder.Services.AddScoped<IAccount, AccountRepository>();
builder.Services.AddScoped<IProductDetails, ProductDetails>();
builder.Services.AddScoped<ICollection, CollectionRepo>();
builder.Services.AddScoped<IBrand, BrandRepo>();
builder.Services.AddScoped<IPurchaseOrder, PurchaseOrderRepo>();
builder.Services.AddScoped<IShoe, ShoeRepo>();
builder.Services.AddScoped<ISize, SizeRepo>();
builder.Services.AddScoped<ISupplier, SupplierRepo>();
builder.Services.AddScoped<IProduce, ProduceRepo>();
builder.Services.AddScoped<IProvince, ProvinceRepo>();
builder.Services.AddScoped<IDistrict, DistrictRepo>();
builder.Services.AddScoped<ICommune, CommuneRepo>();
builder.Services.AddScoped<IWareHouse, WareHouseRepo>();
builder.Services.AddScoped<IPrice, PriceRepo>();
builder.Services.AddScoped<IShoeDetails, ShoeDetailsRepo>();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<QlyBanGiayContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => 
{ 
    options.LoginPath = "/account/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(3);
    options.Cookie.MaxAge = options.ExpireTimeSpan;
    options.SlidingExpiration = true;   
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
         policy => policy.RequireRole("Admin"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseRouting();
app.UseNToastNotify();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
     name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.Run();
