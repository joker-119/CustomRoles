using UnityEngine;

namespace CustomRoles.Configs.Roles
{
    public class JuggernautZombieConfig
    {
        public string Name { get; set; } = "Juggernaut Zombie";
        public int MaxHealth { get; set; } = 1000;
        public float MovementSpeed { get; set; } = 0.8f;
        public Vector3 Size { get; set; } = new Vector3(1.25f, 1.25f, 1.25f);
        public string Description { get; set; } = "A slightly larger, tankier zombie that moves more slowly.";
    }
}