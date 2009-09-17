//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Telescope Hardware
//
// Description:	This implements a simulated Telescope Hardware
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Telescope Driver template
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Timers;

namespace ASCOM.GeminiTelescope
{
    delegate void SystemMessageDelegate(string message);
    public class TelescopeHardware
    {
        
        private static HelperNET.Profile m_Profile;

        private static Queue m_CommandQueue; //Queue used for messages to the gemini
        private static System.ComponentModel.BackgroundWorker m_BackgroundWorker; // Thread to run for communications
      
        //Telescope Implementation
        
        private static double m_Latitude;
        private static double m_Longitude;
        private static double m_Elevation;
        
        private static double m_RightAscension;
        private static double m_Declination;


        private static bool m_Tracking;

        private static bool m_AtPark;

        private static bool m_SouthernHemisphere = false;

        private static string m_ComPort;


        private static System.IO.Ports.SerialPort m_SerialPort;

        private static bool m_Connected = false; //Keep track of the connection status of the hardware
        private static int m_Clients;

        private static DateTime m_LastUpdate;

        static TelescopeHardware()
        {
            m_Profile = new HelperNET.Profile();
            m_SerialPort = new System.IO.Ports.SerialPort();

            m_BackgroundWorker = new BackgroundWorker();
            m_BackgroundWorker.WorkerSupportsCancellation = true;
            m_BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            m_CommandQueue = new Queue();
            m_Clients = 0;

            if (m_Profile.GetValue(SharedResources.PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                //Main Driver Settings
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ComPort", "COM1");


                
            }

            //Load up the values from saved
            m_ComPort = m_Profile.GetValue(SharedResources.PROGRAM_ID, "ComPort", "");

            if (m_ComPort != "")
            {
                m_SerialPort.PortName = m_ComPort;
            }
            m_SerialPort.BaudRate = 9600;
            m_SerialPort.Parity = System.IO.Ports.Parity.None;
            m_SerialPort.DataBits = 8;
            m_SerialPort.StopBits = System.IO.Ports.StopBits.One;
            

        }
        public static void Start() 
        {
            m_Connected = false;
           


        }

 

        #region Properties For Settings

        public static string ComPort
        {
            get { return m_ComPort; }
            set 
            {
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ComPort", value.ToString());
                m_ComPort = value; 
            }
        }
       
        public static double Elevation
        {
            get { return m_Elevation; }
            set
            {
                m_Elevation = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Elevation", value.ToString());
            }
        }
        public static double Latitude
        {
            get { return m_Latitude; }
            set
            {
                m_Latitude = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Latitude", value.ToString());
                if (m_Latitude < 0) { m_SouthernHemisphere = true; }
            }
        }
        public static double Longitude
        {
            get { return m_Longitude; }
            set
            {
                m_Longitude = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Longitude", value.ToString());
            }
        }
       

        #endregion

        #region Telescope Implementation
        private static void Connect()
        {
            m_Clients += 1;
            if (m_SerialPort.IsOpen == false)
            {
                m_SerialPort.PortName = m_ComPort;
                m_SerialPort.BaudRate = 38400;
                m_SerialPort.Parity = System.IO.Ports.Parity.None;
                m_SerialPort.DataBits = 8;
                m_SerialPort.StopBits = System.IO.Ports.StopBits.One;
                try
                {
                    m_SerialPort.Open();
                }
                catch
                {
                    m_Connected = false;
                    return;
                }


                m_BackgroundWorker.RunWorkerAsync();

            }
            m_Connected = true;
        }
        private static void Disconnect()
        {
            m_Clients -= 1;
            if (m_Clients == 0)
            {
                m_BackgroundWorker.CancelAsync();
                m_SerialPort.Close();
                m_Connected = false;
            }
        }
        private static void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            SystemMessageDelegate message = new SystemMessageDelegate(ProcessMessage);

            while (!worker.CancellationPending)
            {
                try
                {

                    if (m_CommandQueue.Count > 0)
                    {
                        string command = (string)m_CommandQueue.Dequeue();

                        //Send Command to mount

                    }
                    else
                    {
                        //Process status updates
                        //Get RA and DEC etc
                    }

                }
                catch
                {

                }
            }
        }
        private static void ProcessMessage(string message)
        {
        }
        public static bool Connected
        {
            get
            { return m_Connected; }
            set
            { m_Connected = value; }
        }

       public static bool SouthernHemisphere
       { get { return m_SouthernHemisphere; } }

        public static double RightAscension
       { get { return m_RightAscension; } }

        public static double Declination
        { get { return m_Declination; } }
        #endregion

        #region Helper Functions

        #endregion

    }
}
