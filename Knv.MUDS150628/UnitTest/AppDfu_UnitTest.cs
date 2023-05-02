namespace Knv.MUDS150628.UnitTest
{
    using System;
    using NUnit.Framework;
    using System.Diagnostics;
    using NiCanApi;

    [TestFixture]
    public class AppDfu_UnitTest
    {
        [Test]
        public void Basic_DFU_Test()
        {
            using (var canLink = new NiCanApiInterface())
            {
                canLink.Init(baudrate: 250000, intfName: "CAN0");
                var network = new Iso15765NetwrorkLayer(canLink);
                network.Log = false;
                network.TransmittId = 0x603;
                network.ReceiveId = 0x703;


                network.Log = false;
                var dfu = new AppDfu(network);
                IoLog.Instance.FilePath = @"D:\io_log.txt";
                Console.WriteLine("LogPath:" + IoLog.Instance.FilePath);

                int firmwareLength = 100;
                byte[] firmware = new byte[firmwareLength];
                for (int i = 0; i < 100; i++)
                {
                    firmware[i] = (byte)i;
                }

                byte blockSize;
                byte blockSequenceCoutner = 0;
                int dataPtr = 0;

                dfu.TesterPresent(0);
                dfu.DiagSession(AppDfu.Session.ProgrammingMode);
                dfu.FlashErase();


                /*FIGYELJ ARRA HOGY A blockSize csak páros lehet, a szerver 0xFE-jelez vissza!*/
                blockSize = (byte)(dfu.RequestDwonLoad(firmware.Length) - 2);

                /* first block*/
                blockSequenceCoutner++;
                if (firmware.Length > blockSize)
                {
                    byte[] block = new byte[blockSize];
                    Buffer.BlockCopy(firmware, 0, block, 0, blockSize);
                    dataPtr = blockSize;
                    dfu.TransferData(blockSequenceCoutner, block);

                    do
                    {
                        blockSequenceCoutner++;

                        if (blockSequenceCoutner == 0xFF)
                            blockSequenceCoutner = 0;

                        if (firmware.Length - dataPtr > blockSize)
                        {
                            block = new byte[blockSize];
                            Buffer.BlockCopy(firmware, dataPtr, block, 0, blockSize);
                            dfu.TransferData(blockSequenceCoutner, block);
                            dataPtr += blockSize;
                        }
                        else
                        {
                            block = new byte[firmware.Length - dataPtr];
                            Buffer.BlockCopy(firmware, dataPtr, block, 0, firmware.Length - dataPtr);
                            dfu.TransferData(blockSequenceCoutner, block);
                            dataPtr += firmware.Length - dataPtr;
                        }

                    } while (dataPtr != firmware.Length);
                }
                else
                {
                    /*itt vége ha nem kellett hozzá egy egész block...*/
                    dfu.TransferData(blockSequenceCoutner, firmware);
                }
                dfu.TransferExit();
                UInt16 expected = Tools.CalcCrc16Ansi(0, firmware);
                Debug.WriteLine("Cheksum:0x" + dfu.GetChecksum().ToString("X04") + ", Expected 0x" + expected.ToString("X04"));

                dfu.EcuReset(AppDfu.ResetType.HardReset);
            }
        }
    }
}

