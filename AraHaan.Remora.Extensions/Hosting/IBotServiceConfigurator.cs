namespace AraHaan.Remora.Extensions.Hosting;

/// <summary>
/// Interface to define a class to use that will configure the services to use for the Bot.
/// </summary>
public interface IBotServiceConfigurator
{
    /// <summary>
    /// Code to run before <see cref="ConfigureBotServices"/> is run.
    /// </summary>
    /// <param name="entryAssembly">The entry assebly to the bot application.</param>
    [RequiresPreviewFeatures]
    public static abstract void BeforeConfigure(Assembly entryAssembly);

    /// <summary>
    /// Configures the services to use for the bot.
    /// </summary>
    [RequiresPreviewFeatures]
    public static abstract void ConfigureBotServices(IHostBuilder hostBuilder);

    /// <summary>
    /// Code to run after the application shuts down.
    /// </summary>
    [RequiresPreviewFeatures]
    public static abstract void AfterApplicationShutdown();
}
