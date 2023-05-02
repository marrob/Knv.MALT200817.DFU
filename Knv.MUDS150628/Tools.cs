
namespace Knv.MUDS150628
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;


    public static class Tools
    {

        /// <summary>
        /// Byte Array To C-Style String.
        /// </summary>
        /// <param name="data">byte[] data</param>
        /// <returns>0x00,0x01</returns>
        public static string ByteArrayToCStyleString(byte[] data)
        {
            string retval = string.Empty;
            for (int i = 0; i < data.Length; i++)
                retval += string.Format("0x{0:X2},", data[i]);
            //Az utolsó vessző törlése
            if (data.Length > 1)
                retval = retval.Remove(retval.Length - 1, 1);
            return retval;
        }

        //7 5 2 
        public static string ByteArrayToCStyleString(byte[] data, int startIndex, int length)
        {
            string retval = string.Empty;
            for (int i = 0; i < length; i++)
                retval += string.Format("0x{0:X2},", data[i + startIndex]);
            //Az utolsó vessző törlése
            if (data.Length > 1)
                retval = retval.Remove(retval.Length - 1, 1);
            return retval;
        }

        /// <summary>
        /// Array forrmatter.
        /// Supported: offset, prefix, separator, block size.
        /// </summary>
        /// <typeparam name="T">eg.: byte[], string[]...</typeparam>
        /// <param name="array">eg.: byte[]</param>
        /// <param name="offset">eg.: 0</param>
        /// <param name="prefix">eg.: "0x"</param>
        /// <param name="format">eg.: "{0:X2}"</param>
        /// <param name="separator">eg: ','</param>
        /// <returns>eg.: 0x01,0x02,0x03...</returns>
        public static string ArrayToStringFormat<T>(T[] array, int offset, string prefix, string format, char separator, int blockSize)
        {
            string retval = string.Empty;
            if (array.Length == 0)
                return string.Empty;
            if (offset > array.Length)
                return string.Empty;
            int block = 1;

            for (int i = offset; i < array.Length; i++)
            {
                retval += string.Format(prefix + format + separator, array[i]);
                if ((block % blockSize) == 0)
                    retval += "\n";
                block++;
            }

            if (array.Length > 1)
            {
                retval = retval.TrimEnd('\n');
                retval = retval.Remove(retval.Length - 1, 1);
            }
            return (retval);
        }

        internal static string TimestampFormat(long tick)
        {
            return new DateTime(tick).ToString("dd.MM.yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// CRC-16-ANSI
        /// xx16 + x15 + x2 + 1 => 0x8005
        /// Test:
        /// 0x48,0x65,0x6C,0x6C,0x6F,0x20,0x57,0x6F,0x72,0x6C,0x64 -> CRC:0x70C3
        /// </summary>
        /// <param name="data">Erre a tömbre számolja a CRC-t.</param>
        /// <returns>Ez a CRC eredménye.</returns>
        public static UInt16 CalcCrc16Ansi(UInt16 initValue, byte[] data)
        {
            UInt16 remainder = initValue;
            UInt16 polynomial = 0x8005;
            for (long i = 0; i < data.LongLength; ++i)
            {
                remainder ^= (UInt16)(data[i] << 8);
                for (byte bit = 8; bit > 0; --bit)
                {
                    if ((remainder & 0x8000) != 0)
                        remainder = (UInt16)((remainder << 1) ^ polynomial);
                    else
                        remainder = (UInt16)(remainder << 1);
                }
            }
            return (remainder);
        }

        /// <summary>
        /// Bináris beolvasása bájt tömbe
        /// </summary>
        /// <param name="fullPath">Teljes elérési utvonal.</param>
        /// <param name="databytes">Adatbájtok</param>
        public static byte[] OpenFile(string fullPath)
        {
            byte[] databytes;
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                databytes = new byte[fs.Length];
                fs.Read(databytes, 0, databytes.Length);
            }
            return databytes;
        }
    }
}
