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
                    case RoleType.NtfCadet:
                    case RoleType.NtfLieutenant:
                    case RoleType.NtfCommander:
                    case RoleType.NtfScientist:
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

            if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
            {
                if (plugin.Rng.Next(100) <= 40)
                {
                    int r = plugin.Rng.Next(ev.Players.Count);
                    ev.Players[r].GameObject.AddComponent<Shotgunner>();
                    ev.Players.RemoveAt(r);
                }
            }
            else
            {
                if (plugin.Rng.Next(100) <= 40)
                {
                    int r = plugin.Rng.Next(ev.Players.Count);
                    if (plugin.Rng.Next(0, 1) == 0)
                        ev.Players[r].GameObject.AddComponent<Medic>();
                    else
                        ev.Players[r].GameObject.AddComponent<Demolitionist>();
                    ev.Players.RemoveAt(r);
                }
            }
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Target.IsHuman && ev.Target.TryGetEffect(EffectType.Poisoned, out PlayerEffect poisoned) && poisoned is Poisoned && poisoned.Intensity > 0)
            {
                ev.IsAllowed = false;
                ev.Target.GameObject.AddComponent<PlagueZombie>();
            }
        }
        
        public void OnReloadedConfigs() => plugin.Config.LoadConfigs();

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleType.Scp0492)
            {
                Log.Debug($"{nameof(OnChangingRole)}: Trying to spawn new zombie.", plugin.Config.Debug);
                if (plugin.Rng.Next(100) <= 45)
                {
                    Log.Debug($"{nameof(OnChangingRole)}: Selecting random zombie role.", plugin.Config.Debug);
                    Timing.CallDelayed(2f, () => plugin.Methods.SelectRandomZombieType(ev.Player));
                }
            }
        }
    }
}