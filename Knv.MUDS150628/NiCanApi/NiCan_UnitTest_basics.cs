
namespace Knv.MUDS150628.NiCanApi
{
    using NUnit.Framework;
    using System.Diagnostics;
    using System.Text;

    [TestFixture]
    class UnitTest_Nican_basics
    {
        public string Interface = "CAN0";
        public uint Baudrate = 500000;

        [Test]
        public void GetSerialNumber()
        {
            uint result = 0;
            int status = NiCan.ncGetHardwareInfo(1, 1, NiCan.NC_ATTR_HW_SERIAL_NUM, 4, ref result);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
            Assert.AreEqual("018175D0", result.ToString("X8"), "NC_ATTR_HW_SERIAL_NUM");
        }

        [Test]
        public void Config()
        {
            uint NumAttrs = 2;
            uint[] AttrIdList = { NiCan.NC_ATTR_BAUD_RATE, NiCan.NC_ATTR_START_ON_OPEN };
            uint[] AttrValueList = { Baudrate, NiCan.NC_FALSE };
            int status = NiCan.ncConfig(new StringBuilder(Interface), NumAttrs, AttrIdList, AttrValueList);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
        }

        [Test]
        public void ConfigOpenStartStopClose()
        {
            uint handle = 0;

            /*** Config ***/
            uint NumAttrs = 2;
            uint[] AttrIdList = { NiCan.NC_ATTR_BAUD_RATE, NiCan.NC_ATTR_START_ON_OPEN };
            uint[] AttrValueList = { Baudrate, NiCan.NC_FALSE };
            int status = NiCan.ncConfig(new StringBuilder(Interface), NumAttrs, AttrIdList, AttrValueList);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Open Object ***/
            status = NiCan.ncOpenObject(new StringBuilder(Interface), ref handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Start ***/
            status = NiCan.ncAction(handle, NiCan.NC_OP_START, 0);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Stop ***/
            status = NiCan.ncAction(handle, NiCan.NC_OP_STOP, 0);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Close ***/
            status = NiCan.ncCloseObject(handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
        }

        [Test]
        public void GetAttribute()
        {
            uint handle = 0;
            uint attrValue = 0;

            /*** Config ***/
            uint[] AttrIdList = { NiCan.NC_ATTR_BAUD_RATE, NiCan.NC_ATTR_START_ON_OPEN };
            uint[] AttrValueList = { Baudrate, NiCan.NC_FALSE };
            uint NumAttrs = 2;
            int status = NiCan.ncConfig(new StringBuilder(Interface), NumAttrs, AttrIdList, AttrValueList);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Open Object ***/
            status = NiCan.ncOpenObject(new StringBuilder(Interface), ref handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Get NC_ATTR_HW_SERIAL_NUM ***/
            status = NiCan.ncGetAttribute(handle, NiCan.NC_ATTR_HW_SERIAL_NUM, 4, ref attrValue);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
            Assert.AreEqual("018175D0", attrValue.ToString("X8"), "NC_ATTR_HW_SERIAL_NUM");

            /*** Get NC_ATTR_READ_PENDING ***/
            status = NiCan.ncGetAttribute(handle, NiCan.NC_ATTR_READ_PENDING, 4, ref attrValue);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
            Assert.AreEqual(0, attrValue, "NC_ATTR_READ_PENDING");

            /*** Close ***/
            status = NiCan.ncCloseObject(handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
        }

        [Test]
        public void SetAttribute()
        {
            uint handle = 0;
            uint attrValue = 0;

            /*** Config ***/
            uint[] AttrIdList = { NiCan.NC_ATTR_BAUD_RATE, NiCan.NC_ATTR_START_ON_OPEN };
            uint[] AttrValueList = { Baudrate, NiCan.NC_FALSE };
            uint NumAttrs = 2;
            int status = NiCan.ncConfig(new StringBuilder(Interface), NumAttrs, AttrIdList, AttrValueList);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Open Object ***/
            status = NiCan.ncOpenObject(new StringBuilder(Interface), ref handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Set NC_ATTR_SINGLE_SHOT_TX ***/
            attrValue = NiCan.NC_FALSE; ;
            status = NiCan.ncSetAttribute(handle, NiCan.NC_ATTR_SINGLE_SHOT_TX, 4, ref attrValue);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Set NC_ATTR_SERIES2_FILTER_MODE ***/
            attrValue = NiCan.NC_FILTER_SINGLE_EXTENDED;
            status = NiCan.ncSetAttribute(handle, NiCan.NC_ATTR_SERIES2_FILTER_MODE, 4, ref attrValue);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Set NC_ATTR_SERIES2_MASK ***/
            attrValue = 0x00000000 << 3;
            status = NiCan.ncSetAttribute(handle, NiCan.NC_ATTR_SERIES2_MASK, 4, ref attrValue);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Set NC_ATTR_SERIES2_COMP ***/
            uint CanId = 01;
            attrValue = CanId << 3;
            status = NiCan.ncSetAttribute(handle, NiCan.NC_ATTR_SERIES2_COMP, 4, ref attrValue);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Close ***/
            status = NiCan.ncCloseObject(handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
        }


        [Test]
        public void WriteFrame()
        {

            uint handle = 0;
            uint canId = 0xFF;

            /*** Config ***/
            uint[] AttrIdList = { NiCan.NC_ATTR_BAUD_RATE, NiCan.NC_ATTR_START_ON_OPEN };
            uint[] AttrValueList = { Baudrate, NiCan.NC_FALSE };
            uint NumAttrs = 2;
            int status = NiCan.ncConfig(new StringBuilder(Interface), NumAttrs, AttrIdList, AttrValueList);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Open Object ***/
            status = NiCan.ncOpenObject(new StringBuilder(Interface), ref handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Start ***/
            status = NiCan.ncAction(handle, NiCan.NC_OP_START, 0);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Frame ***/
            var frame = new NiCan.NCTYPE_CAN_FRAME();

            frame.ArbitrationId = canId | NiCan.NC_FL_CAN_ARBID_XTD;
            frame.IsRemote = NiCan.NC_FALSE;
            frame.DataLength = 8;
            frame.Data0 = 0;
            frame.Data1 = 1;
            frame.Data2 = 2;
            frame.Data3 = 3;
            frame.Data4 = 4;
            frame.Data5 = 5;
            frame.Data6 = 6;
            frame.Data7 = 7;

            /*** Write ***/
            status = NiCan.ncWrite(handle, NiCan.CanFrameSize, ref frame);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Stop ***/
            status = NiCan.ncAction(handle, NiCan.NC_OP_STOP, 0);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Close ***/
            status = NiCan.ncCloseObject(handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
        }

        [Test]
        public void ReadFrame()
        {
            uint handle = 0;
            var rxMsg = new NiCan.NCTYPE_CAN_STRUCT();

            /*** Config ***/
            uint[] AttrIdList = { NiCan.NC_ATTR_BAUD_RATE, NiCan.NC_ATTR_START_ON_OPEN };
            uint[] AttrValueList = { Baudrate, NiCan.NC_FALSE };
            uint NumAttrs = 2;
            int status = NiCan.ncConfig(new StringBuilder(Interface), NumAttrs, AttrIdList, AttrValueList);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Open Object ***/
            status = NiCan.ncOpenObject(new StringBuilder(Interface), ref handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

            /*** Start ***/
            status = NiCan.ncAction(handle, NiCan.NC_OP_START, 0);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

        var timestamp = System.DateTime.Now;
        bool flag = false;
        int tryCount = 0;

        do
        {

            /*** Get NC_ATTR_READ_PENDING ***/
            uint msgspending = 0;
            status = NiCan.ncGetAttribute(handle, NiCan.NC_ATTR_READ_PENDING, 4, ref msgspending);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));


            if (msgspending != 0)
            {
                /*** Read ***/
                status = NiCan.ncRead(handle, NiCan.CanStructSize, ref rxMsg);
                Assert.AreEqual(0, status, NiCanTools.StatusToString(status));

                /*** Msg Write Console ***/
                Debug.WriteLine("Timestamp:" + rxMsg.TimeStamp.ToString("X"));
                Debug.WriteLine("ArbitrationId:" + rxMsg.ArbitrationId.ToString("X"));
                Debug.WriteLine("FrameType:" + rxMsg.FrameType.ToString());
                Debug.WriteLine("DataLength:" + rxMsg.DataLength.ToString());
                Debug.WriteLine("Data64:" + rxMsg.Data64.ToString("X"));
                Debug.WriteLine("Data7:" + rxMsg.Data7.ToString());

                flag = true;
            }

            tryCount++;

        } while ((System.DateTime.Now - timestamp).Seconds < 10);

            if (!flag)
                Debug.WriteLine("I did not received any messages...");

            Debug.WriteLine("I checked:" + tryCount.ToString() + " times.");

            /*** Stop ***/
            status = NiCan.ncAction(handle, NiCan.NC_OP_STOP, 0);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));


            /*** Close ***/
            status = NiCan.ncCloseObject(handle);
            Assert.AreEqual(0, status, NiCanTools.StatusToString(status));
        }
    }
}
