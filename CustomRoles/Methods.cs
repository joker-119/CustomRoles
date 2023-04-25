namespace CustomRoles;

using System;
using System.Collections.Generic;

using CustomRoles.API;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Loader;

public class Methods
{
    private readonly Plugin plugin;

    public Methods(Plugin plugin)
    {
        this.plugin = plugin;
    }

    public static CustomRole? GetCustomRole(ref List<ICustomRole>.Enumerator? enumerator, bool checkEscape = false, bool checkRevive = false)
    {
        try
        {
            Log.Debug("Getting role from enumerator..");

            if (!enumerator.HasValue)
                return null;

            while (enumerator.Value.MoveNext())
            {
                if (enumerator.Value.Current is not null)
                {
                    if (enumerator.Value.Current.StartTeam.HasFlag(StartTeam.Other)
                        || (enumerator.Value.Current.StartTeam.HasFlag(StartTeam.Revived) && !checkRevive)
                        || (enumerator.Value.Current.StartTeam.HasFlag(StartTeam.Escape) && !checkEscape)
                        || (!enumerator.Value.Current.StartTeam.HasFlag(StartTeam.Revived) && checkRevive)
                        || (!enumerator.Value.Current.StartTeam.HasFlag(StartTeam.Escape) && checkEscape)
                        || Loader.Random.Next(100) > enumerator.Value.Current.Chance)
                    {
                        Log.Debug("Validation check failed");
                        continue;
                    }

                    Log.Debug("Returning a role!");
                    return (CustomRole)enumerator.Value.Current;
                }

                Log.Debug("Enumerator current is null.");
                return null;
            }

            Log.Debug("Cannot move next");

            return null;
        }
        catch (Exception e)
        {
            Log.Error(e);
            return null;
        }
    }
}