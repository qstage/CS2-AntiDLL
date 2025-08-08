namespace AntiDLL
{
    using CounterStrikeSharp.API;
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Modules.Memory;
    using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

    public class IGameEventManager2 : NativeObject
    {
        public unsafe static nint Init()
        {
            nint addr = NativeAPI.FindSignature(Addresses.ServerPath, GameData.GetSignature("rel_GameEventManager"));

            if (addr == nint.Zero)
            {
                return -1;
            }

            const int offset = 3;

            nint rel32 = *(int*)(addr + offset);

            addr += sizeof(int) + offset;
            addr += rel32;

            return *(nint*)addr;
        }

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
