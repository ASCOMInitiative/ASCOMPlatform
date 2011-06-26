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
using ASCOM.GeminiTelescope.Properties;

namespace ASCOM.GeminiTelescope
{
    public partial class frmObservationLog : Form
    {
        List<Observation> m_Observations = new List<Observation>();
        string m_OrderBy = Resources.Time;
        Dictionary<String, Func<Observation, IComparable>> CList = new Dictionary<String, Func<Observation, IComparable>>();
        Dictionary<string, bool> m_DirectionSort = new Dictionary<string, bool>();

        public frmObservationLog()
        {
            InitializeComponent();
            CList.Add(Resources.Time, o => o.Time);
            CList.Add(Resources.Operation, o => o.Operation);
            CList.Add(Resources.RA, o => o.RA);
            CList.Add(Resources.DEC, o => o.DEC);
            CList.Add(Resources.Object, o => o.Object);
            m_DirectionSort.Add(Resources.Time, false);
            m_DirectionSort.Add(Resources.Operation, false);
            m_DirectionSort.Add(Resources.RA, false);
            m_DirectionSort.Add(Resources.DEC, false);
            m_DirectionSort.Add(Resources.Object, false);
        }

        private void UpdateList()
        {
            var qry = (from o in m_Observations orderby o.Time select o);
            if (m_DirectionSort[m_OrderBy])
                qry = qry.OrderByDescending(CList[m_OrderBy]).ThenBy(CList[Resources.Time]);
            else
                qry = qry.OrderBy(CList[m_OrderBy]).ThenBy(CList[Resources.Time]);

            List<Observation> list = qry.ToList();
            BindingSource bs = new BindingSource();
            bs.DataSource = qry;
            try
            {
                gvLog.DataSource = bs;
                gvLog.Columns[Resources.Time].DefaultCellStyle.Format = "G";
                gvLog.Columns[Resources.RA].DefaultCellStyle.Format = "0.00";
                gvLog.Columns[Resources.DEC].DefaultCellStyle.Format = "0.00";
                gvLog.Columns[Resources.RA].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                gvLog.Columns[Resources.DEC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                gvLog.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gvLog.Columns[Resources.Time].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch { }
        }

        private void pbFromGemini_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Instance.Connected)
            {
                frmProgress.Initialize(0, 100, Resources.RetrieveGeminiObservationLog, null, true);
                frmProgress.ShowProgress(this);

                List<string> allobs = GeminiHardware.Instance.ObservationLog;
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
                frmProgress.HideProgress();
            }
        }

        private void pbToGemini_Click(object sender, EventArgs e)
        {
            GeminiHardware.Instance.ObservationLog = null;
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
            GeminiHardware.Instance.OnConnect += new ConnectDelegate(OnConnect);
            ObsTime.UTC = chkUTC.Checked;
            pbFromGemini_Click(pbFromGemini, null);
        }


        void SetButtonState()
        {
            pbToGemini.Enabled = GeminiHardware.Instance.Connected;
            pbFromGemini.Enabled = GeminiHardware.Instance.Connected;
            btnGoto.Enabled = (GeminiHardware.Instance.Connected && gvLog.SelectedRows.Count == 1);

            pbToGemini.BackColor = pbToGemini.Enabled? Color.FromArgb(16,16,16): Color.FromArgb(64,64,64);
            pbFromGemini.BackColor = pbFromGemini.Enabled? Color.FromArgb(16,16,16): Color.FromArgb(64, 64, 64);
            btnGoto.BackColor = btnGoto.Enabled? Color.FromArgb(16,16,16): Color.FromArgb(64, 64, 64);          
        }

        void OnConnect(bool bConnect, int clients)
        {
            SetButtonState();
        }

