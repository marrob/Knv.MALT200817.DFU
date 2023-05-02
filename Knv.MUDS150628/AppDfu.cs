
namespace Knv.MUDS150628
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.Diagnostics;
    using System.ComponentModel;
    using System.Threading;

    public class AppDfu: IDisposable
    {

        public event RunWorkerCompletedEventHandler Completed
        {
            remove { BackgroundWorker.RunWorkerCompleted -= value; }
            add { BackgroundWorker.RunWorkerCompleted += value; }
        }
        public event ProgressChangedEventHandler ProgressChange
        {
            add { BackgroundWorker.ProgressChanged += value; }
            remove { BackgroundWorker.ProgressChanged -= value; }
        }

        readonly BackgroundWorker BackgroundWorker;
        readonly AutoResetEvent WaitForDoneEvent;
        readonly AutoResetEvent WaitForDelayEvent;

        internal class Services
        {
            internal const byte DiagSesssionControl = 0x10;
            internal const byte EcuReset = 0x11;
            internal const byte ClearDiagnosticInformation = 0x14;
            internal const byte ReadDTCInformationService = 0x19;
            internal const byte ReadDtaByIdentifier = 0x22;
            internal const byte RutineControl = 0x31;
            internal const byte RequestDownload = 0x34;
            internal const byte TransferData = 0x36;
            internal const byte RequestTransferExit = 0x37;
            internal const byte TesterPresent = 0x3E;
        }
        internal enum Session : byte
        {
            NormalMode = 1,
            ProgrammingMode = 2
        }
        internal enum ResetType
        { 
            HardReset = 1,
        }


        bool Disposed = false;

        private Iso15765NetwrorkLayer NetwrorkLayer;
       

        public AppDfu(Iso15765NetwrorkLayer netwrorkLayer)
        {
            this.NetwrorkLayer = netwrorkLayer;
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            WaitForDoneEvent = new AutoResetEvent(false);
            WaitForDelayEvent = new AutoResetEvent(false);
        }

        public void Begin(byte[] firmware)
        {
            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.WorkerSupportsCancellation = true;
            BackgroundWorker.RunWorkerAsync(firmware);
        }

        void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] firmware = (byte[])e.Argument;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            try
            {
                byte blockSize;
                byte blockSequenceCoutner = 0;
                int dataPtr = 0;

                BackgroundWorker.ReportProgress(0, "Starting...");
                TesterPresent(0);
                BackgroundWorker.ReportProgress(0, "Request: Tester Present");
                DiagSession(Session.ProgrammingMode);
                BackgroundWorker.ReportProgress(0, "Request: Programming Mode");
                FlashErase();
                BackgroundWorker.ReportProgress(0, "Request: Flash Erase");
                IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
                /*FIGYELJ ARRA HOGY A blockSize csak páros lehet, a szerver 0xFE-jelez vissza!*/
                blockSize = (byte)(RequestDwonLoad(firmware.Length) - 2);
                BackgroundWorker.ReportProgress(0, "I will send bytes:" + firmware.Length);

                /* first block*/
                blockSequenceCoutner++;
                if (firmware.Length > blockSize)
                {
                    byte[] block = new byte[blockSize];
                    Buffer.BlockCopy(firmware, 0, block, 0, blockSize);
                    dataPtr = blockSize;
                    TransferData(blockSequenceCoutner, block);

                    do
                    {
                        blockSequenceCoutner++;

                        if (blockSequenceCoutner == 0xFF)
                            blockSequenceCoutner = 0;

                        if (firmware.Length - dataPtr > blockSize)
                        {
                            block = new byte[blockSize];
                            Buffer.BlockCopy(firmware, dataPtr, block, 0, blockSize);
                            TransferData(blockSequenceCoutner, block);
                            dataPtr += blockSize;
                            
                        }
                        else
                        {
                            block = new byte[firmware.Length - dataPtr];
                            Buffer.BlockCopy(firmware, dataPtr, block, 0, firmware.Length - dataPtr);
                            TransferData(blockSequenceCoutner, block);
                            dataPtr += firmware.Length - dataPtr;
                        }
                        var precent = (int)(((double)dataPtr / firmware.Length) * 100.0);
                        BackgroundWorker.ReportProgress(precent, "Status: " + firmware.Length.ToString() + "/" + dataPtr.ToString() + "byte");

                    } while (dataPtr != firmware.Length);
                }
                else
                {
                    /*itt vége ha nem kellett hozzá egy egész block...*/
                    TransferData(blockSequenceCoutner, firmware);
                }
                TransferExit();
                UInt16 expected = Tools.CalcCrc16Ansi(0, firmware);
                BackgroundWorker.ReportProgress(0, "Cheksum:0x" + GetChecksum().ToString("X04") + ", Expected 0x" + expected.ToString("X04"));

                EcuReset(ResetType.HardReset);

                watch.Stop();

                if (!BackgroundWorker.CancellationPending)
                {

                    BackgroundWorker.ReportProgress(0, "COMPLETE Elapsed:" + (watch.ElapsedMilliseconds / 1000.0).ToString() + "s");
                    e.Result = firmware;
                }
                else
                {
                    BackgroundWorker.ReportProgress(0, "ABORTED Elapsed:" + (watch.ElapsedMilliseconds / 1000.0).ToString() + "s");
                }
            }
            catch (Exception ex)
            {
                BackgroundWorker.ReportProgress(0, "Failed...");
                e.Result = ex;
            }
            finally
            {
                WaitForDoneEvent.Set();
            }
        }

        internal void EcuReset(ResetType reset)
        {
            byte[] response;
            byte[] request = new byte[] { Services.EcuReset, (byte)reset };
            IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
            NetwrorkLayer.Transport(request, out response);
        }

        internal void DiagSession(Session session)
        {
            byte[] response;
            byte[] request = new byte[] {Services.DiagSesssionControl, (byte)session };
            IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
            NetwrorkLayer.Transport(request, out response);
        }

        internal void TesterPresent(byte param)
        {
            byte[] response;
            byte[] request = new byte[] { Services.TesterPresent, param };
            IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
            NetwrorkLayer.Transport(request, out response ); 
        }

        internal byte RequestDwonLoad(int memorySize )
        {
            byte[] response;
            byte[] request = new byte[1 + 1 + 1 + 4 + 4];
            request[0] = Services.RequestDownload;
            Buffer.BlockCopy(BitConverter.GetBytes(memorySize), 0, request, 7, sizeof(int));
            IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
            NetwrorkLayer.Transport(request, out response);
            return response[2];
        }

        internal void FlashErase()
        {
            byte[] response;
            IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
            NetwrorkLayer.Transport(new byte[] {Services.RutineControl, 0x01, 0xFF, 0x01 }, out response);
        }

        internal uint GetChecksum()
        {
            byte[] response;
            IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
            NetwrorkLayer.Transport(new byte[] { Services.RutineControl, 0x01, 0xFF, 0x02 }, out response);
            uint cheksum = BitConverter.ToUInt16(response, 2);
            return cheksum;
        }

        /// <summary>
        /// A transfer data fejléce 3 bájt (hossz, sid, és blockSequenceCounter)
        /// A RequestDownload jelzi a maximális hosszát maxNumberOfBlockLength-ben
        /// 0xFF-től nem lehet nagyobb, és ebben benne van a fejléc is.
        /// </summary>
        /// <param name="blockSequenceCounter"></param>
        /// <param name="data"></param>
        internal void TransferData(byte blockSequenceCounter, byte[] data)
        {
            if (data.Length > 7)
                if (data.Length > 253)
                    throw new Exception("TransferData: data too long...");
            byte[] response;
            IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
            var request = new byte[2 + data.Length];
            request[0] = Services.TransferData;
            request[1] = blockSequenceCounter;
            Buffer.BlockCopy(data, 0, request, 2, data.Length);
            NetwrorkLayer.Transport(request, out response);

            if(response[1] != blockSequenceCounter )
               throw new Iso15765Exception("TransferData: blockSequenceCounter sequence error");
        }
        internal void TransferExit()
        {
            byte[] response;
            byte[] request = new byte[] { Services.RequestTransferExit };
            IoLog.Instance.WirteLine(MethodBase.GetCurrentMethod().Name);
            NetwrorkLayer.Transport(request, out response);
        }
        void ResponseChecek(byte sid, byte[] response)
        {
            if (response[0] == 0x7F)
                throw new Exception(NegativeResponseCodeToString(response[1]));

            if(response[0] - 0x40 != sid)
                throw new Exception(NegativeResponseCodeToString(response[1]));
        }
        string NegativeResponseCodeToString(byte nrc)
        {
            switch (nrc)
            {
                case 0x10: { return "generalReject"; } //0
                case 0x11: { return "serviceNotSupported"; }//1
                case 0x12: { return "subFunctionNotSupported"; }//2
                case 0x13: { return "incorrectMessageLengthOrInvalidFormat"; }//3
                case 0x14: { return "responseTooLong"; }//4
                case 0x21: { return "busyRepeatRequest"; }//5
                case 0x22: { return "conditionsNotCorrect"; }//6
                case 0x24: { return "requestSequenceError"; }//7
                case 0x25: { return "noResponseFromSubnetComponent"; }//8
                case 0x26: { return "failurePreventsExecutionOfRequestedAction"; }//9
                case 0x31: { return "requestOutOfRange"; }//10
                case 0x33: { return "securityAccessDenied"; }//11
                case 0x35: { return "invalidKey"; }//12
                case 0x36: { return "exceedNumberOfAttempts"; }//13
                case 0x37: { return "requiredTimeDelayNotExpired"; }//14
                case 0x70: { return "uploadDownloadNotAccepted"; }//15
                case 0x71: { return "transferDataSuspended"; }//16
                case 0x72: { return "generalProgrammingFailure"; }//17
                case 0x73: { return "wrongBlockSequenceCounter"; }//18
                //the request is received well and allowed, but the VU needs more time and
                //"ResponsePending" Messages will be send by the VU until final "PositiveResponse" or
                //"NegativeResponse"
                case 0x78: { return "requestCorrectlyReceived_ResponsePending"; }//19
                case 0x7E: { return "subFunctionNotSupportedInActiveSession"; }//20
                case 0x7F: { return "serviceNotSupportedInActiveSession"; }//21
                case 0x81: { return "rpmTooHigh"; }//22
                case 0x82: { return "rpmTooLow"; }//23
                case 0x83: { return "engineIsRunning"; }//24
                case 0x84: { return "engineIsNotRunning"; }//25
                case 0x85: { return "engineRunTimeTooLow"; }//26
                case 0x86: { return "temperatureTooHigh"; }//27
                case 0x87: { return "temperatureTooLow"; }//28
                case 0x88: { return "vehicleSpeedTooHigh"; }//29
                case 0x89: { return "vehicleSpeedTooLow"; }//30
                case 0x8A: { return "throttle/PedalTooHigh"; }//31
                case 0x8B: { return "throttle/PedalTooLow"; }//32
                case 0x8C: { return "transmissionRangeNotInNeutral"; }//33
                case 0x8D: { return "transmissionRangeNotInGear"; }//34
                case 0x8F: { return "brakeSwitch(es)NotClosed"; }//35
                case 0x90: { return "shifterLeverNotInPark"; }//36
                case 0x91: { return "torqueConverterClutchLocked"; }//37
                case 0x92: { return "voltageTooHigh"; }//38
                case 0x93: { return "voltageTooLow"; }//39
                default: { return "undefined"; }
            }
        }

        public void Abort()
        {
            if (BackgroundWorker.IsBusy)
            {
                WaitForDelayEvent.Set();
                BackgroundWorker.CancelAsync();
                WaitForDoneEvent.WaitOne();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                if (BackgroundWorker.IsBusy)
                {
                    BackgroundWorker.CancelAsync();
                    WaitForDoneEvent.WaitOne();
                }
            }

            // Free any unmanaged objects here. 
            //
            Disposed = true;
        }
    }
}
