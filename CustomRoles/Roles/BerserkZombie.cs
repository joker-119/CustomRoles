namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using Abilities;
    using Exiled.CustomRoles.API.Features;

    public class BerserkZombie : CustomRole
    {
        public override uint Id { get; set; } = 2;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 250;
        public override string Name { get; set; } = "Berserk Zombie";

        public override string Description { get; set; } = "A zombie that gains more speed when they kill someone.";

        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new SpeedOnKill
            {
                IntensityLimit = 2
            },
            new HealOnKill
            {
                HealAmount = 100f
            }
        };
    }
}