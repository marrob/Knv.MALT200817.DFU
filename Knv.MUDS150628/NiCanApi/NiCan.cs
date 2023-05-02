using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

/**
 * Attention!!!
 * Properties: Build: Target Platform: ONLY x86!!!!
 */

namespace Knv.MUDS150628.NiCanApi
{
    // only functions and constants related to NI-CAN USB-8473 (Frame API)
    // consult Nican.h for more info, in order to run properly file Nican.dll should stay beside your.exe
    // USB-8473 support only Frame API, Only CAN Network Interface Object (no CAN Object like CANx::STDxx), 
    // no suppor for ncCreateNotification
    class NiCan     
    {
        #region FUNCTIONS DEFINITIONS

        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncAction(
                                uint ObjHandle, //NCTYPE_OBJH
                                uint Opcode,    //NCTYPE_OPCODE, only start stop reset 
                                uint Param      //NCTYPE_UINT32
                                );
        /* USAGE:
            int Status = 100;
            StringBuilder StatusStr = new StringBuilder((int)MsgBufSize);
            Status = NicanDll.ncAction(Can0ObjHandle, NicanDll.NC_OP_START, 0);
            NicanDll.ncStatusToString(Status, MsgBufSize, StatusStr);
            MessageBox.Show("ObjHandle:" + Can0ObjHandle.ToString()
                            + "\nStatus:" + Status.ToString()
                            + "\nStatus String:" + StatusStr.ToString()
                            + "\nStatus String size:" + StatusStr.ToString().Length
                            );
        */ 


        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncCloseObject(uint ObjHandle /*NCTYPE_OBJH*/);
        
        /* USAGE:
            int Status = 100;
            StringBuilder StatusStr = new StringBuilder((int)MsgBufSize);
            Status = NicanDll.ncCloseObject(Can0ObjHandle);
            NicanDll.ncStatusToString(Status, MsgBufSize, StatusStr);
            MessageBox.Show("ObjHandle:" + Can0ObjHandle.ToString()
                            + "\nStatus:" + Status.ToString()
                            + "\nStatus string:" + StatusStr.ToString()
                            + "\nString size:" + StatusStr.ToString().Length );
        */

        // ncConfig: Only the Baudrate and StartOnOpen attributes are used; other values are ignored.
        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncConfig(
                                StringBuilder ObjName,      //NCTYPE_STRING
                                uint NumAttrs,              //NCTYPE_UINT32
                                uint[] AttrIdList,      //NCTYPE_ATTRID_P
                                uint[] AttrValueList    //NCTYPE_UINT32_P
                                );
        /* USAGE:
        int Status = 100;
        StringBuilder ObjName = new StringBuilder("CAN0");
        uint NumAttrs = 12;
        uint[] AttrIdList = {   NicanDll.NC_ATTR_BAUD_RATE, 
                                NicanDll.NC_ATTR_START_ON_OPEN, 
                                NicanDll.NC_ATTR_READ_Q_LEN, 
                                NicanDll.NC_ATTR_WRITE_Q_LEN, 
                                NicanDll.NC_ATTR_SINGLE_SHOT_TX, 
                                NicanDll.NC_ATTR_SERIES2_FILTER_MODE, 
                                NicanDll.NC_ATTR_SERIES2_COMP, 
                                NicanDll.NC_ATTR_SERIES2_MASK, 
                                NicanDll.NC_ATTR_COMP_STD, 
                                NicanDll.NC_ATTR_MASK_STD, 
                                NicanDll.NC_ATTR_COMP_XTD, 
                                NicanDll.NC_ATTR_MASK_XTD 
                            };
        uint[] AttrValueList = {    1000000, 
                                    NicanDll.NC_FALSE, 
                                    100, 
                                    10, 
                                    NicanDll.NC_TRUE, 
                                    NicanDll.NC_FILTER_SINGLE_EXTENDED, 
                                    0x00000000 << 3, 
                                    0x00000000 << 3, 
                                    0, 
                                    0, 
                                    0, 
                                    0 
                               };
        StringBuilder StatusStr = new StringBuilder((int)MsgBufSize);
        Status = NicanDll.ncConfig(ObjName, NumAttrs, AttrIdList, AttrValueList);
        NicanDll.ncStatusToString(Status, MsgBufSize, StatusStr);
        MessageBox.Show(  "\nStatus:" + Status.ToString()
                        + "\nStatus string:" + StatusStr.ToString()
                        + "\nString size:" + StatusStr.ToString().Length );
        */

