namespace CustomRoles.Roles;

using System.Collections.Generic;
using CustomRoles.Abilities;
using CustomRoles.API;

using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using PlayerRoles;

[CustomRole(RoleTypeId.ChaosConscript)]
public class Phantom : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 100;

    public StartTeam StartTeam { get; set; } = StartTeam.Guard;

    public override uint Id { get; set; } = 10;

    public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosConscript;

    public override int MaxHealth { get; set; } = 120;

    public override string Name { get; set; } = "Chaos Phantom";

    public override string Description { get; set; } =
        "A Chaos Insurgency outfitted with an active-camo suit that allows them to go invisible at will.\n\nUse the Client console command \".special\" to activate this ability. This can be keybound with \"cmdbind KEY .special\"";

    public override string CustomInfo { get; set; } = "Chaos Phantom";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
        RoleSpawnPoints = new List<RoleSpawnPoint>
        {
            new()
            {
                Role = RoleTypeId.FacilityGuard,
                Chance = 100,
            },
        },
    };

    public override List<string> Inventory { get; set; } = new()
    {
        "SR-119",
        "IG-119",
        "EM-119",
        "SCP-127",
        $"{ItemType.Medkit}",
        $"{ItemType.KeycardChaosInsurgency}",
        $"{ItemType.Adrenaline}",
        $"{ItemType.SCP268}",
    };

    public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new()
    {
        {
            AmmoType.Nato556, 25
        },
    };

    public override List<CustomAbility>? CustomAbilities { get; set; } = new()
    {
        new ActiveCamo(),
    };

    protected override void SubscribeEvents()
    {
        Player.DroppingItem += OnDroppingItem;
        Player.PickingUpItem += OnPickingUpItem;
        Player.UsingItem += OnUsingMedicalItem;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Player.DroppingItem -= OnDroppingItem;
        Player.PickingUpItem -= OnPickingUpItem;
        Player.UsingItem -= OnUsingMedicalItem;
        base.UnsubscribeEvents();
    }

    private void OnUsingMedicalItem(UsingItemEventArgs ev)
    {
        if (Check(ev.Player) && ev.Item.Type == ItemType.SCP268)
            ev.IsAllowed = false;
    }

    private void OnPickingUpItem(PickingUpItemEventArgs ev)
    {
        if (Check(ev.Player) && ev.Pickup.Type == ItemType.SCP268)
            ev.IsAllowed = false;
    }

    private void OnDroppingItem(DroppingItemEventArgs ev)
    {
        if (Check(ev.Player) && ev.Item.Type == ItemType.SCP268)
            ev.IsAllowed = false;
    }
}