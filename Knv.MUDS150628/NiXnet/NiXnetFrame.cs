namespace Knv.MUDS150628.NiXnet
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct NiXnetFrame
    {
        [MarshalAs(UnmanagedType.U8)]
        public UInt64 Timestamp;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 ArbitrationId;
        [MarshalAs(UnmanagedType.U1)]
        public Byte Type;
        [MarshalAs(UnmanagedType.U1)]
        public Byte Flags;
        [MarshalAs(UnmanagedType.U1)]
        public Byte Info;
        [MarshalAs(UnmanagedType.U1)]
        public Byte Length;
        [MarshalAs(UnmanagedType.U8)]
        public UInt64 Payload;
    };
}
