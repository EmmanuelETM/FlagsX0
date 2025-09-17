using FlagsX0.Business.Services;
using FlagsX0.Business.UseCases;
using FlagsX0.Business.UseCases.Flags;
using FlagsX0.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// HTTPContextAccessor
builder.Services.AddHttpContextAccessor();

// Dependency Injection
builder.Services.AddScoped<FlagsUseCases>();
builder.Services.AddScoped<AddFlagUseCase>();
builder.Services.AddScoped<GetPaginatedFlagsUseCase>();
builder.Services.AddScoped<GetSingleFlagUseCase>();
builder.Services.AddScoped<UpdateFlagUseCase>();
builder.Services.AddScoped<DeleteFlagUseCase>();
builder.Services.AddScoped<IFlagUserDetails, FlagUserDetails>();

// Building the app
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    ctx.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();