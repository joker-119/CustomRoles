using CustomRoles.API;
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
        protected override void RoleAdded() => Timing.CallDelayed(2.5f, () => Player.Scale = new Vector3(0.75f, 0.75f, 0.75f));

        protected override void RoleRemoved() => Player.Scale = Vector3.one;
    }
}