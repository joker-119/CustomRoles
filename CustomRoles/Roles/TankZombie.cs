namespace CustomRoles.Roles;

using System.Collections.Generic;
using System.ComponentModel;
using CustomRoles.Abilities;
using CustomRoles.API;

using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;

using PlayerRoles;
using PlayerStatsSystem;

[CustomRole(RoleTypeId.Scp0492)]
public class TankZombie : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 10;

    public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

    public override uint Id { get; set; } = 13;

    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;

    public override int MaxHealth { get; set; } = 1100;

    public override string Name { get; set; } = "Juggernaut Zombie";

    public override string Description { get; set; } =
        "A slightly slower zombie with double the regular health. As you take damage your AHP meter will fill. The higher it's value, the less damage you take.";

    public override string CustomInfo { get; set; } = "Juggernaut Zombie";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };

    [Description(
        "The maximum value of his hume shield. Higher values take longer for the hume to fill, meaning he takes more damage before reaching the maximum reduction from his shield.")]
    public int HumeMax { get; set; } = 500;

    [Description("The rate at which his hume shield will decay.")]
    public float HumeDecayRate { get; set; } = 2.5f;

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new ReactiveHume(),
        new MoveSpeedReduction(),
    };

    protected override void RoleAdded(Player player)
    {
        Log.Debug($"{Name}: Setting Max AHP and Decay");

        // Please work
        ((AhpStat)player.ReferenceHub.playerStats.StatModules[1]).ServerAddProcess(0, HumeMax, HumeDecayRate, 10f, 0f, true);
    }
}