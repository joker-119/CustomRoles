using CustomRoles.API;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace CustomRoles.Roles
{
    public class BerserkZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.BerserkZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.BerserkZombieCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.BerserkZombieCfg.Name;

        protected override string Description { get; set; } =
            "A zombie that gains more speed when they kill someone.";

        protected override void LoadEvents()
        {
            Log.Debug($"{Name}:{nameof(LoadEvents)}: loading events.", Plugin.Singleton.Config.Debug);
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        protected override void UnloadEvents()
        {
            Log.Debug($"{Name}:{nameof(UnloadEvents)}: unloading events.", Plugin.Singleton.Config.Debug);
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
            Exiled.Events.Handlers.Player.Died -= OnDied;
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Target == Player && ev.DamageType == DamageTypes.Scp207) 
                ev.IsAllowed = false;
            
            if (ev.Attacker == Player)
            {
                ev.Amount *= 2f;
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (ev.Killer == Player) 
                Player.EnableEffect(EffectType.Scp207, 10f, true);
        }
    }
}