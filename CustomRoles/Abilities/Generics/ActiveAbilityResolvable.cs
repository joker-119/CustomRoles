namespace CustomRoles.Abilities.Generics
{
    using Exiled.CustomRoles.API.Features;

    public abstract class ActiveAbilityResolvable : ActiveAbility, IResolvableAbility
    {
        protected ActiveAbilityResolvable()
        {
            AbilityType = GetType().Name;
        }

        public string AbilityType { get; }
    }
}