        /*
         * NI-CAN 8473 does not have RTSI i/o, so there is no terminal connections available
        public static extern NCTYPE_STATUS _NCFUNC_ ncConnectTerminals(
                           NCTYPE_OBJH          ObjHandle,
                           NCTYPE_UINT32        SourceTerminal,
                           NCTYPE_UINT32        DestinationTerminal,
                           NCTYPE_UINT32        Modifiers);

        public static extern NCTYPE_STATUS _NCFUNC_ ncDisconnectTerminals(
                           NCTYPE_OBJH          ObjHandle,
                           NCTYPE_UINT32        SourceTerminal,
                           NCTYPE_UINT32        DestinationTerminal,
                           NCTYPE_UINT32        Modifiers);
        */

        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncGetAttribute(
                                uint ObjHandle,     //NCTYPE_OBJH
                                uint AttrId,        //NCTYPE_ATTRID
                                uint SizeofAttr,    //NCTYPE_UINT32
                                ref uint Attr       //NCTYPE_ANY_P
                                );
            /* USAGE:
            int Status = 100;
            StringBuilder StatusStr = new StringBuilder((int)MsgBufSize);
            uint Attr = 0;
            Status = NicanDll.ncGetAttribute(Can0ObjHandle, NicanDll.NC_ATTR_BAUD_RATE, 4, ref Attr);
            //Status = NicanDll.ncGetAttribute(Can0ObjHandle, NicanDll.NC_ATTR_HW_SERIAL_NUM, 4, ref Attr);
            NicanDll.ncStatusToString(Status, MsgBufSize, StatusStr);
            MessageBox.Show("ObjHandle:" + Can0ObjHandle.ToString()
                            + "\nAttribute:" + Attr.ToString()
                            + "\nStatus:" + Status.ToString()
                            + "\nStatus String:" + StatusStr.ToString()
                            + "\nStatus String size:" + StatusStr.ToString().Length
                            );
            */

        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncGetHardwareInfo(
                                uint CardIndex,   //NCTYPE_UINT32
                                uint PortIndex,   //NCTYPE_UINT32
                                uint AttrId,      //NCTYPE_ATTRID
                                uint AttrSize,    //NCTYPE_UINT32
                                ref uint  Attr    //NCTYPE_ANY_P
                                );
            /* USAGE:
            StringBuilder StatusStr = new StringBuilder((int)MsgBufSize);
            uint Result = 0;
            int Status = 0;

            Status = NicanDll.ncGetHardwareInfo(5, 1, NicanDll.NC_ATTR_HW_SERIAL_NUM, 4, ref Result);

            NicanDll.ncStatusToString(Status, MsgBufSize, StatusStr);

            MessageBox.Show("Result: " + Result.ToString() 
                            + "\nStatus:" + Status.ToString()
                            + "\nStatus string:" + StatusStr.ToString()
                            + "\nString size:" + StatusStr.ToString().Length
                            );
            */ 

        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncOpenObject(
                                StringBuilder ObjName,  //NCTYPE_STRING
                                ref uint ObjHandle      //NCTYPE_OBJH_P
                                );
            /* USAGE:
            int Status = 100;
            StringBuilder ObjName = new StringBuilder("CAN0");
            StringBuilder StatusStr = new StringBuilder((int)MsgBufSize);
            uint ObjHandle = 0;
            Status = NicanDll.ncOpenObject(ObjName, ref ObjHandle);
            NicanDll.ncStatusToString(Status, MsgBufSize, StatusStr);
            MessageBox.Show("ObjHandle:" + ObjHandle.ToString()  
                            + "\nStatus:" + Status.ToString()
                            + "\nStatus string:" + StatusStr.ToString()
                            + "\nString size:" + StatusStr.ToString().Length );
         */

        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncRead(
                                uint ObjHandle, //NCTYPE_OBJH
                                uint SizeofData,  //NCTYPE_UINT32
                                ref NCTYPE_CAN_STRUCT Data    // IntPtr Data    //NCTYPE_ANY_P
                                );
        /*
        [DllImport("Nican.dll")]
        public static extern int ncRead(
                                uint ObjHandle, //NCTYPE_OBJH
                                uint SizeofData,  //NCTYPE_UINT32
                                IntPtr Data    //NCTYPE_ANY_P
                                );
        */
        /*
        [DllImport("Nican.dll" )]
        public static extern int ncReadMult(
                                uint ObjHandle, //NCTYPE_OBJH
                                uint SizeofData,  //NCTYPE_UINT32
                                ref NCTYPE_CAN_STRUCT[] Data,   //NCTYPE_ANY_P, or IntPtr?
                                ref uint ActualDataSize //NCTYPE_UINT32_P
                                );
        */

        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncReadMult(
                                uint ObjHandle, //NCTYPE_OBJH
                                uint SizeofData,  //NCTYPE_UINT32
                                IntPtr Data,   //NCTYPE_ANY_P,  IntPtr? ref NCTYPE_CAN_STRUCT[] Data?-conflict bw managed an unmanaged operations
                                ref uint ActualDataSize //NCTYPE_UINT32_P
                                );


        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncSetAttribute(
                                uint ObjHandle,     //NCTYPE_OBJH
                                uint AttrId,        //NCTYPE_ATTRID
                                uint SizeofAttr,    //NCTYPE_UINT32
                                ref uint Attr       //NCTYPE_ANY_P ref uint (most Attributes are uint) or IntPtr
                                );
            /* USAGE:
            int Status = 100;
            StringBuilder StatusStr = new StringBuilder((int)MsgBufSize);
            uint Attr = 125000;
            Status = NicanDll.ncSetAttribute(Can0ObjHandle, NicanDll.NC_ATTR_BAUD_RATE, 4, ref Attr);
            //Status = NicanDll.ncSetAttribute(Can0ObjHandle, NicanDll.NC_ATTR_HW_SERIAL_NUM, 4, ref Attr);
            NicanDll.ncStatusToString(Status, MsgBufSize, StatusStr);
            MessageBox.Show("ObjHandle:" + Can0ObjHandle.ToString()
                            + "\nSetAttribute:" + Attr.ToString()
                            + "\nStatus:" + Status.ToString()
                            + "\nStatus String:" + StatusStr.ToString()
                            + "\nStatus String size:" + StatusStr.ToString().Length );
            */
       


        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern void ncStatusToString(
                                    int Status,             //NCTYPE_STATUS
                                    uint SizeofString,      //NCTYPE_UINT32
                                    StringBuilder ErrorString      //NCTYPE_STRING
                                    );
        /* USAGE:
        StringBuilder str = new StringBuilder(1024); int status = -1074388957;
        NicanDll.ncStatusToString(status, 1024, str);
        MessageBox.Show("\nStatus:" + status.ToString()+ "\nStatus string:"+str.ToString()+ "\nString size:" + str.ToString().Length );
        */
          
        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncWaitForState(
                                    uint ObjHandle,         //NCTYPE_OBJH
                                    uint DesiredState,      //NCTYPE_STATE
                                    uint Timeout,           //NCTYPE_DURATION
                                    ref uint CurrentState   //NCTYPE_STATE_P
                                    );

        

        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncWrite(
                                    uint ObjHandle,     //NCTYPE_OBJH
                                    uint SizeofData,    //NCTYPE_UINT32
                                    ref NCTYPE_CAN_FRAME Data       //NCTYPE_ANY_P 
                                    );

        [DllImport("Nican.dll" /*, CallingConvention = CallingConvention.Cdecl*/)]
        public static extern int ncWriteMult(
                                    uint ObjHandle, //NCTYPE_OBJH
                                    uint SizeofData,  //NCTYPE_UINT32
                                    IntPtr FrameArray    //NCTYPE_ANY_P or NCTYPE_CAN_STRUCT_P
                                    );
        
        #endregion

        #region STRUCTURES DEFINITIONS

        public const uint CanFrameSize = 14;    //size of block of data passed to ncWrite

        [StructLayout(LayoutKind.Explicit, Size=14)]
        public struct NCTYPE_CAN_FRAME      // size of structure is 14 bytes(4,2,8)
        {
            [FieldOffset(0)]    public uint ArbitrationId;  //NCTYPE_CAN_ARBID
            [FieldOffset(4)]    public byte IsRemote;       //NCTYPE_BOOL
            [FieldOffset(5)]    public byte DataLength;     //NCTYPE_UINT8, length of Data in bytes
            [FieldOffset(6)]    public byte Data0;          //NCTYPE_UINT8
            [FieldOffset(7)]    public byte Data1;          //NCTYPE_UINT8
            [FieldOffset(8)]    public byte Data2;          //NCTYPE_UINT8
            [FieldOffset(9)]    public byte Data3;          //NCTYPE_UINT8
            [FieldOffset(10)]   public byte Data4;          //NCTYPE_UINT8
            [FieldOffset(11)]   public byte Data5;          //NCTYPE_UINT8
            [FieldOffset(12)]   public byte Data6;          //NCTYPE_UINT8
            [FieldOffset(13)]   public byte Data7;          //NCTYPE_UINT8
            [FieldOffset(6)]    public ulong Data64;          //union
        }

        public const uint CanStructSize = 22;    //size of block of data passed to ncWrite

