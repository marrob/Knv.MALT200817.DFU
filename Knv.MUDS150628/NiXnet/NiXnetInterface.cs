
namespace Knv.MUDS150628.NiXnet
{
    using System.Runtime.InteropServices;
    using Common;
    using System;


    public  class NiXnetInterface : ICanInterface
    {
        public bool IsOpen { get; private set; }
        bool disposed;

        public string NameOfApi { get { return "NI-XNET";  } }

        private NiXnet SessionIn;
        NiXnetFrame[] BufferIn;
        IntPtr PtrBuffRIn;


        NiXnet SessionOut;
        NiXnetFrame[] BufferOut;
        IntPtr PtrBufferOut;


        public void Init(UInt64 baudrate, string intfName)
        {
            SessionIn = new NiXnet(":memory:", "", "", intfName, NiXnet.xNETConstants.nxMode_FrameInStream);
            SessionIn.nx_SetProperty(NiXnet.xNETConstants.nxPropSession_IntfBaudRate, sizeof(uint), (uint)baudrate);


            BufferIn = new NiXnetFrame[100];
            GCHandle gchIn = GCHandle.Alloc(BufferIn, GCHandleType.Pinned);
            PtrBuffRIn = gchIn.AddrOfPinnedObject();

            SessionOut = new NiXnet(":memory:", "", "", intfName, NiXnet.xNETConstants.nxMode_FrameOutStream);
            SessionOut.nx_SetProperty(NiXnet.xNETConstants.nxPropSession_IntfBaudRate, sizeof(uint), (uint)baudrate);
            BufferOut = new NiXnetFrame[4];
            GCHandle gchOut = GCHandle.Alloc(BufferOut, GCHandleType.Pinned);
            PtrBufferOut = gchOut.AddrOfPinnedObject();


            SessionIn.nx_Start(NiXnet.xNETConstants.nxStartStop_Normal);

            IsOpen = true;

            uint stateBuffer;
            int fault;
            SessionIn.nx_ReadState(NiXnet.xNETConstants.nxState_CANComm, (uint)sizeof(uint), out stateBuffer, out fault);
          
        }

        public CanMsg[] ReadFrame()
        {
            UInt32 readBytes = 0;
            CanMsg[] frames = null;
            byte[] buffer = new byte[200];
            unsafe
            {
                //SessionIn.nx_ReadFrame(buffer, (uint)BufferIn.Length * (uint)sizeof(NiXnetFrame), 0, out readBytes);

                SessionIn.nx_ReadFrame((void*)PtrBuffRIn, (uint)BufferIn.Length * (uint)sizeof(NiXnetFrame), 0, out readBytes);
                var framesCnt = readBytes / sizeof(NiXnetFrame);
                frames = new CanMsg[framesCnt];
                for (int i = 0; i < framesCnt; i++)
                {
                    frames[i].Id = BufferIn[i].ArbitrationId;
                    frames[i].Length = BufferIn[i].Length;
                    frames[i].Payload = BitConverter.GetBytes(BufferIn[i].Payload);
                }
            }
            return frames;
        }

        public void WriteFrame(CanMsg[] frames)
        {

            if (frames.Length > BufferOut.Length)
                throw new ApplicationException("XNET Buffer Out too small.");

            for (int i = 0; i < frames.Length; i++)
            {
                BufferOut[i].ArbitrationId = frames[i].Id;
                BufferOut[i].Length = (byte)frames[i].Length;
                BufferOut[i].Payload = BitConverter.ToUInt64(frames[i].Payload, 0);
            }

            unsafe
            {
                SessionOut.nx_WriteFrame((void*)PtrBufferOut, (uint)frames.Length * (uint)sizeof(NiXnetFrame), 10);
            }
        }

        public void Dispose()
        {
            //SessionIn.nx_Stop(NiXnet.xNETConstants.nxStartStop_Normal);
            //SessionOut.nx_Stop(NiXnet.xNETConstants.nxStartStop_Normal);
            IsOpen = false;
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                SessionIn?.Dispose();
                SessionOut?.Dispose();

            }
            disposed = true;
        }

    }
}
