using CustomRoles.API;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace CustomRoles.Roles
{
    public class Dwarf : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.DwarfCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.DwarfCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.DwarfCfg.Name;
        protected override string Description { get; set; } =
            "A normal player who has unlimited stamina, and is slightly smaller than normal.";
        
        protected override bool KeepInventoryOnChange { get; set; } = true;
        protected override bool LoseRoleOnChange { get; set; } = false;

        protected override void RoleAdded() => Timing.CallDelayed(2.5f, () => Player.Scale = new Vector3(0.75f, 0.75f, 0.75f));

        protected override void RoleRemoved() => Player.Scale = Vector3.one;

        private void FixedUpdate() => Player.Stamina.RemainingStamina = 1f;

        protected override void Add()
        {
            if (Type != RoleType.None)
                Player.SetRole(Type, SpawnReason.ForceClass, true);

            Timing.CallDelayed(1.5f, () =>
            {
                Vector3 pos = GetSpawnPosition();
                if (pos != Vector3.zero)
                    Player.Position = pos;

                if (!KeepInventoryOnChange)
                    Player.ClearInventory();

                foreach (string itemName in Inventory)
                    TryAddItem(itemName);

                Player.Ammo[ItemType.Ammo9x19] = 100;
                Player.Ammo[ItemType.Ammo12gauge] = 100;
                Player.Ammo[ItemType.Ammo44cal] = 100;
                Player.Ammo[ItemType.Ammo556x45] = 100;
                Player.Ammo[ItemType.Ammo762x39] = 100;

                if (Type.GetSide() != Side.Scp)
                    Player.MaxHealth = MaxHealth;
            });

            Player.CustomInfo = $"{Name} (Custom Role)";
            ShowMessage();
            RoleAdded();
        }
    }
}