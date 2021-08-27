namespace CustomRoles.Configs.Roles
{
    public class TankZombieConfig
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; } = "Juggernaut Zombie";
        public RoleType RoleType { get; set; } = RoleType.Scp0492;
        public int MaxHealth { get; set; } = 1000;
    }
}