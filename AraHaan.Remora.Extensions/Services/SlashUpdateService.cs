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
    public SlashUpdateService(
        ILogger<SlashUpdateService> logger,
        IHostApplicationLifetime appLifetime,
        SlashService slashService,
        SlashUpdateServiceOptions options)
    {
        _logger = logger;
        AppLifetime = appLifetime;
        SlashService = slashService;
        Options = options;
    }

    internal IHostApplicationLifetime AppLifetime { get; private set; }

    internal SlashService SlashService { get; private set; }

    internal SlashUpdateServiceOptions Options { get; private set; }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Checking for slash commands support.");

        // Tokens should be considered secret data, and never hard-coded.
        var checkSlashSupport = SlashService.SupportsSlashCommands();
        if (!checkSlashSupport.IsSuccess)
        {
            Options.SlashUpdateErrorMessage = $"Error: {string.Format(
                Resources.ErrorSlashCommandsNotSupported!,
                checkSlashSupport.Error.Message)}";
            _logger.LogError(Resources.ErrorSlashCommandsNotSupported!, checkSlashSupport.Error.Message);
            AppLifetime.StopApplication();
        }
        else
        {
            _logger.LogInformation("About to update slash commands list.");
            var updateSlash = await SlashService.UpdateSlashCommandsAsync(
                Options.Guild, ct: default).ConfigureAwait(false);
            if (!updateSlash.IsSuccess)
            {
                Options.SlashUpdateErrorMessage = $"Error: {string.Format(
                    Resources.ErrorUpdatingSlashCommands!,
                    updateSlash.Error.Message)}";
                _logger.LogError(Resources.ErrorUpdatingSlashCommands!, updateSlash.Error.Message);
                AppLifetime.StopApplication();
            }
            _logger.LogInformation("Slash commands list updated.");
        }
    }
}
