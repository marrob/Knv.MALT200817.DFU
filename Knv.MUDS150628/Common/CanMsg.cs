namespace Knv.MUDS150628.Common
{
    using System;
    public struct CanMsg
    {
        public UInt32 Id;
        public int Length;
        public byte[] Payload;
       
        public CanMsg(UInt32 id, byte[] payload)
        {
            Id = id;
            Length = payload.Length;
            Payload = new byte[payload.Length];
            Buffer.BlockCopy(payload, 0, Payload, 0, payload.Length);
        }
        public override string ToString()
        {
            //fix length to show
            byte[] bytes = new byte[Length];
            Buffer.BlockCopy(Payload, 0, bytes, 0, Length);

            return $"{Id:X8} {Length} {Tools.ConvertByteArrayToLogString(bytes)}";
        }
    }
}