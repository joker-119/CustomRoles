using System;
using CustomRoles.API;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace CustomRoles.Roles
{
    public class PlagueZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.Name;
        protected override string Description { get; set; } = 
            "A slightly slower zombie that is infectious with SCP-008 while being much weaker than regular zombies.";

        private Random _random;
        
        protected override void RoleAdded()
        {
            Player.ChangeRunningSpeed(0.90f);
            Player.ChangeWalkingSpeed(0.90f);
        }

        protected override void RoleRemoved()
        {
            Player.ChangeRunningSpeed(1f);
            Player.ChangeWalkingSpeed(1f);
        }

        protected override void LoadEvents()
        {
            Log.Debug($"{Name} loading events.");
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
        }

        protected override void UnloadEvents()
        {
            Log.Debug($"{Name} unloading events.");
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
        }

        private void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Attacker == Player)
            {
                ev.Amount = 10f;
                if (_random.Next(1, 100) < 60)
                {
                    ev.Target.EnableEffect(EffectType.Poisoned);
                }
            }
        }
    }
}