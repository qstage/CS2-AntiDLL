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
        private HashSet<string> Punishedplayers = new HashSet<string>();

        private void OnDetection(CCSPlayerController player, string eventName)
        {
            var playerid = player.UserId;
            string steamid = player.SteamID.ToString();

            base.Logger.LogInformation("Player {0} cooks {1}", player.PlayerName, eventName);

            if (!Punishedplayers.Contains(steamid))
            {
                Punishedplayers.Add(steamid);
                Server.ExecuteCommand($"css_ban {playerid} 0 \"{Config.BanReason}\"");
                //AddTimer(300.0f, () => Punishedplayers.Remove(steamid));
            }
        }

        public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
        {
            if (@event == null) return HookResult.Continue;
            var player = @event.Userid;
            if (player == null || !player.IsValid || player.IsBot) return HookResult.Continue;
            string steamid = player.SteamID.ToString();

            Punishedplayers.Remove(steamid);

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
