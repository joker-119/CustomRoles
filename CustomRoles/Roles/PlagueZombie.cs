using System;
using CustomRoles.API;
using Exiled.API.Enums;
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
            try
            {
                Log.Debug($"{Name} added to {Player.Nickname}");
            }
            catch (Exception e)
            {
                Log.Error($"{Name}: {e}\n{e.StackTrace}");
            }
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