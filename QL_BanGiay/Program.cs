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
builder.Services.AddScoped<ICheckout, CheckoutRepository>();
builder.Services.AddScoped<IOrder, OrderRepo>();
builder.Services.AddScoped<IBill, BillRepo>();
builder.Services.AddScoped<IAddress, AddressRepo>();
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IChart, ChartRepo>();
builder.Services.AddScoped<IRole, RoleRepo>();
builder.Services.AddRazorPages();
builder.Services.AddSession();

builder.Services.AddDbContext<QlyBanGiayContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => 
{ 
    options.LoginPath = "/account/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

    options.Cookie.MaxAge = options.ExpireTimeSpan;
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = false;
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
         policy => policy.RequireRole("Admin"));
});
var app = builder.Build();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHFqVkNrWU5GdkBAXWFKblB8RWtTe11gFChNYlxTR3ZbQF1iTH5bckZkWXZb;Mgo+DSMBPh8sVXJ1S0d+X1RPc0BAXXxLflF1VWBTf1Z6cFZWACFaRnZdQV1nS35Sc0VrXXxZcnFQ;ORg4AjUWIQA/Gnt2VFhhQlJBfVpdX2NWfFN0RnNadV54flFFcDwsT3RfQF5jTXxTd0BjUXtdd3BXQw==;MTc5MTgyNUAzMjMxMmUzMTJlMzMzNVgxUTlaU004dmlNdm1tNFh1OE1GTi9rZWJPTnhZdElQQW9oQnlxNXVWdVU9;MTc5MTgyNkAzMjMxMmUzMTJlMzMzNVZoMkJIT0w1UEk2dy80dkpaZWZNaTZERVRXSUZlZDNKN0RiVVFsb0NWSEk9;NRAiBiAaIQQuGjN/V0d+XU9Hc1RHQmFJYVF2R2BJfFRwfF9CY0wxOX1dQl9gSXpRdkRmWXdac3xWTmU=;MTc5MTgyOEAzMjMxMmUzMTJlMzMzNUgxY3NHRDl1L3BQZ0VNbmI2eVpLRUhYOCt0bTF1VVlhL1hiWmZqMWFDdDg9;MTc5MTgyOUAzMjMxMmUzMTJlMzMzNWkrR0pEN1VEZmJTZWEwUWNuNmRUSVhYNXJ1QTZrTzQ4b0pXUU1BUW5laTg9;Mgo+DSMBMAY9C3t2VFhhQlJBfVpdX2NWfFN0RnNadV54flFFcDwsT3RfQF5jTXxTd0BjUXtdeH1TQw==;MTc5MTgzMUAzMjMxMmUzMTJlMzMzNU8yM2NjT0tvM1BYUFRFRlFnaWVHYnA4MFh2T05SSEZWdkdycTBOcmd1MUk9;MTc5MTgzMkAzMjMxMmUzMTJlMzMzNUMvR2RWTUFMVG9hUjd1MEcvS0g2T29PTERlaUNKOU9JUDVQSk1Yd1M3MmM9;MTc5MTgzM0AzMjMxMmUzMTJlMzMzNUgxY3NHRDl1L3BQZ0VNbmI2eVpLRUhYOCt0bTF1VVlhL1hiWmZqMWFDdDg9");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
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
