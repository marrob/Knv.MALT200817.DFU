

namespace Knv.MUDS150628.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NiCanApi;
    using MUDS150628;
    using NUnit.Framework;

    [TestFixture]
    class Tachograph_UnitTest
    {
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