        // no constructor, no initial values, no arrays, direct access to data, works well?
        [StructLayout(LayoutKind.Explicit, Size = 22)]
        public struct NCTYPE_CAN_STRUCT     // size of structure is 22 bytes (8,4,2,8)
        {
            [FieldOffset(0)]    public ulong TimeStamp;      //NCTYPE_ABS_TIME, which is NCTYPE_UINT64, 8 bytes
            [FieldOffset(8)]    public uint ArbitrationId;   //NCTYPE_CAN_ARBID, which is NCTYPE_UINT32, 4 bytes
            [FieldOffset(12)]   public byte FrameType;       //NCTYPE_UINT8,   RTR or DATA
            [FieldOffset(13)]   public byte DataLength;      //NCTYPE_UINT8
            [FieldOffset(14)]   public byte Data0;          //NCTYPE_UINT8
            [FieldOffset(15)]   public byte Data1;          //NCTYPE_UINT8
            [FieldOffset(16)]   public byte Data2;          //NCTYPE_UINT8
            [FieldOffset(17)]   public byte Data3;          //NCTYPE_UINT8
            [FieldOffset(18)]   public byte Data4;          //NCTYPE_UINT8
            [FieldOffset(19)]   public byte Data5;          //NCTYPE_UINT8
            [FieldOffset(20)]   public byte Data6;          //NCTYPE_UINT8
            [FieldOffset(21)]   public byte Data7;          //NCTYPE_UINT8
            [FieldOffset(14)]   public ulong Data64;        // all 8 bytes of data inone chunk, union with Data0..7
        }

        #endregion

        #region STATUS DEFINITIONS

        public const int NICAN_WARNING_BASE = (int)(0x3FF62000);    // positive
        public const int NICAN_ERROR_BASE = -1074388992;   //Signed integer (32-bit) Two's complement: (int)( 0xBFF62000 );    // negative
        
        public const int CanSuccess = 0;
        public const int DnetSuccess = 0;

        public const int CanErrFunctionTimeout = (NICAN_ERROR_BASE | 0x001);    // was NC_ERR_TIMEOUT
        public const int CanErrWatchdogTimeout = (NICAN_ERROR_BASE | 0x021);    // was NC_ERR_TIMEOUT
        public const int DnetErrConnectionTimeout = (NICAN_ERROR_BASE | 0x041);    // was NC_ERR_TIMEOUT
        public const int DnetWarnConnectionTimeout = (NICAN_WARNING_BASE | 0x041);
        public const int CanErrScheduleTimeout = (NICAN_ERROR_BASE | 0x0A1);    // was NC_ERR_TIMEOUT
        public const int CanErrDriver = (NICAN_ERROR_BASE | 0x002);    // was NC_ERR_DRIVER
        public const int CanWarnDriver = (NICAN_WARNING_BASE | 0x002);    // was NC_ERR_DRIVER
        public const int CanErrBadNameSyntax = (NICAN_ERROR_BASE | 0x003);    // was NC_ERR_BAD_NAME
        public const int CanErrBadIntfName = (NICAN_ERROR_BASE | 0x023);    // was NC_ERR_BAD_NAME
        public const int CanErrBadCanObjName = (NICAN_ERROR_BASE | 0x043);    // was NC_ERR_BAD_NAME
        public const int CanErrBadParam = (NICAN_ERROR_BASE | 0x004);    // was NC_ERR_BAD_PARAM
        public const int CanErrBadHandle = (NICAN_ERROR_BASE | 0x024);    // was NC_ERR_BAD_PARAM
        public const int CanErrBadAttributeValue = (NICAN_ERROR_BASE | 0x005);    // was NC_ERR_BAD_ATTR_VALUE
        public const int CanErrAlreadyOpen = (NICAN_ERROR_BASE | 0x006);    // was NC_ERR_ALREADY_OPEN
        public const int CanWarnAlreadyOpen = (NICAN_WARNING_BASE | 0x006);  // was NC_ERR_ALREADY_OPEN
        public const int DnetErrOpenIntfMode = (NICAN_ERROR_BASE | 0x026);    // was NC_ERR_ALREADY_OPEN
        public const int DnetErrOpenConnType = (NICAN_ERROR_BASE | 0x046);    // was NC_ERR_ALREADY_OPEN
        public const int CanErrNotStopped = (NICAN_ERROR_BASE | 0x007);    // was NC_ERR_NOT_STOPPED
        public const int CanErrOverflowWrite = (NICAN_ERROR_BASE | 0x008);    // was NC_ERR_OVERFLOW
        public const int CanErrOverflowCard = (NICAN_ERROR_BASE | 0x028);    // was NC_ERR_OVERFLOW
        public const int CanErrOverflowChip = (NICAN_ERROR_BASE | 0x048);    // was NC_ERR_OVERFLOW
        public const int CanErrOverflowRxQueue = (NICAN_ERROR_BASE | 0x068);    // was NC_ERR_OVERFLOW
        public const int CanWarnOldData = (NICAN_WARNING_BASE | 0x009);  // was NC_ERR_OLD_DATA
        public const int CanErrNotSupported = (NICAN_ERROR_BASE | 0x00A);    // was NC_ERR_NOT_SUPPORTED
        public const int CanWarnComm = (NICAN_WARNING_BASE | 0x00B);  // was NC_ERR_CAN_COMM
        public const int CanErrComm = (NICAN_ERROR_BASE | 0x00B);    // was NC_ERR_CAN_COMM
        public const int CanWarnCommStuff = (NICAN_WARNING_BASE | 0x02B);  // was NC_ERR_CAN_COMM
        public const int CanErrCommStuff = (NICAN_ERROR_BASE | 0x02B);    // was NC_ERR_CAN_COMM
        public const int CanWarnCommFormat = (NICAN_WARNING_BASE | 0x04B);  // was NC_ERR_CAN_COMM
        public const int CanErrCommFormat = (NICAN_ERROR_BASE | 0x04B);    // was NC_ERR_CAN_COMM
        public const int CanWarnCommNoAck = (NICAN_WARNING_BASE | 0x06B);  // was NC_ERR_CAN_COMM
        public const int CanErrCommNoAck = (NICAN_ERROR_BASE | 0x06B);    // was NC_ERR_CAN_COMM
        public const int CanWarnCommTx1Rx0 = (NICAN_WARNING_BASE | 0x08B);  // was NC_ERR_CAN_COMM
        public const int CanErrCommTx1Rx0 = (NICAN_ERROR_BASE | 0x08B);    // was NC_ERR_CAN_COMM
        public const int CanWarnCommTx0Rx1 = (NICAN_WARNING_BASE | 0x0AB);  // was NC_ERR_CAN_COMM
        public const int CanErrCommTx0Rx1 = (NICAN_ERROR_BASE | 0x0AB);    // was NC_ERR_CAN_COMM
        public const int CanWarnCommBadCRC = (NICAN_WARNING_BASE | 0x0CB);  // was NC_ERR_CAN_COMM
        public const int CanErrCommBadCRC = (NICAN_ERROR_BASE | 0x0CB);    // was NC_ERR_CAN_COMM
        public const int CanWarnCommUnknown = (NICAN_WARNING_BASE | 0x0EB);  // was NC_ERR_CAN_COMM
        public const int CanErrCommUnknown = (NICAN_ERROR_BASE | 0x0EB);    // was NC_ERR_CAN_COMM
        public const int CanWarnTransceiver = (NICAN_WARNING_BASE | 0x00C);  // was NC_ERR_CAN_XCVR
        public const int CanWarnRsrcLimitQueues = (NICAN_WARNING_BASE | 0x02D);  // was NC_ERR_RSRC_LIMITS
        public const int CanErrRsrcLimitQueues = (NICAN_ERROR_BASE | 0x02D);    // was NC_ERR_RSRC_LIMITS
        public const int DnetErrRsrcLimitIO = (NICAN_ERROR_BASE | 0x04D);    // was NC_ERR_RSRC_LIMITS
        public const int DnetErrRsrcLimitWriteSrvc = (NICAN_ERROR_BASE | 0x06D);    // was NC_ERR_RSRC_LIMITS
        public const int DnetErrRsrcLimitReadSrvc = (NICAN_ERROR_BASE | 0x08D);    // was NC_ERR_RSRC_LIMITS
        public const int DnetErrRsrcLimitRespPending = (NICAN_ERROR_BASE | 0x0AD);   // was NC_ERR_RSRC_LIMITS
        public const int DnetWarnRsrcLimitRespPending = (NICAN_WARNING_BASE | 0x0AD);
        public const int CanErrRsrcLimitRtsi = (NICAN_ERROR_BASE | 0x0CD);    // was NC_ERR_RSRC_LIMITS
        public const int DnetErrNoReadAvail = (NICAN_ERROR_BASE | 0x00E);    // was NC_ERR_READ_NOT_AVAIL
        public const int DnetErrBadMacId = (NICAN_ERROR_BASE | 0x00F);    // was NC_ERR_BAD_NET_ID
        public const int DnetErrDevInitOther = (NICAN_ERROR_BASE | 0x010);    // was NC_ERR_DEVICE_INIT
        public const int DnetErrDevInitIoConn = (NICAN_ERROR_BASE | 0x030);    // was NC_ERR_DEVICE_INIT
        public const int DnetErrDevInitInputLen = (NICAN_ERROR_BASE | 0x050);    // was NC_ERR_DEVICE_INIT
        public const int DnetErrDevInitOutputLen = (NICAN_ERROR_BASE | 0x070);    // was NC_ERR_DEVICE_INIT
        public const int DnetErrDevInitEPR = (NICAN_ERROR_BASE | 0x090);    // was NC_ERR_DEVICE_INIT
        public const int DnetErrDevInitVendor = (NICAN_ERROR_BASE | 0x0B0);    // was NC_ERR_DEVICE_INIT
        public const int DnetErrDevInitDevType = (NICAN_ERROR_BASE | 0x0D0);    // was NC_ERR_DEVICE_INIT
        public const int DnetErrDevInitProdCode = (NICAN_ERROR_BASE | 0x0F0);    // was NC_ERR_DEVICE_INIT
        public const int DnetErrDeviceMissing = (NICAN_ERROR_BASE | 0x011);    // was NC_ERR_DEVICE_MISSING
        public const int DnetWarnDeviceMissing = (NICAN_WARNING_BASE | 0x011);  // was NC_ERR_DEVICE_MISSING
        public const int DnetErrFragmentation = (NICAN_ERROR_BASE | 0x012);    // was NC_ERR_FRAGMENTATION
        public const int DnetErrIntfNotOpen = (NICAN_ERROR_BASE | 0x033);    // was NC_ERR_NO_CONFIG
        public const int DnetErrErrorResponse = (NICAN_ERROR_BASE | 0x014);    // was NC_ERR_DNET_ERR_RESP
        public const int CanWarnNotificationPending = (NICAN_WARNING_BASE | 0x015);  // was NC_ERR_NOTIF_PENDING
        public const int CanErrConfigOnly = (NICAN_ERROR_BASE | 0x017);    // was NC_ERR_CONFIG_ONLY
        public const int CanErrPowerOnSelfTest = (NICAN_ERROR_BASE | 0x018);    // PowerOn self test Failure


