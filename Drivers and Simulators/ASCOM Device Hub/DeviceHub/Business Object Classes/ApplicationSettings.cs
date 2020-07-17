using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ASCOM.DeviceHub
{
	public class ApplicationSettings
	{
		#region Static Properties and Methods

		private const string _settingsFilename = "DeviceHubApplication.settings";
		private const string _rootElementName = "ApplicationSettings";

		public static ApplicationSettings FromXmlFile()
		{
			XDocument doc = ReadSettingsFromFile();
			XmlSerializer ser = new XmlSerializer( typeof( ApplicationSettings ) );

			return (ApplicationSettings)ser.Deserialize( doc.CreateReader() );
		}

		#endregion Static Properties and Methods

		#region Constructor

		public ApplicationSettings()
		{}

		#endregion Constructor

		#region Public Properties

		public Point MainWindowLocation { get; set; }
		public Point ActivityWindowLocation { get; set; }
		public Point ActivityWindowSize { get; set; }
		public double RegistryVersion { get; set; }
		public bool SuppressTrayBubble { get; set; }
		public bool UseCustomTheme { get; set; }
		public bool UseExpandedScreenLayout { get; set; }

		#endregion Public Properties

		#region Public Methods

		public void ToXmlFile()
		{
			XDocument doc = new XDocument();

			using ( XmlWriter writer = doc.CreateWriter() )
			{
				var ns = new XmlSerializerNamespaces();
				ns.Add( "", "" );
				var ser = new XmlSerializer( this.GetType() );
				ser.Serialize( writer, this, ns );
			}

			WriteSettingsToFile( doc );
		}

		#endregion Public Methods

		#region Helper Methods

		private void WriteSettingsToFile( XDocument settingsDoc )
		{
			Stream strm = GetIOStream( FileMode.Create, FileAccess.Write );

			XmlWriterSettings settings = new XmlWriterSettings
			{
				ConformanceLevel = ConformanceLevel.Document,
				CloseOutput = true,
				Indent = true,
				IndentChars = "\t",
				WriteEndDocumentOnClose = true  // not available before .Net 4.5
			};

			using ( XmlWriter writer = XmlWriter.Create( strm, settings ) )
			{
				settingsDoc.Save( writer );
			}
		}

		#endregion Helper Methods

		#region Static Helper Methods

		private static XDocument ReadSettingsFromFile()
		{
			XDocument doc;

			try
			{
				using ( Stream strm = GetIOStream(  FileMode.Open, FileAccess.Read ) )
				{
					XmlReaderSettings settings = new XmlReaderSettings
					{
						ConformanceLevel = ConformanceLevel.Document,
						IgnoreWhitespace = true,
						IgnoreComments = true,
						CloseInput = true
					};

					using ( XmlReader reader = XmlReader.Create( strm, settings ) )
					{
						doc = XDocument.Load( reader );
					}
				}
			}
			catch ( FileNotFoundException )
			{
				// No settings document exists, so create and return an empty document.

				doc = new XDocument( new XElement( _rootElementName ) );
			}

			return doc;
		}

		private static Stream GetIOStream( FileMode fileMode, FileAccess fileAccess )
		{
			string folderPath = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );

			string companyName = GetCompanyName();
			string productName = GetProductName();

			folderPath = Path.Combine( folderPath, companyName, productName );

			if ( !Directory.Exists( folderPath ) )
			{
				Directory.CreateDirectory( folderPath );
			}

			string filePath = Path.Combine( folderPath, _settingsFilename );

			// File Modes Open for read, Create for write

			return new FileStream( filePath, fileMode, fileAccess );
		}

		private static string GetCompanyName()
		{
			string retval = "";

			AssemblyCompanyAttribute attribute = GetAssemblyAttribute<AssemblyCompanyAttribute>( false );

			if ( attribute != null )
			{
				retval = attribute.Company;
			}

			return retval;
		}

		private static string GetProductName()
		{
			string retval = "";

			AssemblyProductAttribute attribute = GetAssemblyAttribute<AssemblyProductAttribute>( false );

			if ( attribute != null )
			{
				retval = attribute.Product;
			}

			return retval;
		}

		private static T GetAssemblyAttribute<T>( bool inherit )
		{
			Assembly assembly = Assembly.GetEntryAssembly();

			return (T)assembly.GetCustomAttributes( typeof( T ), inherit ).First();
		}

		#endregion Static Helper Methods
	}
}
