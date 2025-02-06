namespace AntiDLL
{
    using CounterStrikeSharp.API;

    using System.Runtime.InteropServices;

    public class CLCMsg_ListenEvents : NativeObject
    {
        private unsafe CPlayerSlot m_Slot => Marshal.PtrToStructure<CPlayerSlot>(base.Handle + 80);

        public CLCMsg_ListenEvents(nint ptr) : base(ptr)
            { }

        public CPlayerSlot GetPlayerSlot() => this.m_Slot;
    }
}