        //------------------------
        public const int CanErrMaxObjects = (NICAN_ERROR_BASE | 0x100);
        public const int CanErrMaxChipSlots = (NICAN_ERROR_BASE | 0x101);
        public const int CanErrBadDuration = (NICAN_ERROR_BASE | 0x102);
        public const int CanErrFirmwareNoResponse = (NICAN_ERROR_BASE | 0x103);
        public const int CanErrBadIdOrOpcode = (NICAN_ERROR_BASE | 0x104);
        public const int CanWarnBadSizeOrLength = (NICAN_WARNING_BASE | 0x105);
        public const int CanErrBadSizeOrLength = (NICAN_ERROR_BASE | 0x105);
        public const int CanErrNotifAlreadyInUse = (NICAN_ERROR_BASE | 0x107);
        public const int CanErrOneProtocolPerCard = (NICAN_ERROR_BASE | 0x108);
        public const int CanWarnPeriodsTooFast = (NICAN_WARNING_BASE | 0x109);
        public const int CanErrDllNotFound = (NICAN_ERROR_BASE | 0x10A);
        public const int CanErrFunctionNotFound = (NICAN_ERROR_BASE | 0x10B);
        public const int CanErrLangIntfRsrcUnavail = (NICAN_ERROR_BASE | 0x10C);
        public const int CanErrRequiresNewHwSeries = (NICAN_ERROR_BASE | 0x10D);
        public const int CanErrHardwareNotSupported = CanErrRequiresNewHwSeries;
        public const int CanErrSeriesOneOnly = (NICAN_ERROR_BASE | 0x10E);  //depreciated error code
        public const int CanErrSetAbsTime = (NICAN_ERROR_BASE | 0x10F);
        public const int CanErrBothApiSameIntf = (NICAN_ERROR_BASE | 0x110);
        public const int CanErrWaitOverlapsSameObj = (NICAN_ERROR_BASE | 0x111);
        public const int CanErrNotStarted = (NICAN_ERROR_BASE | 0x112);
        public const int CanErrConnectTwice = (NICAN_ERROR_BASE | 0x113);
        public const int CanErrConnectUnsupported = (NICAN_ERROR_BASE | 0x114);
        public const int CanErrStartTrigBeforeFunc = (NICAN_ERROR_BASE | 0x115);
        public const int CanErrStringSizeTooLarge = (NICAN_ERROR_BASE | 0x116);
        public const int CanErrQueueReqdForReadMult = (NICAN_ERROR_BASE | 0x117);
        public const int CanErrHardwareInitFailed = (NICAN_ERROR_BASE | 0x118);
        public const int CanErrOldDataLost = (NICAN_ERROR_BASE | 0x119);
        public const int CanErrOverflowChannel = (NICAN_ERROR_BASE | 0x11A);
        public const int CanErrUnsupportedModeMix = (NICAN_ERROR_BASE | 0x11C);
        public const int CanErrNoNetIntfConfig = (NICAN_ERROR_BASE | 0x11D);
        public const int CanErrBadTransceiverMode = (NICAN_ERROR_BASE | 0x11E);
        public const int CanErrWrongTransceiverAttr = (NICAN_ERROR_BASE | 0x11F);
        public const int CanErrRequiresXS = (NICAN_ERROR_BASE | 0x120);
        public const int CanErrDisconnected = (NICAN_ERROR_BASE | 0x121);
        public const int CanErrNoTxForListenOnly = (NICAN_ERROR_BASE | 0x122);
        public const int CanErrSetOnly = (NICAN_ERROR_BASE | 0x123);
        public const int CanErrBadBaudRate = (NICAN_ERROR_BASE | 0x124);
        public const int CanErrOverflowFrame = (NICAN_ERROR_BASE | 0x125);
        public const int CanWarnRTSITooFast = (NICAN_WARNING_BASE | 0x126);
        public const int CanErrNoTimebase = (NICAN_ERROR_BASE | 0x127);
        public const int CanErrTimerRunning = (NICAN_ERROR_BASE | 0x128);
        public const int DnetErrUnsupportedHardware = (NICAN_ERROR_BASE | 0x129);
        public const int CanErrInvalidLogfile = (NICAN_ERROR_BASE | 0x12A);
        public const int CanErrMaxPeriodicObjects = (NICAN_ERROR_BASE | 0x130);
        public const int CanErrUnknownHardwareAttribute = (NICAN_ERROR_BASE | 0x131);
        public const int CanErrDelayFrameNotSupported = (NICAN_ERROR_BASE | 0x132);
        public const int CanErrVirtualBusTimingOnly = (NICAN_ERROR_BASE | 0x133);

