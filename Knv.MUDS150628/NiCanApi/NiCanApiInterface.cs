namespace Knv.MUDS150628.NiCanApi
{
    using System;
    using Common;

    public class NiCanApiInterface : ICanInterface, IDisposable
    {
        public bool IsOpen { get; private set; }
        public string NameOfApi { get { return "NI-CAN Channel API"; } }
        bool _disposed = false;
        uint _handle = 0;

        public void Init(UInt64 baudrate, string intfName)
        {
            _handle = NiCanTools.Open(intfName, (uint)baudrate);
        }
        
        public void Close()
        {
            NiCanTools.Close(_handle);
            _handle= 0;
        }

        public void WriteFrame(CanMsg[] frames)
        {
            foreach (CanMsg frame in frames)
            {
                var niTx = new NiCan.NCTYPE_CAN_FRAME();
                /*
                 * TODO
                if (frame.ExtendedId)
                    niTx.ArbitrationId = (UInt32)TransmittId | 0x20000000;
                else
                    niTx.ArbitrationId = (UInt32)TransmittId;
                */
                niTx.ArbitrationId = (UInt32)frame.Id;
                niTx.DataLength = (byte)frame.Length;
                niTx.IsRemote = NiCan.NC_FALSE;
                if(frame.Length >= 1)
                    niTx.Data0 = frame.Payload[0];
                if (frame.Length >= 2)
                    niTx.Data1 = frame.Payload[1];
                if (frame.Length >= 3)
                    niTx.Data2 = frame.Payload[2];
                if (frame.Length >= 4)
                    niTx.Data3 = frame.Payload[3];
                if (frame.Length >= 5)
                    niTx.Data4 = frame.Payload[4];
                if (frame.Length >= 6)
                    niTx.Data5 = frame.Payload[5];
                if (frame.Length >= 7)
                    niTx.Data6 = frame.Payload[6];
                if (frame.Length >= 8)
                    niTx.Data7 = frame.Payload[7];
                NiCan.ncWrite(_handle, NiCan.CanFrameSize, ref niTx);
            }
        }

        public CanMsg[] ReadFrame()
        {
            var rx = new NiCan.NCTYPE_CAN_STRUCT();
            int framesCount = NiCanTools.ReadPending(_handle);
            CanMsg[] msgs = new CanMsg[framesCount];
            for (int i = 0; i < framesCount; i++) 
            {
                NiCanTools.NiCanStatusCheck(NiCan.ncRead(_handle, NiCan.CanStructSize, ref rx));
                msgs[i] = new CanMsg();
                msgs[i].Length = rx.DataLength;
                msgs[i].Id = rx.ArbitrationId & 0x1FFFFFFF;
                msgs[i].Payload = new byte[8];
                msgs[i].Payload[0] = rx.Data0;
                msgs[i].Payload[1] = rx.Data1;
                msgs[i].Payload[2] = rx.Data2;
                msgs[i].Payload[3] = rx.Data3;
                msgs[i].Payload[4] = rx.Data4;
                msgs[i].Payload[5] = rx.Data5;
                msgs[i].Payload[6] = rx.Data6;
                msgs[i].Payload[7] = rx.Data7;
            }
            return msgs;
        }

        public void Dispose()
        {
            IsOpen = false;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                NiCanTools.Close(_handle);
                _handle = 0;
            }
            _disposed = true;
        }
    }
}
