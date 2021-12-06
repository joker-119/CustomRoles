namespace CustomRoles.Roles
{
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using MEC;
    using UnityEngine;

    public class Dwarf : CustomRole
    {
        public override uint Id { get; set; } = 5;
        public override RoleType Role { get; set; } = RoleType.None;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Dwarf";

        public override string Description { get; set; } =
            "A normal player who has unlimited stamina, and is slightly smaller than normal.";

        protected override bool KeepInventoryOnSpawn { get; set; } = true;
        protected override bool KeepRoleOnDeath { get; set; } = true;
        protected override bool RemovalKillsPlayer { get; set; } = false;

        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(2.5f, () => player.Scale = new Vector3(0.75f, 0.75f, 0.75f));
            player.IsUsingStamina = false;
        }

        protected override void RoleRemoved(Player player)
        {
            player.IsUsingStamina = true;
            player.Scale = Vector3.one;
        }
    }
}