        public const int CanErrVirtualNotSupported = (NICAN_ERROR_BASE | 0x135);
        public const int CanErrWriteMultLimit = (NICAN_ERROR_BASE | 0x136);   // WriteMult does not allow write more that 512 frames at a time
        public const int CanErrObsoletedHardware = (NICAN_ERROR_BASE | 0x137);
        public const int CanErrVirtualBusTimingMismatch = (NICAN_ERROR_BASE | 0x138);
        public const int CanErrVirtualBusOnly = (NICAN_ERROR_BASE | 0x139);
        public const int CanErrConversionTimeRollback = (NICAN_ERROR_BASE | 0x13A);
        public const int CanErrInterFrameDelayExceeded = (NICAN_ERROR_BASE | 0x140);
        public const int CanErrLogConflict = (NICAN_ERROR_BASE | 0x141);
        public const int CanErrBootLoaderUpdated = (NICAN_ERROR_BASE | 0x142);// Error, bootloader not compatible with firmware.

        public const int CanWarnLowSpeedXcvr = CanWarnTransceiver;   // applies to HS as well as LS
        public const int CanErrOverflowRead = CanErrOverflowCard;   // overflow in card memory now lower level

        public const int CanErrorCompatibility = (NICAN_ERROR_BASE | 0x1B0);
        public const int CanErrorMissingFeature = (NICAN_ERROR_BASE | 0x1B1);

        #endregion STATUS

        #region ATTRIBUTE ID DEFINITIONS

        public const uint NC_ATTR_ABS_TIME = (uint)(0x80000008);
        public const uint NC_ATTR_BAUD_RATE = (uint)(0x80000007);
        public const uint NC_ATTR_BEHAV_FINAL_OUT = (uint)(0x80010018);

        public const uint NC_ATTR_BKD_CAN_RESPONSE = (uint)(0x80010006);
        public const uint NC_ATTR_BKD_CHANGES_ONLY = (uint)(0x80000015);
        public const uint NC_ATTR_BKD_PERIOD = (uint)(0x8000000F);
        public const uint NC_ATTR_BKD_READ_SIZE = (uint)(0x8000000B);
        public const uint NC_ATTR_BKD_TYPE = (uint)(0x8000000D);
        public const uint NC_ATTR_BKD_WHEN_USED = (uint)(0x8000000E);
        public const uint NC_ATTR_BKD_WRITE_SIZE = (uint)(0x8000000C);

        public const uint NC_ATTR_CAN_BIT_TIMINGS = (uint)(0x80010005);
        public const uint NC_ATTR_CAN_COMP_STD = (uint)(0x80010001);
        public const uint NC_ATTR_CAN_COMP_XTD = (uint)(0x80010003);
        public const uint NC_ATTR_CAN_DATA_LENGTH = (uint)(0x80010007);
        public const uint NC_ATTR_CAN_MASK_STD = (uint)(0x80010002);
        public const uint NC_ATTR_CAN_MASK_XTD = (uint)(0x80010004);
        public const uint NC_ATTR_CAN_TX_RESPONSE = (uint)(0x80010006);

