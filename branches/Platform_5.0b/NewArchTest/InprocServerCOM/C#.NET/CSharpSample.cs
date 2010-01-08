//#define ForV2

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using Interop.IAscomSample;


namespace ASCOM.InprocServerCOM
{
	//
	// The Guid attribute sets the CLSID for ASCOM.InprocServerCOM.CSharpSample
	// The ClassInterface/None addribute prevents an empty interface called
	// _CSharpSample from being created and used as the [default] interface
	//
	[Guid("D3D36E2E-5378-4a47-BA4B-9620FEAE224E")]
	[ClassInterface(ClassInterfaceType.None)]
	public class CSharpSample : IAscomSample
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
		private SampleEnumType m_enumVal;

		//
		// Constructor - Must be public for COM registration!
		//
		public CSharpSample()
		{
			m_X = m_Y = Double.NaN;										// [sentinels]
			m_enumVal = SampleEnumType.sampleType1;
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

#if ForV2
		protected double calcArea()
		{
			checkX();
			checkY();
			return m_X * m_Y;
		}
#endif

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

		SampleEnumType IAscomSample.EnumTest
		{
			get { return m_enumVal; }
			set { m_enumVal = value; }
		}

#if ForV2
		double IAscomSample.Area
		{
			get { return calcArea(); }
		}

		double IAscomSample.CalculateArea(double X, double Y)
		{
			m_X = X;
			m_Y = Y;
			return calcArea();
		}

#endif

	}
}
