namespace CustomRoles.Roles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Abilities;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;
    using MEC;
    using Mirror;
    using PlayableScps;
    using PlayerStatsSystem;
    using UnityEngine;
    using Cassie = Exiled.API.Features.Cassie;
    using Map = Exiled.Events.Handlers.Map;
    using Player = Exiled.API.Features.Player;

    public class Scp575 : CustomRole
    {
        public readonly Dictionary<Player, int> ConsumptionStacks = new();

        private readonly List<CoroutineHandle> _coroutines = new();

        public override uint Id { get; set; } = 12;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 550;
        public override string Name { get; set; } = "SCP-575";

        public override string Description { get; set; } =
            "An entity that appears as a shapeless void, that moves slowly but grows in power the more biological material it consumes. Capable of causing wide-spread power outages.\n\nUse client command \".special\" to trigger a blackout. This can be keybound with \"cmdbind KEY .special\"";

        [Description("The base(minimum) damage his hits will deal.")]
        public float BaseDamage { get; set; } = 30;

        [Description("Damage calculation is base x (consumptionStacks / this value)")]
        public int DamageScalar { get; set; } = 2;

        [Description("The maximum number of consumption stacks 575 can have.")]
        public int MaxConsumption { get; set; } = 10;

        [Description("The amount of consumption stacks he must have to use his ability.")]
        public int AbilityStackRequirement { get; set; } = 5;

        [Description("The base(maximum) damage a flashbang will deal to 575.")]
        public float FlashbangBaseDamage { get; set; } = 450f;

        [Description("The amount of damage removed from the base amount for every 1ft away from the grenade he is.")]
        public float FlashbangFalloffMultiplier { get; set; } = 20f;

        [Description("Whether or not consumption stacks are removed when he is hit with a flashbang.")]
        public bool ResetConsumptionOnFlashed { get; set; } = true;

        [Description("Whether or not 575 is teleported to a random HCZ room when flashbanged.")]
        public bool TeleportOnFlashed { get; set; } = true;

        protected override SpawnProperties SpawnProperties { get; set; } = new()
        {
            RoleSpawnPoints = new List<RoleSpawnPoint>
            {
                new()
                {
                    Role = RoleType.Scp173,
                    Chance = 10
                },
                new()
                {
                    Role = RoleType.Scp079,
                    Chance = 20
                },
                new()
                {
                    Role = RoleType.Scp049,
                    Chance = 30
                },
                new()
                {
                    Role = RoleType.Scp106,
                    Chance = 100
                }
            }
        };

        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new BlackoutAbility()
        };

        protected override void RoleAdded(Player player)
        {
            try
            {
                foreach (var ability in CustomAbilities)
                    if (ability is BlackoutAbility blackout)
                        blackout.CanUseOverride = () => CanUseAbility(player, blackout);

                if (!ConsumptionStacks.ContainsKey(player))
                    ConsumptionStacks[player] = 1;
                player.UnitName = "Scp575";
                Log.Debug($"{Name} added to {player.Nickname}");
                Cassie.GlitchyMessage("Alert . scp 5 7 5 has breached containment", 0.5f, 0.1f);
                _coroutines.Add(Timing.RunCoroutine(Invisibility(player)));
                player.CustomInfo = $"<color=red>{player.Nickname}\nSCP-575</color>";
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Nickname;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Badge;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.PowerStatus;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.UnitName;
            }
            catch (Exception e)
            {
                Log.Error($"{Name}: {e}\n{e.StackTrace}");
            }
        }

        protected override void SubscribeEvents()
        {
            Log.Debug($"{Name} loading events.");
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Scp106.Teleporting += OnTeleporting;
            Scp106.CreatingPortal += OnCreatingPortal;
            Map.ExplodingGrenade += OnExplodingGrenade;
            Map.AnnouncingScpTermination += OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocketDimension;
            base.SubscribeEvents();
        }

        protected override void UnSubscribeEvents()
        {
            Log.Debug($"{Name} unloading events.");
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Scp106.Teleporting -= OnTeleporting;
            Scp106.CreatingPortal -= OnCreatingPortal;
            Map.ExplodingGrenade -= OnExplodingGrenade;
            Map.AnnouncingScpTermination -= OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocketDimension;
            base.UnSubscribeEvents();
        }

        public bool CanUseAbility(Player player, BlackoutAbility ability)
        {
            if (ConsumptionStacks[player] < AbilityStackRequirement)
            {
                player.ShowHint(
                    $"You are unable to use Blackout until you are power level {AbilityStackRequirement}. You are currently at {ConsumptionStacks[player]}. Gain power levels by killing players.");
                return false;
            }

            var usableTime = ability.LastUsed[player] + TimeSpan.FromSeconds(ability.Cooldown);
            var flag = DateTime.Now > usableTime;
            if (!flag)
                player.ShowHint(
                    $"You must wait another {Math.Round((DateTime.Now - usableTime).TotalSeconds, 2)} more seconds to use this ability.");

            return flag;
        }

        private void OnAnnouncingScpTermination(AnnouncingScpTerminationEventArgs ev)
        {
            if (ev.Role.roleId == RoleType.Scp106)
            {
                var message = $"scp 5 7 5 has been successfully terminated . termination cause {ev.TerminationCause}";

                ev.IsAllowed = false;
                Cassie.Message(message);
            }
        }

        protected override void RoleRemoved(Player player)
        {
            player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.PowerStatus;
            player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.UnitName;
            player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Nickname;
            player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;
            foreach (var handle in _coroutines)
                Timing.KillCoroutines(handle);
            player.IsInvisible = false;
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Killer) && !Check(ev.Target))
            {
                IncreasePower(ev.Killer);
            }
            else if (Check(ev.Target))
            {
                Log.Warn($"Adding {ev.Target.Nickname} to stop doll list.");
                Plugin.Singleton.StopRagdollList.Add(ev.Target);
                Ragdoll.Spawn(new RagdollInfo(ev.Target.ReferenceHub, ev.Handler.Base, ev.Target.Role,
                    ev.Target.Position, Quaternion.Euler(ev.Target.Rotation), ev.Target.Nickname, NetworkTime.time));
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Target)) return;
            switch (ev.Handler.Type)
            {
                case DamageType.Scp207:
                    ev.Amount = 0.0f;
                    break;
                case DamageType.Explosion:
                    ev.Amount *= 0.40f;
                    break;
                case DamageType.Decontamination:
                    ev.Amount = 0;
                    ev.IsAllowed = false;
                    break;
                default:
                {
                    if (ev.Handler.Type != DamageType.Warhead)
                        ev.Amount *= 0.70f;
                    break;
                }
            }
        }

        private void OnTeleporting(TeleportingEventArgs ev)
        {
            if (Check(ev.Player))
                ev.IsAllowed = false;
        }

        private void OnCreatingPortal(CreatingPortalEventArgs ev)
        {
            if (Check(ev.Player))
                ev.IsAllowed = false;
        }

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (ev.GrenadeType == GrenadeType.FragGrenade) return;
            foreach (var player in TrackedPlayers)
            {
                var dist = Vector3.Distance(ev.Grenade.transform.position, player.Position);
                if (!(dist <= 20f)) continue;
                if (Physics.Linecast(ev.Grenade.transform.position, player.Position,
                    player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                    return;
                var damage = FlashbangBaseDamage - dist * FlashbangFalloffMultiplier;
                if (damage < 0)
                    damage = 0f;
                else if (damage > FlashbangBaseDamage)
                    damage = FlashbangBaseDamage;

                Log.Debug($"{nameof(OnExplodingGrenade)}: Damage: {damage} - {dist} {player.Nickname}",
                    Plugin.Singleton.Config.Debug);
                player.Hurt(new UniversalDamageHandler(damage, DeathTranslations.Warhead));
                DoFlashEffect(player, dist);
            }
        }

        private void DoFlashEffect(Player player, float distance)
        {
            if (ResetConsumptionOnFlashed)
            {
                if (ConsumptionStacks[player] > 0)
                {
                    if (distance < 6 && ConsumptionStacks[player] > 1)
                        ConsumptionStacks[player]--;
                    else if (distance > 15)
                        ConsumptionStacks[player]++;

                    if (ConsumptionStacks[player] / 2 >= 3)
                        ConsumptionStacks[player] -= 3;
                    else if (ConsumptionStacks[player] / 2 >= 2)
                        ConsumptionStacks[player] -= 2;
                    else
                        ConsumptionStacks[player]--;
                }

                player.ShowHint($"You now have {ConsumptionStacks[player]} stacks of Consumption!", 10);
                var newIntensity = ConsumptionStacks[player] / (MaxConsumption / 2);
                player.ChangeEffectIntensity(EffectType.Scp207, (byte)newIntensity);
            }

            if (!TeleportOnFlashed) return;
            player.Position = Vector3.down * 1950f;

            Timing.CallDelayed(15f, () =>
            {
                foreach (var ply in Player.Get(Team.SCP))
                {
                    if (player == ply)
                        continue;
                    player.Position = ply.Position + Vector3.up;
                    break;
                }

                var hczArmoryDoor = Exiled.API.Features.Map.GetDoorByName("HCZ_ARMORY");
                var transform = hczArmoryDoor.Base.transform;
                player.Position = transform.position + transform.forward * 2f;
            });
        }

        private void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (Check(ev.Scp106))
                ev.IsAllowed = false;
            // ev.Player.Hurt(Mathf.Max(BaseDamage, BaseDamage * (ConsumptionStacks[ev.Scp106] / DamageScalar)), DamageTypes.RagdollLess, ev.Scp106.Nickname, ev.Scp106.Id);
        }

        public void IncreasePower(Player player)
        {
            if (ConsumptionStacks[player] >= MaxConsumption)
                return;

            Log.Debug($"{Name} power increase for {player.Nickname}");
            ConsumptionStacks[player]++;

            var newIntensity = ConsumptionStacks[player] / (MaxConsumption / 2);
            player.ChangeEffectIntensity(EffectType.Scp207, (byte)newIntensity);
            player.ShowHint($"You now have {ConsumptionStacks[player]} stacks of Consumption!");
        }

        private IEnumerator<float> Invisibility(Player player)
        {
            Log.Debug($"{nameof(Scp575)}: {nameof(Invisibility)}: Starting 268 loop for {player.Nickname}",
                Plugin.Singleton.Config.Debug);
            for (;;)
            {
                foreach (var ply in Player.List)
                {
                    if (VisionInformation.GetVisionInformation(ply.ReferenceHub, player.Position, -2f, 40f, false,
                        false, ply.ReferenceHub.localCurrentRoomEffects).IsLooking)
                    {
                        player.IsInvisible = false;
                        break;
                    }

                    player.IsInvisible = true;
                }

                if (!player.CurrentRoom.LightsOff)
                    player.CurrentRoom.TurnOffLights(10f);

                yield return Timing.WaitForSeconds(0.25f);
            }
        }
    }
}