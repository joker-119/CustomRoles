using System.Collections.Generic;
using CustomRoles.API;
using Exiled.API.Features;
using UnityEngine;

namespace CustomRoles.Roles
{
    public class Shotgunner : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.ShotCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.ShotCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.ShotCfg.Name;
        protected override string Description { get; set; } =
            "A heavily armored Chaos Insurgency with a shotgun, pistol, and several healing items.";
        protected override List<string> Inventory { get; set; } = Plugin.Singleton.Config.RoleConfigs.ShotCfg.Inventory;

        protected override Dictionary<Vector3, float> SpawnLocations { get; set; } = new Dictionary<Vector3, float>
        {
            { Exiled.API.Extensions.Role.GetRandomSpawnPoint(RoleType.ChaosInsurgency), 100 },
        };
    }
}