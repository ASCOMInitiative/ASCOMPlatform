using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.OpenWeatherMap;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;


namespace ASCOM.OpenWeatherMap
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private Util util = new Utilities.Util();

        public SetupDialogForm()
        {
            InitializeComponent();
            radioButtonCity.Tag = LocationType.CityName;
            radioButtonLatLong.Tag = LocationType.LatLong;
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            Log.Enabled = chkTrace.Checked;
            if (radioButtonCity.Checked)
            {
                OpenWeatherMap.locationType = LocationType.CityName;
            }
            else if (radioButtonLatLong.Checked)
            {
                OpenWeatherMap.locationType = LocationType.LatLong;
            }
            OpenWeatherMap.SiteElevation = double.Parse(textBoxSiteElevation.Text);
            OpenWeatherMap.CityName = textBoxCityName.Text;
            OpenWeatherMap.SiteLatitude = util.DMSToDegrees(textBoxSiteLatitude.Text);
            OpenWeatherMap.SiteLongitude = util.DMSToDegrees(textBoxSiteLongitude.Text);
            OpenWeatherMap.apiKey = textBoxApiKey.Text;
            OpenWeatherMap.apiUrl = textBoxApiUrl.Text;
            OpenWeatherMap.MinimumQueryInterval = (double)MinimumRefreshInterval.Value;
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("https://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            chkTrace.Checked = Log.Enabled;
            textBoxSiteElevation.Text = OpenWeatherMap.SiteElevation.ToString();
            switch (OpenWeatherMap.locationType)
            {
                case LocationType.CityName:
                    radioButtonCity.Checked = true;
                    break;
                case LocationType.LatLong:
                    radioButtonLatLong.Checked = true;
                    break;
            }
            textBoxCityName.Text = OpenWeatherMap.CityName;
            textBoxSiteLatitude.Text = util.DegreesToDM(OpenWeatherMap.SiteLatitude);
            textBoxSiteLongitude.Text = util.DegreesToDM(OpenWeatherMap.SiteLongitude);
            textBoxApiKey.Text = OpenWeatherMap.apiKey;
            textBoxApiUrl.Text = OpenWeatherMap.apiUrl;
            if (string.IsNullOrWhiteSpace(OpenWeatherMap.apiKey))
            {
                tabControl.SelectTab(0);
            }
            else
                tabControl.SelectTab(1);
            MinimumRefreshInterval.Value = (decimal)OpenWeatherMap.MinimumQueryInterval;
        }

        private void ShowLatLonList()
        {
            var lat = util.DMSToDegrees(textBoxSiteLatitude.Text);
            var lon = util.DMSToDegrees(textBoxSiteLongitude.Text);
            // get 5 reports round a location
            // http://api.openweathermap.org/data/2.5/find?lat=51.6&lon=-0.73&cnt=5
            var uri = string.Format("http://api.openweathermap.org/data/2.5/find?lat={0}&lon={1}&cnt=5&APPID={2}",
                lat, lon, textBoxApiKey.Text);
            Log.LogMessage("ShowLatLonList", "URI {0}", uri);

            GetResponse(uri);
        }

        private void ShowCityList()
        {
            // get 5 reports round a location
            var uri = string.Format("http://api.openweathermap.org/data/2.5/find?q={0}&cnt=5&APPID={1}",
            textBoxCityName.Text, textBoxApiKey.Text);

            Log.LogMessage("ShowCityList", "URI {0}", uri);

            GetResponse(uri);
        }

        private void GetResponse(string uri)
        {
            if (string.IsNullOrEmpty(textBoxApiKey.Text))
            {
                MessageBox.Show("The API key has not been set", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string ret = "";

                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    try
                    {
                        ret = wc.DownloadString(uri);
                    }
                    catch (WebException ex)
                    {
                        if (ex.Status == WebExceptionStatus.ProtocolError)
                        {
                            var response = ex.Response as HttpWebResponse;
                            if (response.StatusCode == HttpStatusCode.Unauthorized) MessageBox.Show("The API key was rejected, please check that the key is valid. (Status code: 401)");
                            else MessageBox.Show(string.Format("The GetResponse call was unsuccessul, (Status code: {0}): {1}", (int)response.StatusCode), ex.Message);
                            Log.LogMessage("GetResponse", "Received {0} error: {1}", ex.Status.ToString(), ex.Message);
                        }
                        else
                        {
                            MessageBox.Show("Received " + ex.Status.ToString() + " error: " + ex.Message);
                            Log.LogMessage("GetResponse", "Received {0} error: {1}", ex.Status.ToString(), ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogMessage("GetResponse", "DownLoadString error {0}", ex.ToString());
                    }
                }

                Log.LogMessage("GetResponse", "reply {0}", ret);

                dataGridView.Rows.Clear();
                var jss = new JavaScriptSerializer();
                try
                {
                    var wl = jss.Deserialize<WeatherList>(ret);
                    foreach (var item in wl.list)
                    {
                        dataGridView.Rows.Add(item.name, util.DegreesToDM(item.coord.lat), util.DegreesToDM(item.coord.lon));
                    }
                }
                catch (Exception)
                { }
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            switch ((LocationType)rb.Tag)
            {
                case LocationType.CityName:
                    textBoxCityName.Enabled = true;
                    textBoxSiteLatitude.Enabled = false;
                    textBoxSiteLongitude.Enabled = false;
                    break;
                case LocationType.LatLong:
                    textBoxCityName.Enabled = false;
                    textBoxSiteLatitude.Enabled = true;
                    textBoxSiteLongitude.Enabled = true;
                    break;
                default:
                    textBoxCityName.Enabled = false;
                    textBoxSiteLatitude.Enabled = false;
                    textBoxSiteLongitude.Enabled = false;
                    break;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var cells = dataGridView.CurrentRow.Cells;
            textBoxCityName.Text = (string)cells["colName"].Value;
            textBoxSiteLatitude.Text = (string)cells["colLat"].Value;
            textBoxSiteLongitude.Text = (string)cells["colLon"].Value;
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            if (radioButtonCity.Checked)
                ShowCityList();
            if (radioButtonLatLong.Checked)
                ShowLatLonList();
        }

        private void buttonObtainKey_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://openweathermap.org/appid");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
    }
}