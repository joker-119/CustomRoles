namespace CustomRoles.Roles;

using System.Collections.Generic;
using CustomRoles.Abilities;
using CustomRoles.API;

using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;

using PlayerRoles;

[CustomRole(RoleTypeId.Scp0492)]
public class ChargerZombie : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 30;

    public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

    public override uint Id { get; set; } = 3;

    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;

    public override int MaxHealth { get; set; } = 700;

    public override string Name { get; set; } = "Charger Zombie";

    public override string Description { get; set; } =
        "A zombie that is able to occasionally charge at high speed in a certain direction. If they come into contact with another player they will lock them in place for a few seconds.";

    public override string CustomInfo { get; set; } = "Charger Zombie";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new ChargeAbility(),
    };
}