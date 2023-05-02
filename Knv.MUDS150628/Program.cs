namespace Knv.MUDS150628
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NiCanApi;

    class Program
    {
        static void Main(string[] args)
        {
            using (var canLink = new NiCanApiInterface())
            {
                canLink.Init(baudrate: 250000, intfName: "CAN0");
                var network = new Iso15765NetwrorkLayer(canLink);
                network.Log = false;
                network.TransmittId = 0x603;
                network.ReceiveId = 0x703;

                var dfu = new AppDfu(network);
                IoLog.Instance.FilePath = @"D:\io_log.txt";
                Console.WriteLine("LogPath:" + IoLog.Instance.FilePath);


                byte[] temp;
                //                var path = @"D:\@@@!ProjectS\KonvolucioApp\Konvolucio.MDFU200325\resources\MALT132_V0603.bin";
                var path = @"D:\@@@!ProjectS\KonvolucioApp\MDFU200325\Resources\BINARY_FF_500byte.bin";
                temp = Tools.OpenFile(path);

                dfu.ProgressChange += (o, e) =>
                  {
                      Console.WriteLine(e.UserState);
                  };

                dfu.Begin(temp);
                Console.WriteLine("Complete");
                Console.Read();
                canLink.Close();
            }
        }
    }
}
