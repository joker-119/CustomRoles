namespace CustomRoles.Roles;

using System.Collections.Generic;
using CustomRoles.Abilities;
using CustomRoles.API;

using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

using Player = Exiled.Events.Handlers.Player;

[CustomRole(RoleTypeId.Scp0492)]
public class MedicZombie : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 15;

    public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

    public override uint Id { get; set; } = 8;

    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;

    public override int MaxHealth { get; set; } = 500;

    public override string Name { get; set; } = "Medic Zombie";

    public override string Description { get; set; } = "A slightly slower and weaker zombie that heals nearby SCPs";

    public override string CustomInfo { get; set; } = "Medic Zombie";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new HealingMist(),
        new MoveSpeedReduction(),
    };

    protected override void SubscribeEvents()
    {
        Log.Debug($"{Name} loading events.");
        Player.Hurting += OnHurt;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Log.Debug($"{Name} unloading events.");
        Player.Hurting -= OnHurt;
        base.UnsubscribeEvents();
    }

    private void OnHurt(HurtingEventArgs ev)
    {
        if (Check(ev.Attacker))
            ev.Amount *= 0.75f;
    }
}