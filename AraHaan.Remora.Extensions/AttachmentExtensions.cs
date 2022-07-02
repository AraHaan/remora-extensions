namespace AraHaan.Remora.Extensions;

/// <summary>
/// Discord Attachment Extensions.
/// </summary>
public static class AttachmentExtensions
{
    /// <summary>
    /// Downloads a Discord Attachment to a <see cref="Stream"/>.
    /// </summary>
    /// <param name="attachment">The attachment to download.</param>
    /// <param name="httpClient">The <see cref="HttpClient"/> to use to download the attachment.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A result containing the resulting <see cref="Stream"/>.</returns>
    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP005:Return type should indicate that the value should be disposed", Justification = "🖕")]
    public static async Task<Result<Stream>> DownloadAsync(
        this IPartialAttachment attachment,
        HttpClient httpClient,
        CancellationToken ct)
        => await httpClient.GetStreamAsync(attachment.Url.Value, ct).ConfigureAwait(false);

    /// <summary>
    /// Downloads a Discord Attachment to a <see cref="string"/>.
    /// </summary>
    /// <param name="attachment">The attachment to download.</param>
    /// <param name="httpClient">The <see cref="HttpClient"/> to use to download the attachment.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A result containing the resulting <see cref="string"/>.</returns>
    public static async Task<Result<string>> DownloadStringAsync(
        this IPartialAttachment attachment,
        HttpClient httpClient,
        CancellationToken ct)
    {
        var downloadResult = await attachment.DownloadAsync(httpClient, ct).ConfigureAwait(false);
        using var attachmentData = downloadResult.Entity;
        using var reader = new StreamReader(attachmentData, Encoding.UTF8, true);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Downloads a Discord Attachment to a file.
    /// </summary>
    /// <param name="attachment">The attachment to download.</param>
    /// <param name="httpClient">The <see cref="HttpClient"/> to use to download the attachment.</param>
    /// <param name="filePath">The path to where the file should be saved.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A result containing whether the operation was successful or not.</returns>
    public static async Task<Result> DownloadToFileAsync(
        this IPartialAttachment attachment,
        HttpClient httpClient,
        string filePath,
        CancellationToken ct)
    {
        try
        {
            var attachmentData = (await attachment.DownloadStringAsync(httpClient, ct).ConfigureAwait(false)).Entity;
            await File.WriteAllTextAsync(filePath, attachmentData, ct).ConfigureAwait(false);
            return Result.FromSuccess();
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
