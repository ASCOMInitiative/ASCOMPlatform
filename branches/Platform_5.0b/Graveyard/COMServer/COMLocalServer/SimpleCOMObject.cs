using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;								// For using MessageBox.
using System.Runtime.InteropServices;					// For use of the GuidAttribute, ProgIdAttribute and ClassInterfaceAttribute.
using Interop.ISimpleCOMObject;							// In order to implement ISimpleCOMObject.

namespace ManagedCOMLocalServer_Impl01
{
	[
	  Guid("E1FE1223-45C2-4872-9B1E-634FB850E753"),		// We indicate a specific CLSID for "ManagedCOMLocalServer_Impl01.SimpleCOMObject" for convenience of searching the registry.
	  ProgId("ManagedCOMLocalServer_Impl01.SimpleCOMObject"),  // This ProgId is used by default. Not 100% necessary.
	  ClassInterface(ClassInterfaceType.None)			// Specify that we will not generate any additional interface with a name like _SimpleCOMObject.
    ]
	public class SimpleCOMObject : 
		ReferenceCountedObjectBase,						// SimpleCOMObject is derived from ReferenceCountedObjectBase so that we can track its creation and destruction.
		ISimpleCOMObject								// SimpleCOMObject must implement the ISimpleCOMObject interface.
	{
		private int m_iLongProperty;

		public SimpleCOMObject()
		{
			// ReferenceCountedObjectBase constructor will be invoked.
			Console.WriteLine("SimpleCOMObject constructor.");
		}

		~SimpleCOMObject()
		{
			// ReferenceCountedObjectBase destructor will be invoked.
			Console.WriteLine("SimpleCOMObject destructor.");
		}

		public int LongProperty
		{
			get
			{
				return m_iLongProperty;
			}
		  
			set
			{
				m_iLongProperty = value;
			}
		}
		
		public void Method01 (String strMessage)
		{
			StringBuilder sb = new StringBuilder(strMessage);
		  
			sb.Append(LongProperty.ToString());
		  
			MessageBox.Show(sb.ToString(), "ManagedCOMLocalServer_Impl01.SimpleCOMObject");
		}
	}

	//
	// Here's where the magic is: Allow creation if an ISimpleComObject, 
	// or an IDispatch, or an IUnknown. 
	//
	class SimpleCOMObjectClassFactory : ClassFactoryBase
	{
		public override void virtual_CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
		{
			Console.WriteLine("SimpleCOMObjectClassFactory.CreateInstance().");
			Console.WriteLine("Requesting Interface : " + riid.ToString());

			if (riid == Marshal.GenerateGuidForType(typeof(ISimpleCOMObject)) ||
				riid == ManagedCOMLocalServer_Impl01.IID_IDispatch ||
				riid == ManagedCOMLocalServer_Impl01.IID_IUnknown)
			{
				SimpleCOMObject SimpleCOMObject_New = new SimpleCOMObject();

				ppvObject = Marshal.GetComInterfaceForObject(SimpleCOMObject_New, typeof(ISimpleCOMObject));
			} 
			else
			{
				throw new COMException("No interface",  unchecked((int) 0x80004002));
			}
		}
	}
}
