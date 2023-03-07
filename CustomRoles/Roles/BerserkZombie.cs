namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using CustomRoles.Abilities;
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;
    using PlayerRoles;

    [CustomRole(RoleTypeId.Scp0492)]
    public class BerserkZombie : CustomRole
    {
        public override uint Id { get; set; } = 2;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public override int MaxHealth { get; set; } = 250;
        public override string Name { get; set; } = "Berserk Zombie";

        public override string Description { get; set; } = "A zombie that gains more speed when they kill someone.";
        public override string CustomInfo { get; set; } = "Berserk Zombie";

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
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