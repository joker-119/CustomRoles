namespace CustomRoles.Roles
{
    using CustomRoles.Abilities;
    using CustomRoles.API;

    public class ChargerZombie: CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.ChargerZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.ChargerZombieCfg.MaxHealth;
        public override int AbilityCooldown { get; set; } = Plugin.Singleton.Config.RoleConfigs.ChargerZombieCfg.AbilityCooldown;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.ChargerZombieCfg.Name;

        protected override string Description { get; set; } =
            "A zombie that is able to occasionally charge at high speed in a certain direction. If they come into contact with another player they will lock them in place for a few seconds.";

        public override string UseAbility()
        {
            gameObject.AddComponent<ChargeAbility>();
            return "Ability used.";
        }
    }
}