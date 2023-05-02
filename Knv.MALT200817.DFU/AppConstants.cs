namespace Knv.MALT200817.DFU
{
    using System;
    public class AppConstants
    {
        public const string ValueNotAvailable2 = "n/a";
        public const string InvalidFlieNameChar = "A file name can't contain any of flowing characters:";
        public const string SoftwareCustomer = "Konvolúció Bt.";
        public const string SoftwareTitle = "Knv.MALT200817.DFU - MALT Device Firmware Upgrade Tool";
        public const string GenericTimestampFormat = "yyyy.MM.dd HH:mm:ss";
        public const string FileNameTimestampFormat = "yyMMdd_HHmmss";
        public const string FileFilter = "Binary File(*.bin)|*.bin";
        public const string NewLine = "\r\n";
        public const string CsvFileSeparator = ",";
        public static string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Konvolucio.MATL200319.IO.Log";
    }
}
