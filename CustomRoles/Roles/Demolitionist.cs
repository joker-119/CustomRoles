using System.Collections.Generic;
using CustomRoles.API;
using Exiled.API.Features;

namespace CustomRoles.Roles
{
    public class Demolitionist : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.DemoCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.DemoCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.DemoCfg.Name;
        protected override List<string> Inventory { get; set; } = Plugin.Singleton.Config.RoleConfigs.DemoCfg.Inventory;

        protected override string Description { get; set; } = Plugin.Singleton.Config.RoleConfigs.DemoCfg.Description;
    }
}