        private void pbExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pbToFile_Click(object sender, EventArgs e)
        {
            var qry = (from o in m_Observations select o);
            if (m_DirectionSort[m_OrderBy])
                qry = qry.OrderByDescending(CList[m_OrderBy]).ThenBy(CList[Resources.Time]);
            else
                qry = qry.OrderBy(CList[m_OrderBy]).ThenBy(CList[Resources.Time]);

            List<Observation> list = qry.ToList();

            string path = "";
            try
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME;
                System.IO.Directory.CreateDirectory(path);
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Observation Logs";
                System.IO.Directory.CreateDirectory(path);
            }
            catch
            {
            }

            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Observation Logs";
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
                    MessageBox.Show(Resources.ErrorWritingFile + " " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chkUTC_CheckedChanged(object sender, EventArgs e)
        {
            ObsTime.UTC = chkUTC.Checked;
            UpdateList();
        }

        private void btnGoto_Click(object sender, EventArgs e)
        {
            if (gvLog.SelectedRows.Count < 1 || gvLog.SelectedRows.Count > 1)
            {
                MessageBox.Show(Resources.PleaseSelectOne, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Observation obs = gvLog.SelectedRows[0].DataBoundItem as Observation;

            double ra, dec;

            obs.GetCoords(out ra, out dec);

            GeminiHardware.Instance.TargetRightAscension = ra;
            GeminiHardware.Instance.TargetDeclination = dec;
            GeminiHardware.Instance.TargetName = obs.Object;

            try
            {
                if (sender == btnGoto)
                    GeminiHardware.Instance.SlewEquatorial();
#if false
                else if (sender == btnSync)
                    GeminiHardware.Instance.SyncEquatorial();
                else
                    GeminiHardware.Instance.AlignEquatorial();
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, Resources.GotoFailed + " " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pbSendtObject_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvLog.SelectedRows.Count == 1)
                {
                    string id = gvLog.SelectedRows[0].Cells["Object"].Value.ToString().ToLower();

                    if (!string.IsNullOrEmpty(id ))
                    {
                        string[] ids = id.Split(new char [] { ' ' });

                        // strip off the catalog name first:
                        id = ids[0].TrimStart("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());

                        // if there's a '-' (as in Sh2-xxx) then remove everything before and including the dash
                        int i = id.IndexOf('-');
                        if (i > 0) id = id.Substring(i + 1);

                        string cat = ids[0].Substring(0, ids[0].Length - id.Length);    // first part is the catalog
                        int catnbr = 0;
                        // need a way to extract catalog number from object name... don't know how to do that....
                        string cmd = string.Format(":OI{0}{1}", catnbr, id);
                        GeminiHardware.Instance.DoCommandResult(cmd, GeminiHardware.Instance.MAX_TIMEOUT, false);
                    }
                }
            }
            catch { }

        }

        private void gvLog_SelectionChanged(object sender, EventArgs e)
        {
            SetButtonState();
        }

        private void pbFromFile_Click(object sender, EventArgs e)
        {
            string path;

            try
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME;
                System.IO.Directory.CreateDirectory(path);
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Observation Logs";
                System.IO.Directory.CreateDirectory(path);
            }
            catch
            {
            }

            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Observation Logs";
            openFileDialog1.InitialDirectory = path;

            DialogResult res = openFileDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                try
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(openFileDialog1.FileName);
                    m_Observations.Clear();
                    if (LoadObservations(fi.FullName, m_Observations))
                    {
                        UpdateList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.ErrorReadingFile + " " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool LoadObservations(string p, List<Observation> m_Observations)
        {
            System.IO.StreamReader fi = null;

            try
            {
                fi = System.IO.File.OpenText(p);
                while (!fi.EndOfStream)
                {
                    string newl = fi.ReadLine();
                    newl = newl.Trim();
                    if (newl.Length > 0)
                    {
                        Observation obj = null;
                        if (Observation.TryParseFixed(newl, out obj))
                            m_Observations.Add(obj);
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fi != null) fi.Close();
            }

            return true;
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
            return GeminiHardware.Instance.m_Util.HoursToHMS(RA);
        }

        public string ToString(string s1, string s2)
        {
            return GeminiHardware.Instance.m_Util.HoursToHMS(RA, s1, s2);
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
            return GeminiHardware.Instance.m_Util.DegreesToDMS(DEC);
        }
        public  string ToString(string s1, string s2)
        {
            return GeminiHardware.Instance.m_Util.DegreesToDMS(DEC,s1,s2);
        }

        int IComparable.CompareTo(Object obj)
        {
            if (this.DEC > ((DECCoord)obj).DEC) return 1;
            if (this.DEC < ((DECCoord)obj).DEC) return -1;
            return 0;
        }
    }

    public class ObsTime : IComparable
    {
        public static bool UTC = true;

        public DateTime Time { get; set; }
        public ObsTime(DateTime tm)
        {
            Time = tm;
        }

        public override string ToString()
        {
            return ToString("G");
        }

        public string ToString(string format)
        {
            if (UTC)
                return Time.ToString(format);
            else
                return Time.ToLocalTime().ToString(format);
        }
        int IComparable.CompareTo(Object obj)
        {
            if (this.Time > ((ObsTime)obj).Time) return 1;
            if (this.Time < ((ObsTime)obj).Time) return -1;
            return 0;
        }

    }

    public class Observation
    {
        public ObsTime Time { get; set; }
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

               obs.Time = new ObsTime( new DateTime(y, mo, d, h, m, s, 0, DateTimeKind.Utc));

               obs.Operation = m_Ops[line.Substring(12, 1)];
               obs.RA = new RACoord(GeminiHardware.Instance.m_Util.HMSToHours(line.Substring(13, 8)));
               obs.DEC = new DECCoord(GeminiHardware.Instance.m_Util.DMSToDegrees(line.Substring(21, 9)));
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

        // return object coordinates
        // precessed and refraction-adjusted, as needed based on
        // current Gemini settings. DEC and RA are stored in J2000 coordinates:
        internal void GetCoords(out double ra, out double dec)
        {
            // observations are already in the correct coordinate system for Gemini, no
            // need to adjust:
            ra = RA.RA;
            dec = DEC.DEC;
        }


        /// <summary>
        /// parse a text line generated by this observation log viewer, usually from a file
        /// </summary>
        internal static bool TryParseFixed(string line, out Observation obs)
        {
            try
            {
                obs = new Observation();
                string date = line.Substring(0,30);
                date = date.Trim();
                DateTime tm;
                
                tm = DateTime.Parse(date, new System.Globalization.DateTimeFormatInfo(), System.Globalization.DateTimeStyles.NoCurrentDateDefault) ;
                obs.Time = new ObsTime(tm);

                string op = line.Substring(31, 15);
                op = op.Trim();
                obs.Operation = op;

                string ra = line.Substring(47,15);
                string dec= line.Substring(47+15,15);
                ra = ra.Trim();
                dec = dec.Trim();
                obs.RA = new RACoord(GeminiHardware.Instance.m_Util.HMSToHours(ra));
                obs.DEC = new DECCoord(GeminiHardware.Instance.m_Util.DMSToDegrees(dec));

                string obj = "";
                if (line.Length > 47+30) obj = line.Substring(47+30+1);
                obj = obj.Trim();
                obs.Object = obj;
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
