using System.Collections.Generic;
using CustomRoles.Abilities;
using CustomRoles.API;
using Exiled.API.Features;
using UnityEngine;

namespace CustomRoles.Roles
{
    public class Medic : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.MediCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.MediCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.MediCfg.Name;

        protected override string Description { get; set; } =
            "A medic, equipped with a Medigun, TranqGun, EMP Grenade, and has the ability to activate a mist of healing chemicals around them, protecting nearby allies.\nYou can use \".special\" to activate a spray of healing mist to heal and fortify nearby allies.\nYou can keybind this ability with \"cmdbind KEY .special\"";

        protected override int AbilityCooldown { get; set; } = Plugin.Singleton.Config.RoleConfigs.MediCfg.AbilityCooldown;

        protected override List<string> Inventory { get; set; } = Plugin.Singleton.Config.RoleConfigs.MediCfg.Inventory;

        protected override Dictionary<Vector3, float> SpawnLocations { get; set; } = new Dictionary<Vector3, float>
        {
            { Exiled.API.Extensions.Role.GetRandomSpawnPoint(RoleType.NtfScientist), 100 }
        };

        public override string UseAbility()
        {
            Player.GameObject.AddComponent<HealingMist>();
            return "Ability used.";
        }
    }
}