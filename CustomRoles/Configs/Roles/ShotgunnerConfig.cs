using System.Collections.Generic;

namespace CustomRoles.Configs.Roles
{
    public class ShotgunnerConfig
    {
        public string Name { get; set; } = "Shotgunner";
        public int MaxHealth { get; set; } = 300;
        public RoleType RoleType { get; set; } = RoleType.ChaosInsurgency;

        public List<string> Inventory { get; set; } = new List<string>
        {
            "SG-119",
            "COM15",
            "IG-119",
            $"{ItemType.Medkit}",
            $"{ItemType.KeycardChaosInsurgency}",
            $"{ItemType.Painkillers}",
            $"{ItemType.Adrenaline}"
        };
    }
}