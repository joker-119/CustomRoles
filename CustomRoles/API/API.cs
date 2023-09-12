namespace CustomRoles.API;

using System.Collections.Generic;

using Exiled.API.Features;

public class API
{
    internal static Dictionary<Player, ExemptionType> ExemptPlayers { get; } = new();

    public static void ExemptPlayer(Player player, ExemptionType type)
    {
        if (ExemptPlayers.ContainsKey(player))
            ExemptPlayers[player] |= type;
        else
            ExemptPlayers[player] = type;
    }

    public static void UnExemptPlayer(Player player, ExemptionType type)
    {
        if (ExemptPlayers.TryGetValue(player, out ExemptionType exemptionType) && exemptionType != type)
            ExemptPlayers[player] &= type;
        else if (ExemptPlayers.ContainsKey(player))
            ExemptPlayers.Remove(player);
    }
}