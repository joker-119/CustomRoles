namespace CustomRoles
{
    using System.Linq;
    using CustomRoles.Roles;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
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
            var isPhantom = false;
            var isDwarf = false;

            var spawn575 = plugin.Rng.Next(100) <= 50;
            var spawnPhantom = plugin.Rng.Next(100) <= 20;
            var spawnDwarf = plugin.Rng.Next(100) <= 35;

            foreach (var player in Player.List)
                switch (player.Role)
                {
                    case RoleType.Scp106 when spawn575:
                    {
                        CustomRole.Get(typeof(Scp575))?.AddRole(player);

                        break;
                    }
                    case RoleType.FacilityGuard when !isPhantom && spawnPhantom:
                    {
                        CustomRole.Get(typeof(Phantom))?.AddRole(player);

                        isPhantom = true;
                        break;
                    }
                    case RoleType.Scientist:
                    case RoleType.ClassD:
                    {
                        if (spawnDwarf && !isDwarf)
                        {
                            CustomRole.Get(typeof(Dwarf))?.AddRole(player);
                            isDwarf = true;
                        }

                        break;
                    }
                }
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.Players.Count == 0)
            {
                Log.Warn(
                    $"{nameof(OnRespawningTeam)}: The respawn list is empty ?!? -- {ev.NextKnownTeam} / {ev.MaximumRespawnAmount}");

                foreach (var player in Player.Get(RoleType.Spectator))
                    ev.Players.Add(player);
                ev.MaximumRespawnAmount = ev.Players.Count;
            }

            if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                if (plugin.Rng.Next(100) <= 40)
                {
                    Log.Debug($"{nameof(OnRespawningTeam)}: Spawning NTF Special");
                    var r = plugin.Rng.Next(ev.Players.Count);
                    if (plugin.Rng.Next(0, 1) == 0)
                    {
                        if (ev.Players[r].GetCustomRoles().Any())
                            return;
                        Log.Debug($"{nameof(OnRespawningTeam)}: Spawning medic!");
                        CustomRole.Get("Medic").AddRole(ev.Players[r]);
                    }
                    else
                    {
                        if (ev.Players[r].GetCustomRoles().Any())
                            return;
                        Log.Debug($"{nameof(OnRespawningTeam)}: Spawning Demo!");
                        CustomRole.Get("Demolitionist").AddRole(ev.Players[r]);
                    }

                    ev.Players.RemoveAt(r);
                }
        }

        public void OnReloadedConfigs()
        {
            plugin.Config.LoadConfigs();
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleType.Scp0492)
            {
                if (ev.Player.GetCustomRoles().Any())
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
            if (!plugin.StopRagdollList.Contains(ev.Owner)) return;
            Log.Warn($"Stopped doll for {ev.Owner.Nickname}");
            ev.IsAllowed = false;
            plugin.StopRagdollList.Remove(ev.Owner);
        }
    }
}