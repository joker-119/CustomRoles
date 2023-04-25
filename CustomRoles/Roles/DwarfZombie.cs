namespace CustomRoles.Roles;

using CustomRoles.API;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;

using MEC;
using PlayerRoles;
using UnityEngine;

[CustomRole(RoleTypeId.Scp0492)]
public class DwarfZombie : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 20;

    public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

    public override uint Id { get; set; } = 6;

    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;

    public override int MaxHealth { get; set; } = 450;

    public override string Name { get; set; } = "Dwarf Zombie";

    public override string Description { get; set; } =
        "A weaker, smaller, amd faster zombie than its brothers.";

    public override string CustomInfo { get; set; } = "Dwarf Zombie";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };

    protected override void RoleAdded(Player player)
    {
        Timing.CallDelayed(2.5f, () =>
        {
            player.Scale = new Vector3(0.75f, 0.75f, 0.75f);
            player.EnableEffect(EffectType.MovementBoost);
        });
    }

    protected override void RoleRemoved(Player player)
    {
        player.DisableEffect(EffectType.Scp207);
        player.Scale = Vector3.one;
    }
}