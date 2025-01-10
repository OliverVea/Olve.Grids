using Olve.Utilities.AsyncOnStartup;
using UI.Blazor.Components;
using UI.Blazor.Interop;
using UI.Core;

namespace UI.Blazor;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder
            .Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddLogging(c => c.AddConsole());

        builder.Services.AddServices();
        builder.Services.AddBlazorServices();

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

        await app.Services.RunAsyncOnStartup();
        await app.RunAsync();

        return 0;
    }
}