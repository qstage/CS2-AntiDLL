namespace AntiDLL.Example
{
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Capabilities;

    using Microsoft.Extensions.Logging;

    using AntiDLL.API;

    public sealed class Plugin : BasePlugin
    {
        public override string ModuleName => "Example consumer for AntiDLL API";

        public override string ModuleVersion => "1.0";

        private static PluginCapability<IAntiDLL> AntiDLL { get; } = new PluginCapability<IAntiDLL>("AntiDLL");

        private void OnDetection(CCSPlayerController player, string eventName)
        {
            // you would want to take action with the player here
            base.Logger.LogInformation("Player {0} cooks {1}", player.PlayerName, eventName);
        }

        public override void OnAllPluginsLoaded(bool hotReload)
        {
            IAntiDLL? antidll = AntiDLL.Get();

            if (antidll != null)
            {
                antidll.OnDetection += this.OnDetection;
            }
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
