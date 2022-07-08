namespace AraHaan.Remora.Extensions.Hosting;

/// <summary>
/// Base class for a configurator to use to configure services to use for the Bot.
/// </summary>
public abstract class BotServiceConfiguratorBase : IDisposable
{
    private bool _disposed;

    internal bool ShouldDispose { get; set; } = true;

    /// <summary>
    /// Code to run before <see cref="ConfigureBotServices"/> is run.
    /// </summary>
    /// <param name="entryAssembly">The entry assebly to the bot application.</param>
    public abstract void BeforeConfigure(Assembly entryAssembly);

    /// <summary>
    /// Configures the services to use for the bot.
    /// </summary>
    public abstract void ConfigureBotServices(IHostBuilder hostBuilder);

    /// <summary>
    /// Code to run after the application shuts down.
    /// </summary>
    public abstract void AfterApplicationShutdown();

    /// <summary>
    /// Code to run at application shutdown due to an error with updating slash commands.
    /// </summary>
    /// <param name="options">The options to the <see cref="SlashUpdateService"/> for when that service triggered the shutdown.</param>
    public abstract void AtSlashUpdateErrorApplicationShutdown(SlashUpdateServiceOptions options);

    /// <inheritdoc/>
    /// <remarks>Derived types should never provide a finalizer.</remarks>
    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Derived classes should not provide a finalizer.")]
    public virtual void Dispose()
    {
        if (this.ShouldDispose)
        {
            if (this._disposed)
            {
                return;
            }

            this.AfterApplicationShutdown();
            this._disposed = true;
        }
        else
        {
            // Reset ShouldDispose back to true.
            this.ShouldDispose = true;
        }
    }

    /// <summary>
    /// Throws if the <see cref="BotServiceConfiguratorBase" /> is disposed.
    /// </summary>
    /// <remarks>Derived types should call this.</remarks>
    protected virtual void ThrowIfDisposed()
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
