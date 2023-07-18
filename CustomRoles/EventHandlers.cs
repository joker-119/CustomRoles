namespace CustomRoles;

using System.Collections.Generic;

using CustomRoles.API;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Server;

using PlayerRoles;
using Respawning;

public class EventHandlers
{
    private readonly Plugin plugin;

    public EventHandlers(Plugin plugin)
    {
        this.plugin = plugin;
    }

    public void OnRoundStarted()
    {
        List<ICustomRole>.Enumerator dClassRoles = new();
        List<ICustomRole>.Enumerator scientistRoles = new();
        List<ICustomRole>.Enumerator guardRoles = new();
        List<ICustomRole>.Enumerator scpRoles = new();

        foreach (KeyValuePair<StartTeam, List<ICustomRole>> kvp in plugin.Roles)
        {
            Log.Debug($"Setting enumerator for {kvp.Key} - {kvp.Value.Count}");
            switch (kvp.Key)
            {
                case StartTeam.ClassD:
                    Log.Debug("Class d funny");
                    dClassRoles = kvp.Value.GetEnumerator();
                    break;
                case StartTeam.Scientist:
                    scientistRoles = kvp.Value.GetEnumerator();
                    break;
                case StartTeam.Guard:
                    guardRoles = kvp.Value.GetEnumerator();
                    break;
                case StartTeam.Scp:
                    scpRoles = kvp.Value.GetEnumerator();
                    break;
            }
        }

        foreach (Player player in Player.List)
        {
            Log.Debug($"Trying to give {player.Nickname} a role | {player.Role.Type}");
            CustomRole? role = null;
            switch (player.Role.Type)
            {
                case RoleTypeId.FacilityGuard:
                    role = Methods.GetCustomRole(ref guardRoles);
                    break;
                case RoleTypeId.Scientist:
                    role = Methods.GetCustomRole(ref scientistRoles);
                    break;
                case RoleTypeId.ClassD:
                    role = Methods.GetCustomRole(ref dClassRoles);
                    break;
                case { } when player.Role.Side == Side.Scp:
                    role = Methods.GetCustomRole(ref scpRoles);
                    break;
            }

            role?.AddRole(player);
        }

        guardRoles.Dispose();
        scientistRoles.Dispose();
        dClassRoles.Dispose();
        scpRoles.Dispose();
    }

    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (ev.Players.Count == 0)
        {
            Log.Warn(
                $"{nameof(OnRespawningTeam)}: The respawn list is empty ?!? -- {ev.NextKnownTeam} / {ev.MaximumRespawnAmount}");

            foreach (Player player in Player.Get(RoleTypeId.Spectator))
                ev.Players.Add(player);
            ev.MaximumRespawnAmount = ev.Players.Count;
        }

        List<ICustomRole>.Enumerator roles = new();
        switch (ev.NextKnownTeam)
        {
            case SpawnableTeamType.ChaosInsurgency:
                if (plugin.Roles.TryGetValue(StartTeam.Chaos, out List<ICustomRole>? role))
                    roles = role.GetEnumerator();
                break;
            case SpawnableTeamType.NineTailedFox:
                if (plugin.Roles.TryGetValue(StartTeam.Ntf, out List<ICustomRole>? pluginRole))
                    roles = pluginRole.GetEnumerator();
                break;
        }

        foreach (Player player in ev.Players)
        {
            CustomRole? role = Methods.GetCustomRole(ref roles);

            role?.AddRole(player);
        }

        roles.Dispose();
    }

    public void OnReloadedConfigs()
    {
        plugin.Config.LoadConfigs();
    }

    public void FinishingRecall(FinishingRecallEventArgs ev)
    {
        Log.Debug($"{nameof(FinishingRecall)}: Selecting random zombie role.");
        if (plugin.Roles.ContainsKey(StartTeam.Scp) && ev.Target is not null)
        {
            Log.Debug($"{nameof(FinishingRecall)}: List count {plugin.Roles[StartTeam.Scp].Count}");
            List<ICustomRole>.Enumerator roles = plugin.Roles[StartTeam.Scp].GetEnumerator();
            CustomRole? role = Methods.GetCustomRole(ref roles, false, true);

            Log.Debug($"Got custom role {role?.Name}");
            if (ev.Target.GetCustomRoles().Count == 0)
                role?.AddRole(ev.Target);
            roles.Dispose();
        }
    }

    public void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
    {
        if (!plugin.StopRagdollList.Contains(ev.Player))
            return;

        Log.Warn($"Stopped doll for {ev.Player.Nickname}");
        ev.IsAllowed = false;
        plugin.StopRagdollList.Remove(ev.Player);
    }
}