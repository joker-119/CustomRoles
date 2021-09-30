using Exiled.API.Enums;
using MEC;

namespace CustomRoles.Roles
{
    using System.Collections.Generic;
    using CustomRoles.Abilities;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;

    public class TankZombie : CustomRole
    {
        public override uint Id { get; set; } = 13;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 1100;
        public override string Name { get; set; } = "Juggernaut Zombie";
        public override string Description { get; set; } = 
            "A slightly slower zombie with double the regular health. As you take damage your AHP meter will fill. The higher it's value, the less damage you take.";

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new ReactiveHume(),
            new MoveSpeedReduction(),
        };

        protected override void RoleAdded(Player player)
        {
            Log.Debug($"{Name}: Setting Max AHP and Decay", Plugin.Singleton.Config.Debug);
            player.MaxArtificialHealth = 500;
            player.ArtificialHealthDecay = 1.5f;
        }

        protected override void RoleRemoved(Player player)
        {
            Log.Debug($"{Name}: Resetting AHP values.", Plugin.Singleton.Config.Debug);
            player.MaxArtificialHealth = 75;
            player.ArtificialHealth = 0;
            player.ArtificialHealthDecay = 0.75f;
        }
    }
}