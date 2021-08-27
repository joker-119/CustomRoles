using System;
using CustomRoles.API;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using CustomRoles.Abilities;
    using Exiled.API.Extensions;
    using Exiled.API.Features.Items;
    using Mirror;
    using UnityEngine;

    public class PlagueZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.Name;
        public override int AbilityCooldown { get; set; } = 60;

        protected override string Description { get; set; } = 
            "A slower and weaker zombie that is infectious with SCP-008. You can launch a projectile that will poison enemies near where it hits with the console command `.special`.\nIt is recommended you keybind this by running the console command `cmdbind g .special`.\nThis keybind applies to all roles with special abilities.";

        public static List<Pickup> Grenades = new List<Pickup>();

        public override string UseAbility()
        {
            gameObject.AddComponent<ProjectileAbility>();
            return "Ability used.";
        }

        protected override void RoleAdded()
        {
            Timing.CallDelayed(2.5f, () =>
            {
                Player.EnableEffect(EffectType.SinkHole);
            });
        }

        protected override void RoleRemoved()
        {
            Player.DisableEffect(EffectType.SinkHole);
        }

        protected override void LoadEvents()
        {
            Log.Debug($"{Name} loading events.", Plugin.Singleton.Config.Debug);
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
            Exiled.Events.Handlers.Map.ExplodingGrenade += OnExplodingGrenade;
        }

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (Grenades.Contains(Pickup.Get(ev.Grenade)))
            {
                ev.IsAllowed = false;
                foreach (Player player in ev.TargetsToAffect)
                {
                    if (player.Team == Team.SCP || (player.Position - ev.Grenade.transform.position).sqrMagnitude > 200)
                        continue;
                    player.Hurt(30f, DamageTypes.Poison, ev.Thrower.Nickname, ev.Thrower.Id);
                    player.EnableEffect(EffectType.Poisoned, 25f);
                }

                return;
            }
        }

        protected override void UnloadEvents()
        {
            Log.Debug($"{Name} unloading events.", Plugin.Singleton.Config.Debug);
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
            Exiled.Events.Handlers.Map.ExplodingGrenade -= OnExplodingGrenade;
        }

        private void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Target.IsHuman && (ev.Target.Health - ev.Amount) <= 0 && (ev.Target.TryGetEffect(EffectType.Poisoned, out PlayerEffect poisoned) && poisoned.Intensity > 0))
            {
                ev.IsAllowed = false;
                ev.Amount = 0;
                ev.Target.DropItems();
                ev.Target.SetRole(RoleType.Scp0492, SpawnReason.ForceClass, true);
            }

            if (ev.Attacker != Player) 
                return;

            if (ev.Target.Team == Team.SCP)
            {
                ev.Amount = 0f;
                return;
            }
            
            if (ev.DamageType == DamageTypes.Grenade)
            {
                ev.Amount = 0;
                ev.Target.Hurt(30, DamageTypes.Poison, ev.Attacker.Nickname, ev.Attacker.Id);
                ev.Target.EnableEffect(EffectType.Poisoned);
                return;
            }

            ev.Amount = 10f;
                
            if (Plugin.Singleton.Rng.Next(100) < 60) 
                ev.Target.EnableEffect(EffectType.Poisoned);
        }
    }
}