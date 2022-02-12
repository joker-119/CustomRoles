namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;

    [CustomRole(RoleType.NtfSpecialist)]
    public class Demolitionist : CustomRole
    {
        public override uint Id { get; set; } = 4;
        public override RoleType Role { get; set; } = RoleType.NtfSpecialist;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "Demolitionist";

        public override List<string> Inventory { get; set; } = new List<string>
        {
            "GL-119",
            "C4-119",
            "C4-119",
            ItemType.GrenadeHE.ToString(),
            ItemType.GrenadeHE.ToString(),
            ItemType.GrenadeHE.ToString(),
            ItemType.GrenadeHE.ToString(),
            ItemType.Radio.ToString()
        };

        public override string Description { get; set; } =
            "An NTF Specialist who specializes in explosive ordinance. Spawns with a Grenade Launcher, a number of grenades and some C4.";

        public override string CustomInfo { get; set; } = "Demolitionist";
    }
}