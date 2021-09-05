namespace CustomRoles.Roles
{
    using Exiled.CustomRoles.API.Features;

    public class GenericRole : CustomRole
    {
        public override uint Id { get; set; } = 99;
        public override RoleType Role { get; set; } = RoleType.None;
        public override int MaxHealth { get; set; } = 1000;
        public override string Name { get; set; } = "Generic Role";
        public override string Description { get; set; } = "A generic role.";
    }
}