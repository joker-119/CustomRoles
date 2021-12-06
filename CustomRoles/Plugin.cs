namespace CustomRoles
{
    using System;
    using System.Collections.Generic;
    using Configs;
    using Exiled.API.Features;
    using PlayerEvents = Exiled.Events.Handlers.Player;
    using Scp049Events = Exiled.Events.Handlers.Scp049;
    using ServerEvents = Exiled.Events.Handlers.Server;

    public class Plugin : Plugin<Config>
    {
        public static Plugin Singleton;
        public Random Rng = new();

        public readonly List<Player> StopRagdollList = new();
        public override string Author { get; } = "Galaxy119";
        public override string Name { get; } = "CustomRoles";
        public override string Prefix { get; } = "CustomRoles";
        public override Version Version { get; } = new(2, 0, 0);
        public override Version RequiredExiledVersion { get; } = new(3, 0, 0);
        public Methods Methods { get; private set; }
        public EventHandlers EventHandlers { get; private set; }

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers(this);
            Methods = new Methods(this);

            Config.LoadConfigs();
            Methods.RegisterRoles();

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
            Methods.UnregisterRoles();

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