using CustomRoles.API;
using Exiled.API.Extensions;
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
            "A weaker and smaller zombie with greater speed than its brothers.";
        protected override void RoleAdded()
        {
            Timing.CallDelayed(2.5f, () => Player.Scale = new Vector3(0.75f, 0.75f, 0.75f));
            Player.ChangeRunningSpeed(1.2f);
            Player.ChangeWalkingSpeed(1.2f);
        }

        protected override void RoleRemoved()
        {
            Player.ChangeRunningSpeed(1f);
            Player.ChangeWalkingSpeed(1f);
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
            if (ev.Attacker == Player)
            {
                ev.Amount *= 0.7f;
            }
        }
    }
}