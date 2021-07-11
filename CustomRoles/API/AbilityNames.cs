using System;
using System.Collections.Generic;

namespace CustomRoles.Abilities
{
    public struct AbilityNames
    {
        public static Dictionary<string, Type> NameDictionary = new Dictionary<string, Type>
        {
            { "Acamo", typeof(ActiveCamo) }
        };
    }
}