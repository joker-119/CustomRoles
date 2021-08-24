namespace CustomRoles.Configs.Roles
{
    public class PhantomConfig
    {
        public RoleType RoleType { get; set; } = RoleType.ChaosConscript;
        public string Name { get; set; } = "Chaos Phantom";
        public int MaxHealth { get; set; } = 100;
        public int AbilityCooldown { get; set; } = 90;
    }
}