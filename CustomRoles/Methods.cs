namespace CustomRoles;

using System;
using System.Collections.Generic;

using CustomRoles.API;

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

    public static CustomRole? GetCustomRole(ref List<ICustomRole>.Enumerator enumerator, bool checkEscape = false, bool checkRevive = false)
    {
        try
        {
            Log.Debug("Getting role from enumerator..");

            while (enumerator.MoveNext())
            {
                Log.Debug(enumerator.Current?.StartTeam);
                if (enumerator.Current is not null)
                {
                    if (enumerator.Current.StartTeam.HasFlag(StartTeam.Other)
                        || (enumerator.Current.StartTeam.HasFlag(StartTeam.Revived) && !checkRevive)
                        || (enumerator.Current.StartTeam.HasFlag(StartTeam.Escape) && !checkEscape)
                        || (!enumerator.Current.StartTeam.HasFlag(StartTeam.Revived) && checkRevive)
                        || (!enumerator.Current.StartTeam.HasFlag(StartTeam.Escape) && checkEscape)
                        || Loader.Random.Next(100) > enumerator.Current.Chance)
                    {
                        Log.Debug("Validation check failed");
                        continue;
                    }

                    Log.Debug("Returning a role!");
                    return (CustomRole)enumerator.Current;
                }
            }

            Log.Debug("Cannot move next");

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}