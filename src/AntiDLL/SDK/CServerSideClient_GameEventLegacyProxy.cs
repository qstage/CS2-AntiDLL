using System.Runtime.InteropServices;

namespace AntiDLL
{
    using System.Runtime.InteropServices;

    public class CServerSideClient_GameEventLegacyProxy : IGameEventListener2
    {
        private unsafe CPlayerSlot m_Slot => Marshal.PtrToStructure<CPlayerSlot>(base.Handle + 8);

        public CServerSideClient_GameEventLegacyProxy(nint ptr) : base(ptr)
            { }

        public CPlayerSlot GetPlayerSlot() => this.m_Slot;
    }
}
