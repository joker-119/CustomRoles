using System;
using CustomPlayerEffects;
using CustomRoles.API;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomItems.API;
using Exiled.Events.EventArgs;
using ServerOutput;

namespace CustomRoles.Roles
{
    public class PDZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.Name;
        protected override string Description { get; set; } =
            "A zombie with ballistic damage resistance, but is instantly killed by flash grenades. Has a 25% chance when hitting someone to teleport them to the Pocket Dimension";
        
        protected override void RoleAdded() => Log.Debug($"{Name} added to {Player.Nickname}", Plugin.Singleton.Config.Debug);

        protected override void LoadEvents()
        {
            Log.Debug($"{Name}:{nameof(LoadEvents)}: loading events.", Plugin.Singleton.Config.Debug);
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
            Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;
        }

        protected override void UnloadEvents()
        {
            Log.Debug($"{Name}:{nameof(UnloadEvents)}: unloading events.");
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
            Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;
        }
        
        private void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Attacker == Player)
            {
                int chance = Plugin.Singleton.Rng.Next(100);
                
                if (chance < 25)
                    ev.Target.EnableEffect(EffectType.Corroding);
            }

            if (ev.Target == Player && ev.Attacker.IsHuman && ev.Attacker.CurrentItem.Type.IsWeapon(false)) 
                ev.Amount *= 0.20f;
        }

        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (ev.Player == Player && ev.Effect is Flashed) 
                Player.Kill(DamageTypes.Grenade);
        }
    }
}