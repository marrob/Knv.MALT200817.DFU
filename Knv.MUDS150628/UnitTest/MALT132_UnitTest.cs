namespace Knv.MUDS150628.UnitTest
{
    using NUnit.Framework;
    using NiCanApi;

    [TestFixture]
    class MALT132_UnitTest
    {

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
    }
}
