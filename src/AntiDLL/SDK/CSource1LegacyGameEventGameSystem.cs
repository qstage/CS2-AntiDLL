namespace AntiDLL
{
    using CounterStrikeSharp.API;
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

    using System.Runtime.InteropServices;

    public class CSource1LegacyGameEventGameSystem : NativeObject
    {
        public static readonly MemoryFunctionWithReturn<CSource1LegacyGameEventGameSystem, CLCMsg_ListenEvents, bool> ListenBitsReceived = new MemoryFunctionWithReturn<CSource1LegacyGameEventGameSystem, CLCMsg_ListenEvents, bool>(GameData.GetSignature("CSource1LegacyGameEventGameSystem::ListenBitsReceived"));

        private static class Offsets
        {
            public static readonly int Listeners = GameData.GetOffset("CSource1LegacyGameEventGameSystem::Listeners");
        }

        public unsafe CUtlString m_Name => Marshal.PtrToStructure<CUtlString>(base.Handle + 8);

        public CSource1LegacyGameEventGameSystem(nint ptr) : base(ptr)
            { }

        public CServerSideClient_GameEventLegacyProxy? GetLegacyGameEventListener(CPlayerSlot slot)
        {
            if (slot < 0 || slot > 63)
            {
                throw new IndexOutOfRangeException($"No proxy listener for slot '{slot.Get()}'");
            }

            return new CServerSideClient_GameEventLegacyProxy(base.Handle + (16 * slot) + Offsets.Listeners);
        }
    }
}
