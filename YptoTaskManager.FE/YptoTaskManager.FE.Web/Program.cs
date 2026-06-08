using Fluxor;
using YptoTaskManager.FE.Web.Components;
using YptoTaskManager.FE.Web.Services.Tasks;
using YptoTaskManager.FE.Web.Services.Users;

namespace YptoTaskManager.FE.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var apiUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

        builder.Services.AddScoped<ITaskItemsApiClient, TaskItemsApiClient>();
        builder.Services.AddScoped<IUsersApiClient, UsersApiClient>();

        builder.Services.AddFluxor(options =>
        {
            options.ScanAssemblies(typeof(Program).Assembly);
        });

        builder.Services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });

        var app = builder.Build();

        var supportedCultures = new[] { "en", "fr", "nl" };

        app.UseRequestLocalization(options =>
        {
            options.SetDefaultCulture("en");
            options.AddSupportedCultures(supportedCultures);
            options.AddSupportedUICultures(supportedCultures);
        });

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