        public const uint NC_ATTR_COMM_TYPE = (uint)(0x80000016);
        public const uint NC_ATTR_COMP_STD = (uint)(0x80010001);
        public const uint NC_ATTR_COMP_XTD = (uint)(0x80010003);
        public const uint NC_ATTR_DATA_LEN = (uint)(0x80010007);
        public const uint NC_ATTR_HW_FORMFACTOR = (uint)(0x80020004);     // Formfactor of card - NC_HW_FORMFACTOR_???
        public const uint NC_ATTR_HW_SERIAL_NUM = (uint)(0x80020003);     // Serial Number of card
        public const uint NC_ATTR_HW_SERIES = (uint)(0x80020005);     // Series of Card - NC_HW_SERIES_???
        public const uint NC_ATTR_HW_TRANSCEIVER = NC_ATTR_TRANSCEIVER_TYPE; // NC_HW_TRANSCEIVER_???
        public const uint NC_ATTR_INTERFACE_NUM = (uint)(0x80020008);     // 0 for CAN0, 1 for CAN1, etc...
        public const uint NC_ATTR_IS_NET_SYNC = (uint)(0x8001000E);
        public const uint NC_ATTR_LIN_CHECKSUM_TYPE = (uint)(0x80020043);
        public const uint NC_ATTR_LIN_ENABLE_DLC_CHECK = (uint)(0x80020045);
        public const uint NC_ATTR_LIN_LOG_WAKEUP = (uint)(0x80020046);
        public const uint NC_ATTR_LIN_RESPONSE_TIMEOUT = (uint)(0x80020044);
        public const uint NC_ATTR_LIN_SLEEP = (uint)(0x80020042);
        public const uint NC_ATTR_LISTEN_ONLY = (uint)(0x80010010);
        public const uint NC_ATTR_LOG_BUS_ERROR = (uint)(0x80020037);
        public const uint NC_ATTR_LOG_COMM_ERRS = (uint)(0x8001000A);
        public const uint NC_ATTR_LOG_START_TRIGGER = (uint)(0x80020031);
        public const uint NC_ATTR_LOG_TRANSCEIVER_FAULT = (uint)(0x80020038);
        public const uint NC_ATTR_MASK_STD = (uint)(0x80010002);
        public const uint NC_ATTR_MASK_XTD = (uint)(0x80010004);
        public const uint NC_ATTR_MASTER_TIMEBASE_RATE = (uint)(0x80020033);
        public const uint NC_ATTR_NET_SYNC_COUNT = (uint)(0x8001000D);
        public const uint NC_ATTR_NOTIFY_MULT_LEN = (uint)(0x8001000B);
        public const uint NC_ATTR_NOTIFY_MULT_SIZE = (uint)(0x8001000B);
        public const uint NC_ATTR_NUM_CARDS = (uint)(0x80020002);     // Number of Cards present in system.
        public const uint NC_ATTR_NUM_PORTS = (uint)(0x80020006);     // Number of Ports present on card
        public const uint NC_ATTR_PERIOD = (uint)(0x8000000F);
        public const uint NC_ATTR_PROTOCOL = (uint)(0x80000001);
        public const uint NC_ATTR_PROTOCOL_VERSION = (uint)(0x80000002);
        public const uint NC_ATTR_READ_PENDING = (uint)(0x80000011);
        public const uint NC_ATTR_READ_Q_LEN = (uint)(0x80000013);
        public const uint NC_ATTR_RESET_ON_START = (uint)(0x80010008);
        public const uint NC_ATTR_RTSI_FRAME = (uint)(0x80000020);
        public const uint NC_ATTR_RTSI_MODE = (uint)(0x80000017);
        public const uint NC_ATTR_RTSI_SIG_BEHAV = (uint)(0x80000019);
        public const uint NC_ATTR_RTSI_SIGNAL = (uint)(0x80000018);
        public const uint NC_ATTR_RTSI_SKIP = (uint)(0x80000021);
        public const uint NC_ATTR_RX_CHANGES_ONLY = (uint)(0x80000015);
        public const uint NC_ATTR_RX_ERROR_COUNTER = (uint)(0x80010011);
        public const uint NC_ATTR_RX_Q_LEN = (uint)(0x8001000C);
        public const uint NC_ATTR_SELF_RECEPTION = (uint)(0x80010016);
        public const uint NC_ATTR_SERIAL_NUMBER = (uint)(0x800000A0);
        public const uint NC_ATTR_SERIES2_COMP = (uint)(0x80010013);
        public const uint NC_ATTR_SERIES2_ERR_ARB_CAPTURE = (uint)(0x8001001C);
        public const uint NC_ATTR_SERIES2_FILTER_MODE = (uint)(0x80010015);
        public const uint NC_ATTR_SERIES2_MASK = (uint)(0x80010014);
        public const uint NC_ATTR_SINGLE_SHOT_TX = (uint)(0x80010017);
        public const uint NC_ATTR_SOFTWARE_VERSION = (uint)(0x80000003);
        public const uint NC_ATTR_START_ON_OPEN = (uint)(0x80000006);
        public const uint NC_ATTR_START_TRIG_BEHAVIOR = (uint)(0x80010023);
        public const uint NC_ATTR_STATE = (uint)(0x80000009);
        public const uint NC_ATTR_STATUS = (uint)(0x8000000A);
        public const uint NC_ATTR_TERMINATION = (uint)(0x80020041);
        public const uint NC_ATTR_TIMELINE_RECOVERY = (uint)(0x80020035);
        public const uint NC_ATTR_TIMESTAMP_FORMAT = (uint)(0x80020032);
        public const uint NC_ATTR_TIMESTAMPING = (uint)(0x80000010);
        public const uint NC_ATTR_TRANSCEIVER_EXTERNAL_IN = (uint)(0x8001001B);
        public const uint NC_ATTR_TRANSCEIVER_EXTERNAL_OUT = (uint)(0x8001001A);
        public const uint NC_ATTR_TRANSCEIVER_MODE = (uint)(0x80010019);
        public const uint NC_ATTR_TRANSCEIVER_TYPE = (uint)(0x80020007);
        public const uint NC_ATTR_TRANSMIT_MODE = (uint)(0x80020029);
        public const uint NC_ATTR_TX_ERROR_COUNTER = (uint)(0x80010012);
        public const uint NC_ATTR_TX_RESPONSE = (uint)(0x80010006);
        public const uint NC_ATTR_VERSION_BUILD = (uint)(0x8002000D);     // U32 build (primarily useful for beta)
        public const uint NC_ATTR_VERSION_COMMENT = (uint)(0x8002000E);     // String comment on version (max 80 chars)
        public const uint NC_ATTR_VERSION_MAJOR = (uint)(0x80020009);     // U32 major version (X in X.Y.Z)
        public const uint NC_ATTR_VERSION_MINOR = (uint)(0x8002000A);     // U32 minor version (Y in X.Y.Z)
        public const uint NC_ATTR_VERSION_PHASE = (uint)(0x8002000C);     // U32 phase (1=alpha, 2=beta, 3=release)
        public const uint NC_ATTR_VERSION_UPDATE = (uint)(0x8002000B);     // U32 minor version (Z in X.Y.Z)
        public const uint NC_ATTR_VIRTUAL_BUS_TIMING = (uint)(0xA0000031);
        public const uint NC_ATTR_WRITE_ENTRIES_FREE = (uint)(0x80020034);
        public const uint NC_ATTR_WRITE_PENDING = (uint)(0x80000012);
        public const uint NC_ATTR_WRITE_Q_LEN = (uint)(0x80000014);


        public const uint NC_BKD_TYPE_PEER2PEER = (uint)(0x00000001);
        public const uint NC_BKD_TYPE_REQUEST = (uint)(0x00000002);
        public const uint NC_BKD_TYPE_RESPONSE = (uint)(0x00000003);
        public const uint NC_BKD_WHEN_PERIODIC = (uint)(0x00000001);
        public const uint NC_BKD_WHEN_UNSOLICITED = (uint)(0x00000002);
        public const uint NC_BKD_CAN_ZERO_SIZE = (uint)(0x00008000);
        #endregion

        #region OTHER CONSTANTS

        public const byte NC_TRUE = 1;    
        public const byte NC_FALSE = 0;

        /* NCTYPE_DURATION (values in one millisecond ticks) */
        public const uint NC_DURATION_NONE = 0;              /* zero duration */
        public const uint NC_DURATION_INFINITE = (uint)(0xFFFFFFFF);     /* infinite duration */
        public const uint NC_DURATION_1MS = 1;              /* one millisecond */
        public const uint NC_DURATION_10MS = 10;
        public const uint NC_DURATION_100MS = 100;
        public const uint NC_DURATION_1SEC = 1000;           /* one second */
        public const uint NC_DURATION_10SEC = 10000;
        public const uint NC_DURATION_100SEC = 100000;
        public const uint NC_DURATION_1MIN = 60000;          /* one minute */

