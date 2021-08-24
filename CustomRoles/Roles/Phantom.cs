using System.Collections.Generic;
using CustomRoles.Abilities;
using CustomRoles.API;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace CustomRoles.Roles
{
    using Exiled.API.Extensions;

    public class Phantom : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.PhantomCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.PhantomCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.PhantomCfg.Name;
        protected override string Description { get; set; } = "A Chaos Insurgency outfitted with an active-camo suit that allows them to go invisible at will.\n\nUse the Client console command \".special\" to activate this ability. This can be keybound with \"cmdbind KEY .special\"";

        public override int AbilityCooldown { get; set; } =
            Plugin.Singleton.Config.RoleConfigs.PhantomCfg.AbilityCooldown;

        protected override Dictionary<Vector3, float> SpawnLocations { get; set; } = new Dictionary<Vector3, float>
        {
            { RoleType.FacilityGuard.GetRandomSpawnProperties().Item1, 100 },
        };

        protected override List<string> Inventory { get; set; } = new List<string>
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

        public override string UseAbility()
        {
            Player.GameObject.AddComponent<ActiveCamo>();
            return "Ability used.";
        }

        protected override void LoadEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingMedicalItem;
        }

        private void OnUsingMedicalItem(UsingItemEventArgs ev)
        {
            if (ev.Player == Player && ev.Item.Type == ItemType.SCP268)
                ev.IsAllowed = false;
        }

        protected override void UnloadEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingMedicalItem;
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (ev.Player == Player && ev.Pickup.Type == ItemType.SCP268)
                ev.IsAllowed = false;
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.Player == Player && ev.Item.Type == ItemType.SCP268)
                ev.IsAllowed = false;
        }
    }
}