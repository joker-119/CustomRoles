namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using Abilities;
    using Exiled.API.Features;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Player = Exiled.Events.Handlers.Player;

    public class Medic : CustomRole
    {
        public override uint Id { get; set; } = 7;
        public override RoleType Role { get; set; } = RoleType.NtfSpecialist;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "Medic";

        public override string Description { get; set; } =
            "A medic, equipped with a Medigun, TranqGun, EMP Grenade, and has the ability to activate a mist of healing chemicals around them, protecting nearby allies.\nYou can use \".special\" to activate a spray of healing mist to heal and fortify nearby allies.\nYou can keybind this ability with \"cmdbind KEY .special\"";

        protected override List<string> Inventory { get; set; } = new()
        {
            "MG-119",
            "TG-119",
            "EM-119",
            $"{ItemType.Medkit}",
            $"{ItemType.Adrenaline}",
            $"{ItemType.Painkillers}",
            $"{ItemType.KeycardNTFLieutenant}"
        };

        protected override SpawnProperties SpawnProperties { get; set; } = new()
        {
            RoleSpawnPoints = new List<RoleSpawnPoint>
            {
                new()
                {
                    Role = RoleType.NtfSpecialist,
                    Chance = 100
                }
            }
        };

        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new HealingMist()
        };

        protected override void SubscribeEvents()
        {
            Log.Debug($"{nameof(SubscribeEvents)}: Loading medic events..");
            Player.PickingUpItem += OnPickingUpItem;
            base.SubscribeEvents();
        }

        protected override void UnSubscribeEvents()
        {
            Log.Debug($"{nameof(UnSubscribeEvents)}: Unloading medic events..");
            Player.PickingUpItem -= OnPickingUpItem;
            base.UnSubscribeEvents();
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!Check(ev.Player))
                return;

            ev.Player.ShowHint("You are not able to pick up this item, because it is of the same type as mediguns.");
            ev.IsAllowed = false;
        }
    }
}