using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using CustomRoles.Abilities;
    using Exiled.CustomRoles.API.Features;

    public class MedicZombie : CustomRole
    {
        public override uint Id { get; set; } = 8;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 500;
        public override string Name { get; set; } = "Medic Zombie";

        public override string Description { get; set; } = "A slightly slower and weaker zombie that heals nearby SCPs";

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new HealingMist(),
            new MoveSpeedReduction(),
        };

        protected override void SubscribeEvents()
        {
            Log.Debug($"{Name} loading events.");
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
            base.SubscribeEvents();
        }
        protected override void UnSubscribeEvents()
        {
            Log.Debug($"{Name} unloading events.");
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
            base.UnSubscribeEvents();
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                ev.Amount *= 0.75f;
            }
        }
    }
}