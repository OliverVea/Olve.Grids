using UI.Blazor.Components;
using UI.Core.Services;

namespace UI.Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder
            .Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddServices();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();


        app.UseAntiforgery();

        app.MapStaticAssets();
        app
            .MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}