

namespace Knv.MUDS150628.UnitTest
{
    using NUnit.Framework;
    using MUDS150628;
    using NiCanApi;

    [TestFixture]
    class CP1540_UnitTest
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
    }
}
