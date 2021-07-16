using CustomRoles.API;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace CustomRoles.Roles
{
    public class DwarfZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.DwarfZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.DwarfZombieCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.DwarfZombieCfg.Name;

        protected override string Description { get; set; } =
            "A weaker, smaller, amd faster zombie than its brothers.";
        protected override void RoleAdded()
        {
            Timing.CallDelayed(2.5f, () =>
            {
                Player.Scale = new Vector3(0.75f, 0.75f, 0.75f);
                Player.EnableEffect(EffectType.Scp207);
            });
        }

        protected override void RoleRemoved()
        {
            Player.DisableEffect(EffectType.Scp207);
            Player.Scale = Vector3.one;
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
            if (ev.Target == Player && ev.DamageType == DamageTypes.Scp207) ev.IsAllowed = false;
            if (ev.Attacker == Player)
            {
                ev.Amount *= 0.7f;
            }
        }
    }
}