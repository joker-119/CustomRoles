namespace CustomRoles.Roles;

using System.Collections.Generic;
using CustomRoles.Abilities;
using CustomRoles.API;

using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;

using PlayerRoles;

[CustomRole(RoleTypeId.Scp0492)]
public class BallisticZombie : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 25;

    public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

    public override uint Id { get; set; } = 1;

    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;

    public override int MaxHealth { get; set; } = 500;

    public override string Name { get; set; } = "Ballistic Zombie";

    public override string Description { get; set; } =
        "A regular zombie that will explode when killed.";

    public override string CustomInfo { get; set; } = "Ballistic Zombie";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };

    public override List<CustomAbility>? CustomAbilities { get; set; } = new()
    {
        new Martyrdom(),
    };
}