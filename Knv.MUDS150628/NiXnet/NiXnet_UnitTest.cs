

namespace Knv.MUDS150628.NiXnet
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Common;


    [TestFixture]
    internal class NiXnet_UnitTest
    {


        [Test]
        public void Begin()
        {

            var itf = new NiXnetInterface();
            itf.Init(500000, "CAN1");


            var msg = new CanMsg()
            {
                Id = 0x11,
                Length = 8,
                Payload = new byte[8] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }
            };

            itf.WriteFrame(new CanMsg[] { msg });


            CanMsg[] x = itf.ReadFrame();


        }
    }
}
