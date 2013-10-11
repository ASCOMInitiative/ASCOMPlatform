//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	This file implements a player of configured video frames
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Video;
using ASCOM.Simulator.Utils;
using ASCOM.Simulator.VideoCameraImpl;

namespace Simulator.VideoCameraImpl
{
	internal struct BufferedFrameInfo
	{
		public int FirstIntegratedFrameIndex;
		public int LastIntegratedFrameIndex;
		public long CurrentFrameNo;
		public bool IsIntegratedFrame;
	}

	public class BitmapVideoPlayer : IDisposable
	{
		private List<string> allFiles = new List<string>();
		private Bitmap errorBitmap = null;
		private int[,] errorPixels = null;

		private int width = 640;
		private int height = 480;

		private CameraImage cameraImage;

		private Thread playerThread = null;
		private static List<int[,]> allImagesPixels = new List<int[,]>();
		private static int bufferedImageWidth;
		private static int bufferedImageHeight;
		private static bool imagesBuffered = false;

		private bool running = false;
		private long currentFrameNo = 0;
		private int playbackBufferSize = 0;
		private int integration = 1;

		private int currentFrameIndex = -1;

		private int frameWaitTime = 40;

		private static Queue<BufferedFrameInfo> playbackBuffer = new Queue<BufferedFrameInfo>();
		private static object syncRoot = new object();
		private int[,] lastBufferedImage;

		private bool isRecording = false;
		private bool stopRecordingRequested = false;
		
		public static void InvalidateBufferedVideoFrames()
		{
			imagesBuffered = false;
			allImagesPixels.Clear();
		}

		public BitmapVideoPlayer(bool useEmbeddedVideo, string bitmapFilesLocation, int playbackBufferSize)
		{
			this.cameraImage = new CameraImage();
			this.playbackBufferSize = playbackBufferSize;

			integration = 1;

			if (!imagesBuffered)
			{
				if (useEmbeddedVideo)
					BufferEmbeddedOccultationVideo();
				else
					BufferVideoFrames(bitmapFilesLocation);

				imagesBuffered = true;
			}
			
			if (errorBitmap != null)
			{
				width = errorBitmap.Width;
				height = errorBitmap.Height;
			}
			else if (allImagesPixels.Count > 0)
			{
				width = bufferedImageWidth;
				height = bufferedImageHeight;
			}
			else
			{
				width = 0;
				height = 0;
			}
		}

		private frmLoadingImages OpenProgressForm()
		{
			frmLoadingImages frmLoading = null;
			Form ownerForm = null;

			ownerForm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x != null && x.Owner == null);
			frmLoading = new frmLoadingImages();
			frmLoading.Show(ownerForm);
			frmLoading.Cursor = Cursors.WaitCursor;
			frmLoading.Invalidate();
			if (ownerForm != null)
			{
				try
				{
					ownerForm.Parent = ownerForm;
				}
				catch
				{ }
				ownerForm.Cursor = Cursors.WaitCursor;
				ownerForm.Update();
			}
			Application.DoEvents();

			return frmLoading;
		}

		private void CloseProgressForm(frmLoadingImages frmLoading)
		{
			if (frmLoading != null)
			{
				if (frmLoading.Owner != null)
				{
					frmLoading.Owner.Cursor = Cursors.WaitCursor;
					frmLoading.Owner.Update();
				}

				frmLoading.Close();
			}
		}

		private void BufferVideoFrames(string bitmapFilesLocation)
		{

			frmLoadingImages frmLoading = OpenProgressForm();
			try
			{
				string[] files = Directory.GetFiles(bitmapFilesLocation, "*.bmp");
				allFiles.AddRange(files);

				allFiles.Sort();

				if (allFiles.Count > 0)
				{
					width = 0;
					height = 0;

					// TODO: Use .NET 4 multithreaded loading 

					for(int i = 0; i < allFiles.Count; i++)
					{
						var bmp = new Bitmap(allFiles[i]);

						if (width == 0 && height == 0)
						{
							width = bmp.Width;
							height = bmp.Height;
						}
						else if (width != bmp.Width || height != bmp.Height)
						{
							throw new ApplicationException("All bitmaps must be the same size.");
						}

						int[,] pixels = CopyBitmapPixels(bmp);
						allImagesPixels.Add(pixels);

						frmLoading.SetProgress(i + 1, allFiles.Count);
					}

					errorBitmap = null;
					errorPixels = null;
					bufferedImageWidth = width;
					bufferedImageHeight = height;

					DebugTrace.TraceInfo(string.Format("Loaded {0} camera images from '{1}'", allFiles.Count, bitmapFilesLocation));
				}
				else
				{
					errorBitmap = new Bitmap(width, height);
					PrepareErrorMessage(string.Format("No bmp file found in '{0}'", bitmapFilesLocation));
					errorPixels = CopyBitmapPixels(errorBitmap);

					DebugTrace.TraceError(string.Format("No camera images found in '{0}'", bitmapFilesLocation));
				}
			}
			catch (Exception ex)
			{
				DebugTrace.TraceError(ex);

				errorBitmap = new Bitmap(width, height);
				PrepareErrorMessage(ex.Message);
				errorPixels = CopyBitmapPixels(errorBitmap);
			}
			finally
			{
				CloseProgressForm(frmLoading);
			}
			
		}

