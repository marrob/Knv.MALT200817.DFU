

namespace Knv.MUDS150628.UnitTest
{
    using System;
    using NUnit.Framework;
    using NiCanApi;

    [TestFixture]
    class Bxxx_UnitTest
    {
        [Test]
        public void Bxxx_TesterPresent()
        {
            using (var canLink = new NiCanApiInterface())
            {
                canLink.Init(baudrate: 500000, intfName: "CAN0");
                var network = new Iso15765NetwrorkLayer(canLink);
                network.Log = false;
                network.TransmittId = 0x714;
                network.ReceiveId = 0x734;


                byte[] response;

                Console.WriteLine("Tester Present:");
                network.Transport(new byte[] { 0x3E, 0x00 }, out response);
                Assert.AreEqual(new byte[] { 0x7E, 0x00 }, response);

                Console.WriteLine("Diag Session:");
                network.Transport(new byte[] { 0x10, 0x03 }, out response);
                Assert.AreEqual(new byte[] { 0x50, 0x03, 0x00, 0x32, 0x01, 0xF4 }, response);

                Console.WriteLine("Read Dtc:");
                network.Transport(new byte[] { 0x19, 0x02, 0x08 }, out response);
                Console.WriteLine("Dtc:");
                Console.WriteLine(Tools.ByteArrayToCStyleString(response));
                Assert.AreEqual(new byte[] { 0x59, 0x02, 0xCA, 0x90, 0xB5, 0x15, //6
                                                         0x0A, 0x90, 0xB6, 0x15, //4
                                                         0x0A, 0x90, 0xB9, 0x12, //4
                                                         0x0A, 0x9A, 0x61, 0x15, //4
                                                         0x0A, 0x9A, 0x63, 0x15, //4
                                                         0x0A, 0x9A, 0x64, 0x15,
                                                         0x0A, 0xC1, 0x40, 0x00,
                                                         0x0A, 0xC1, 0x55, 0x00, //
                                                         0x0A }, response);      //Szum: 35

            }
        }
    }
}