        /* NCTYPE_PROTOCOL (values for supported protocols) */
        public const uint NC_PROTOCOL_CAN = 1;              /* Controller Area Net */
        public const uint NC_PROTOCOL_DNET = 2;              /* DeviceNet */
        public const uint NC_PROTOCOL_LIN = 3;              /* LIN */


        public const uint NC_ST_READ_AVAIL = (uint)(0x00000001);
        public const uint NC_ST_WRITE_SUCCESS = (uint)(0x00000002);
        public const uint NC_ST_ESTABLISHED = (uint)(0x00000008);
        public const uint NC_ST_STOPPED = (uint)(0x00000004);
        public const uint NC_ST_ERROR = (uint)(0x00000010);
        public const uint NC_ST_WARNING = (uint)(0x00000020);
        public const uint NC_ST_READ_MULT = (uint)(0x00000008);
        public const uint NC_ST_REMOTE_WAKEUP = (uint)(0x00000040);
        public const uint NC_ST_WRITE_MULT = (uint)(0x00000080);
        public const uint NC_OP_START = (uint)(0x80000001);
        public const uint NC_OP_STOP = (uint)(0x80000002);
        public const uint NC_OP_RESET = (uint)(0x80000003);
        public const uint NC_OP_ACTIVE = (uint)(0x80000004);
        public const uint NC_OP_IDLE = (uint)(0x80000005);
        public const uint NC_OP_RTSI_OUT = (uint)(0x80000004);

        /* NCTYPE_BAUD_RATE (values for baud rates) */
        public const uint NC_BAUD_10K = 10000;
        public const uint NC_BAUD_100K = 100000;
        public const uint NC_BAUD_125K = 125000;
        public const uint NC_BAUD_250K = 250000;
        public const uint NC_BAUD_500K = 500000;
        public const uint NC_BAUD_1000K = 1000000;

        /* NCTYPE_COMM_TYPE values */
        public const uint NC_CAN_COMM_RX_UNSOL = (uint)(0x00000000);  /* rx unsolicited data */
        public const uint NC_CAN_COMM_TX_BY_CALL = (uint)(0x00000001);  /* tx data by call */
        public const uint NC_CAN_COMM_RX_PERIODIC = (uint)(0x00000002);  /* rx periodic using remote */
        public const uint NC_CAN_COMM_TX_PERIODIC = (uint)(0x00000003);  /* tx data periodically */
        public const uint NC_CAN_COMM_RX_BY_CALL = (uint)(0x00000004);  /* rx by call using remote */
        public const uint NC_CAN_COMM_TX_RESP_ONLY = (uint)(0x00000005);  /* tx by response only */
        public const uint NC_CAN_COMM_TX_WAVEFORM = (uint)(0x00000006);  /* tx periodic "waveform" */

        /* NCTYPE_RTSI_MODE values */
        public const uint NC_RTSI_NONE = 0;           /* no RTSI usage */
        public const uint NC_RTSI_TX_ON_IN = 1;           /* transmit on input pulse */
        public const uint NC_RTSI_TIME_ON_IN = 2;           /* timestamp on in pulse */
        public const uint NC_RTSI_OUT_ON_RX = 3;           /* output on receive */
        public const uint NC_RTSI_OUT_ON_TX = 4;           /* output on transmit cmpl */
        public const uint NC_RTSI_OUT_ACTION_ONLY = 5;           /* output by ncAction only */

        /* NCTYPE_RTSI_SIG_BEHAV values */
        public const uint NC_RTSISIG_PULSE = 0;           /* pulsed input / output */
        public const uint NC_RTSISIG_TOGGLE = 1;           /* toggled output */

        /* NC_ATTR_START_TRIG_BEHAVIOUR values */
        public const uint NC_START_TRIG_NONE = 0;
        public const uint NC_RESET_TIMESTAMP_ON_START = 1;
        public const uint NC_LOG_START_TRIG = 2;

        public const uint NC_FL_CAN_ARBID_XTD = (uint)(0x20000000);
        public const uint NC_CAN_ARBID_NONE = (uint)(0xCFFFFFFF);

        /* Values for the FrameType (IsRemote) field of CAN frames.  */
        public const byte NC_FRMTYPE_DATA = (byte)(0x00);
        public const byte NC_FRMTYPE_REMOTE = (byte)(0x01);
        /* NI only */
        public const byte NC_FRMTYPE_COMM_ERR = (byte)(0x02);     // Communication warning/error (NC_ATTR_LOG_COMM_ERRS)
        public const byte NC_FRMTYPE_RTSI = (byte)(0x03);     // RTSI pulse (NC_ATTR_RTSI_MODE=NC_RTSI_TIME_ON_IN)

        public const byte NC_FRMTYPE_TRIG_START = (byte)(0x04);
        public const byte NC_FRMTYPE_DELAY = (byte)(0x05);    // Adds a delay between 2 timestamped frames.
        public const byte NC_FRMTYPE_BUS_ERR = (byte)(0x06);
        public const byte NC_FRMTYPE_TRANSCEIVER_ERR = (byte)(0x07);


        /* Special values for CAN mask attributes (NC_ATTR_MASK_STD/XTD) */
        public const uint NC_MASK_STD_MUSTMATCH = (uint)(0x000007FF);
        public const uint NC_MASK_XTD_MUSTMATCH = (uint)(0x1FFFFFFF);
        public const uint NC_MASK_STD_DONTCARE = (uint)(0x00000000);     // recommended for Series 2
        public const uint NC_MASK_XTD_DONTCARE = (uint)(0x00000000);     // recommended for Series 2
        public const uint NC_SERIES2_MASK_MUSTMATCH = (uint)(0x00000000);
        public const uint NC_SERIES2_MASK_DONTCARE = (uint)(0xFFFFFFFF);

        // Values for NC_ATTR_HW_SERIES attribute
        public const uint NC_HW_SERIES_1 = 0;        // Intel 82527 CAN chip, legacy RTSI
        public const uint NC_HW_SERIES_2 = 1;        // Phillips SJA1000 CAN chip, updated RTSI
        public const uint NC_HW_SERIES_847X = 2;        // Low-cost USB without sync
        public const uint NC_HW_SERIES_847X_SYNC = 3;        // Low-cost USB with sync
        public const uint NC_HW_SERIES_NIXNET = 4;        // XNET series hardware, for compatibility

        // Values for SourceTerminal of ncConnectTerminals.
        public const uint NC_SRC_TERM_RTSI0 = 0;
        public const uint NC_SRC_TERM_RTSI1 = 1;
        public const uint NC_SRC_TERM_RTSI2 = 2;
        public const uint NC_SRC_TERM_RTSI3 = 3;
        public const uint NC_SRC_TERM_RTSI4 = 4;
        public const uint NC_SRC_TERM_RTSI5 = 5;
        public const uint NC_SRC_TERM_RTSI6 = 6;
        public const uint NC_SRC_TERM_RTSI_CLOCK = 7;
        public const uint NC_SRC_TERM_PXI_STAR = 8;
        public const uint NC_SRC_TERM_INTF_RECEIVE_EVENT = 9;
        public const uint NC_SRC_TERM_INTF_TRANSCEIVER_EVENT = 10;
        public const uint NC_SRC_TERM_PXI_CLK10 = 11;
        public const uint NC_SRC_TERM_20MHZ_TIMEBASE = 12;
        public const uint NC_SRC_TERM_10HZ_RESYNC_CLOCK = 13;
        public const uint NC_SRC_TERM_START_TRIGGER = 14;

