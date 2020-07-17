using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace ASCOM.DeviceHub
{
	public class DomeSynchronize : NormalizedValueBase
	{
		private double _domeRadius;
		private double _otaOffset;
		private double _siteLatitudeRadians;
		private bool _isGem;
		private Vector3D _mountOffset;
		private Vector3D _domeCenter;

		public DomeSynchronize( DomeLayoutSettings layout, double siteLatitude )
		{
			// Define the origin of the dome.

			_domeCenter = new Vector3D( 0.0, 0.0, 0.0 );

			_domeRadius = layout.DomeRadius;
			_otaOffset = layout.GemAxisOffset;
			_isGem = _otaOffset > 0.0;
			_mountOffset = new Vector3D( layout.DomeScopeOffset.X, layout.DomeScopeOffset.Y, layout.DomeScopeOffset.Z );
			_siteLatitudeRadians = siteLatitude * Globals.DEG_TO_RAD;
		}

		/// <summary>
		/// Gets the dome azimuth that is required so the scope can see out using the
		/// scope pointing direction, the mount parameters, and the dome radius.
		/// 
		/// This method uses an algorithm that solves equations that are derived in the Wikipedia article
		/// entitled Line-sphere intersection (https://en.wikipedia.org/wiki/Line%E2%80%93sphere_intersection)
		/// 
		/// It uses the .NET 3D Vector class and a bit of trigonometry.
		/// 
		/// Based on the article, here is the formula that we are solving.
		/// 
		/// d = -(l · (o - c)) + sqrt((l · (o - c))² - (‖o - c‖² - r²))
		/// 
		/// where 
		/// 
		/// d is distance from the OTA's COG to the point where the line intersects the dome,
		/// l is unit vector of a line along the pointing direction of the OTA
		/// o is the vector to the intersection of the RA and Dec axes (the mount's COG)
		/// c is the vector to the center of the dome
		/// 
		/// Note that the · operator is used to represent the Dot Product of 2 vectors
		/// and that the ‖ operator is used to represent the length/magnitude of a vector.
		/// 
		/// Thanks to Tom How for helping with this method, both with advice and finished code.
		/// </summary>
		/// <param name="scopePosition">the azimuth and altitude of the telescope, in degrees</param>
		/// <param name="hourAngle">The hour angle in degrees</param>
		/// <param name="isPierWest">if set to <c>true</c> [is pier west].</param>
		/// <returns>the dome altitude and azimuth in degrees</returns>
		public Point DomePosition( Point scopePosition, double hourAngle, bool isPierWest )
		{
			Point retval = new Point( 0.0, 0.0 );

			// Convert the formal arguments to the correct units.

			double altitudeRadians = scopePosition.Y * Globals.DEG_TO_RAD;
			double azimuthRadians = scopePosition.X * Globals.DEG_TO_RAD;
			double hourAngleRadians = hourAngle * Globals.DEG_TO_RAD;
			double pierFactor = isPierWest ? -1.0 : 1.0;

			// Define the vector to the intersection of the RA and DEC axes in our 3D coordinate system.
			// This is the mount's center of gravity (COG).

			Vector3D mountCOG = _domeCenter + _mountOffset;

			// Find the intersection of the telescope (declination) optical axis relative to the mount's COG
			// and adjust for which side of the pier we are on.

			Vector3D opticalCOG = ToCartesianCoordinates( _otaOffset, hourAngleRadians, _siteLatitudeRadians );
			opticalCOG = opticalCOG * pierFactor;

			// Add the 2 vectors to get the vector from the origin to the opticalCOG.

			Vector3D otaOrigin = mountCOG + opticalCOG;

			// Calculate the pointing direction of the OTA, expressed as a unit vector.

			double inc = Math.PI / 2 - altitudeRadians;
			double az = Math.PI / 2 - azimuthRadians;

			double xm = Math.Sin( inc ) * Math.Cos( az );
			double ym = Math.Sin( inc ) * Math.Sin( az );
			double zm = Math.Cos( inc );

			Vector3D scopeUnitVector = new Vector3D( xm, ym, zm );
			scopeUnitVector.Normalize();  // l

			// Calculate the distance along the unit vector where the line intersects the sphere.

			Vector3D vectorDiff = otaOrigin - _domeCenter; // (o - c)
			double dotProduct = Vector3D.DotProduct( scopeUnitVector, vectorDiff ); // l · (o - c)

			// Calculate the value under the square root operator
			// ( l · (o - c))² - ‖o - c‖² + r²
			// Note that vectorDiff.LengthSquared is the ‖o - c‖² term.
			// The ‖ symbol denotes the magnitude (length) of the vector.

			double underRoot = ( dotProduct * dotProduct ) - vectorDiff.LengthSquared + ( _domeRadius * _domeRadius );
			double distance = -dotProduct + Math.Sqrt( underRoot );

			// Calculate the point on the dome where the line through the scope intersects.

			Vector3D intersection = _domeCenter + otaOrigin + ( scopeUnitVector * distance );

			// Finally calculate the azimuth and altitude of that intersection point.

			double azimuth = Math.PI / 2 - Math.Atan2( intersection.Y, intersection.X );

			if ( azimuth < 0 )
			{
				azimuth += 2 * Math.PI;
			}

			double altitude = Math.PI / 2 - Math.Acos( intersection.Z / _domeRadius );

			// Convert the azimuth and altitude to degrees and return to the caller.

			retval.X = azimuth * Globals.RAD_TO_DEG;
			retval.Y = altitude * Globals.RAD_TO_DEG;

			return retval;
		}
	
		/// <summary>
		/// Convert 3D polar coordinates to Cartesian (XYZ) coordinates.
		/// </summary>
		/// <param name="radius">length of the sperical vector</param>
		/// <param name="phi">hour angle, in radians</param>
		/// <param name="theta">in radians</param>
		/// <returns></returns>
		private Vector3D ToCartesianCoordinates( double radius, double phi, double theta )
		{
			// phi is in the x-y plane, relative to the x-axis
			// theta is the zenith angle from the z-axis to the radial line.

			double x = radius * Math.Cos( phi );
			double radiusProjection = radius * Math.Sin( phi );
			double z = radiusProjection * Math.Cos( theta );
			double y = -radiusProjection * Math.Sin( theta );

			return new Vector3D( x, y, z );
		}
	}
}
