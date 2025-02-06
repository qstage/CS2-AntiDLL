namespace AntiDLL
{
    using CounterStrikeSharp.API;
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

    public class IGameEventManager2 : NativeObject
    {
        public static readonly MemoryFunctionVoid<IGameEventManager2> Init = new MemoryFunctionVoid<IGameEventManager2>(GameData.GetSignature("CGameEventManager_Init"));

        private static class VTable
        {
            public static readonly MemoryFunctionWithReturn<IGameEventManager2, IGameEventListener2, string, bool> FindListener = new MemoryFunctionWithReturn<IGameEventManager2, IGameEventListener2, string, bool>(GameData.GetSignature("IGameEventManager2::FindListener"));
        }

        public IGameEventManager2(nint ptr) : base(ptr)
            { }

        public bool FindListener(IGameEventListener2 listener, string eventName)
        {
            return VTable.FindListener.Invoke(this, listener, eventName);
        }
    }
}
