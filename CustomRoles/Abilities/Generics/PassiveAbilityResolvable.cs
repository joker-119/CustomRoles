namespace CustomRoles.Abilities.Generics
{
    using Exiled.CustomRoles.API.Features;

    public abstract class PassiveAbilityResolvable : PassiveAbility, IResolvableAbility
    {
        protected PassiveAbilityResolvable()
        {
            AbilityType = GetType().Name;
        }

        public string AbilityType { get; }
    }
}