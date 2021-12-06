namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Abilities;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using PlayerStatsSystem;

    public class TankZombie : CustomRole
    {
        public override uint Id { get; set; } = 13;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 1100;
        public override string Name { get; set; } = "Juggernaut Zombie";

        public override string Description { get; set; } =
            "A slightly slower zombie with double the regular health. As you take damage your AHP meter will fill. The higher it's value, the less damage you take.";

        [Description(
            "The maximum value of his hume shield. Higher values take longer for the hume to fill, meaning he takes more damage before reaching the maximum reduction from his shield.")]
        public int HumeMax { get; set; } = 500;

        [Description("The rate at which his hume shield will decay.")]
        public float HumeDecayRate { get; set; } = 1.5f;

        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new ReactiveHume(),
            new MoveSpeedReduction()
        };

        protected override void RoleAdded(Player player)
        {
            Log.Debug($"{Name}: Setting Max AHP and Decay", Plugin.Singleton.Config.Debug);

            // Please work
            ((AhpStat)player.ReferenceHub.playerStats.StatModules[1]).ServerAddProcess(0, HumeMax,
                HumeDecayRate, 10f, 0f, true);
        }

        protected override void RoleRemoved(Player player)
        {
            Log.Debug($"{Name}: Resetting AHP values.", Plugin.Singleton.Config.Debug);
            // Those should be handled by AhpStat.ClassChanged()
            // player.MaxArtificialHealth = 75;
            // player.ArtificialHealth = 0;
            // player.ArtificialHealthDecay = 0.75f;
        }
    }
}