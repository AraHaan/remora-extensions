namespace AraHaan.Remora.Extensions;

/// <summary>
/// Extensions for <see cref="IDiscordRestGuildAPI" />.
/// </summary>
public static class DiscordRestGuildAPIExtensions
{
    /// <summary>
    /// Adds multiple roles in bulk to a user in a guild.
    /// <br></br>
    /// if roles is empty after the user's existing roles are added to it,
    /// then no requests are sent to Discord.
    /// </summary>
    /// <remarks>
    /// Posts 0~1 API Requests to Discord.
    /// </remarks>
    /// <returns>
    /// A rest result which may or may not have succeeded.
    /// </returns>
    public static async Task<Result> AddRoles(
        this IDiscordRestGuildAPI discordRestGuildAPI,
        Snowflake guildID,
        IGuildMember user,
        List<Snowflake> roles,
        Optional<string> reason,
        CancellationToken ct)
    {
        // ensure that existing roles do not get accidentally removed.
        roles.AddRange(from role in user.Roles
                       where !roles.Contains(role)
                       select role);
        if (roles.Count > 0)
        {
            return await discordRestGuildAPI.ModifyGuildMemberAsync(guildID, user.User.Value.ID, roles: roles, reason: reason, ct: ct).ConfigureAwait(false);
        }

        return Result.FromSuccess();
    }

    /// <summary>
    /// Removes multiple roles in bulk from a user in a guild.
    /// <br></br>
    /// if roles is empty, then no requests are sent to Discord.
    /// </summary>
    /// <remarks>
    /// Posts 0~1 API Requests to Discord.
    /// </remarks>
    /// <returns>
    /// A rest result which may or may not have succeeded.
    /// </returns>
    public static async Task<Result> RemoveRoles(
        this IDiscordRestGuildAPI discordRestGuildAPI,
        Snowflake guildID,
        IGuildMember user,
        List<Snowflake> roles,
        Optional<string> reason,
        CancellationToken ct)
    {
        var keepRoles = user.Roles.ToList();
        foreach (var role in roles)
        {
            _ = keepRoles.Remove(role);
        }

        if (roles.Count > 0)
        {
            return await discordRestGuildAPI.ModifyGuildMemberAsync(guildID, user.User.Value.ID, roles: keepRoles, reason: reason, ct: ct).ConfigureAwait(false);
        }

        return Result.FromSuccess();
    }
}
