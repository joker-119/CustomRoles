using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using CustomRoles.Abilities;
using CustomRoles.API;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MEC;
using UnityEngine;
using Utf8Json.Resolvers.Internal;

namespace CustomRoles.Roles
{
    public class Scp575 : CustomRole
    {
        public int powerLevel = 1;
        public Scp207 Scp207;
        public bool canUseAbility = false;

        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.Name;
        protected override string Description { get; set; } = $"An entity that appears as a shapeless void, that moves slowly but grows in power the more biological material it consumes. Capable of causing wide-spread power outages.\n\nUse client command \".special\" to trigger a blackout. This can be keybound with \"cmdbind KEY .special\"";

        protected override int AbilityCooldown { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityCooldown;

        protected override Dictionary<Vector3, float> SpawnLocations { get; set; } = new Dictionary<Vector3, float>
        {
            { Exiled.API.Extensions.Role.GetRandomSpawnPoint(RoleType.Scp106), 100 }
        };

        private List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        protected override void RoleAdded()
        {
            try
            {
                Log.Debug($"{Name} added to {Player.Nickname}");
                Scp207 = Player.GetEffect(EffectType.Scp207) as Scp207;
                Cassie.GlitchyMessage("Alert . scp 5 7 5 has breached containment", 0.5f, 0.1f);
                Coroutines.Add(Timing.RunCoroutine(Invisibility()));
            }
            catch (Exception e)
            {
                Log.Error($"{Name}: {e}\n{e.StackTrace}");
            }
        }

        public override string UseAbility()
        {
            Player.GameObject.AddComponent<BlackoutAbility>();
            return "Ability used.";
        }

        protected override void LoadEvents()
        {
            Log.Debug($"{Name} loading events.");
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Scp106.Containing += OnContaining;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Scp106.Teleporting += OnTeleporting;
            Exiled.Events.Handlers.Scp106.CreatingPortal += OnCreatingPortal;
            Exiled.Events.Handlers.Map.ExplodingGrenade += OnExplodingGrenade;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination += OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocketDimension;
        }

        public override bool CanUseAbility(out DateTime usableTime)
        {
            if (powerLevel < Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityPowerLevelRequirement)
            {
                Player.ShowHint($"You are unable to use Blackout until you are power level 10. You are currently at {powerLevel}. Gain power levels by killing players.");
                usableTime = DateTime.Now;
                return false;
            }

            usableTime = UsedAbility + TimeSpan.FromSeconds(AbilityCooldown);

            return DateTime.Now > usableTime;
        }

        private void OnAnnouncingScpTermination(AnnouncingScpTerminationEventArgs ev)
        {
            if (ev.Role.roleId == RoleType.Scp106)
            {
                string message = "scp 5 7 5 has been successfully terminated .";

                if (ev.Killer.Side == Side.Mtf && !string.IsNullOrEmpty(ev.Killer.UnitName))
                    message += $" termination cause {ev.Killer.UnitName}";
                else
                    message += " termination cause unspecified";
                
                ev.IsAllowed = false;
                Cassie.Message(message);
            }
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Destroy(this);
        }

        protected override void UnloadEvents()
        {
            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);
            Player.IsInvisible = false;

            Log.Debug($"{Name} unloading events.");
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Scp106.Containing -= OnContaining;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Scp106.Teleporting -= OnTeleporting;
            Exiled.Events.Handlers.Scp106.CreatingPortal -= OnCreatingPortal;
            Exiled.Events.Handlers.Map.ExplodingGrenade -= OnExplodingGrenade;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination -= OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocketDimension;
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.Killer == Player)
                IncreasePower();
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Target == Player)
            {
                if (ev.DamageType == DamageTypes.Scp207)
                    ev.Amount = 0.0f;
                else if (ev.DamageType == DamageTypes.Grenade)
                    ev.Amount *= 0.40f;
                else if (ev.DamageType != DamageTypes.Nuke)
                    ev.Amount *= 0.70f;
            }
        }

        private void OnContaining(ContainingEventArgs ev)
        {
            if (ev.Player == Player)
                ev.IsAllowed = false;
        }

        private void OnTeleporting(TeleportingEventArgs ev)
        {
            if (ev.Player == Player)
                ev.IsAllowed = false;
        }

        private void OnCreatingPortal(CreatingPortalEventArgs ev)
        {
            if (ev.Player == Player)
                ev.IsAllowed = false;
        }

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (!ev.IsFrag)
            {
                float dist = Vector3.Distance(ev.Grenade.transform.position, Player.Position);
                if (dist <= 20f)
                {
                    float damage = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.FlashbangBaseDamage - (dist * Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.FlashbangFalloffMultiplier);

                    Player.Hurt(damage, DamageTypes.Nuke, ev.Thrower.Nickname, ev.Thrower.Id);
                    DoFlashEffect();
                }
            }
        }

        private void DoFlashEffect()
        {
            if (Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.ResetPowerOnFlashbang)
            {
                powerLevel = 1;
                canUseAbility = powerLevel >= Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityPowerLevelRequirement;
                Player.DisableEffect<Scp207>();
            }

            if (Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.TeleportOnFlashbang)
            {
                Player.Position = Vector3.zero;

                Timing.CallDelayed(15f, () =>
                {
                    foreach (Player player in Player.Get(Team.SCP))
                    {
                        if (player == Player)
                            continue;
                        Player.Position = player.Position + Vector3.up;
                        break;
                    }

                    DoorVariant hczArmoryDoor = Map.GetDoorByName("HCZ_ARMORY");
                    Transform transform = hczArmoryDoor.transform;
                    Player.Position = transform.position + transform.forward * 2f;
                });
            }
        }

        private void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (ev.Scp106 == Player)
            {
                ev.IsAllowed = false;
                ev.Player.Hurt(Mathf.Max(Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.BaseDamage, 30 * (powerLevel / Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.DamageScalerValue)), DamageTypes.RagdollLess, ev.Scp106.Nickname, ev.Scp106.Id);
            }
        }

        public void IncreasePower()
        {
            if (powerLevel >= Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.MaxPowerLevel)
                return;

            Log.Debug($"{Name} power increase for {Player.Nickname}");
            powerLevel++;

            if (powerLevel%2 == 0)
                Scp207.ServerChangeIntensity((byte)(Scp207.Intensity + 1));

            if (powerLevel == Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityPowerLevelRequirement)
                canUseAbility = true;
        }

        private IEnumerator<float> Invisibility()
        {
            Log.Debug($"{nameof(Scp575)}: {nameof(Scp268)}: Starting 268 loop for {Player.Nickname}", Plugin.Singleton.Config.Debug);
            for (;;)
            {
                foreach (Player player in Player.List)
                {
                    if (player.ReferenceHub.characterClassManager.Scp173.LookFor173(Player.GameObject, true))
                    {
                        Player.IsInvisible = false;
                        break;
                    }

                    Player.IsInvisible = true;
                }


                yield return Timing.WaitForSeconds(0.25f);
            }
        }

        private void FixedUpdate()
        {
            if (!Player.CurrentRoom.LightsOff)
                Player.CurrentRoom.TurnOffLights(10f);
        }
    }
}