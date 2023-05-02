namespace Knv.MUDS150628
{
    using System;
    using System.Collections.Generic;

    public class Constants
    {
        public const string ValueNotAvailable2 = "n/a";
        public const string InvalidFlieNameChar = "A file name can't contain any of flowing characters:";
        public const string SoftwareCustomer = "Konvolúció Bt.";
        public const string SoftwareTitle = "Knv.MALT200817.DFU";
        public const string GenericTimestampFormat = "yyyy.MM.dd HH:mm:ss";
        public const string FileNameTimestampFormat = "yyMMdd_HHmmss";
        public const string FileFilter = "*.csv,*.mes,*.typ|*.csv;*.mes;*.typ|*.csv|*.csv|*.mes|*.mes|*.typ|*.mes";
        public const string NewLine = "\r\n";
        public const string CsvFileSeparator = ",";
        public static string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Konvolucio.MATL200319.IO.Log.txt";

        public static Dictionary<string, int> Baudrate
        {
            get
            {
                return new Dictionary<string, int>
                {
                    { "50kBaud", 50000 },
                    { "100kBaud", 100000},
                    { "125kBaud", 125000},
                    { "250kBaud", 250000},
                    { "500kBaud", 500000},
                };

            }
        }

    }
}
