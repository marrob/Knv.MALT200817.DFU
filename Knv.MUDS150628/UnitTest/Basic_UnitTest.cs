

namespace Knv.MUDS150628.UnitTest
{
    using System;
    using NUnit.Framework;
    using NiCanApi;

    [TestFixture]
    class UnitTest_Basic
    {
        [Test]
        public void CP1540_TesterPresent()
        {
            using (var canLink = new NiCanApiInterface())
            {
                canLink.Init(baudrate: 250000, intfName: "CAN0");
                var network = new Iso15765NetwrorkLayer(canLink);
                network.Log = false;
                network.TransmittId = 0x714;
                network.ReceiveId = 0x734;
                byte[] response;
                network.Transport(new byte[] { 0x3E, 0x00 }, out response);
            }
        }

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

        [Test]
        public void MALT132_TesterPresent()
        {
            using (var canLink = new NiCanApiInterface())
            {
                canLink.Init(baudrate: 250000, intfName: "CAN0");
                var network = new Iso15765NetwrorkLayer(canLink);
                network.Log = false;
                network.TransmittId = 0x15510403;
                network.ReceiveId = 0x15520403;
                byte[] response;
                network.Transport(new byte[] { 0x3E, 0x00 }, out response);
            }
        }

        [Test]
        public void Tachograph_TesterPresent()
        {
            using (var canLink = new NiCanApiInterface())
            {
                canLink.Init(baudrate: 250000, intfName: "CAN0");
                var network = new Iso15765NetwrorkLayer(canLink);
                network.Log = false;
                network.TransmittId = 0x38DAEEFB;
                network.ReceiveId = 0x38DAFBEE;

                byte[] response;

                Console.WriteLine("Tester Present:");
                network.Transport(new byte[] { 0x3E, 0x00 }, out response);
                Assert.AreEqual(new byte[] { 0x7E, 0x00 }, response);

                Console.WriteLine("DiagnosticSession:");
                network.Transport(new byte[] { 0x10, 0x7E }, out response);
                Assert.AreEqual(new byte[] { 0x50, 0x7E }, response);

                Console.WriteLine("CloseRemoteAuthentication:");
                network.Transport(new byte[] { 0x31, 0x01, 0x01, 0x80, 0x09 }, out response);
                Assert.AreEqual(new byte[] { 0x71, 0x01, 0x01, 0x80, 0x0A }, response);

                Console.WriteLine("RemoteCompanyCardReady:");
                network.Transport(new byte[] { 0x31, 0x01, 0x01, 0x80, 0x01, 0x3B, 0x3B, 0x9A,
                                            0x96, 0xC0, 0x10, 0x31, 0xFE, 0x5D, 0x00, 0x64,
                                            0x05, 0x7B, 0x01, 0x02, 0x31, 0x80, 0x90, 0x00,
                                            0x76 }, out response); //25:Length
                Assert.AreEqual(new byte[] { 0x71, 0x01, 0x01, 0x80, 0x02 }, response);
            }
        }
    }
}
