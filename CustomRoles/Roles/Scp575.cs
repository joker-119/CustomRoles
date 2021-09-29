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
    using Dissonance.Integrations.MirrorIgnorance;
    using Exiled.API.Extensions;
    using PlayableScps;
    using UnityEngine.Serialization;

    public class Scp575 : CustomRole
    {
        public static List<Player> Scp575Players = new List<Player>();
        public int consumptionStacks = 1;
        public Scp207 Scp207;
        public bool canUseAbility = false;
        
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.Name;
        protected override string Description { get; set; } = $"An entity that appears as a shapeless void, that moves slowly but grows in power the more biological material it consumes. Capable of causing wide-spread power outages.\n\nUse client command \".special\" to trigger a blackout. This can be keybound with \"cmdbind KEY .special\"";

        public override int AbilityCooldown { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityCooldown;

        protected override Dictionary<Vector3, float> SpawnLocations { get; set; } = new Dictionary<Vector3, float>
        {
            { RoleType.Scp106.GetRandomSpawnProperties().Item1, 100 }
        };

        private List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        protected override void RoleAdded()
        {
            try
            {
                Player.UnitName = "Scp575";
                Scp575Players.Add(Player);
                Log.Debug($"{Name} added to {Player.Nickname}");
                Scp207 = Player.GetEffect(EffectType.Scp207) as Scp207;
                Cassie.GlitchyMessage("Alert . scp 5 7 5 has breached containment", 0.5f, 0.1f);
                Coroutines.Add(Timing.RunCoroutine(Invisibility()));
                Player.CustomInfo = $"<color=red>{Player.Nickname}\nSCP-575</color>";
                Player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Nickname;
                Player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Badge;
                Player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                Player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.PowerStatus;
                Player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.UnitName;
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
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Scp106.Teleporting += OnTeleporting;
            Exiled.Events.Handlers.Scp106.CreatingPortal += OnCreatingPortal;
            Exiled.Events.Handlers.Map.ExplodingGrenade += OnExplodingGrenade;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination += OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocketDimension;
        }

        public override bool CanUseAbility(out DateTime usableTime)
        {
            if (consumptionStacks < Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityPowerLevelRequirement)
            {
                Player.ShowHint($"You are unable to use Blackout until you are power level 10. You are currently at {consumptionStacks.toString()}. Gain power levels by killing players.");
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
                string message = $"scp 5 7 5 has been successfully terminated . termination cause {ev.TerminationCause}";

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
            Scp575Players.Remove(Player);
            Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.PowerStatus;
            Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.UnitName;
            Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Nickname;
            Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;
            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);
            Player.IsInvisible = false;

            Log.Debug($"{Name} unloading events.");
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Scp106.Teleporting -= OnTeleporting;
            Exiled.Events.Handlers.Scp106.CreatingPortal -= OnCreatingPortal;
            Exiled.Events.Handlers.Map.ExplodingGrenade -= OnExplodingGrenade;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination -= OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocketDimension;
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.Killer == Player && ev.Target != Player)
                IncreasePower();
            else if (ev.Target == Player)
            {
                Log.Warn($"Adding {Player.Nickname} to stop doll list.");
                Plugin.Singleton.StopRagdollList.Add(Player);
                Role role = CharacterClassManager._staticClasses.SafeGet(Type);
                Ragdoll.Info info = new Ragdoll.Info
                {
                    ClassColor = role.classColor,
                    DeathCause = ev.HitInformation,
                    FullName = "SCP-575",
                    Nick = Player.Nickname,
                    ownerHLAPI_id = Player.GameObject.GetComponent<MirrorIgnorancePlayer>().PlayerId,
                    PlayerId = Player.Id,
                };
                Exiled.API.Features.Ragdoll.Spawn(CharacterClassManager._staticClasses.SafeGet(Player.Role), info, Player.Position, Quaternion.Euler(Player.Rotation));
                Destroy(this);
            }
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
                else if (ev.DamageType == DamageTypes.Contain)
                {
                    ev.Amount = 0;
                    ev.IsAllowed = false;
                }
            }
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
                    if (Physics.Linecast(ev.Grenade.transform.position, Player.Position, Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                        return;
                    float damage = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.FlashbangBaseDamage - (dist * Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.FlashbangFalloffMultiplier);
                    if (damage < 0)
                        damage = 0f;
                    else if (damage > Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.FlashbangBaseDamage)
                        damage = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.FlashbangBaseDamage;

                    Log.Debug($"{nameof(OnExplodingGrenade)}: Damage: {damage} - {dist} {Player.Nickname}", Plugin.Singleton.Config.Debug);
                    Player.Hurt(damage, DamageTypes.Nuke, ev.Thrower.Nickname, ev.Thrower.Id);
                    DoFlashEffect(dist);
                }
            }
        }

        private void DoFlashEffect(float distance)
        {
            if (Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.ResetPowerOnFlashbang)
            {
                if (consumptionStacks > 0)
                {
                    if (distance < 6 && consumptionStacks > 1)
                        consumptionStacks--;
                    else if (distance > 15)
                        consumptionStacks++;

                    if ((consumptionStacks / 2) >= 3)
                        consumptionStacks -= 3;
                    else if ((consumptionStacks / 2) >= 2)
                        consumptionStacks -= 2;
                    else
                        consumptionStacks--;
                }

                Player.ShowHint($"You now have {consumptionStacks} stacks of Consumption!", 10);
                canUseAbility = consumptionStacks >= Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityPowerLevelRequirement;
                int newIntensity = consumptionStacks / (Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.MaxPowerLevel / 2);
                Player.ChangeEffectIntensity(EffectType.Scp207, (byte)newIntensity);
            }

            if (Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.TeleportOnFlashbang)
            {
                Player.Position = Vector3.down * 1950f;

                Timing.CallDelayed(15f, () =>
                {
                    foreach (Player player in Player.Get(Team.SCP))
                    {
                        if (player == Player)
                            continue;
                        Player.Position = player.Position + Vector3.up;
                        break;
                    }

                    Door hczArmoryDoor = Map.GetDoorByName("HCZ_ARMORY");
                    Transform transform = hczArmoryDoor.Base.transform;
                    Player.Position = transform.position + transform.forward * 2f;
                });
            }
        }

        private void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (ev.Scp106 == Player)
            {
                ev.IsAllowed = false;
                ev.Player.Hurt(Mathf.Max(Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.BaseDamage, 30 * (consumptionStacks / Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.DamageScalerValue)), DamageTypes.RagdollLess, ev.Scp106.Nickname, ev.Scp106.Id);
            }
        }

        public void IncreasePower()
        {
            if (consumptionStacks >= Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.MaxPowerLevel)
                return;

            Log.Debug($"{Name} power increase for {Player.Nickname}");
            consumptionStacks++;

            int newIntensity = consumptionStacks / (Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.MaxPowerLevel / 2);
            Player.ChangeEffectIntensity(EffectType.Scp207, (byte)newIntensity);

            if (consumptionStacks == Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityPowerLevelRequirement)
                canUseAbility = true;
            Player.ShowHint($"You now have {consumptionStacks} stacks of Consumption!");
        }

        private IEnumerator<float> Invisibility()
        {
            Log.Debug($"{nameof(Scp575)}: {nameof(Invisibility)}: Starting 268 loop for {Player.Nickname}", Plugin.Singleton.Config.Debug);
            for (;;)
            {
                foreach (Player player in Player.List)
                {
                    if (VisionInformation.GetVisionInformation(player.ReferenceHub, Player.Position, -2f, 40f, false, false, player.ReferenceHub.localCurrentRoomEffects).IsLooking)
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
