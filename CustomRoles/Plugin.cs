using System.Collections.Generic;
using CustomRoles.Configs;
using Exiled.API.Features;
using MapEvents = Exiled.Events.Handlers.Map;
using PlayerEvents = Exiled.Events.Handlers.Player;
using Scp049Events = Exiled.Events.Handlers.Scp049;
using Scp079Events = Exiled.Events.Handlers.Scp079;
using Scp096Events = Exiled.Events.Handlers.Scp096;
using Scp106Events = Exiled.Events.Handlers.Scp106;
using Scp914Events = Exiled.Events.Handlers.Scp914;
using ServerEvents = Exiled.Events.Handlers.Server;
using WarheadEvents = Exiled.Events.Handlers.Warhead;

namespace CustomRoles
{
    using Random = System.Random;
    using Version = System.Version;

    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "Galaxy119";
        public override string Name { get; } = "CustomRoles";
        public override string Prefix { get; } = "CustomRoles";
        public override Version Version { get; } = new Version(2, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 0);

        public List<Player> StopRagdollList = new List<Player>();
        public Methods Methods { get; private set; }
        public EventHandlers EventHandlers { get; private set; }

        public static Plugin Singleton;
        public Random Rng = new Random();

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers(this);
            Methods = new Methods(this);
            
            Config.LoadConfigs();
            Methods.RegisterRoles();

            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnChangingRole;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.OnRespawningTeam;
            Exiled.Events.Handlers.Server.ReloadedConfigs += EventHandlers.OnReloadedConfigs; 
            Exiled.Events.Handlers.Scp049.FinishingRecall += EventHandlers.FinishingRecall;
            Exiled.Events.Handlers.Player.SpawningRagdoll += EventHandlers.OnSpawningRagdoll;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Methods.UnregisterRoles();
            
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnChangingRole;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.OnRespawningTeam;
            Exiled.Events.Handlers.Server.ReloadedConfigs -= EventHandlers.OnReloadedConfigs;
            EventHandlers = null;
            Methods = null;

            base.OnDisabled();
        }
    }
}