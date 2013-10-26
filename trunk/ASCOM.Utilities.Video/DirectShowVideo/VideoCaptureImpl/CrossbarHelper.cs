//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - DirectShow
//
// Description:	This is a set of helper methods to work with TV Tuner Crossbars
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// xx-XXX-2013	HDP	6.0.0	Initial commit
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities.Video.DirectShowVideo;
using DirectShowLib;

namespace ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl
{
    internal class CrossbarHelper
    {
	    private Settings settings;

		public CrossbarHelper(Settings settings)
		{
			this.settings = settings;
		}

        internal class CrossbarPinEntry
        {
            public string PinName { get; set; }
            public int PinIndex { get; set; }

            public override string ToString()
            {
                return PinName;
            }
        }

        private IBaseFilter CreateFilter(Guid category, string friendlyname)
        {
            object source = null;
            Guid iid = typeof(IBaseFilter).GUID;
            foreach (DsDevice device in DsDevice.GetDevicesOfCat(category))
            {
                if (device.Name.CompareTo(friendlyname) == 0)
                {
                    device.Mon.BindToObject(null, null, ref iid, out source);
                    break;
                }
            }

            return (IBaseFilter)source;
        }

        private bool IsVideoPin(PhysicalConnectorType connectorType)
        {
            switch (connectorType)
            {
                case PhysicalConnectorType.Video_Tuner:
                case PhysicalConnectorType.Video_Composite:
                case PhysicalConnectorType.Video_SVideo:
                case PhysicalConnectorType.Video_RGB:
                case PhysicalConnectorType.Video_YRYBY:
                case PhysicalConnectorType.Video_SerialDigital:
                case PhysicalConnectorType.Video_ParallelDigital:
                case PhysicalConnectorType.Video_SCSI:
                case PhysicalConnectorType.Video_AUX:
                case PhysicalConnectorType.Video_1394:
                case PhysicalConnectorType.Video_USB:
                case PhysicalConnectorType.Video_VideoDecoder:
                case PhysicalConnectorType.Video_VideoEncoder:
                    return true;
            }

            return false;
        }

        private string GetPhysicalPinName(PhysicalConnectorType connectorType)
        {
            switch (connectorType)
            {
                case PhysicalConnectorType.Video_Tuner: return "Video Tuner";
                case PhysicalConnectorType.Video_Composite: return "Video Composite";
                case PhysicalConnectorType.Video_SVideo: return "S-Video";
                case PhysicalConnectorType.Video_RGB: return "Video RGB";
                case PhysicalConnectorType.Video_YRYBY: return "Video YRYBY";
                case PhysicalConnectorType.Video_SerialDigital: return "Video Serial Digital";
                case PhysicalConnectorType.Video_ParallelDigital: return "Video Parallel Digital";
                case PhysicalConnectorType.Video_SCSI: return "Video SCSI";
                case PhysicalConnectorType.Video_AUX: return "Video AUX";
                case PhysicalConnectorType.Video_1394: return "Video 1394";
                case PhysicalConnectorType.Video_USB: return "Video USB";
                case PhysicalConnectorType.Video_VideoDecoder: return "Video Decoder";
                case PhysicalConnectorType.Video_VideoEncoder: return "Video Encoder";

                case PhysicalConnectorType.Audio_Tuner: return "Audio Tuner";
                case PhysicalConnectorType.Audio_Line: return "Audio Line";
                case PhysicalConnectorType.Audio_Mic: return "Audio Microphone";
                case PhysicalConnectorType.Audio_AESDigital: return "Audio AES/EBU Digital";
                case PhysicalConnectorType.Audio_SPDIFDigital: return "Audio S/PDIF";
                case PhysicalConnectorType.Audio_SCSI: return "Audio SCSI";
                case PhysicalConnectorType.Audio_AUX: return "Audio AUX";
                case PhysicalConnectorType.Audio_1394: return "Audio 1394";
                case PhysicalConnectorType.Audio_USB: return "Audio USB";
                case PhysicalConnectorType.Audio_AudioDecoder: return "Audio Decoder";

                default: return "Unknown Type";
            }
        }

        private delegate void CrossbarCallback(IAMCrossbar crossbar);

        private void DoCrossbarOperation(string deviceName, CrossbarCallback callback)
        {
            IGraphBuilder graphBuilder = null;
            ICaptureGraphBuilder2 captureGraphBuilder = null;
            IBaseFilter theDevice = null;

            try
            {
                graphBuilder = (IGraphBuilder)new FilterGraph();
                captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
                theDevice = CreateFilter(FilterCategory.VideoInputDevice, deviceName);

                // Attach the filter graph to the capture graph
                int hr = captureGraphBuilder.SetFiltergraph(graphBuilder);
                DsError.ThrowExceptionForHR(hr);

                // Add the Video input device to the graph
                hr = graphBuilder.AddFilter(theDevice, "source filter");
                DsError.ThrowExceptionForHR(hr);

                // Render any preview pin of the device
                hr = captureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, theDevice, null, null);
                DsError.ThrowExceptionForHR(hr);

                IAMCrossbar crossbar = null;
                object o;

                hr = captureGraphBuilder.FindInterface(null, null, theDevice, typeof(IAMCrossbar).GUID, out o);
                if (hr >= 0)
                {
                    crossbar = (IAMCrossbar)o;
                    o = null;

                    if (crossbar != null)
                    {
                        callback(crossbar);
                        return;
                    }
                }

                callback(null);
            }
            finally
            {
                Marshal.ReleaseComObject(theDevice);
                Marshal.ReleaseComObject(graphBuilder);
                Marshal.ReleaseComObject(captureGraphBuilder);
            }
        }

