using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace CustomRoles.Roles
{
    using Exiled.CustomRoles.API.Features;

    public class DwarfZombie : CustomRole
    {
        public override uint Id { get; set; } = 6;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 450;
        public override string Name { get; set; } = "Dwarf Zombie";

        public override string Description { get; set; } =
            "A weaker, smaller, amd faster zombie than its brothers.";
        protected override void SubscribeEvents()
        {
            Log.Debug($"{Name}:{nameof(SubscribeEvents)} loading events.", Plugin.Singleton.Config.Debug);
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
            base.SubscribeEvents();
        }

        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(2.5f, () =>
            {
                player.Scale = new Vector3(0.75f, 0.75f, 0.75f);
                player.EnableEffect(EffectType.Scp207);
            });
        }

        protected override void UnSubscribeEvents()
        {
            Log.Debug($"{Name}:{nameof(UnSubscribeEvents)} unloading events.", Plugin.Singleton.Config.Debug);
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
            base.UnSubscribeEvents();
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.Scp207);
            player.Scale = Vector3.one;
        }

        private void OnHurt(HurtingEventArgs ev)
        {
            if (Check(ev.Target) && ev.DamageType.Equals(DamageTypes.Scp207)) 
                ev.IsAllowed = false;
            
            if (Check(ev.Attacker))
            {
                ev.Amount *= 0.7f;
            }
        }
    }
}