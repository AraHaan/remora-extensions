namespace AraHaan.Remora.Extensions.Options;

/// <summary>
/// Options that configures <see cref="SlashUpdateService" />.
/// </summary>
public sealed class SlashUpdateServiceOptions
{
    /// <summary>
    /// Gets the guild to update slash commands for, <see langword="null" /> for global slash commands.
    /// </summary>
    public Snowflake? Guild { get; internal set; } = null;

    /// <summary>
    /// Gets the error message from the slash update service.
    /// </summary>
    public string? SlashUpdateErrorMessage { get; internal set; }
};
