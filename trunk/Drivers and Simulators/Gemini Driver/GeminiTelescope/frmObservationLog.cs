//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Observation Log form
//
// Description:	This implements download and saving of Gemini observation log
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 08-OCT-2009  pk  1.0.0   Initial Implementation
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class frmObservationLog : Form
    {
        List<Observation> m_Observations = new List<Observation>();
        string m_OrderBy = "Time";
        Dictionary<String, Func<Observation, IComparable>> CList = new Dictionary<String, Func<Observation, IComparable>>();
        Dictionary<string, bool> m_DirectionSort = new Dictionary<string, bool>();

        public frmObservationLog()
        {
            InitializeComponent();
            CList.Add("Time", o => o.Time);
            CList.Add("Operation", o => o.Operation);
            CList.Add("RA", o => o.RA);
            CList.Add("DEC", o => o.DEC);
            CList.Add("Object", o => o.Object);
            m_DirectionSort.Add("Time", false);
            m_DirectionSort.Add("Operation", false);
            m_DirectionSort.Add("RA", false);
            m_DirectionSort.Add("DEC", false);
            m_DirectionSort.Add("Object", false);
        }

        private void UpdateList()
        {
            var qry = (from o in m_Observations orderby o.Time select o);
            if (m_DirectionSort[m_OrderBy])
                qry = qry.OrderByDescending(CList[m_OrderBy]).ThenBy(CList["Time"]);
            else
                qry = qry.OrderBy(CList[m_OrderBy]).ThenBy(CList["Time"]);

            List<Observation> list = qry.ToList();
            BindingSource bs = new BindingSource();
            bs.DataSource = qry;
            try
            {
                gvLog.DataSource = bs;
                gvLog.Columns["Time"].DefaultCellStyle.Format = "G";
                gvLog.Columns["RA"].DefaultCellStyle.Format = "0.00";
                gvLog.Columns["DEC"].DefaultCellStyle.Format = "0.00";
                gvLog.Columns["RA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                gvLog.Columns["DEC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                gvLog.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gvLog.Columns["Time"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch { }
        }

        private void pbFromGemini_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                List<string> allobs = GeminiHardware.ObservationLog;
                if (allobs != null)
                {
                    m_Observations.Clear();
                    foreach (string s in allobs)
                    {
                        Observation obs = null;
                        if (Observation.TryParse(s, out obs))
                        {
                            m_Observations.Add(obs);
                        }
                    }
                }

                UpdateList();
            }
        }

        private void pbToGemini_Click(object sender, EventArgs e)
        {
            GeminiHardware.ObservationLog = null;
            m_Observations.Clear();
            UpdateList();
        }

        private void gvLog_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = gvLog.Columns[e.ColumnIndex].HeaderText;
            if (col == m_OrderBy)
            {
                m_DirectionSort[col] = !m_DirectionSort[col];
            }
            m_OrderBy = col;
            UpdateList();
        }

        private void frmObservationLog_Load(object sender, EventArgs e)
        {
            SetButtonState();
            GeminiHardware.OnConnect += new ConnectDelegate(OnConnect);
            if (GeminiHardware.Connected) pbFromGemini_Click(pbFromGemini, null);
        }


        void SetButtonState()
        {
            pbToGemini.Enabled = GeminiHardware.Connected;
            pbFromGemini.Enabled = GeminiHardware.Connected;
            pbToGemini.BackColor = GeminiHardware.Connected ? Color.FromArgb(16,16,16): Color.FromArgb(64,64,64);
            pbFromGemini.BackColor = GeminiHardware.Connected ? Color.FromArgb(16,16,16): Color.FromArgb(64, 64, 64);

          
        }

        void OnConnect(bool bConnect, int clients)
        {
            SetButtonState();
        }

        private void pbExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void pbToFile_Click(object sender, EventArgs e)
        {
            var qry = (from o in m_Observations select o);
            if (m_DirectionSort[m_OrderBy])
                qry = qry.OrderByDescending(CList[m_OrderBy]).ThenBy(CList["Time"]);
            else
                qry = qry.OrderBy(CList[m_OrderBy]).ThenBy(CList["Time"]);

            List<Observation> list = qry.ToList();

            string path = "";
            try
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME;
                System.IO.Directory.CreateDirectory(path);
                path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Observation Logs";
                System.IO.Directory.CreateDirectory(path);
            }
            catch
            {
            }

            path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Observation Logs";
            saveFileDialog1.InitialDirectory = path;
            string fn = "Observation ";
            if (m_Observations.Count > 0)
            {
                fn += m_Observations[0].Time.ToString("yyyy-MM-dd-HHmmss");
            }
            else
                fn += DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
         
            saveFileDialog1.FileName = fn;

            DialogResult res = saveFileDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamWriter fi = System.IO.File.CreateText(saveFileDialog1.FileName);
                    fi.WriteLine(string.Format("{0,-30} {1,-15} {2,15}{3,15}  {4,-20}", "Time", "Operation", "RA", "DEC", "Object"));
                    foreach (Observation obj in list)
                    {
                        fi.WriteLine(obj.ToString());
                    }
                    fi.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing file: " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }    
    }

    public class RACoord : IComparable
    {
        public double RA { get; set; }
        public RACoord(double ra)
        {
            RA = ra;
        }
        public override string ToString()
        {
            return GeminiHardware.m_Util.HoursToHMS(RA);
        }

        public string ToString(string s1, string s2)
        {
            return GeminiHardware.m_Util.HoursToHMS(RA, s1, s2);
        }

        int IComparable.CompareTo(Object obj)
        {
            if (this.RA > ((RACoord)obj).RA) return 1;
            if (this.RA < ((RACoord)obj).RA) return -1;
            return 0;
        }

    }

    public class DECCoord : IComparable
    {
        public double DEC { get; set; }
        public DECCoord(double dec)
        {
            DEC = dec;
        }
        public override string ToString()
        {
            return GeminiHardware.m_Util.DegreesToDMS(DEC);
        }
        public  string ToString(string s1, string s2)
        {
            return GeminiHardware.m_Util.DegreesToDMS(DEC,s1,s2);
        }

        int IComparable.CompareTo(Object obj)
        {
            if (this.DEC > ((DECCoord)obj).DEC) return 1;
            if (this.DEC < ((DECCoord)obj).DEC) return -1;
            return 0;
        }
    }

    public class Observation
    {
        public DateTime Time { get; set; }
        public string Operation {get; set; }
        public RACoord RA {get; set; }
        public DECCoord DEC {get; set; }
        public string Object {get; set; }

        private static Dictionary<string, string> m_Ops = new Dictionary<string, string>();

        static Observation()
        {
            m_Ops.Add("A", "Additional Align");
            m_Ops.Add("C", "Cold Start");
            m_Ops.Add("F", "Meridian Flip");
            m_Ops.Add("D", "First Align");
            m_Ops.Add("G", "Goto");
            m_Ops.Add("H", "Alt/Az object selected");
            m_Ops.Add("O", "Power off");
            m_Ops.Add("R", "Warm Restart");
            m_Ops.Add("s", "Synchronize");
            m_Ops.Add("S", "Object selected");
            m_Ops.Add("W", "Warm Start");
        }

        public override string ToString()
        {
            return string.Format("{0,-30:G} {1,-15} {2,15}{3,15}  {4,-20}", Time, Operation, RA.ToString(), DEC.ToString(), Object);
        }

        public static bool TryParse(string line, out Observation obs)
        {
           try
           {
               obs = new Observation();
               int y, mo, d, h, m, s;
               y = int.Parse(line.Substring(0,2))+2000;
               mo = int.Parse(line.Substring(2, 2));
               d = int.Parse(line.Substring(4, 2));
               h = int.Parse(line.Substring(6, 2));
               m = int.Parse(line.Substring(8, 2));
               s = int.Parse(line.Substring(10, 2));

               obs.Time = new DateTime(y, mo, d, h, m, s, 0, DateTimeKind.Utc);

               obs.Operation = m_Ops[line.Substring(12, 1)];
               obs.RA = new RACoord(GeminiHardware.m_Util.HMSToHours(line.Substring(13, 8)));
               obs.DEC = new DECCoord(GeminiHardware.m_Util.DMSToDegrees(line.Substring(21, 9)));
               if (line.Length > 30)
               {
                   obs.Object = line.Substring(30);
                   if (obs.Object.EndsWith("#")) obs.Object = obs.Object.Remove(obs.Object.Length - 1);
               }
               else
                   obs.Object = "";
               return true;
           }
           catch
           {
               obs = null;
               return false;
           }
        }
    }
}
