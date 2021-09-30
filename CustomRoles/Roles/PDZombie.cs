using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace CustomRoles.Roles
{
    using Exiled.CustomRoles.API.Features;

    public class PDZombie : CustomRole
    {
        public override uint Id { get; set; } = 9;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 500;
        public override string Name { get; set; } = "Pocket Dimension Zombie";
        public override string Description { get; set; } =
            "A zombie with ballistic damage resistance, but is instantly killed by flash grenades. Has a 25% chance when hitting someone to teleport them to the Pocket Dimension";

        protected override void SubscribeEvents()
        {
            Log.Debug($"{Name}:{nameof(SubscribeEvents)}: loading events.", Plugin.Singleton.Config.Debug);
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
            Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;
            base.SubscribeEvents();
        }

        protected override void UnSubscribeEvents()
        {
            Log.Debug($"{Name}:{nameof(UnSubscribeEvents)}: unloading events.");
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
            Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;
            base.UnSubscribeEvents();
        }
        
        private void OnHurt(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                int chance = Plugin.Singleton.Rng.Next(100);
                
                if (chance < 25)
                    ev.Target.EnableEffect(EffectType.Corroding);
            }

            if (Check(ev.Target) && ev.Attacker.IsHuman && ev.Attacker.CurrentItem.Type.IsWeapon(false)) 
                ev.Amount *= 0.20f;
        }

        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (Check(ev.Player) && ev.Effect is Flashed) 
                ev.Player.Kill(DamageTypes.Grenade);
        }
    }
}