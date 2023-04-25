namespace CustomRoles.Roles;

using System.Collections.Generic;

using CustomRoles.API;

using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;

using PlayerRoles;

[CustomRole(RoleTypeId.NtfSpecialist)]
public class Demolitionist : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 100;

    public StartTeam StartTeam { get; set; } = StartTeam.Ntf;

    public override uint Id { get; set; } = 4;

    public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSpecialist;

    public override int MaxHealth { get; set; } = 120;

    public override string Name { get; set; } = "Demolitionist";

    public override List<string> Inventory { get; set; } = new()
    {
        "GL-119",
        "C4-119",
        "C4-119",
        ItemType.GrenadeHE.ToString(),
        ItemType.GrenadeHE.ToString(),
        ItemType.GrenadeHE.ToString(),
        ItemType.GrenadeHE.ToString(),
        ItemType.Radio.ToString(),
    };

    public override string Description { get; set; } =
        "An NTF Specialist who specializes in explosive ordinance. Spawns with a Grenade Launcher, a number of grenades and some C4.";

    public override string CustomInfo { get; set; } = "Demolitionist";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };
}