namespace BanModule
{

    using CounterStrikeSharp.API;
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Capabilities;
    using Microsoft.Extensions.Logging;
    using AntiDLL.API;
    using System.Text.Json.Serialization;

    public sealed class Config : BasePluginConfig
    {
        [JsonPropertyName("Banreason")] public string BanReason { get; set; } = "[AntiDLL] your reason here";
    }

    public sealed class Plugin : BasePlugin, IPluginConfig<Config>
    {
        public override string ModuleName => "BanModule";

        public override string ModuleVersion => "1.0";

        public override string ModuleAuthor => "verneri";

        private static PluginCapability<IAntiDLL> AntiDLL { get; } = new PluginCapability<IAntiDLL>("AntiDLL");
        public Config Config { get; set; } = new();
        private HashSet<ulong> Punishedplayers = [];

        private void OnDetection(CCSPlayerController player, string eventName)
        {

            base.Logger.LogInformation("Player {0} cooks {1}", player.PlayerName, eventName);

            if (!Punishedplayers.Contains(player.SteamID))
            {
                Punishedplayers.Add(player.SteamID);
                Server.ExecuteCommand($"css_ban #{player.UserId.Value} 0 \"{Config.BanReason}\"");
            }
        }

        public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
        {
            if (@event == null) return HookResult.Continue;
            var player = @event.Userid;
            if (player == null || !player.IsValid || player.IsBot) return HookResult.Continue;

            Punishedplayers.Remove(player.SteamID);

            return HookResult.Continue;
        }

        public override void OnAllPluginsLoaded(bool hotReload)
        {
            IAntiDLL? antidll = AntiDLL.Get();

            if (antidll != null)
            {
                antidll.OnDetection += this.OnDetection;
            }
        }
        public void OnConfigParsed(Config config)
        {
            Config = config;
        }

        public override void Unload(bool hotReload)
        {
            IAntiDLL? antidll = AntiDLL.Get();

            if (antidll != null)
            {
                antidll.OnDetection -= this.OnDetection;
            }
        }
    }
}
