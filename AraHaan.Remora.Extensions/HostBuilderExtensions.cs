namespace AraHaan.Remora.Extensions;

/// <summary>
/// Extensions for <see cref="IHostBuilder" />.
/// </summary>
public static class HostBuilderExtensions
{
    /// <summary>
    /// Enables console support, builds and starts the host, and waits for Ctrl+C or SIGTERM to shut down.
    ///
    /// When shut down starts, closes and flushes the serilog loggers.
    /// </summary>
    /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the console.</param>
    /// <returns>A <see cref="Task"/> that only completes when the token is triggered or shutdown is triggered.</returns>
    [RequiresPreviewFeatures]
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public static async Task RunBotConsoleAsync<TServiceConfigurator>(
        this IHostBuilder hostBuilder,
        CancellationToken cancellationToken = default)
        where TServiceConfigurator : class, IBotServiceConfigurator
    {
        using var host = hostBuilder.UseConsoleLifetime().Build();
        var options = host.Services.GetRequiredService<IOptions<SlashUpdateServiceOptions>>().Value;
        await host.RunAsync(cancellationToken).ConfigureAwait(false);
        TServiceConfigurator.AfterApplicationShutdown(options);
    }
}
