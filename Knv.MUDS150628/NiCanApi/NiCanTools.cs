using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knv.MUDS150628.NiCanApi
{
    public class NiCanTools
    {
        public static List<string> GetAdapters()
        {
            var adapaters = new List<string>();
            adapaters.AddRange( new string[]{ "CAN0", "CAN1"} );
            return adapaters;
        }

        public static string StatusToString(int status)
        {
            const uint msgBufSize = 1024;
            StringBuilder statusStr = new StringBuilder((int)msgBufSize);
            NiCan.ncStatusToString(status, msgBufSize, statusStr);
            return statusStr.ToString();
        }

        public static int ReadPending(uint handle)
        {
            uint attrValue = 0;
            var status = NiCanStatusCheck(NiCan.ncGetAttribute(handle, NiCan.NC_ATTR_READ_PENDING, 4, ref attrValue));
            if (status != null)
                throw status;
            return (int)attrValue; 
        }

        public static Exception NiCanStatusCheck(int status)
        {

            switch ((uint)status)
            {
                case 0xBFF62023:
                    {
                        /* The Interface is invalid or unknown.*/
                        /* 1. Az Adaptert menetköben eltávolították */
                        return new NiCanIoException(status);

                    }
                default:
                    {
                        return null;
                    }
            }
        }



        public static uint Open(string canInterface, uint baudrate)
        {
            int status = 0;
            uint handle = 0;

            /*** Config ***/
            uint[] AttrIdList = { NiCan.NC_ATTR_BAUD_RATE, NiCan.NC_ATTR_START_ON_OPEN };
            uint[] AttrValueList = { baudrate, NiCan.NC_FALSE };
            uint NumAttrs = 2;
            status = NiCan.ncConfig(new StringBuilder(canInterface), NumAttrs, AttrIdList, AttrValueList);
            if (status != 0)
                throw new NiCanIoException(status);

            /*** Open Object ***/
            status = NiCan.ncOpenObject(new StringBuilder(canInterface), ref handle);
            if (status != 0)
                throw new NiCanIoException(status);

            /*** Start ***/
            status = NiCan.ncAction(handle, NiCan.NC_OP_START, 0);
            if (status != 0)
                throw new NiCanIoException(status);
            return handle;
        }

        public static void Close(uint handle)
        {
            NiCan.ncAction(handle, NiCan.NC_OP_STOP, 0);
            NiCan.ncCloseObject(handle);
        }

    }
}
