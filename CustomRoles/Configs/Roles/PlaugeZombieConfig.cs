namespace CustomRoles.Configs.Roles
{
    public class PlagueZombieConfig
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; } = "Plague Zombie";
        public int MaxHealth { get; set; } = 500;
        public RoleType RoleType { get; set; } = RoleType.Scp0492;
        
    }
}