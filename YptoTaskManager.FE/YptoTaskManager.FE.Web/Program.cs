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

        var app = builder.Build();

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