		private void BufferEmbeddedOccultationVideo()
		{
			string occultationVideoFileName = Path.GetFullPath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Resources\simulator.pix");

			width = 320;
			height = 264;

			frmLoadingImages frmLoading = OpenProgressForm();

			try
			{
				using (MemoryStream decompressedData = new MemoryStream())
				{
					using (FileStream sourceFile = new FileStream(occultationVideoFileName, FileMode.Open))
					using (var gzip = new GZipStream(sourceFile, CompressionMode.Decompress))
					{
						const int size = 4096;

						byte[] bytes = new byte[size];
						int numBytes;
						while ((numBytes = gzip.Read(bytes, 0, size)) > 0)
							decompressedData.Write(bytes, 0, numBytes);

						decompressedData.Flush();
					}

					decompressedData.Position = 0;

					byte[] frameBuffer = new byte[width * height];

					for (int i = 0; i < 200; i++)
					{
						int[,] frame = new int[height, width];
						decompressedData.Read(frameBuffer, 0, frameBuffer.Length);

						for (int j = 0; j < height; j++)
						{
							for (int k = 0; k < width; k++)
							{
								frame[j, k] = (int)frameBuffer[k + j * width];
							}
							
						}

						allImagesPixels.Add(frame);
						frmLoading.SetProgress(i, 200);
					}
				}

				errorBitmap = null;
				errorPixels = null;
				bufferedImageWidth = width;
				bufferedImageHeight = height;
			}
			catch(Exception ex)
			{
				DebugTrace.TraceError(ex);

				errorBitmap = new Bitmap(width, height);
				PrepareErrorMessage(ex.Message);
				errorPixels = CopyBitmapPixels(errorBitmap);
			}
			finally
			{
				CloseProgressForm(frmLoading);
			}
		}

		private int[,] CopyBitmapPixels(Bitmap bmp)
		{
			var pixels = new int[bmp.Height, bmp.Width];

			BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			try
			{
				unsafe
				{
					int stride = bmData.Stride;
					IntPtr Scan0 = bmData.Scan0;
					byte* p = (byte*)(void*)Scan0;

					int nOffset = stride - bmp.Width * 3;

					for (int y = 0; y < bmp.Height; ++y)
					{
						for (int x = 0; x < bmp.Width; ++x)
						{
							byte red = p[2];

							pixels[y, x] = red;

							p += 3;
						}
						p += nOffset;
					}
				}
			}
			finally
			{
				bmp.UnlockBits(bmData);
			}

			return pixels;
		}

		private void PrepareErrorMessage(string message)
		{
			using (Graphics g = Graphics.FromImage(errorBitmap))
			{
				g.Clear(Color.Tomato);

				using(Font fnt = new Font(FontFamily.GenericMonospace, 9f))
				{
					g.DrawString(message, fnt, Brushes.Black, 10, 10);
				}
			}
		}

		public int Width
		{
			get { return width; }
		}

		public int Height
		{
			get { return height; }
		}

		public void SetFrameWaitTime(int milliseconds)
		{
			frameWaitTime = milliseconds;
		}

		public void SetIntegration(int newintegration)
		{
			if (newintegration <= 1)
				integration = 1;
			else if (newintegration >= allImagesPixels.Count - 1)
				integration = allImagesPixels.Count - 1;
			else
				integration = newintegration;
		}

		public void Start()
		{
			if (playerThread == null)
				playerThread = new Thread(PlayerWorkerThread);

			running = true;

			if (!playerThread.IsAlive)
			{
				playerThread.SetApartmentState(ApartmentState.STA);
				playerThread.Start();
			}				
		}

