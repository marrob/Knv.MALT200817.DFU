namespace Knv.MUDS150628.Common
{
    using System;
    public interface ICanInterface: IDisposable
    {
        bool IsOpen { get; }
        string NameOfApi { get; } 
        void Init(UInt64 baudrate, string intfName);
        void WriteFrame(CanMsg[] frames);
        CanMsg[] ReadFrame();
    }
}
