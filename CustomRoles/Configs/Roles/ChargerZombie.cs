namespace CustomRoles.Configs.Roles
{
    public class ChargerZombie
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; } = "Charger Zombie";
        public int MaxHealth { get; set; } = 700;
        public RoleType RoleType { get; set; } = RoleType.Scp0492;
        public int AbilityCooldown { get; set; } = 45;
    }
}