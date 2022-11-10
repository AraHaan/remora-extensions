namespace AraHaan.Remora.Extensions.Services;

/// <summary>
/// BackgroundService to update Discord Slash Commands.
/// </summary>
public sealed class SlashUpdateService : BackgroundService
{
    private readonly ILogger<SlashUpdateService> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="SlashUpdateService" />.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/>.</param>
    /// <param name="appLifetime">The application lifetime.</param>
    /// <param name="slashService">The Discord Slash Service.</param>
    /// <param name="options">The service options.</param>
    /// <param name="configurator">The configurator.</param>
    public SlashUpdateService(
        ILogger<SlashUpdateService> logger,
        IHostApplicationLifetime appLifetime,
        SlashService slashService,
        IOptions<SlashUpdateServiceOptions> options,
        BotServiceConfiguratorBase configurator)
    {
        _logger = logger;
        AppLifetime = appLifetime;
        SlashService = slashService;
        Options = options.Value;
        Configurator = configurator;
    }

    private IHostApplicationLifetime AppLifetime { get; set; }

    private SlashService SlashService { get; set; }

    private SlashUpdateServiceOptions Options { get; set; }

    private BotServiceConfiguratorBase Configurator { get; set; }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Tokens should be considered secret data, and never hard-coded.
        _logger.LogInformation("About to update slash commands list.");
        var updateSlash = await SlashService.UpdateSlashCommandsAsync(
            Options.Guild, ct: default).ConfigureAwait(false);
        if (!updateSlash.IsSuccess)
        {
            Options.SlashUpdateErrorMessage = $"Error: {string.Format(
                Resources.ErrorUpdatingSlashCommands!,
                updateSlash.Error.Message)}";
            _logger.LogError(Resources.ErrorUpdatingSlashCommands!, updateSlash.Error.Message);
            Configurator.AtSlashUpdateErrorApplicationShutdown(Options);
            AppLifetime.StopApplication();
        }

        _logger.LogInformation("Slash commands list updated.");
    }
}
