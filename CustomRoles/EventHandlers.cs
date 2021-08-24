using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using CustomRoles.API;
using CustomRoles.Roles;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;

namespace CustomRoles
{
    using System.Net;
    using UnityEngine;

    public class EventHandlers
    {
        private readonly Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        public void OnWaitingForPlayers()
        {
            if (!plugin.Methods.CheckForCustomItems())
            {
                Log.Error($"Unable to find Custom Items plugin. This plugin will now be disabled.");
                
                plugin.OnDisabled();
            }
        }

        public void OnRoundStarted()
        {
            bool isPhantom = false;
            bool isDwarf = false;

            bool spawn575 = plugin.Rng.Next(100) <= 50;
            bool spawnPhantom = plugin.Rng.Next(100) <= 20;
            bool spawnDwarf = plugin.Rng.Next(100) <= 35;
            
            foreach (Player player in Player.List)
            {
                switch (player.Role)
                {
                    case RoleType.Scp106 when spawn575:
                    {
                        player.GameObject.AddComponent<Scp575>();
                        
                        break;
                    }
                    case RoleType.FacilityGuard when !isPhantom && spawnPhantom:
                    {
                        player.GameObject.AddComponent<Phantom>();
                        
                        isPhantom = true;
                        break;
                    }
                    case RoleType.Scientist:
                    case RoleType.ClassD:
                    {
                        if (spawnDwarf && !isDwarf)
                        {
                            player.GameObject.AddComponent<Dwarf>();
                            isDwarf = true;
                        }

                        break;
                    }
                }
            }
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.Players.Count == 0)
            {
                Log.Warn($"{nameof(OnRespawningTeam)}: The respawn list is empty ?!? -- {ev.NextKnownTeam} / {ev.MaximumRespawnAmount}");
                
                foreach (Player player in Player.Get(RoleType.Spectator))
                    ev.Players.Add(player);
                ev.MaximumRespawnAmount = ev.Players.Count;
            }
            
            if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
            {
                if (plugin.Rng.Next(100) <= 40)
                {
                    Log.Debug($"{nameof(OnRespawningTeam)}: Spawning NTF Special");
                    int r = plugin.Rng.Next(ev.Players.Count);
                    if (plugin.Rng.Next(0, 1) == 0)
                    {
                        if (ev.Players[r].GetPlayerRoles().Any()) 
                            return;
                        Log.Debug($"{nameof(OnRespawningTeam)}: Spawning medic!");
                        ev.Players[r].GameObject.AddComponent<Medic>();
                    }
                    else
                    {
                        if (ev.Players[r].GetPlayerRoles().Any()) 
                            return;
                        Log.Debug($"{nameof(OnRespawningTeam)}: Spawning Demo!");
                        ev.Players[r].GameObject.AddComponent<Demolitionist>();
                    }

                    ev.Players.RemoveAt(r);
                }
            }
        }

        public void OnReloadedConfigs() => plugin.Config.LoadConfigs();

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            foreach (CustomRole role in ev.Player.GetPlayerRoles())
            {
                if (ev.NewRole == RoleType.Spectator || role.Type != ev.NewRole)
                {
                    Log.Debug($"Destroying {role.Name} for {ev.Player.Nickname}");
                    Object.Destroy(role);
                }
            }

            if (ev.NewRole == RoleType.Scp0492)
            {
                if (ev.Player.GetPlayerRoles().Any()) 
                    return;
                Log.Debug($"{nameof(OnChangingRole)}: Trying to spawn new zombie.");
                if (plugin.Rng.Next(100) <= 45)
                {
                    Log.Debug($"{nameof(OnChangingRole)}: Selecting random zombie role.");
                    Timing.CallDelayed(2f, () => plugin.Methods.SelectRandomZombieType(ev.Player));
                }
            }
        }

        public void FinishingRecall(FinishingRecallEventArgs ev)
        {
            Log.Debug($"{nameof(OnChangingRole)}: Selecting random zombie role.");
            Timing.CallDelayed(2f, () => plugin.Methods.SelectRandomZombieType(ev.Target));
        }

        public void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            Log.Warn($"{ev.Killer} -- {ev.Owner}");
            if (plugin.StopRagdollList.Contains(ev.Owner))
            {
                Log.Warn($"Stopped doll for {ev.Owner.Nickname}");
                ev.IsAllowed = false;
                plugin.StopRagdollList.Remove(ev.Owner);
            }
        }
    }
}