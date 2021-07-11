using System.Collections.Generic;

namespace CustomRoles.Configs.Roles
{
    public class MedicConfig
    {
        public string Name { get; set; } = "Medic";
        public RoleType RoleType { get; set; } = RoleType.NtfScientist;
        public int MaxHealth { get; set; } = 120;
        public int AbilityCooldown { get; set; } = 90;
        public List<string> Inventory { get; set; } = new List<string>
        {
            "MG-119",
            "TG-119",
            "EM-119",
            $"{ItemType.Medkit}",
            $"{ItemType.Adrenaline}",
            $"{ItemType.Painkillers}",
            $"{ItemType.KeycardNTFLieutenant}"
        };

        public float AbilityDuration { get; set; } = 15f;
        public float AbilityHealAmount { get; set; } = 6f;
        public float AbilityFinaleProtection { get; set; } = 45f;
    }
}