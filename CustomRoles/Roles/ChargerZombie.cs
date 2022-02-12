namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using CustomRoles.Abilities;
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;

    [CustomRole(RoleType.Scp0492)]
    public class ChargerZombie : CustomRole
    {
        public override uint Id { get; set; } = 3;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 700;
        public override string Name { get; set; } = "Charger Zombie";

        public override string Description { get; set; } =
            "A zombie that is able to occasionally charge at high speed in a certain direction. If they come into contact with another player they will lock them in place for a few seconds.";

        public override string CustomInfo { get; set; } = "Charger Zombie";

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new ChargeAbility()
        };
    }
}