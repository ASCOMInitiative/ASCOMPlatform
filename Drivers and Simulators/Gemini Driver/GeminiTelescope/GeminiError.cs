using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.GeminiTelescope
{
    static class GeminiError
    {
        static ASCOM.Utilities.Serial m_Ser;

        static GeminiError()
        {
            m_Ser = new ASCOM.Utilities.Serial();
        }

        public static void LogSerialError(string from, string s)
        {
            m_Ser.LogMessage(from, s);
            
            string sF = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            System.Diagnostics.Trace.TraceError(sF + "\t" + from + "\t" + s);
        }

        public static void LogSerialTrace(string from, string s)
        {
            m_Ser.LogMessage(from, s);
            string sF = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            System.Diagnostics.Trace.TraceError(sF + "\t" + from + "\t" + s);
        }

        public static void UserErrorReport(string from, string s, bool ResponseRequired)
        {
            string sF = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            m_Ser.LogMessage(from, s);
            System.Diagnostics.Trace.TraceError(sF + "\t" + from + "\t" + s);
            if (ResponseRequired)
            {
                System.Windows.Forms.MessageBox.Show(s, from, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
            }
        }

        public  static void UserWarningReport(string from, string s)
        {
            string sF = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            System.Diagnostics.Trace.TraceError(sF + "\t" + from + "\t" + s);
        }

        public static void UserInfoReport(string from, string s)
        {
            string sF = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            System.Diagnostics.Trace.TraceError(sF + "\t" + from + "\t" + s);
        }
    
    }

}
