namespace CustomRoles.Roles;

using CustomRoles.API;

using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;

using MEC;
using PlayerRoles;
using UnityEngine;

[CustomRole(RoleTypeId.None)]
public class Dwarf : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 100;

    public StartTeam StartTeam { get; set; } = StartTeam.ClassD;

    public override uint Id { get; set; } = 5;

    public override RoleTypeId Role { get; set; } = RoleTypeId.None;

    public override int MaxHealth { get; set; } = 100;

    public override string Name { get; set; } = "Dwarf";

    public override string Description { get; set; } =
        "A normal player who has unlimited stamina, and is slightly smaller than normal.";

    public override string CustomInfo { get; set; } = "Dwarf";

    public override bool KeepInventoryOnSpawn { get; set; } = true;

    public override bool KeepRoleOnDeath { get; set; } = true;

    public override bool RemovalKillsPlayer { get; set; } = false;

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };

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