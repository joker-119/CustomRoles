namespace CustomRoles.Configs.Roles
{
    public class Scp575Config
    {
        public RoleType RoleType { get; set; } = RoleType.Scp106;
        public string Name { get; set; } = "SCP-575";
        public int MaxHealth { get; set; } = 550;
        public float KeterDamage { get; set; } = 5f;
        public string KeterBroadcast { get; set; } = "You have been damaged by SCP-575!";
        public float KeterBroadcastDuration { get; set; } = 5f;
        public float AbilityDuration { get; set; } = 120f;
        public int AbilityCooldown { get; set; } = 300;
        public int AbilityPowerLevelRequirement { get; set; } = 10;
        public int MaxPowerLevel { get; set; } = 10;
        public float FlashbangBaseDamage { get; set; } = 450f;
        public float FlashbangFalloffMultiplier { get; set; } = 30f;
        public bool ResetPowerOnFlashbang { get; set; } = true;
        public bool TeleportOnFlashbang { get; set; } = true;
        public float BaseDamage { get; set; } = 30;
        public int DamageScalerValue { get; set; } = 2;
    }
}