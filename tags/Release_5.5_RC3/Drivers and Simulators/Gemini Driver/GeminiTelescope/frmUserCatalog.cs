//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Catalog Management form
//
// Description:	This implements editing and upload/download of user-defined catalogs to Gemini
//
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 05-OCT-2009  pk  1.0.0   Initial Implementation
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
    public partial class frmUserCatalog : Form
    {
        SerializableDictionary<string, CatalogObject> m_Objects;
        SerializableDictionary<string, CatalogObject> m_GeminiObjects;

        string m_OrderBy = "Name";
        string m_GeminiOrderBy = "Name";

        string m_Search = "";

        Dictionary<string, bool> m_DirectionSort = new Dictionary<string, bool>();
        Dictionary<string, bool> m_GeminiDirectionSort = new Dictionary<string, bool>();

        Dictionary<String, Func<CatalogObject, IComparable>> CList = new Dictionary<String, Func<CatalogObject, IComparable>>();


        public frmUserCatalog()
        {
            InitializeComponent();
            m_DirectionSort.Add("Name", false);
            m_DirectionSort.Add("Catalog", false);
            m_DirectionSort.Add("RA", false);
            m_DirectionSort.Add("DEC", false);
            m_GeminiDirectionSort.Add("Name", false);
            m_GeminiDirectionSort.Add("Catalog", false);
            m_GeminiDirectionSort.Add("RA", false);
            m_GeminiDirectionSort.Add("DEC", false);
            m_GeminiObjects = new SerializableDictionary<string, CatalogObject>();
            CList.Add("Name", o => o.Name);
            CList.Add("Catalog", o => o.Catalog);
            CList.Add("RA", o => o.RA);
            CList.Add("DEC", o => o.DEC);
        }

        private void PopulateCatalogs()
        {
            string path = "";
            try
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME;
                System.IO.Directory.CreateDirectory(path);
                path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Catalogs";
                System.IO.Directory.CreateDirectory(path);
            }
            catch
            {
            }

            path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Catalogs";

            m_Objects = new SerializableDictionary<string, CatalogObject>();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            System.IO.FileInfo[] files = di.GetFiles("*.guc");

            GeminiHardware.m_Profile.DeviceType = "Telescope";
            foreach (System.IO.FileInfo fi in files)
            {
                string cn = fi.Name.Substring(0, fi.Name.Length - 4);
                if (LoadCatalog(fi.FullName, cn))
                {
                    int idx = lbCatalogs.Items.Add(cn);
                    string v = GeminiHardware.m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Catalog " + cn);
                    if (!string.IsNullOrEmpty(v))
                    {
                        bool b  = false;
                        bool.TryParse(v, out b);
                        lbCatalogs.SetItemChecked(idx, b);
                    } else 
                        lbCatalogs.SetItemChecked(idx, true);
                }
            }
        }

        private bool LoadCatalog(string p, string catalog)
        {
            System.IO.StreamReader fi = null;

            try
            {
                fi = System.IO.File.OpenText(p);
                while (!fi.EndOfStream)
                {
                    string newl = fi.ReadLine();
                    newl.Trim();
                    if (newl.Length > 0)
                    {
                        CatalogObject obj = null;
                        if (CatalogObject.TryParse(newl, catalog, out obj) && !m_Objects.ContainsKey(obj.Name))
                            m_Objects.Add(obj.Name, obj);
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

        private void frmUserCatalog_Load(object sender, EventArgs e)
        {
            PopulateCatalogs();
            PopulateAllObjects("");
            UpdateGeminiCatalog();
            lbCatalogs.ItemCheck += new ItemCheckEventHandler(lbCatalogs_ItemCheck);
            GeminiHardware.OnConnect += new ConnectDelegate(OnConnect);
            OnConnect(true, 1);
        }

        void OnConnect(bool bConnect, int clients)
        {
            pbToGemini.Enabled = GeminiHardware.Connected;
            pbFromGemini.Enabled = GeminiHardware.Connected;
            btnGoto.Enabled = GeminiHardware.Connected;
            btnSync.Enabled = GeminiHardware.Connected;
            btnAddAlign.Enabled = GeminiHardware.Connected;
        }

        void PopulateAllObjects(string clicked)
        {
            GeminiHardware.m_Profile.DeviceType = "Telescope";

            string wh = "";

            // save all the selected catalogs 
            for (int i = 0; i < lbCatalogs.Items.Count; ++i)
            {
                bool bChecked = lbCatalogs.GetItemChecked(i);

                if (lbCatalogs.Items[i].ToString() == clicked) bChecked = !bChecked;

                GeminiHardware.m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Catalog " + lbCatalogs.Items[i],
                    lbCatalogs.GetItemChecked(i).ToString());

                if (bChecked) wh = wh + lbCatalogs.Items[i].ToString() + ",";
            }


            var qry = (from o in m_Objects.Values where (wh.Contains(o.Catalog) && o.Name.ToLower().Contains(m_Search)) select o);

            //select o);
            if (m_DirectionSort[m_OrderBy])
                qry = qry.OrderByDescending(CList[m_OrderBy]).ThenBy(CList["Name"]);
            else
                qry = qry.OrderBy(CList[m_OrderBy]).ThenBy(CList["Name"]);

            BindingSource bs = new BindingSource();
            bs.DataSource = qry;
            try
            {
                gvAllObjects.DataSource = bs;
                gvAllObjects.Columns["RA"].DefaultCellStyle.Format = "0.00";
                gvAllObjects.Columns["DEC"].DefaultCellStyle.Format = "0.00";
                gvAllObjects.Columns["RA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                gvAllObjects.Columns["DEC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch { }
        }

        void lbCatalogs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            PopulateAllObjects(lbCatalogs.Items[e.Index].ToString());
        }

        private void lvAllObjects_VirtualItemsSelectionRangeChanged(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
        {
        }

        private void lvAllObjects_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {

        }

        private void gvAllObjects_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = gvAllObjects.Columns[e.ColumnIndex].HeaderText;
            if (col == m_OrderBy)
            {
                m_DirectionSort[col] = !m_DirectionSort[col];
            }
            m_OrderBy = col;
            PopulateAllObjects("");
        }

        private void AddRow(int idx)
        {
            string name = gvAllObjects.Rows[idx].Cells["Name"].Value.ToString();

            if (!m_GeminiObjects.ContainsKey(name))
            {
                m_GeminiObjects.Add(name, m_Objects[name]);
            }

        }

        private void gvAllObjects_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                AddRow(e.RowIndex);
                UpdateGeminiCatalog();
            }
        }

        private void pbAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in gvAllObjects.SelectedRows)
            {
                AddRow(r.Index);
            }
            UpdateGeminiCatalog();
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in gvGeminiCatalog.SelectedRows)
            {
                m_GeminiObjects.Remove(r.Cells["Name"].Value.ToString());
            }
            UpdateGeminiCatalog();
        }

        private void UpdateGeminiCatalog()
        {
            var qry = (from o in m_GeminiObjects.Values select o);
            if (m_GeminiDirectionSort[m_GeminiOrderBy])
                qry = qry.OrderByDescending(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);
            else
                qry = qry.OrderBy(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);

            BindingSource bs = new BindingSource();
            bs.DataSource = qry;
            gvGeminiCatalog.DataSource = bs;
            try
            {
                gvGeminiCatalog.Columns["RA"].DefaultCellStyle.Format = "0.00";
                gvGeminiCatalog.Columns["DEC"].DefaultCellStyle.Format = "0.00";
                gvGeminiCatalog.Columns["RA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                gvGeminiCatalog.Columns["DEC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch { }
        }

        private void pbClear_Click(object sender, EventArgs e)
        {
            m_GeminiObjects.Clear();
            UpdateGeminiCatalog();
        }

        private void gvGeminiCatalog_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = gvGeminiCatalog.Columns[e.ColumnIndex].HeaderText;
            if (col == m_GeminiOrderBy)
            {
                m_GeminiDirectionSort[col] = !m_GeminiDirectionSort[col];
            }
            m_GeminiOrderBy = col;
            UpdateGeminiCatalog();
        }

        //private void pbAddAll_Click(object sender, EventArgs e)
        //{
        //    //m_GeminiObjects.Clear();
        //    foreach (KeyValuePair<string, CatalogObject> kv in m_Objects)
        //    {
        //        if (!m_GeminiObjects.ContainsKey(kv.Key))
        //            m_GeminiObjects.Add(kv.Key, kv.Value);
        //    }
        //    UpdateGeminiCatalog();
        //}

        private void pbExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            m_Search = textSearch.Text;
            PopulateAllObjects("");
        }

        private void pbClearSearch_Click(object sender, EventArgs e)
        {
            textSearch.Text = "";
        }

        private void pbFromGemini_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {
                this.UseWaitCursor = true;
                SerializableDictionary<string, CatalogObject> cat = GeminiHardware.GetUserCatalog;
                if (cat != null)
                {
                    m_GeminiObjects = cat;
                    UpdateGeminiCatalog();
                }
                this.UseWaitCursor = false;
            }
        }

        private void pbToGemini_Click(object sender, EventArgs e)
        {
            if (GeminiHardware.Connected)
            {                
                DialogResult res = MessageBox.Show("Are you sure you want to send this catalog to Gemini?", SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
                if (res == DialogResult.Yes)
                {
                    if (m_GeminiObjects.Count > 4096)
                    {
                        res = MessageBox.Show("There are more than 4096 entries in the user catalog! Gemini will only accept the first 4096.\r\nDo you want to continue?", SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (res != DialogResult.Yes) return;
                    }
                    this.UseWaitCursor = true;
                    var qry = (from o in m_GeminiObjects.Values select o);
                    if (m_GeminiDirectionSort[m_GeminiOrderBy])
                        qry = qry.OrderByDescending(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);
                    else
                        qry = qry.OrderBy(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);

                    GeminiHardware.SetUserCatalog = qry.ToList();
                    this.UseWaitCursor = false;
                }
            }
        }

        private void pbSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbCatalogs.Items.Count; ++i)
            {
                lbCatalogs.SetItemChecked(i, true);
            }
            PopulateAllObjects("");
        }

        private void pbUnselAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbCatalogs.Items.Count; ++i)
            {
                lbCatalogs.SetItemChecked(i, false);
            }
            PopulateAllObjects("");
        }

        private void btnGoto_Click(object sender, EventArgs e)
        {
            if (gvAllObjects.SelectedRows.Count < 1 || gvAllObjects.SelectedRows.Count > 1)
            {
                MessageBox.Show("Please select one object!", SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            CatalogObject obj = m_Objects[gvAllObjects.SelectedRows[0].Cells["Name"].Value.ToString()];
            double ra, dec;
            obj.GetCoords(out ra, out dec);
            GeminiHardware.TargetRightAscension = ra;
            GeminiHardware.TargetDeclination = dec;
            GeminiHardware.TargetName = string.Format("{0} {1}", obj.Name, obj.Catalog);

            try
            {
                if (sender == btnGoto)
                    GeminiHardware.SlewEquatorial();
                else if (sender == btnSync)
                    GeminiHardware.SyncEquatorial();
                else
                    GeminiHardware.AlignEquatorial();
            }
            catch (Exception ex) {
                MessageBox.Show("Gemini reported an error: " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pbToFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Catalogs";
            DialogResult res = saveFileDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamWriter fi = System.IO.File.CreateText(saveFileDialog1.FileName);
                    foreach (CatalogObject obj in m_GeminiObjects.Values)
                    {
                        string s = string.Format("{0},{1},{2}#", obj.Name, GeminiHardware.m_Util.HoursToHMS(obj.RA, ":", ":", ""),
                            GeminiHardware.m_Util.DegreesToDMS(obj.DEC, ":", ":", ""));
                        fi.Write(s+"\n");

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

    public class CatalogObject
    {
        public string Name { get; set; }
        public double RA { get; set; }
        public double DEC { get; set; }
        public string Catalog { get; set; }

        public static bool TryParse(string s, string catalog, out CatalogObject obj)
        {
            obj = null;
            string[] sp = s.Split(new char[] { ',', '#' }, StringSplitOptions.RemoveEmptyEntries);
            if (sp.Length != 3) return false;
            try
            {
                obj = new CatalogObject { 
                    Catalog = catalog, Name = sp[0], 
                    RA = GeminiHardware.m_Util.HMSToHours(sp[1]), 
                    DEC = GeminiHardware.m_Util.DMSToDegrees(sp[2]) };
            }
            catch
            {
                return false;
            }
            return true;
        }

        // return object coordinates
        // precessed and refraction-adjusted, as needed based on
        // current Gemini settings. DEC and RA are stored in J2000 coordinates:
        internal void GetCoords(out double ra, out double dec)
        {
            ra = RA;
            dec =DEC;

            if (!GeminiHardware.Refraction)
                GeminiHardware.m_Transform.Refraction = true;
            else
                GeminiHardware.m_Transform.Refraction = false;

            if (!GeminiHardware.Precession) //need to precess!
                GeminiHardware.m_Transform.SetJ2000(ra, dec);
            else
                GeminiHardware.m_Transform.SetTopocentric(ra, dec);

            GeminiHardware.m_Transform.SiteElevation = GeminiHardware.Elevation;
            GeminiHardware.m_Transform.SiteLatitude = GeminiHardware.Latitude;
            GeminiHardware.m_Transform.SiteLongitude = GeminiHardware.Longitude;
            ra = GeminiHardware.m_Transform.RATopocentric;
            dec = GeminiHardware.m_Transform.DECTopocentric;
        }
    }
}
