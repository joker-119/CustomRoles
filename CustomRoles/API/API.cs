using System.Collections.Generic;
using CustomRoles.Abilities;
using Exiled.API.Features;
using UnityEngine;

namespace CustomRoles.API
{
    public static class API
    {
        public static List<CustomRole> GetPlayerRoles(this Player player)
        {
            List<CustomRole> roles = new List<CustomRole>();
            Component[] components = player.GameObject.GetComponents(typeof(CustomRole));
            foreach (Component comp in components)
                if (comp is CustomRole role)
                    roles.Add(role);
            return roles;
        }
    }
}