using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace ASCOM.DeviceHub
{
	public class DomeControl : NormalizedValueBase
	{
		// ---------------------------------------------------------------------
		// Copyright © 2012 Chris Rowland
		// 
		// Permission is hereby granted to use this Software for any purpose
		// including combining with commercial products, creating derivative
		// works, and redistribution of source or binary code, without
		// limitation or consideration. Any redistributed copies of this
		// Software must include the above Copyright Notice.
		// 
		// THIS SOFTWARE IS PROVIDED "AS IS". I MAKE NO
		// WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
		// SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
		// ---------------------------------------------------------------------
		// 
		// ==============
		// DomeControl.cs
		// ==============
		// 
		// Written:  2012/05/06   Chris Rowland <chris.rowland@dsl.pipex.com>
		// 
		// This class gets the dome pointing direction using Paul Bourke's
		// algorithim for determining the intersections of a sphere and a line.
		// 
		// The line starts at the scope OTA position and goes in the direction
		// the scope is looking.
		// The sphere represents the dome, there should be one intersection which
		// is where the scope pointing direction intercepts the sphere.
		// 
		// Edits:
		// 
		// When       Who     What
		// ---------  ---     --------------------------------------------------
		// 2012/05/06 cdr     Initial edit
		// 2018/10/14 rdb	  Converted to C# and refactored to incorporate into POTH replacement application
		// -----------------------------------------------------------------------------

		// Initialization variables

		private Point3D _scopeOffset;
		private double _domeRadius;
		private double _otaOffset;
		private double _siteLatitude;
		private bool _isGem;

		public DomeControl( DomeLayoutSettings layout, double siteLatitude )
		{
			_domeRadius = layout.DomeRadius;
			_otaOffset = layout.GemAxisOffset;
			_isGem = _otaOffset > 0.0;
			_scopeOffset =  new Point3D( layout.DomeScopeOffset.Y, layout.DomeScopeOffset.X, layout.DomeScopeOffset.Z );
			_siteLatitude = siteLatitude;
		}

		/// <summary>
		/// Gets the dome azimuth that is required so the scope can see out using the
		/// scope pointing direction, the mount parameters and the dome radius
		/// </summary>
		/// <param name="scopePosition">the azimuth and altitude of the telescope</param>
		/// <param name="hourAngle">The hour angle in degrees</param>
		/// <param name="isPierWest">if set to <c>true</c> [is pier west].</param>
		/// <returns>the dome azimuth in degrees</returns>
		public Point DomePosition( Point scopePosition, double hourAngle, bool isPierWest )
		{
			Point domePosition;

			// Get the ota position with respect to the centre of the dome, allowing for the offsets,
			// the scope direction, and the latitude

			Point3D otaPosn = OtaPosition( hourAngle, isPierWest );

			domePosition = DomeAltAzm( scopePosition, otaPosn );

			// Set the altitude to the range -180 to +180

			domePosition.Y = NormalizeValue( domePosition.Y, -180.0, 180.0 );

			// Set the azimuth to the range 0 to 360 degrees

			domePosition.X = NormalizeValue( domePosition.X, 0.0, 360.0 );

			//Debug.WriteLine( "DomeAzimuth: scopePosition ({0:F5}, {1:F5}), Ha: {2:F5}, isPierWest: {3} = domePosition ({4:F5},{5:F5})"
			//				, scopePosition.X, scopePosition.Y, hourAngle, isPierWest, domePosition.X, domePosition.Y );

			return domePosition;
		}

		/// <summary>
		/// Return the OTA position wrt the dome centre
		/// X is to the N,
		/// Y is to the W,
		/// Z is up
		/// Currently assumes the Nothern hemisphere
		/// </summary>
		/// <param name="ha">hour angle in degrees</param>
		/// <param name="isPierWest"></param>
		private Point3D OtaPosition( double ha, bool isPierWest )
		{
			// Get the position of the mount

			Point3D p1 = _scopeOffset;
			p1.Y = -p1.Y; // Change the sign of the y direction to match the WPF coordinate system

			Vector3D os = new Vector3D( 0, _otaOffset, 0 ); // OTA offset is along the dec axis

			if ( _otaOffset > 0.00001 )
			{
				if ( _isGem && isPierWest )
				{
					// Reverse the dec OTA offset

					os.Y = -os.Y;
				}

				// Rotate by Ha

				os = Rotate( os, new Point3D( -1, 0, 0 ), ha );

				// Rotate by latitude

				os = Rotate( os, new Point3D( 0, -1, 0 ), Math.Abs( _siteLatitude ) );
			}

			Point3D retval = Point3D.Add( p1, os );

			//Debug.WriteLine( "OtaPosition X:" + retval.X + ", Y:" + retval.Y + ", Z:" + retval.Z );

			return retval;
		}

		/// <summary>
		/// Determine the position of the dome opening using Paul Bourke's intersection of
		/// a ray and a sphere code.  The ray represents the scope pointing direction and the
		/// sphere the dome.  The OTA offset is determined from the mount offset and the
		/// scope pointing direction specifies the direction of the ray.
		/// </summary>
		/// <param name="scopeAltAz">scope pointing direction in Alt(y) Azm(x), as radians</param>
		/// <param name="otaPosn">OTA position wrt the centre of the dome, allowing for the Ha and Dec and the pier side</param>
		/// <returns>Dome Altitude(y), Azimuth(x), in degrees</returns>
		private Point DomeAltAzm( Point scopeAltAz, Point3D otaPosn )
		{
			Point retval;

			// Get the position of the OTA

			Point3D p1 = otaPosn;

			// Get the offset from the OTA position towards the scope pointing direction.

			Point3D dp = PolarToCartesian( scopeAltAz, _domeRadius * 4 );

			// The conversion is simplified because we are using the centre of the dome as the origin.

			double a = dp.X * dp.X + dp.Y * dp.Y + dp.Z * dp.Z;
			double b = 2 * ( dp.X * p1.X + dp.Y * p1.Y + dp.Z * p1.Z );
			double c = p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z;
			c -= _domeRadius * _domeRadius;
			double bb4ac = b * b - 4 * a * c;

			if ( Math.Abs( a ) < 0.000001 | bb4ac < 0 )
			{
				throw new Exception( "DomeControl - scope is not pointing out of the dome" );
			}

			double mu = ( -b + Math.Sqrt( bb4ac ) ) / ( 2 * a );

			if ( mu <= 0 || mu > 1.0 )
			{
				mu = ( -b - Math.Sqrt( bb4ac ) ) / ( 2 * a );
			}

			// There are potentially two points of intersection given by
			// p = p1 + mu1 (p2 - p1)
			// p = p1 + mu2 (p2 - p1)

			Vector3D vector = new Vector3D( p1.X + mu * dp.X, p1.Y + mu * dp.Y, p1.Z + mu * dp.Z );

			retval = CartesianToPolar( vector );

			//Debug.WriteLine( "DomeAltAzm Azm:" + retval.X + ", Alt:" + retval.Y );

			return retval;
		}

		/// <summary>
		/// Rotates the specified val.
		/// </summary>
		/// <param name="vector">The vector that's rotated</param>
		/// <param name="axis">A unit vector defining the axis direction, must be in the X or Y direction</param>
		/// <param name="angle">The rotation angle in degrees</param>
		private Vector3D Rotate( Vector3D vector, Point3D axis, double angle )
		{
			Vector3D retval = new Vector3D();

			double ar = D2R( angle );   // convert the angle to radians

			// Choose the axis to rotate about

			if ( Math.Abs( axis.X ) > Math.Abs( axis.Y ) )
			{
				if ( axis.X > 0 )
				{
					ar = -ar;
				}

				retval.X = vector.X;
				retval.Z = Math.Cos( ar ) * vector.Z - Math.Sin( ar ) * vector.Y;
				retval.Y = Math.Sin( ar ) * vector.Z + Math.Cos( ar ) * vector.Y;
			}
			else
			{
				if ( axis.Y < 0 )
				{
					ar = -ar;
				}

				retval.Y = vector.Y;
				retval.X = Math.Cos( ar ) * vector.X - Math.Sin( ar ) * vector.Z;
				retval.Z = Math.Sin( ar ) * vector.X + Math.Cos( ar ) * vector.Z;
			}

			return retval;
		}

		/// <summary>
		/// Converts angle in degres to radians.
		/// </summary>
		/// <param name="angle">The angle in degrees</param>
		/// <returns>the angle in radians</returns>
		private double D2R( double angle )
		{
			return angle * Globals.DEG_TO_RAD;
	    }

		/// <summary>
		/// Converts angle in radians to degrees
		/// </summary>
		/// <param name="rad">The angle in radians</param>
		/// <returns>the angle in degrees</returns>
		private double R2D( double rad )
		{
			return rad * Globals.RAD_TO_DEG;
	    }

		/// <summary>
		/// Converts an alt azm vector in radians and a distance in radius to an xyz coordinate
		/// </summary>
		/// <param name="azmAlt">The azm(X) and alt(Y) angles in degrees</param>
		/// <param name="radius">The radius.</param>
		/// <returns>XYZ vector, length is radius</returns>
		private Point3D PolarToCartesian( Point azmAlt, double radius )
		{
			Point3D retval;

			double azm = D2R( azmAlt.X );
			double alt = D2R( azmAlt.Y );

			retval = new Point3D( radius * Math.Cos( alt ) * Math.Cos( azm )
							, radius * Math.Cos( alt ) * Math.Sin( azm )
							, radius * Math.Sin( alt ) );

			return retval;
		}

		/// <summary>
		/// Converts a vector to azimuth and altitude in degrees
		/// </summary>
		/// <param name="vector">The vector.</param>
		/// <returns>azimuth and altitude in degrees</returns>
		private Point CartesianToPolar( Vector3D vector )
		{
			Point retval = new Point( R2D( Atn2( vector.Y, vector.X ) )
							, 90 - R2D( Atn2( Math.Sqrt( vector.X * vector.X + vector.Y * vector.Y ), vector.Z ) ) );

			return retval;
		}

		/// <summary>
		/// Atn2 function, returns angle in radians
		/// </summary>
		/// <param name="Y">The Y.</param>
		/// <param name="X">The X.</param>
		/// <returns>angle in radians</returns>
		private double Atn2( double Y, double X )
		{
			double retval;

			switch ( X )
			{
				case object _ when X > 0.0:
                {
					retval = Math.Atan( Y / X );
					break;
				}

				case object _ when X < 0.0:
                {
					retval = Math.Atan( Y / X ) + Math.PI * ( ( Y > 0 ) ? 1 : -1 );
					break;
				}

				default:
				{
					retval = ( Math.PI / 2.0 ) * Math.Sign( Y );
					break;
				}
			}

			return retval;
		}
	}
}
