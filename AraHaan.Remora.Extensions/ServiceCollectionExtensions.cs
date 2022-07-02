namespace AraHaan.Remora.Extensions;

/// <summary>
/// Extensions to <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Discord Gateway Client Options to the collection.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to use.</param>
    /// <param name="gatewayClientOptionsFactory">The factory to use to create the client options.</param>
    /// <returns>The original collection to be used for chaining.</returns>
    public static IServiceCollection AddDiscordGatewayClientOptions(
        this IServiceCollection serviceCollection,
        Func<IServiceProvider, IConfigureOptions<DiscordGatewayClientOptions>>
            gatewayClientOptionsFactory)
    {
        _ = serviceCollection
            .AddOptions()
            .AddSingleton(
                serviceProvider => gatewayClientOptionsFactory(serviceProvider));
        return serviceCollection;
    }

    /// <summary>
    /// Adds the slash update service to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="guildSnowflakeFactory">The factory to use to create the <see cref="Snowflake"/> for the slash update service.</param>
    /// <returns>The <see cref="IServiceCollection"/> used for chaining.</returns>
    public static IServiceCollection AddSlashUpdateService(
        this IServiceCollection serviceCollection,
        Func<IServiceProvider, Snowflake> guildSnowflakeFactory)
        => serviceCollection
            .AddSingleton(
                serviceProvider =>
                {
                    var guildId = guildSnowflakeFactory(serviceProvider);
                    return serviceProvider.Configure<SlashUpdateServiceOptions>(
                        o => o.Guild = guildId);
                })
            .AddHostedService<SlashUpdateService>();

    /// <summary>
    /// Reversed Engineered Configure method for configuring types after an <see cref="IServiceProvider"/> is created.
    /// </summary>
    /// <typeparam name="TOptions">The options to configure.</typeparam>
    /// <param name="_">The <see cref="IServiceProvider"/> to use (not really it's discarded).</param>
    /// <param name="configureOptions">The options action to run.</param>
    /// <returns>The <see cref="IConfigureOptions{TOptions}"/> instance which will hold the configured values.</returns>
    public static IConfigureOptions<TOptions> Configure<TOptions>(
        this IServiceProvider _,
        Action<TOptions> configureOptions)
        where TOptions : class
        => new ConfigureNamedOptions<TOptions>(
            Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
}
