namespace CustomRoles.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CustomRoles.Abilities.Generics;
    using Exiled.CustomRoles.API.Features;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public class AggregateExpectationTypeResolver<T> : ITypeDiscriminator where T : class
    {
        private const string TargetKey = nameof(IResolvableAbility.AbilityType);
        private readonly string _targetKey;
        private readonly Dictionary<string, Type> _typeLookup;

        public AggregateExpectationTypeResolver(INamingConvention namingConvention)
        {
            _targetKey = namingConvention.Apply(TargetKey);
            _typeLookup = new Dictionary<string, Type>();
            foreach (Type t in Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))
                .ToList())
                _typeLookup.Add(t.Name, t);
        }

        public Type BaseType => typeof(CustomAbility);

        public bool TryResolve(ParsingEventBuffer buffer, out Type suggestedType)
        {
            if (buffer.TryFindMappingEntry(
                scalar => _targetKey == scalar.Value,
                out Scalar key,
                out ParsingEvent value))
            {
                if (value is Scalar valueScalar)
                {
                    suggestedType = CheckName(valueScalar.Value);

                    return true;
                }

                FailEmpty();
            }

            suggestedType = null;
            return false;
        }


        private void FailEmpty()
        {
            throw new Exception($"Could not determine expectation type, {_targetKey} has an empty value");
        }

        private Type CheckName(string value)
        {
            if (_typeLookup.TryGetValue(value, out Type childType)) return childType;

            string known = string.Join(",", _typeLookup.Keys);
            throw new Exception(
                $"Could not match `{_targetKey}: {value}` to a known expectation. Expecting one of: {known}");
        }
    }
}