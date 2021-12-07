namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using Abilities;
    using Exiled.CustomRoles.API.Features;

    public class BallisticZombie : CustomRole
    {
        public override uint Id { get; set; } = 1;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 500;
        public override string Name { get; set; } = "Ballistic Zombie";

        public override string Description { get; set; } =
            "A regular zombie that will explode when killed.";

        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new Martyrdom()
        };
    }
}