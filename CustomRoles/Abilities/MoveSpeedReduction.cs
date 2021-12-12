namespace CustomRoles.Abilities
{
    using CustomRoles.Abilities.Generics;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;

    public class MoveSpeedReduction : PassiveAbilityResolvable
    {
        public override string Name { get; set; } = "Reduced movement speed.";
        public override string Description { get; set; } = "Reduces the player's movement speed.";

        protected override void AbilityAdded(Player player)
        {
            Timing.CallDelayed(2.5f, () => { player.EnableEffect(EffectType.SinkHole); });
        }

        protected override void AbilityRemoved(Player player)
        {
            player.DisableEffect(EffectType.SinkHole);
        }
    }
}