		public void Stop()
		{
			DebugTrace.TraceVerbose("Stop called by: \r\n" + new StackTrace(1, true).ToString());

			if (playerThread != null)
			{
				if (playerThread.IsAlive)
				{
					running = false;
					isRecording = false;

					DebugTrace.TraceInfo("Camera stop request sent. Waiting ...");

					playerThread.Join(500);

					if (playerThread.IsAlive)
					{
						DebugTrace.TraceWarning("Killing camera thread");

						playerThread.Abort();
						playerThread.Join();						
					}
				}

				playerThread = null;
			}
		}

		public bool Running
		{
			get { return running; }
		}
		
		private BufferedFrameInfo currentFrame;

		private int integratedFramesSoFar = 0;

		private void PlayerWorkerThread(object state)
		{
			DebugTrace.TraceInfo(string.Format("Camera started. Buffer size: {0}", playbackBufferSize));

			BufferedFrameInfo newFrame;

			lock (syncRoot)
			{
				playbackBuffer.Clear();
				newFrame = new BufferedFrameInfo() { FirstIntegratedFrameIndex = -1 };
				integratedFramesSoFar = 0;
			}

			lastBufferedImage = new int[height, width];
			isRecording = false;

			Profiler.ResetAndStartTimer(100);

			while(running)
			{
				currentFrameIndex++;

				long startTicks = Profiler.GetTicks();
				
				if (currentFrameIndex > allImagesPixels.Count - 1)
				{
					Profiler.StopTimer(100);
					double msForAllImages = Profiler.GetElapsedMillisecondsForTimer(100);
					double fps = allImagesPixels.Count * 1000.0 / msForAllImages;
					DebugTrace.TraceVerbose(string.Format("Camera running at {0} fps", fps.ToString("#0.00")));

					currentFrameIndex = 0;

					Profiler.ResetAndStartTimer(100);
				}

				if (playbackBufferSize > 0)
				{
					lock(syncRoot)
					{
						while(playbackBuffer.Count > playbackBufferSize)
							playbackBuffer.Dequeue();

						if (newFrame.FirstIntegratedFrameIndex == -1)
						{
							newFrame.FirstIntegratedFrameIndex = currentFrameIndex;
							newFrame.LastIntegratedFrameIndex = -1;
						}

						if (integration > 1)
						{
							integratedFramesSoFar++;

							if (integratedFramesSoFar >= integration)
							{
								newFrame.LastIntegratedFrameIndex = currentFrameIndex;
								newFrame.IsIntegratedFrame = true;
							}
						}
						else
						{
							newFrame.LastIntegratedFrameIndex = newFrame.FirstIntegratedFrameIndex;
							newFrame.IsIntegratedFrame = false;
						}

						if (newFrame.LastIntegratedFrameIndex != -1)
						{
							currentFrameNo++;
							newFrame.CurrentFrameNo = currentFrameNo;

							playbackBuffer.Enqueue(newFrame);

							newFrame = new BufferedFrameInfo() { FirstIntegratedFrameIndex = -1 };
							integratedFramesSoFar = 0;
						}
					}
				}
				else
				{
					// No buffering

					if (newFrame.FirstIntegratedFrameIndex == -1)
					{
						newFrame.FirstIntegratedFrameIndex = currentFrameIndex;
						newFrame.LastIntegratedFrameIndex = -1;
					}

					if (integration > 1)
					{
						integratedFramesSoFar++;

						if (integratedFramesSoFar >= integration)
						{
							newFrame.LastIntegratedFrameIndex = currentFrameIndex;
							newFrame.IsIntegratedFrame = true;
						}
					}
					else
					{
						newFrame.LastIntegratedFrameIndex = newFrame.FirstIntegratedFrameIndex;
						newFrame.IsIntegratedFrame = false;
					}

					if (newFrame.LastIntegratedFrameIndex != -1)
					{
						currentFrameNo++;
						newFrame.CurrentFrameNo = currentFrameNo;

						currentFrame = newFrame;
						newFrame = new BufferedFrameInfo() { FirstIntegratedFrameIndex = -1 };
						integratedFramesSoFar = 0;
					}

					if (isRecording)
					{
						int[,] pixels = GetCurrentImageFromBufferedFrame(currentFrame);
                        AviTools.AddAviVideoFrame(pixels);
					}
				}

				long endTicks = Profiler.GetTicks();
				int  msThisFrame = (int)Profiler.TranslateTicksToMilliseconds(endTicks - startTicks);

				if (msThisFrame < frameWaitTime)
				{
					Thread.Sleep(frameWaitTime - msThisFrame);
				}
				
				if (stopRecordingRequested)
				{
					stopRecordingRequested = false;
					isRecording = false;
				}
			}

			DebugTrace.TraceInfo("Camera stopped");
		}

