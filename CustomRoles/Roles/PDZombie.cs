namespace CustomRoles.Roles
{
    using System.ComponentModel;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using PlayerStatsSystem;
    using Player = Exiled.Events.Handlers.Player;

    public class PDZombie : CustomRole
    {
        public override uint Id { get; set; } = 9;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 500;
        public override string Name { get; set; } = "Pocket Dimension Zombie";

        public override string Description { get; set; } =
            "A zombie with ballistic damage resistance, but is instantly killed by flash grenades. Has a 25% chance when hitting someone to teleport them to the Pocket Dimension";

        [Description("The chance the zombie has on each melee hit to teleport the target to the pocket dimension.")]
        public int TeleportChance { get; set; } = 25;

        protected override void SubscribeEvents()
        {
            Log.Debug($"{Name}:{nameof(SubscribeEvents)}: loading events.", Plugin.Singleton.Config.Debug);
            Player.Hurting += OnHurt;
            Player.ReceivingEffect += OnReceivingEffect;
            base.SubscribeEvents();
        }

        protected override void UnSubscribeEvents()
        {
            Log.Debug($"{Name}:{nameof(UnSubscribeEvents)}: unloading events.");
            Player.Hurting -= OnHurt;
            Player.ReceivingEffect -= OnReceivingEffect;
            base.UnSubscribeEvents();
        }

        private void OnHurt(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                var chance = Plugin.Singleton.Rng.Next(100);

                if (chance < TeleportChance)
                    ev.Target.EnableEffect(EffectType.Corroding);
            }

            if (Check(ev.Target) && ev.Attacker.IsHuman && ev.Attacker.CurrentItem.Type.IsWeapon(false))
                ev.Amount *= 0.20f;
        }

        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (Check(ev.Player) && ev.Effect is Flashed)
                ev.Player.Kill(DeathTranslations.Explosion.LogLabel);
        }
    }
}