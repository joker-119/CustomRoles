namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using CustomRoles.Abilities;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Player = Exiled.Events.Handlers.Player;

    [CustomRole(RoleType.Scp0492)]
    public class MedicZombie : CustomRole
    {
        public override uint Id { get; set; } = 8;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 500;
        public override string Name { get; set; } = "Medic Zombie";

        public override string Description { get; set; } = "A slightly slower and weaker zombie that heals nearby SCPs";
        public override string CustomInfo { get; set; } = "Medic Zombie";

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new HealingMist(),
            new MoveSpeedReduction()
        };

        protected override void SubscribeEvents()
        {
            Log.Debug($"{Name} loading events.", Plugin.Singleton.Config.Debug);
            Player.Hurting += OnHurt;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Log.Debug($"{Name} unloading events.", Plugin.Singleton.Config.Debug);
            Player.Hurting -= OnHurt;
            base.UnsubscribeEvents();
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker)) ev.Amount *= 0.75f;
        }
    }
}