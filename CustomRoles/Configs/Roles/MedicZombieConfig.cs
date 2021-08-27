namespace CustomRoles.Configs.Roles
{
    public class MedicZombieConfig
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; } = "Medic Zombie";
        public int MaxHealth { get; set; } = 500;
        public RoleType RoleType { get; set; } = RoleType.Scp0492;
        public int AbilityCooldown { get; set; } = 90;
        public float AbilityDuration { get; set; } = 15f;
        public float AbilityHealAmount { get; set; } = 10;
        public ushort AbilityFinaleProtection { get; set; } = 45;
    }
}