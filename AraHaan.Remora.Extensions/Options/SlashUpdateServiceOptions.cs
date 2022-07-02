namespace AraHaan.Remora.Extensions.Options;

/// <summary>
/// Options that configures <see cref="SlashUpdateService" />.
/// </summary>
/// <param name="Guild">The guild to update slash commands for, <see langword="null" /> for global slash commands.</param>
public sealed record SlashUpdateServiceOptions(
    Snowflake? Guild = null)
{
    /// <summary>
    /// Gets the error message from the slash update service.
    /// </summary>
    public string? SlashUpdateErrorMessage { get; internal set; }
};
