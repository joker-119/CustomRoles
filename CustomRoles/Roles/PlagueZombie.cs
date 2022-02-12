namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using CustomRoles.Abilities;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using PlayerStatsSystem;
    using Map = Exiled.Events.Handlers.Map;
    using Player = Exiled.Events.Handlers.Player;

    [CustomRole(RoleType.Scp0492)]
    public class PlagueZombie : CustomRole
    {
        public static List<Pickup> Grenades = new List<Pickup>();
        public override uint Id { get; set; } = 11;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 500;
        public override string Name { get; set; } = "Plague Zombie";

        public override string Description { get; set; } =
            "A slower and weaker zombie that is infectious with SCP-008. You can launch a projectile that will poison enemies near where it hits with the console command `.special`.\nIt is recommended you keybind this by running the console command `cmdbind g .special`.\nThis keybind applies to all roles with special abilities.";

        public override string CustomInfo { get; set; } = "Plague Zombie";

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new ProjectileAbility(),
            new MoveSpeedReduction()
        };

        protected override void SubscribeEvents()
        {
            Log.Debug($"{Name} loading events.", Plugin.Singleton.Config.Debug);
            Player.Hurting += OnHurt;
            Map.ExplodingGrenade += OnExplodingGrenade;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Log.Debug($"{Name} unloading events.", Plugin.Singleton.Config.Debug);
            Player.Hurting -= OnHurt;
            Map.ExplodingGrenade -= OnExplodingGrenade;
            base.UnsubscribeEvents();
        }

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (!Grenades.Contains(Pickup.Get(ev.Grenade))) return;
            ev.IsAllowed = false;
            foreach (Exiled.API.Features.Player player in ev.TargetsToAffect)
            {
                if (player.Role.Team == Team.SCP || (player.Position - ev.Grenade.transform.position).sqrMagnitude > 200)
                    continue;
                player.Hurt(new UniversalDamageHandler(30f, DeathTranslations.Poisoned));
                player.EnableEffect(EffectType.Poisoned, 25f);
            }
        }

        private void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Target.IsHuman && ev.Target.Health - ev.Amount <= 0 &&
                ev.Target.TryGetEffect(EffectType.Poisoned, out PlayerEffect poisoned) && poisoned.Intensity > 0)
            {
                ev.IsAllowed = false;
                ev.Amount = 0;
                ev.Target.DropItems();
                ev.Target.SetRole(RoleType.Scp0492, SpawnReason.ForceClass, true);
            }

            if (!Check(ev.Attacker))
                return;

            if (ev.Target.Role.Team == Team.SCP)
            {
                ev.Amount = 0f;
                return;
            }

            if (ev.Handler.Type == DamageType.Explosion)
            {
                ev.Amount = 0;
                ev.Target.Hurt(new UniversalDamageHandler(30f, DeathTranslations.Poisoned));
                ev.Target.EnableEffect(EffectType.Poisoned);
                return;
            }

            ev.Amount = 10f;

            if (Plugin.Singleton.Rng.Next(100) < 60)
                ev.Target.EnableEffect(EffectType.Poisoned);
        }
    }
}