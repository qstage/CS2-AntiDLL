namespace AntiDLL
{
    using CounterStrikeSharp.API;
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

    using CounterStrikeSharp.API.ValveConstants.Protobuf;

    using Microsoft.Extensions.Logging;

    using AntiDLL.API;

    public sealed partial class Plugin : BasePlugin, IPluginConfig<PluginConfig>, IAntiDLL
    {
        public required PluginConfig Config { get; set; } = new PluginConfig();

        private IGameEventManager2? GameEventManager;

        public event AntiDLLCallback? OnDetection;

        public void OnConfigParsed(PluginConfig config)
        {
            if (config.Version < this.Config.Version)
            {
                base.Logger.LogWarning("Configuration is out of date. Consider updating the plugin.");
            }

            this.Config = config;
        }

        public override void Load(bool hotReload)
        {
            nint addr = IGameEventManager2.Init();

            if (addr == -1)
            {
                base.Logger.LogError("Not found `CGameEventManager2`");
                return;
            }

            GameEventManager = new(addr);

            CSource1LegacyGameEventGameSystem.ListenBitsReceived.Hook(this.OnSource1LegacyGameEventListenBitsReceived, HookMode.Pre);
            this.RegisterAPI(this);
        }

        private HookResult OnSource1LegacyGameEventListenBitsReceived(DynamicHook hook)
        {
            if (this.GameEventManager == null)
                return HookResult.Continue;

            CSource1LegacyGameEventGameSystem pLegacyEventSystem = hook.GetParam<CSource1LegacyGameEventGameSystem>(0);
            CLCMsg_ListenEvents pMsg = hook.GetParam<CLCMsg_ListenEvents>(1);
            CPlayerSlot slot = pMsg.GetPlayerSlot();
            CServerSideClient_GameEventLegacyProxy? pClientProxyListener = pLegacyEventSystem.GetLegacyGameEventListener(slot);

            if (pClientProxyListener == null)
                return HookResult.Continue;

            CCSPlayerController? player = Utilities.GetPlayerFromSlot(slot);

            if (player == null || player.IsBot)
                return HookResult.Continue;

            IEnumerable<string> events = this.Config.Blacklist.Where((eventName) => this.GameEventManager.FindListener(pClientProxyListener, eventName));

            if (events.Any())
            {
                if (this.OnDetection != null)
                {
                    foreach (string eventName in events)
                    {
                        this.OnDetection?.Invoke(player, eventName);
                    }
                } else
                {
                    base.Logger.LogInformation("Kicking player {0} for blacklisted event listener. ({1} total)", player.PlayerName, events.Count());
                    player.Disconnect(NetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_UNTRUSTEDACCOUNT);
                }
            }

            return HookResult.Continue;
        }

        public override void Unload(bool hotReload)
        {
            CSource1LegacyGameEventGameSystem.ListenBitsReceived.Unhook(this.OnSource1LegacyGameEventListenBitsReceived, HookMode.Pre);
        }
    }
}
