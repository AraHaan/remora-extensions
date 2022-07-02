namespace AraHaan.Remora.Extensions;

/// <summary>
/// Extensions for <see cref="IDiscordRestGuildAPI" />.
/// </summary>
public static class DiscordRestGuildAPIExtensions
{
    /// <summary>
    /// Adds multiple roles in bulk to a user in a guild.
    /// </summary>
    /// <remarks>
    /// Posts 1 API Request to Discord.
    /// </remarks>
    public static async ValueTask AddRoles(
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
            await discordRestGuildAPI.ModifyGuildMemberAsync(guildID, user.User.Value.ID, roles: roles, reason: reason, ct: ct).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Removes multiple roles in bulk from a user in a guild.
    /// </summary>
    /// <remarks>
    /// Posts 1 API Request to Discord.
    /// </remarks>
    public static async ValueTask RemoveRoles(
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
            await discordRestGuildAPI.ModifyGuildMemberAsync(guildID, user.User.Value.ID, roles: keepRoles, reason: reason, ct: ct).ConfigureAwait(false);
        }
    }
}