        private int FindVideoDecoderOutputPin(IAMCrossbar crossbar)
        {
            int rv = -1;

            int outputPinsCount;
            int inputPinsCount;
            int hr = crossbar.get_PinCounts(out outputPinsCount, out inputPinsCount);
            DsError.ThrowExceptionForHR(hr);

            for (int i = 0; i < outputPinsCount; i++)
            {
                int relatedIndex;
                PhysicalConnectorType connectorType;
                hr = crossbar.get_CrossbarPinInfo(false, i, out relatedIndex, out connectorType);
                if (hr == 0)
                {
                    int inputPinIndex;
                    crossbar.get_IsRoutedTo(i, out inputPinIndex);

                    Trace.WriteLine(string.Format("Crossbar Output Pin {0}: '{1}' routed to pin {2}", i, GetPhysicalPinName(connectorType), inputPinIndex));
                    if (connectorType == PhysicalConnectorType.Video_VideoDecoder)
                        rv = i;
                }
            }

            return rv;
        }

        public void LoadCrossbarSources(string deviceName, ComboBox cbxCrossbarInput)
        {
            DoCrossbarOperation(
                deviceName,
                delegate(IAMCrossbar crossbar)
                {
                    if (crossbar != null)
                    {
                        Trace.WriteLine("Found Crossbar");

						settings.UsesTunerCrossbar = true;
	                    settings.Save();

                        int connectedInputPin = -1;
                        int videoDecoderOutPinIndex = FindVideoDecoderOutputPin(crossbar);
                        cbxCrossbarInput.Enabled = videoDecoderOutPinIndex != -1;
                        if (videoDecoderOutPinIndex != -1)
                            crossbar.get_IsRoutedTo(videoDecoderOutPinIndex, out connectedInputPin);

                        int outputPinsCount;
                        int inputPinsCount;
                        int hr = crossbar.get_PinCounts(out outputPinsCount, out inputPinsCount);
                        DsError.ThrowExceptionForHR(hr);

                        for (int i = 0; i < inputPinsCount; i++)
                        {
                            int relatedIndex;
                            PhysicalConnectorType connectorType;
                            hr = crossbar.get_CrossbarPinInfo(true, i, out relatedIndex, out connectorType);
                            if (hr == 0)
                            {
                                Trace.WriteLine(string.Format("Crossbar Input Pin {0}: {1}", i, GetPhysicalPinName(connectorType)));

                                if (IsVideoPin(connectorType))
                                {
                                    int addedIdnex = cbxCrossbarInput.Items.Add(
                                        new CrossbarPinEntry()
                                        {
                                            PinIndex = i,
                                            PinName = GetPhysicalPinName(connectorType)
                                        }
                                    );

                                    if (connectedInputPin == i)
                                        cbxCrossbarInput.SelectedIndex = addedIdnex;
                                }
                            }
                        }

                        cbxCrossbarInput.Enabled = true;
                    }
                    else
                    {
                        UpdateNoCrossbarSettings(cbxCrossbarInput);

                        settings.UsesTunerCrossbar = false;
						settings.Save();
                    }
                }
            );
        }

        public void UpdateNoCrossbarSettings(ComboBox cbxCrossbarInput)
        {
            cbxCrossbarInput.Items.Clear();
            cbxCrossbarInput.Items.Add("No Crossbar Found");
            cbxCrossbarInput.SelectedIndex = 0;
            cbxCrossbarInput.Enabled = false;            
        }

        public void SetupTunerAndCrossbar(ICaptureGraphBuilder2 graphBuilder, IBaseFilter deviceFilter)
        {
            if (settings.UsesTunerCrossbar)
            {
                object o;

                int hr = graphBuilder.FindInterface(null, null, deviceFilter, typeof(IAMTVTuner).GUID, out o);
                if (hr >= 0)
                {
                    hr = graphBuilder.FindInterface(null, null, deviceFilter, typeof(IAMCrossbar).GUID, out o);
                    if (hr >= 0)
                    {
                        IAMCrossbar crossbar = (IAMCrossbar)o;

                        if (crossbar != null)
                        {
                            hr = crossbar.Route(settings.CrossbarOutputPin, settings.CrossbarInputPin);
                            DsError.ThrowExceptionForHR(hr);
                        }
                    }
                }
            }
        }
    }
}