        // Values for DestinationTerminal of ncConnectTerminals.
        public const uint NC_DEST_TERM_RTSI0 = 0;
        public const uint NC_DEST_TERM_RTSI1 = 1;
        public const uint NC_DEST_TERM_RTSI2 = 2;
        public const uint NC_DEST_TERM_RTSI3 = 3;
        public const uint NC_DEST_TERM_RTSI4 = 4;
        public const uint NC_DEST_TERM_RTSI5 = 5;
        public const uint NC_DEST_TERM_RTSI6 = 6;
        public const uint NC_DEST_TERM_RTSI_CLOCK = 7;
        public const uint NC_DEST_TERM_MASTER_TIMEBASE = 8;
        public const uint NC_DEST_TERM_10HZ_RESYNC_CLOCK = 9;
        public const uint NC_DEST_TERM_START_TRIGGER = 10;

        // Values for NC_ATTR_HW_FORMFACTOR attribute
        public const uint NC_HW_FORMFACTOR_PCI = 0;
        public const uint NC_HW_FORMFACTOR_PXI = 1;
        public const uint NC_HW_FORMFACTOR_PCMCIA = 2;
        public const uint NC_HW_FORMFACTOR_AT = 3;
        public const uint NC_HW_FORMFACTOR_USB = 4;

        // Values for NC_ATTR_TRANSCEIVER_TYPE attribute
        public const byte NC_TRANSCEIVER_TYPE_HS = 0;  // High-Speed
        public const byte NC_TRANSCEIVER_TYPE_LS = 1;  // Low-Speed / Fault-Tolerant
        public const byte NC_TRANSCEIVER_TYPE_SW = 2;  // Single-Wire
        public const byte NC_TRANSCEIVER_TYPE_EXT = 3;  // External connection
        public const byte NC_TRANSCEIVER_TYPE_DISC = 4;  // Disconnected
        public const byte NC_TRANSCEIVER_TYPE_LIN = 5;  // LIN
        public const byte NC_TRANSCEIVER_TYPE_UNKNOWN = (byte)(0xFF);  // Unknown (Get for Series 1 PCMCIA HS dongle)

        // Values for legacy NC_ATTR_HW_TRANSCEIVER attribute
        public const byte NC_HW_TRANSCEIVER_HS = NC_TRANSCEIVER_TYPE_HS;
        public const byte NC_HW_TRANSCEIVER_LS = NC_TRANSCEIVER_TYPE_LS;
        public const byte NC_HW_TRANSCEIVER_SW = NC_TRANSCEIVER_TYPE_SW;
        public const byte NC_HW_TRANSCEIVER_EXT = NC_TRANSCEIVER_TYPE_EXT;
        public const byte NC_HW_TRANSCEIVER_DISC = NC_TRANSCEIVER_TYPE_DISC;

        // Values for NC_ATTR_TRANSCEIVER_MODE attribute.
        public const uint NC_TRANSCEIVER_MODE_NORMAL = 0;
        public const uint NC_TRANSCEIVER_MODE_SLEEP = 1;
        public const uint NC_TRANSCEIVER_MODE_SW_WAKEUP = 2;  // Single-Wire Wakeup
        public const uint NC_TRANSCEIVER_MODE_SW_HIGHSPEED = 3;  // Single-Wire High Speed

        // Values for NC_ATTR_BEHAV_FINAL_OUT attribute (CAN Objs of type NC_CAN_COMM_TX_PERIODIC)
        public const uint NC_OUT_BEHAV_REPEAT_FINAL = 0;
        public const uint NC_OUT_BEHAV_CEASE_TRANSMIT = 1;

        // Values for NC_ATTR_SERIES2_FILTER_MODE
        public const uint NC_FILTER_SINGLE_STANDARD = 0;
        public const uint NC_FILTER_SINGLE_EXTENDED = 1;
        public const uint NC_FILTER_DUAL_STANDARD = 2;
        public const uint NC_FILTER_DUAL_EXTENDED = 3;

        // Values for SourceTerminal of ncConnectTerminals.
        public const uint NC_SRC_TERM_10HZ_RESYNC_EVENT = NC_SRC_TERM_10HZ_RESYNC_CLOCK;
        public const uint NC_SRC_TERM_START_TRIG_EVENT = NC_SRC_TERM_START_TRIGGER;
        // Values for DestinationTerminal of ncConnectTerminals.
        public const uint NC_DEST_TERM_10HZ_RESYNC = NC_DEST_TERM_10HZ_RESYNC_CLOCK;
        public const uint NC_DEST_TERM_START_TRIG = NC_DEST_TERM_START_TRIGGER;
        /* NCTYPE_VERSION (NC_ATTR_SOFTWARE_VERSION); ncGetHardwareInfo preferable */
        public const uint NC_MK_VER_MAJOR = (uint)(0xFF000000);
        public const uint NC_MK_VER_MINOR = (uint)(0x00FF0000);
        public const uint NC_MK_VER_SUBMINOR = (uint)(0x0000FF00);
        public const uint NC_MK_VER_BETA = (uint)(0x000000FF);
        /* ArbitrationId; use IsRemote or FrameType to determine RTSI frame. */
        public const uint NC_FL_CAN_ARBID_INFO = (uint)(0x40000000);
        public const uint NC_ARBID_INFO_RTSI_INPUT = (uint)(0x00000001);
        /* NC_ATTR_STD_MASK and NC_ATTR_XTD_MASK */
        public const uint NC_CAN_MASK_STD_MUSTMATCH = NC_MASK_STD_MUSTMATCH;
        public const uint NC_CAN_MASK_XTD_MUSTMATCH = NC_MASK_XTD_MUSTMATCH;
        public const uint NC_CAN_MASK_STD_DONTCARE = NC_MASK_STD_DONTCARE;
        public const uint NC_CAN_MASK_XTD_DONTCARE = NC_MASK_XTD_DONTCARE;

        /* Values for NC_ATTR_TRANSMIT_MODE(Immediate or timestamped).*/
        public const uint NC_TX_MODE_IMMEDIATE = 0;
        public const uint NC_TX_MODE_TIMESTAMPED = 1;

        /* Values for NC_ATTR_TIMESTAMP_FORMAT.*/
        public const uint NC_TIME_FORMAT_ABSOLUTE = 0;
        public const uint NC_TIME_FORMAT_RELATIVE = 1;
        #endregion
    }
}
