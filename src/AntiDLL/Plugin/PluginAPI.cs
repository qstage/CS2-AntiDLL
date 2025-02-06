namespace AntiDLL
{
    using AntiDLL.API;

    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Capabilities;

    public sealed partial class Plugin : BasePlugin
    {
        internal static PluginCapability<IAntiDLL> API { get; } = new PluginCapability<IAntiDLL>("AntiDLL");

        internal void RegisterAPI(IAntiDLL service)
        {
            Capabilities.RegisterPluginCapability<IAntiDLL>(API, () => service);
        }
    }
}
