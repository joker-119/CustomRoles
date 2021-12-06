namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using Abilities;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;

    public class Phantom : CustomRole
    {
        public override uint Id { get; set; } = 10;
        public override RoleType Role { get; set; } = RoleType.ChaosConscript;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "Chaos Phantom";

        public override string Description { get; set; } =
            "A Chaos Insurgency outfitted with an active-camo suit that allows them to go invisible at will.\n\nUse the Client console command \".special\" to activate this ability. This can be keybound with \"cmdbind KEY .special\"";

        protected override SpawnProperties SpawnProperties { get; set; } = new()
        {
            RoleSpawnPoints = new List<RoleSpawnPoint>
            {
                new()
                {
                    Role = RoleType.FacilityGuard,
                    Chance = 100
                }
            }
        };

        protected override List<string> Inventory { get; set; } = new()
        {
            "SR-119",
            "IG-119",
            "EM-119",
            "SCP-127",
            $"{ItemType.Medkit}",
            $"{ItemType.KeycardChaosInsurgency}",
            $"{ItemType.Adrenaline}",
            $"{ItemType.SCP268}"
        };

        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new ActiveCamo()
        };

        protected override void SubscribeEvents()
        {
            Player.DroppingItem += OnDroppingItem;
            Player.PickingUpItem += OnPickingUpItem;
            Player.UsingItem += OnUsingMedicalItem;
            base.SubscribeEvents();
        }

        private void OnUsingMedicalItem(UsingItemEventArgs ev)
        {
            if (Check(ev.Player) && ev.Item.Type == ItemType.SCP268)
                ev.IsAllowed = false;
        }

        protected override void UnSubscribeEvents()
        {
            Player.DroppingItem -= OnDroppingItem;
            Player.PickingUpItem -= OnPickingUpItem;
            Player.UsingItem -= OnUsingMedicalItem;
            base.UnSubscribeEvents();
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
}