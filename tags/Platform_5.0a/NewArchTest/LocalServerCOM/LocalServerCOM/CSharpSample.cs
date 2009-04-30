using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using Interop.IAscomSample;

namespace ASCOM.LocalServerCOM
{
	//
	// The Guid attribute sets the CLSID for ASCOM.InprocServerCOM.CSharpSample
	// The ClassInterface/None addribute prevents an empty interface called
	// _CSharpSample from being created and used as the [default] interface
	//
	// This class is identical to that in the InprocServerCOM.CSharpSample
	// except for the derivation from ReferenceCountedObjectBase
	//
	[Guid("6D02B7BD-A2C9-4e39-86CB-E286A00ED4F9")]
	[ClassInterface(ClassInterfaceType.None)]
	public class CSharpSample : 
			ReferenceCountedObjectBase,
			IAscomSample
	{
		//
		// COM Error Constants
		//
		private const int SCODE_X_UNSET = unchecked((int)0x80040001);
		private const int SCODE_Y_UNSET = unchecked((int)0x80040002);
		private const string MSG_X_UNSET = "The value of X has not been set";
		private const string MSG_Y_UNSET = "The value of Y has not been set";

		//
		// Class private vars
		//
		private double m_X;
		private double m_Y;

		//
		// Constructor - Must be public for COM registration!
		//
		public CSharpSample()
		{
			m_X = m_Y = Double.NaN;										// [sentinels]
		}

		//
		// Class private functions
		//
		protected void checkX()
		{
			if(Double.IsNaN(m_X)) throw new COMException(MSG_X_UNSET, SCODE_X_UNSET);
		}

		protected void checkY()
		{
			if (Double.IsNaN(m_Y)) throw new COMException(MSG_Y_UNSET, SCODE_Y_UNSET);
		}

		protected double calcDiag()
		{
			checkX();
			checkY();
			return Math.Sqrt(m_X * m_X + m_Y * m_Y);
		}

		//
		// PUBLIC COM INTERFACE IAscomSample IMPLEMENTATION
		//
		double IAscomSample.X
		{
			get { checkX();  return m_X; }
			set { m_X = value; }
		}

		double IAscomSample.Y
		{
			get { checkY();  return m_Y; }
			set { m_Y = value; }
		}

		double IAscomSample.Diagonal
		{
			get { return calcDiag(); }
		}

		double IAscomSample.CalculateDiagonal(double X, double Y)
		{
			m_X = X;
			m_Y = Y;
			return calcDiag();
		}
	}

	#region Class Factory
	//
	// Here's where the magic is: Allow creation of an IAscomSample, 
	// or an IDispatch, or an IUnknown. 
	//
	internal class CSharpSampleClassFactory : ClassFactoryBase
	{
		public override void CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
		{
			LocalServerCOM.Trace("CSharpSampleClassFactory.CreateInstance().");
			LocalServerCOM.Trace("Requesting Interface: " + riid.ToString());

			if (riid == Marshal.GenerateGuidForType(typeof(IAscomSample)) ||
				riid == LocalServerCOM.IID_IDispatch ||
				riid == LocalServerCOM.IID_IUnknown)
			{
				ppvObject = Marshal.GetComInterfaceForObject(new CSharpSample(), typeof(IAscomSample));
			}
			else
			{
				throw new COMException("No interface", unchecked((int)0x80004002));
			}
		}
	}
	#endregion
}