		public void Dispose()
		{
			StopRecording();

			Stop();
		}

		private BufferedFrameInfo nextBufferedFrame = new BufferedFrameInfo() { FirstIntegratedFrameIndex = -1 };

		public void GetCurrentImage(out int[,] currentFrame, out long currentFrameNo, out byte[] previewBitmapBytes)
		{
			if (errorBitmap != null)
			{
				currentFrame = errorPixels;
				currentFrameNo = this.currentFrameNo;
				previewBitmapBytes = cameraImage.GetBitmapBytes(errorBitmap);
			}
			else
			{
				if (playbackBufferSize > 0)
				{
					lock(syncRoot)
					{
						if (playbackBuffer.Count > 0)
						{
							nextBufferedFrame = playbackBuffer.Dequeue();
						}
						else
						{
							// There is no new buffered frame
						}
					}

					currentFrame = GetCurrentImageFromBufferedFrame(nextBufferedFrame);
					currentFrameNo = nextBufferedFrame.CurrentFrameNo;
				}
				else
				{
					currentFrame = GetCurrentImageFromBufferedFrame(this.currentFrame);
					currentFrameNo = this.currentFrame.CurrentFrameNo;
				}

				cameraImage.SetImageArray(currentFrame, width, height, SensorType.Monochrome);
				previewBitmapBytes = cameraImage.GetDisplayBitmapBytes();
			}
		}

		private int[,] GetCurrentImageNoBuffering(int imageIdx)
		{
			if (imageIdx < 0)
				return allImagesPixels[0];
			else if (imageIdx >= allImagesPixels.Count)
				return allImagesPixels[allImagesPixels.Count - 1];
			else
				return allImagesPixels[imageIdx];
		}

		private int[,] GetCurrentImageFromBufferedFrame(BufferedFrameInfo bufferedFrame)
		{
			if (bufferedFrame.IsIntegratedFrame)
			{
				int counter = 0;

				if (bufferedFrame.LastIntegratedFrameIndex < bufferedFrame.FirstIntegratedFrameIndex)
				{
                    AviTools.InitFrameIntegration(Width, Height);

					// Add all frames from bufferedFrame.FirstIntegratedFrameIndex to allImagesPixels.Count
					for (int i = bufferedFrame.FirstIntegratedFrameIndex; i <= allImagesPixels.Count - 1; i++)
					{
						int[,] frame = GetCurrentImageNoBuffering(i);
                        AviTools.AddIntegrationFrame(frame);
						counter++;
					}

					// Add all frames from 0 to bufferedFrame.LastIntegratedFrameIndex
					for (int i = 0; i <= bufferedFrame.LastIntegratedFrameIndex; i++)
					{
						int[,] frame = GetCurrentImageNoBuffering(i);
                        AviTools.AddIntegrationFrame(frame);
						counter++;
					}

                    lastBufferedImage = AviTools.GetResultingIntegratedFrame(Width, Height);
				}
				else
				{
					// Add all frames from bufferedFrame.FirstIntegratedFrameIndex to bufferedFrame.LastIntegratedFrameIndex

                    AviTools.InitFrameIntegration(Width, Height);

					for (int i = bufferedFrame.FirstIntegratedFrameIndex; i <= bufferedFrame.LastIntegratedFrameIndex; i++)
					{
						int[,] frame = GetCurrentImageNoBuffering(i);
                        AviTools.AddIntegrationFrame(frame);
						counter++;
					}

                    lastBufferedImage = AviTools.GetResultingIntegratedFrame(Width, Height);
				}
			}
			else
			{
				lastBufferedImage = GetCurrentImageNoBuffering(bufferedFrame.FirstIntegratedFrameIndex);
			}

			return lastBufferedImage;
		}

		public void StartRecording(string fileName, double fps, bool showCompressionDialog)
		{
			if (!isRecording)
                AviTools.StartNewAviFile(fileName, width, height, 8, fps, showCompressionDialog);

			isRecording = true;
		}

		public void StopRecording()
		{
			if (isRecording)
			{
				stopRecordingRequested = true;

				SpinWait.SpinUntil(() => !isRecording, 5000);

				if (!isRecording)
                    AviTools.CloseAviFile();
				else
					throw new DriverException("Cannot stop recording.");
			}
		}
	}
}
