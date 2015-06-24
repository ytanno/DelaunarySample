using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	public class Tetrahedron
	{
	
		public PVector P1 { get; private set; }

	
		public PVector P2 { get; private set; }

		
		public PVector P3 { get; private set; }

		
		public PVector P4 { get; private set; }


		/// <summary>
		/// four Points
		/// </summary>
		public PVector[] Vertices { get; set; }

		/// <summary>
		/// center of outer circle
		/// </summary>
		public PVector O { get; private set; }

		/// <summary>
		/// radius of outer circle
		/// </summary>
		public float R { get; private set; }

		
		public Tetrahedron(PVector p1, PVector p2, PVector p3, PVector p4)
		{
			Vertices = new PVector[4];

			P1 = p1;
			Vertices[0] = p1;

			P2 = p2;
			Vertices[1] = p2;

			P3 = p3;
			Vertices[2] = p3;

			P4 = p4;
			Vertices[3] = p4;

			getCenterCircumcircle();
		}



		/// <summary>
		/// calculate radius and circle of outer circle from tetrahedron points 
		/// ref URL　http://www.openprocessing.org/sketch/31295
		/// </summary>
		private void getCenterCircumcircle()
		{
			//translation from P1
			double[,] A = new double[,] {
			{P2.X - P1.X, P2.Y-P1.Y, P2.Z-P1.Z},
			{P3.X - P1.X, P3.Y-P1.Y, P3.Z-P1.Z},
			{P4.X - P1.X, P4.Y-P1.Y, P4.Z-P1.Z}
		};
			//I did not understand this mean
			double[] b = new double[]{
			0.5 * (P2.X*P2.X - P1.X*P1.X + P2.Y*P2.Y - P1.Y*P1.Y + P2.Z*P2.Z - P1.Z*P1.Z),
			0.5 * (P3.X*P3.X - P1.X*P1.X + P3.Y*P3.Y - P1.Y*P1.Y + P3.Z*P3.Z - P1.Z*P1.Z),
			0.5 * (P4.X*P4.X - P1.X*P1.X + P4.Y*P4.Y - P1.Y*P1.Y + P4.Z*P4.Z - P1.Z*P1.Z)
		};

			//solve 
			double[] x = new double[3];
			if ( gauss(A, b, x) == 0 )
			{
				O = null;
				R = -1;
			}
			else
			{
				O = new PVector((float)x[0], (float)x[1], (float)x[2]);
				R = Common.GetDist(O, P1);
			}
		}

		private double gauss(double[,] a, double[] b, double[] x)
		{
			int n = a.GetLength(0);
			int[] ip = new int[n];
			double det = lu(a, ip);

			if ( det != 0 ) { solve(a, b, ip, x); }
			return det;
		}

	
		private double lu(double[,] a, int[] ip)
		{
			int n = a.GetLength(0);
			double[] weight = new double[n];

			for ( int k = 0; k < n; k++ )
			{
				ip[k] = k;
				double u = 0;

				//calculate abs of max in line
				for ( int j = 0; j < n; j++ )
				{
					double t = Math.Abs(a[k, j]);
					if ( t > u ) u = t;
				}
				if ( u == 0 ) return 0;
				weight[k] = 1 / u;
			}


			double det = 1;
			for ( int k = 0; k < n; k++ )
			{
				double u = -1;
				int m = 0;

				//caluclate line and value of max (abs/weight) in top triangular matrix
				for ( int i = k; i < n; i++ )
				{
					int ii = ip[i];
					double t = Math.Abs(a[ii, k]) * weight[ii];
					if ( t > u ) { u = t; m = i; }
				}

				int ik = ip[m];

				//row and col number is not same
				if ( m != k )
				{
					//replace line
					ip[m] = ip[k]; ip[k] = ik;
					det = -det;
				}
				u = a[ik, k]; det *= u;
				if ( u == 0 ) return 0;

				//replace format
				//v1 v2 v3
				//0  v4 v5
				//0  0  v6 
				

				for ( int i = k + 1; i < n; i++ )
				{
					int ii = ip[i];
					double t = ( a[ii, k] /= u );
					for ( int j = k + 1; j < n; j++ )
					{
						//a[i][k] -a[k][k](a[i][k] / a[k][k])
						//ref http://www.kata-lab.itc.u-tokyo.ac.jp/OpenLecture/SP20110118.pdf of 外積形式ガウス法参考
						a[ii, j] -= t * a[ik, j];
					}
				}
			}
			return det;
		}


	
		private void solve(double[,] a, double[] b, int[] ip, double[] x)
		{
			int n = a.GetLength(0);

			//if 3x3
			// x = b1
			// y = b2 - a21 * b1
			// z = b3 - a31 * b1 - a32 * b2
			for ( int i = 0; i < n; i++ )
			{
				int ii = ip[i]; double t = b[ii];
				for ( int j = 0; j < i; j++ ) t -= a[ii, j] * x[j];
				x[i] = t;
			}

			
			//v1 v2 v3    x     b1 
			//0  v4 v5  * y  =  b2 - a21 * b1
			//0  0  v6    z  =  b3 - a31 * b1 - a32 * b2 
			for ( int i = n - 1; i >= 0; i-- )
			{
				double t = x[i]; int ii = ip[i];
				for ( int j = i + 1; j < n; j++ ) t -= a[ii, j] * x[j];
				x[i] = t / a[ii, i];
			}
		}

		public bool Equal(Tetrahedron src)
		{
			var check = false;
			var hitCount = 0;

			for ( int i = 0; i < this.Vertices.Count(); i++ )
			{
				for ( int j = 0; j < src.Vertices.Count(); j++ )
				{
					if ( this.Vertices[i].Equals(src.Vertices[j]) )
					{
						hitCount++;
					}
				}
			}

			if ( hitCount == 4 ) check = true;
			return check;
		}

	}
}
