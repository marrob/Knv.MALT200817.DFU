
namespace Knv.MUDS150628
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IoLog
    {
        public static IoLog Instance { get; } = new IoLog();

        public string FilePath { get; set; } = Constants.LogPath;
        public bool Enabled;

        public double? GetFileSizeKB
        {
            get
            {
                if (File.Exists(FilePath))
                {
                    FileInfo fi = new FileInfo(FilePath);
                    return fi.Length / 1024;
                }
                else
                    return null;
            }
        }

        public IoLog()
        {
            Enabled = true;
            FilePath = "IoLog.txt";
        }

        public void WirteLine(string line)
        {
            if (Enabled)
            {
                line = DateTime.Now.ToString(Constants.GenericTimestampFormat, System.Globalization.CultureInfo.InvariantCulture) + ";" + line + Constants.NewLine;
                var fileWrite = new StreamWriter(FilePath, true, Encoding.ASCII);
                fileWrite.NewLine = Constants.NewLine;
                fileWrite.Write(line);
                fileWrite.Flush();
                fileWrite.Close();
            }
        }
    }
}
