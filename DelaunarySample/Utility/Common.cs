using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{

	public static class Common
	{

		public static double ToRoundDown(double dValue, int iDigits)
		{
			double dCoef = System.Math.Pow(10, iDigits);

			return dValue > 0 ? System.Math.Floor(dValue * dCoef) / dCoef :
								System.Math.Ceiling(dValue * dCoef) / dCoef;
		}


		
		public static float GetDist(PVector p1, PVector p2)
		{
			float width = p1.X - p2.X;
			float height = p1.Y - p2.Y;
			float depth = p1.Z - p2.Z;

			float dist = (float)( Math.Sqrt(Math.Pow(width, 2) +
												Math.Pow(height, 2) +
												Math.Pow(depth, 2)) );

			return dist;
		}
	}
}
