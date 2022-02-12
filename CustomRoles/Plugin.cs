namespace CustomRoles
{
    using System;
    using System.Collections.Generic;
    using CustomRoles.Configs;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;
    using PlayerEvents = Exiled.Events.Handlers.Player;
    using Scp049Events = Exiled.Events.Handlers.Scp049;
    using ServerEvents = Exiled.Events.Handlers.Server;

    public class Plugin : Plugin<Config>
    {
        public static Plugin Singleton;
        public Random Rng = new Random();

        public readonly List<Player> StopRagdollList = new List<Player>();
        public override string Author { get; } = "Galaxy119";
        public override string Name { get; } = "CustomRoles";
        public override string Prefix { get; } = "CustomRoles";
        public override Version RequiredExiledVersion { get; } = new Version(4, 1, 5);
        public Methods Methods { get; private set; }
        public EventHandlers EventHandlers { get; private set; }

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers(this);
            Methods = new Methods(this);

            Config.LoadConfigs();
            Config.RoleConfigs.Demolitionists.Register();
            Config.RoleConfigs.Dwarves.Register();
            Config.RoleConfigs.Medics.Register();
            Config.RoleConfigs.Phantoms.Register();
            Config.RoleConfigs.BallisticZombies.Register();
            Config.RoleConfigs.BerserkZombies.Register();
            Config.RoleConfigs.ChargerZombies.Register();
            Config.RoleConfigs.DwarfZombies.Register();
            Config.RoleConfigs.MedicZombies.Register();
            Config.RoleConfigs.PdZombies.Register();
            Config.RoleConfigs.PlagueZombies.Register();
            Config.RoleConfigs.TankZombies.Register();

            PlayerEvents.ChangingRole += EventHandlers.OnChangingRole;
            ServerEvents.RoundStarted += EventHandlers.OnRoundStarted;
            ServerEvents.RespawningTeam += EventHandlers.OnRespawningTeam;
            ServerEvents.ReloadedConfigs += EventHandlers.OnReloadedConfigs;
            Scp049Events.FinishingRecall += EventHandlers.FinishingRecall;
            PlayerEvents.SpawningRagdoll += EventHandlers.OnSpawningRagdoll;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomRole.UnregisterRoles();

            PlayerEvents.ChangingRole -= EventHandlers.OnChangingRole;
            ServerEvents.RoundStarted -= EventHandlers.OnRoundStarted;
            ServerEvents.RespawningTeam -= EventHandlers.OnRespawningTeam;
            ServerEvents.ReloadedConfigs -= EventHandlers.OnReloadedConfigs;
            EventHandlers = null;
            Methods = null;

            base.OnDisabled();
        }
    }
}