namespace CustomRoles.Configs.Roles
{
    public class DwarfConfig
    {
        public string Name { get; set; } = "Dwarf";
        public RoleType RoleType { get; set; } = RoleType.None;
        public int MaxHealth { get; set; } = 100;
    }
}