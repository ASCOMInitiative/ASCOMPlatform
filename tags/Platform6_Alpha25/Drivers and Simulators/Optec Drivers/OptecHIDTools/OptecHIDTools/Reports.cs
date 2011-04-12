using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace OptecHIDTools
{
    [Guid("854925A1-5525-4fe0-8C9C-1D1AED227F4D")]
    public class FeatureReport
    {
        private short _ReportID = 0;

        private Byte[] _Response1 = null;
        private Byte[] _Response2 = null;

        private Byte[] _DataToSend = null;

        public FeatureReport(short reportID, Byte[] dataToSend)
        {
            _ReportID = reportID;
            _DataToSend = dataToSend;
            if ((dataToSend == null) || (reportID == 0))
            {
                throw new InvalidOperationException(
                    "A Feature Report must have a repordID and dataToSend.");
            }
        }

        public Byte[] Response1
        {
            get { return _Response1; }
            internal set { _Response1 = value; }
        }

        public Byte[] Response2
        {
            get { return _Response2; }
            internal set { _Response2 = value; }
        }

        public byte[] DataToSend
        {
            get
            {
                return _DataToSend;
            }
            set
            {
                _DataToSend = value;
            }
        }

        public short ReportID
        {
            get
            {
                return _ReportID;
            }
            set
            {
                _ReportID = value;
            }
        }   

    }
    [Guid("7E734059-D4B0-40a3-A864-823F0E0BEE0D")]
    public class OutputReport
    {
        private byte[] _DataToSend;
        private short _ReportID = 0;

        public OutputReport(short reportID, byte[] dataToSend)
        {
            DataToSend = dataToSend;
            ReportID = reportID;
        }

        public byte[] DataToSend
        {
            get
            {
                return _DataToSend;
            }
            set
            {
                _DataToSend = value;
            }
        }

        public short ReportID
        {
            get
            {
                return _ReportID;
            }
            set
            {
                _ReportID = value;
            }
        }
    }
    [Guid("21300856-1B26-4f94-98A2-8ECB45184CE3")]
    public class InputReport
    {
        private short _ReportID;
        private byte[] receivedData;

        public InputReport(short reportID)
        {
            _ReportID = reportID;
        }

        public short ReportID
        {
            get { return _ReportID; }
            set { _ReportID = value; }
        }

        public byte[] ReceivedData
        {
            get { return receivedData; }
            internal set { receivedData = value; }
        }


    }
}
