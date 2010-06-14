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
        public static SerializableDictionary<string, CatalogObject> m_Objects = null;
        SerializableDictionary<string, CatalogObject> m_GeminiObjects;

        string m_OrderBy = "Name";
        string m_GeminiOrderBy = "Name";

        string m_Search = "";

        Dictionary<string, bool> m_DirectionSort = new Dictionary<string, bool>();
        Dictionary<string, bool> m_GeminiDirectionSort = new Dictionary<string, bool>();

        Dictionary<String, Func<CatalogObject, IComparable>> CList = new Dictionary<String, Func<CatalogObject, IComparable>>();

        //Catalog-id is a character selecting
        // one of the internal catalogues: '1':
        // Messier, '2': NGC, '3': IC, '4': Sh2,
        // '7': SAO, ':': LDN, ';': LBN.
        // Object-id is a numeric designation
        // of the object in the catalogue; it
        // can be followed by an extension
        // character for NGC and IC
        // catalogues
        Dictionary<string, string> m_GeminiCatalogs = new Dictionary<string, string>();
        
        public frmUserCatalog()
        {
            InitializeComponent();
            m_DirectionSort.Add("Name", false);
            m_DirectionSort.Add("Catalog", false);
            m_DirectionSort.Add("RA", false);
            m_DirectionSort.Add("DEC", false);
            m_DirectionSort.Add("Visible", false);
            m_GeminiDirectionSort.Add("Name", false);
            m_GeminiDirectionSort.Add("Catalog", false);
            m_GeminiDirectionSort.Add("RA", false);
            m_GeminiDirectionSort.Add("DEC", false);
            m_GeminiDirectionSort.Add("Visible", false);

            m_GeminiObjects = new SerializableDictionary<string, CatalogObject>();
            CList.Add("Name", o => o.Name);
            CList.Add("Catalog", o => o.Catalog);
            CList.Add("RA", o => o.RA);
            CList.Add("DEC", o => o.DEC);
            CList.Add("Visible", o => o.Visible);

            m_GeminiCatalogs.Add("messier", "1");
            m_GeminiCatalogs.Add("ngc", "2");
            m_GeminiCatalogs.Add("ic", "3");
            m_GeminiCatalogs.Add("sharpless hii regions", "4");
            m_GeminiCatalogs.Add("sao catalog", "7");
            m_GeminiCatalogs.Add("lynds dark nebulae", ":");  
            m_GeminiCatalogs.Add("lynds bright nebulae", ";");


            gvAllObjects.RowHeadersVisible = false;
            gvGeminiCatalog.RowHeadersVisible = false;
            dtDateTime.Checked = false;
            numHorizon.Value = (decimal)GeminiHardware.HorizonAltitude;
        }

        public void PopulateCatalogs()
        {

            bool bAlreadyLoaded =  (m_Objects != null);  //already loaded

            Cursor.Current = Cursors.WaitCursor;
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

            if (!bAlreadyLoaded) m_Objects = new SerializableDictionary<string, CatalogObject>();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
#if false
            if (!System.IO.File.Exists(di.FullName + "\\Custom.guc"))
            {
                try
                {
                    System.IO.File.WriteAllText(di.FullName + "\\Custom.guc", "");
                }
                catch { }
            }
#endif   
            System.IO.FileInfo[] files = di.GetFiles("*.guc");

            double incr =  1;
            if (files.Length != 0) incr = 100.0/files.Length;

            GeminiHardware.Profile.DeviceType = "Telescope";
            foreach (System.IO.FileInfo fi in files)
            {

                string cn = fi.Name.Substring(0, fi.Name.Length - 4);
                frmProgress.Update(incr, "Loading " + cn + "...");
                if (bAlreadyLoaded || LoadCatalog(fi.FullName, cn, m_Objects))
                {
                    int idx = lbCatalogs.Items.Add(cn);
                    string v = GeminiHardware.Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Catalog " + cn);
                    if (!string.IsNullOrEmpty(v))
                    {
                        bool b  = false;
                        bool.TryParse(v, out b);
                        lbCatalogs.SetItemChecked(idx, b);
                    } else 
                        lbCatalogs.SetItemChecked(idx, true);
                }
                frmProgress.Update(0, "Loading " + cn + "...");          
            }

  
            Cursor.Current = Cursors.Default;
        }

        private bool LoadCatalog(string p, string catalog, SerializableDictionary<string, CatalogObject> dict)
        {

            System.IO.StreamReader fi = null;

            try
            {
                fi = System.IO.File.OpenText(p);
                while (!fi.EndOfStream)
                {
                    string newl = fi.ReadLine();
                    newl.Trim();
                    if (newl.Length > 0 )
                    {
                        CatalogObject obj = null;
                        if (newl.Contains(":"))
                        {
                            if (CatalogObject.TryParse(newl, catalog, out obj) && !dict.ContainsKey(obj.Name))
                                dict.Add(obj.Name, obj);
                        }
                        else
                        {
                            if (CatalogObject.TryParseDouble(newl, catalog, out obj) && !dict.ContainsKey(obj.Name))
                                dict.Add(obj.Name, obj);
                        }
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

        private DateTime GetTime()
        {
            if (dtDateTime.Checked) return dtDateTime.Value;
            else
                return DateTime.Now;
        }

        private void frmUserCatalog_Load(object sender, EventArgs e)
        {
            frmProgress.Initialize(0, 100, "Load Catalogs...", null);
            frmProgress.ShowProgress(this);

            CatalogObject.SetTransform(GetTime(), (double)numHorizon.Value);

            PopulateCatalogs();
            PopulateAllObjects("");
            UpdateGeminiCatalog();
            lbCatalogs.ItemCheck += new ItemCheckEventHandler(lbCatalogs_ItemCheck);
            SetButtonState();
            GeminiHardware.OnConnect += new ConnectDelegate(OnConnect);
            frmProgress.HideProgress();
        }

        void SetButtonState()
        {
            pbToGemini.Enabled = GeminiHardware.Connected;
            pbFromGemini.Enabled = GeminiHardware.Connected;
            btnGoto.Enabled = GeminiHardware.Connected;
            btnSync.Enabled = GeminiHardware.Connected;
            btnAddAlign.Enabled = GeminiHardware.Connected;
            if (gvAllObjects.SelectedRows.Count == 1)
            {
                string cat = gvAllObjects.SelectedRows[0].Cells["Catalog"].Value.ToString().ToLower();
                if (GeminiHardware.Connected && m_GeminiCatalogs.ContainsKey(cat))
                    pbSendtObject.Enabled = true;
                else
                    pbSendtObject.Enabled = false;
            }
        }

        void OnConnect(bool bConnect, int clients)
        {
            SetButtonState();
        }

        void PopulateAllObjects(string clicked)
        {

            try
            {

                Cursor.Current = Cursors.WaitCursor;

                CatalogObject.SetTransform(GetTime(), (double)numHorizon.Value);

                GeminiHardware.Profile.DeviceType = "Telescope";

                string wh = "";

                // save all the selected catalogs 
                for (int i = 0; i < lbCatalogs.Items.Count; ++i)
                {
                    bool bChecked = lbCatalogs.GetItemChecked(i);

                    if (lbCatalogs.Items[i].ToString() == clicked) bChecked = !bChecked;

                    GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Catalog " + lbCatalogs.Items[i],
                        bChecked.ToString());

                    if (bChecked) wh = wh + lbCatalogs.Items[i].ToString() + ",";
                }


                var qry = (from o in m_Objects.Values where (wh.Contains(o.Catalog) && o.Name.IndexOf(m_Search, StringComparison.CurrentCultureIgnoreCase) >= 0) select o);

                if (chkVisibleOnly.Checked)
                    qry = (from o in m_Objects.Values where (wh.Contains(o.Catalog) && o.Visible && o.Name.IndexOf(m_Search, StringComparison.CurrentCultureIgnoreCase) >= 0) select o);

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
                    gvAllObjects.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gvAllObjects.Columns["RA"].Width = 80;
                    gvAllObjects.Columns["DEC"].Width = 80;
                    gvAllObjects.Columns["Visible"].Width = 80;
                }
                catch { }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
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
            CatalogObject.SetTransform(GetTime(), (double)numHorizon.Value);

            var qry = (from o in m_GeminiObjects.Values select o);
            if (m_GeminiDirectionSort[m_GeminiOrderBy])
                qry = qry.OrderByDescending(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);
            else
                qry = qry.OrderBy(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);

            BindingSource bs = new BindingSource();
            bs.DataSource = qry;
            gvGeminiCatalog.DataSource = bs;
            if (gvGeminiCatalog.Columns.Count > 0)
                try
                {
                    gvGeminiCatalog.Columns["RA"].DefaultCellStyle.Format = "0.00";
                    gvGeminiCatalog.Columns["DEC"].DefaultCellStyle.Format = "0.00";
                    gvGeminiCatalog.Columns["RA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gvGeminiCatalog.Columns["DEC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    gvGeminiCatalog.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gvAllObjects.Columns["RA"].Width = 80;
                    gvAllObjects.Columns["DEC"].Width = 80;
                    gvAllObjects.Columns["Visible"].Width = 80;

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
            this.Close();
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
                Cursor.Current = Cursors.WaitCursor;
                frmProgress.Initialize(0, 100, "Loading User Catalog from Gemini", null);
                frmProgress.ShowProgress(this);
                SerializableDictionary<string, CatalogObject> cat = GeminiHardware.GetUserCatalog;
                if (cat != null)
                {
                    m_GeminiObjects = cat;
                    UpdateGeminiCatalog();
                }
                frmProgress.HideProgress();
                Cursor.Current = Cursors.Default;
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
                    frmProgress.Initialize(0, 100, "Send User Catalog to Gemini", null);
                    frmProgress.ShowProgress(this);

                    Cursor.Current = Cursors.WaitCursor;
                    var qry = (from o in m_GeminiObjects.Values select o);
                    if (m_GeminiDirectionSort[m_GeminiOrderBy])
                        qry = qry.OrderByDescending(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);
                    else
                        qry = qry.OrderBy(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);

                    GeminiHardware.SetUserCatalog = qry.ToList();
                    frmProgress.HideProgress();
                    Cursor.Current = Cursors.Default;
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
                {
                    GeminiHardware.SyncEquatorial();
                    GeminiHardware.ReportAlignResult(((Button)sender).Text);
                }
                else
                {                   
                    GeminiHardware.AlignEquatorial();
                    GeminiHardware.ReportAlignResult(((Button)sender).Text);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(this, "Gemini reported an error: " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void pbToFile_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\UserCatalogs";
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch
            {
            }

            saveFileDialog1.InitialDirectory = path;

            var qry = (from o in m_GeminiObjects.Values select o);
            if (m_GeminiDirectionSort[m_GeminiOrderBy])
                qry = qry.OrderByDescending(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);
            else
                qry = qry.OrderBy(CList[m_GeminiOrderBy]).ThenBy(CList["Name"]);

            List<CatalogObject> list = qry.ToList();


            DialogResult res = saveFileDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamWriter fi = System.IO.File.CreateText(saveFileDialog1.FileName);
                    foreach (CatalogObject obj in list)
                    {
                        fi.Write(obj.ToString()+"\n");
                    }
                    fi.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing file: " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pbFromFile_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\UserCatalogs";
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch
            {
            }

            openFileDialog1.InitialDirectory = path;
            
            DialogResult res = openFileDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                try
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(openFileDialog1.FileName);
                    if (LoadCatalog(fi.FullName, fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length), m_GeminiObjects))
                    {
                        UpdateGeminiCatalog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading file: " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void gvAllObjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvAllObjects_SelectionChanged(object sender, EventArgs e)
        {
            SetButtonState();
        
}

        private void pbSendtObject_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvAllObjects.SelectedRows.Count == 1)
                {
                    string cat = gvAllObjects.SelectedRows[0].Cells["Catalog"].Value.ToString().ToLower();

                    if (m_GeminiCatalogs.ContainsKey(cat))
                    {
                        string catnbr = m_GeminiCatalogs[cat];
                        string id = gvAllObjects.SelectedRows[0].Cells["Name"].Value.ToString();
                        // strip off the catalog name first:
                        id = id.TrimStart("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());

                        // if there's a '-' (as in Sh2-xxx) then remove everything before and including the dash
                        int i = id.IndexOf('-');
                        if (i > 0) id = id.Substring(i + 1);

                        string cmd = string.Format(":OI{0}{1}", catnbr, id);
                        GeminiHardware.DoCommandResult(cmd, GeminiHardware.MAX_TIMEOUT, false);
                    }
                }
            }
            catch { }
        }

        private void frmUserCatalog_FormClosed(object sender, FormClosedEventArgs e)
        {
            GeminiHardware.Profile = null;
        }

        private void chkVisibleOnly_CheckedChanged(object sender, EventArgs e)
        {
            chkVisibleOnly.Refresh();
            chkVisibleOnly.Update();
            PopulateAllObjects("");
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        Timer tm = null;

        private void dtDateTime_ValueChanged(object sender, EventArgs e)
        {
            if (tm == null)
            {
                tm = new Timer();
                tm.Tick += new EventHandler(tm_Tick);
                tm.Interval = 1000;
            }
            else
                tm.Stop();

            tm.Start();
        }

        void tm_Tick(object sender, EventArgs e)
        {
            tm.Stop();
            PopulateAllObjects("");
        }

        private void numHorizon_ValueChanged(object sender, EventArgs e)
        {
            GeminiHardware.HorizonAltitude = (double)numHorizon.Value;
            if (tm == null)
            {
                tm = new Timer();
                tm.Tick += new EventHandler(tm_Tick);
                tm.Interval = 1000;
            }
            else
                tm.Stop();

            tm.Start();
        }

        public static string AddCustom(string name, double ra, double dec)
        {
            int idx;

            string k;

            if (string.IsNullOrEmpty(name)) name = "Custom Object";
            if (m_Objects.ContainsKey(name))
                for (idx = 1; m_Objects.ContainsKey(k = string.Format("Custom Object {0}", idx)); ++idx) ;
            else
                k = name;
            m_Objects.Add(k, new CatalogObject
            {
                Catalog = "Custom",
                Name = k,
                RA = new RACoord(ra),
                DEC = new DECCoord(dec)
            });

            return k;
        }

        public static bool SaveCustom()
        {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\" + SharedResources.TELESCOPE_DRIVER_NAME + "\\Catalogs";

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);


            var qry = (from o in m_Objects.Values where o.Catalog=="Custom" select o);
            List<CatalogObject> list = qry.ToList();

            try
            {
                System.IO.StreamWriter fi = System.IO.File.CreateText(di.FullName + "\\Custom.guc");
                foreach (CatalogObject obj in list)
                {
                    fi.Write(obj.ToString() + "\n");
                }
                fi.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing custom catalog file: " + ex.Message, SharedResources.TELESCOPE_DRIVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
#if false
            frmRA_DEC frm = new frmRA_DEC();
            frm.ShowDialog(this);

            return;

            BindingSource bs = (BindingSource)gvAllObjects.DataSource;

            int idx;
            string k;
            for(idx=1; m_Objects.ContainsKey(k=string.Format("Custom Object {0}",idx)); ++idx);


            m_Objects.Add(k, new CatalogObject {                    
                    Catalog = "Custom", Name = k, 
                    RA = new RACoord(GeminiHardware.m_Util.HMSToHours("00:00:00")), 
                    DEC = new DECCoord(GeminiHardware.m_Util.DMSToDegrees("00:00:00")) });

            foreach (int cat in lbCatalogs.CheckedIndices)
                lbCatalogs.SetItemCheckState(cat, ((string)lbCatalogs.Items[cat] == "Custom"? CheckState.Checked : CheckState.Unchecked));
            PopulateAllObjects("");
            gvAllObjects.BeginEdit(true);
            //int row  = gvAllObjects.Rows.Add("[Custom Object]", "00:00:00", "00:00:00");
            //gvAllObjects.Rows[row].Selected = true;
            //gvAllObjects.BeginEdit(true);
#endif
        }
    }

    public class CatalogObject
    {
        public string Name { get; set; }
        public RACoord RA { get; set; }
        public DECCoord DEC { get; set; }
        public string Catalog { get; set; }

        //pre-computed site/date-time variables shared by all instances:
        private static double LST = 0;
        private static double lat = 0;
        private static double lon = 0;
        private static double horizon = 0;

        public static bool TryParse(string s, string catalog, out CatalogObject obj)
        {
            obj = null;
            string[] sp = s.Split(new char[] { ',', '#' }, StringSplitOptions.RemoveEmptyEntries);
            if (sp.Length != 3) return false;
            try
            {
                obj = new CatalogObject { 
                    Catalog = catalog, Name = sp[0], 
                    RA = new RACoord(GeminiHardware.m_Util.HMSToHours(sp[1])), 
                    DEC = new DECCoord(GeminiHardware.m_Util.DMSToDegrees(sp[2])) };
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
            ra = RA.RA;
            dec =DEC.DEC;

            GeminiHardware.m_Transform.SiteElevation = GeminiHardware.Elevation;
            GeminiHardware.m_Transform.SiteLatitude = GeminiHardware.Latitude;
            GeminiHardware.m_Transform.SiteLongitude = GeminiHardware.Longitude;


            if (!GeminiHardware.Refraction)
                GeminiHardware.m_Transform.Refraction = true;
            else
                GeminiHardware.m_Transform.Refraction = false;

            if (!GeminiHardware.Precession) //need to precess!
                GeminiHardware.m_Transform.SetJ2000(ra, dec);
            else
                GeminiHardware.m_Transform.SetTopocentric(ra, dec);

            ra = GeminiHardware.m_Transform.RATopocentric;
            dec = GeminiHardware.m_Transform.DECTopocentric;
        }

        public static void SetTransform(DateTime dt, double horiz)
        {
            GeminiHardware.m_Transform.SiteElevation = GeminiHardware.Elevation;
            GeminiHardware.m_Transform.SiteLatitude = GeminiHardware.Latitude;
            GeminiHardware.m_Transform.SiteLongitude = GeminiHardware.Longitude;

            // [pk] the following values are pre-computed for the Altitude calculation 
            // for Visible property:
            lon = GeminiHardware.Longitude * SharedResources.DEG_RAD;
            lat = GeminiHardware.Latitude * SharedResources.DEG_RAD;
            LST = AstronomyFunctions.LocalSiderealTime(GeminiHardware.Longitude, dt) * SharedResources.DEG_RAD;
            horizon = horiz;
        }

        public bool Visible
        {
            get
            {
                // [pk] SetTransform must been called prior to this to pre-compute values
                // [pk] I'm ignoring precession from J2000 here, since a few arcminutes difference
                // isn't going to matter when computing horizon coordinates to determine visibility
                return AstronomyFunctions.CalculateAltitude(RA.RA / 12 * Math.PI, DEC.DEC * SharedResources.DEG_RAD, lat, lon, LST) > horizon;
            }
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}#", Name, RA.ToString(":",":"), DEC.ToString(":",":"));
        }

        internal static bool TryParseDouble(string s, string catalog, out CatalogObject obj)
        {
            obj = null;
            string[] sp = s.Split(new char[] { ',', '#' }, StringSplitOptions.RemoveEmptyEntries);
            if (sp.Length != 3) return false;
            try
            {
                double ra,dec;
                if (!double.TryParse(sp[1], System.Globalization.NumberStyles.Float, GeminiHardware.m_GeminiCulture, out ra)) return false;
                if (!double.TryParse(sp[2], System.Globalization.NumberStyles.Float, GeminiHardware.m_GeminiCulture, out dec)) return false;

                obj = new CatalogObject
                {
                    Catalog = catalog,
                    Name = sp[0],
                    RA=new RACoord(ra/360.0 * 24.0),     // when specified as double, it is in degrees, so convert to hours 
                    DEC=new DECCoord(dec)
                };
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
