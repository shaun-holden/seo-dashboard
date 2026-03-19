using GymBudgetApp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<GymBudgetApp.Services.NotesPanelState>();
builder.Services.AddScoped<GymBudgetApp.Services.PermissionService>();

var dbFolder = Environment.GetEnvironmentVariable("DB_PATH")
    ?? Directory.GetCurrentDirectory();
var dbPath = Path.Combine(dbFolder, "gymbudget.db");

// Persist Data Protection keys so auth cookies survive redeployments
var keysFolder = Path.Combine(dbFolder, "keys");
Directory.CreateDirectory(keysFolder);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("GymBudgetApp");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"), ServiceLifetime.Scoped);

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

// Stripe configuration
var stripeSecretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY")
    ?? builder.Configuration["Stripe:SecretKey"] ?? "";
if (!string.IsNullOrEmpty(stripeSecretKey))
    Stripe.StripeConfiguration.ApiKey = stripeSecretKey;

builder.Services.AddTransient<IEmailSender, GymBudgetApp.Services.EmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Ensure roles exist and assign roles to existing users without one
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    foreach (var roleName in new[] { "Parent", "Employee" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    var adminEmail = app.Configuration["AdminEmail"] ?? "deshaun@tntgym.org";
    var allUsers = userManager.Users.ToList();
    foreach (var user in allUsers)
    {
        var roles = await userManager.GetRolesAsync(user);
        if (!roles.Any())
        {
            var isAdmin = string.Equals(user.Email, adminEmail, StringComparison.OrdinalIgnoreCase);
            await userManager.AddToRoleAsync(user, isAdmin ? "Employee" : "Parent");
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();

// Stripe webhook handler
async Task<IResult> HandleStripeWebhook(HttpContext context, AppDbContext db)
{
    var webhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET")
        ?? app.Configuration["Stripe:WebhookSecret"] ?? "";
    var json = await new StreamReader(context.Request.Body).ReadToEndAsync();

    try
    {
        var stripeEvent = Stripe.EventUtility.ConstructEvent(json,
            context.Request.Headers["Stripe-Signature"], webhookSecret);

        if (stripeEvent.Type == "checkout.session.completed")
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (session != null)
            {
                var payment = db.Set<GymBudgetApp.Models.Payment>()
                    .FirstOrDefault(p => p.StripeSessionId == session.Id);
                if (payment != null)
                {
                    payment.Status = GymBudgetApp.Models.PaymentStatus.Paid;
                    payment.StripePaymentIntentId = session.PaymentIntentId;
                    payment.PaidAt = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
        }
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest();
    }
}

app.MapPost("/api/stripe-webhook", HandleStripeWebhook).AllowAnonymous();
app.MapPost("/stripe-webhook", HandleStripeWebhook).AllowAnonymous();

app.MapFallbackToPage("/_Host");

app.Run();
