namespace CustomRoles.Configs.Roles
{
    public class BerserkZombieConfig
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; } = "Berserk Zombie";
        public int MaxHealth { get; set; } = 250;
        public RoleType RoleType { get; set; } = RoleType.Scp0492;
    }
}