using System;
using System.Text;
using System.Windows.Forms;  // For using MessageBox.
using Interop.ISimpleCOMObject; // In order to implement ISimpleCOMObject.
//using System.Runtime.InteropServices;

namespace SimpleCOMObject_CSharpImpl // 1st part of ProgID for specific implementation
{
	/// <summary>
	/// Summary description for SimpleCOMObject.
	/// </summary>
	//[Guid("62D910F8-1BBA-4c16-8C86-445F9A7AD007")]
	public class SimpleCOMObject : ISimpleCOMObject	// 2nd part of ProgId of specific implementation
	{
	    private int m_iLongProperty;
	    
		public SimpleCOMObject()
		{
			//
			// TODO: Add constructor logic here
			//
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
		  
		  MessageBox.Show(sb.ToString(), "SimpleCOMObject_CSharpImpl.SimpleCOMObject");
		}
	}
}
