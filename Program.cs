using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mercharteria.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using mercharteria.Services;
using mercharteria.Helpers;
using DotNetEnv;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno
Env.Load();

// Crear el objeto de credenciales de Firebase
var credentials = new Dictionary<string, string>
{
    { "type", "service_account" },
    { "project_id", Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID") },
    { "private_key_id", Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY_ID") },
    { "private_key", Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY") },
    { "client_email", Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL") },
    { "client_id", Environment.GetEnvironmentVariable("FIREBASE_CLIENT_ID") }
};

// Inicializar Firebase
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromJson(System.Text.Json.JsonSerializer.Serialize(credentials))
});
builder.Services.AddSingleton<FirestoreService>();


// Add services to the container
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession();

//Firebase Storage
builder.Services.AddSingleton<FirebaseStorageHelper>();


builder.Services.AddTransient<IEmailSender, DummyEmailSender>();

// Configure cookie authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            context.Response.Redirect("/");
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Initialize Database with roles and users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // Create roles
        var roles = new[] { "Administrador", "Cliente" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create admin user
        var adminEmail = "admin@mail.com";
        var adminPassword = "Admin123$";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrador");
            }
        }

        // Create client user
        var clientEmail = "cliente@mail.com";
        var clientPassword = "Cliente123$";
        if (await userManager.FindByEmailAsync(clientEmail) == null)
        {
            var clientUser = new IdentityUser
            {
                UserName = clientEmail,
                Email = clientEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(clientUser, clientPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(clientUser, "Cliente");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al crear roles o usuarios.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Configure role-based routing
app.MapGet("/", async context =>
{
    var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
    var user = await userManager.GetUserAsync(context.User);

    if (user == null)
    {
        context.Response.Redirect("/Home/Index");
    }
    else
    {
        if (await userManager.IsInRoleAsync(user, "Administrador"))
        {
            context.Response.Redirect("/Productos/Admin");
        }
        else
        {
            context.Response.Redirect("/Home/Index");
        }
    }
});









app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
