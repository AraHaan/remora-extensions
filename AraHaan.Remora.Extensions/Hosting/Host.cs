namespace AraHaan.Remora.Extensions.Hosting;

/// <summary>
/// A special Host class for creating an <see cref="HostBuilder" /> for a Bot.
/// </summary>
public static class Host
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
    /// </summary>
    /// <remarks>
    ///   The following defaults are applied to the returned <see cref="HostBuilder"/>:
    ///   <list type="bullet">
    ///     <item><description>set the <see cref="IHostEnvironment.ContentRootPath"/> to the location result of <see cref="Assembly.GetEntryAssembly()"/></description></item>
    ///     <item><description>load host <see cref="IConfiguration"/> from "DOTNET_" prefixed environment variables</description></item>
    ///     <item><description>load app <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IHostEnvironment.EnvironmentName"/>].json'</description></item>
    ///     <item><description>load app <see cref="IConfiguration"/> from User Secrets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly</description></item>
    ///     <item><description>load app <see cref="IConfiguration"/> from environment variables</description></item>
    ///     <item><description>configure the <see cref="ILoggerFactory"/> to log to the console, debug, and event source output</description></item>
    ///     <item><description>enables scope validation on the dependency injection container when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development'</description></item>
    ///     <item><description>configure the bot's services</description></item>
    ///   </list>
    /// </remarks>
    /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
    [RequiresPreviewFeatures]
    public static IHostBuilder CreateBotDefaultBuilder<TServiceConfigurator>(
        Assembly entryAssembly)
        where TServiceConfigurator : class, IBotServiceConfigurator
    {
        TServiceConfigurator.BeforeConfigure(entryAssembly);
        var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .UseContentRoot(Path.GetDirectoryName(entryAssembly.Location)!);
        TServiceConfigurator.ConfigureBotServices(hostBuilder);
        return hostBuilder;
    }
}
