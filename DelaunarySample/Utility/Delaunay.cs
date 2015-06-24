using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
	/// <summary>
	/// <para>Calculate Delaunay Diagram</para>
	/// <para>if you use a lot of point data, it take many time</para>
	/// </summary>
	public class Delaunay
	{
		/// <summary>
		/// Tetrahedron of Involved all point
		/// </summary>
		public Tetrahedron FirstTetra { private set; get; }

		/// <summary>
		/// Tetrahedron List of Delaunay Triangulation
		/// </summary>
		public List<Tetrahedron> TetraList { private set; get; }

		/// <summary>
		/// All Triangle List made from Tetrahedrone List
		/// </summary>
		public List<Triangle> AllTriangleList { private set; get; }

		/// <summary>
		/// Outer Triangle List made from Tetrahedrone List
		/// </summary>
		public List<Triangle> OutsideTriangleList { private set; get; }

		/// <summary>
		/// <para>if a circumscribed circle has more than four points, Delaunay Triangulation droped in quality.</para>

		/// </summary>
		/// <param name="pVectorList">points list</param>
		public Delaunay(List<PVector> pVectorList)
		{
			this.FirstTetra = GetFirstTetra(pVectorList);

			this.TetraList = GetTetraList(pVectorList);

			this.AllTriangleList = TetraToTriangle(this.TetraList);

			this.OutsideTriangleList = FilterOutSidePoint(this.AllTriangleList);
		}

		/// <summary>
		/// <para>Remove same Triangle and inside Triangle</para>
		/// </summary>
		/// <returns>Outer Triangle List</returns>
		private List<Triangle> FilterOutSidePoint(List<Triangle> allTriangleList)
		{
			List<Triangle> dstList = new List<Triangle>();
			bool[] isSameTriangle = new bool[allTriangleList.Count];
			for ( int i = 0; i < allTriangleList.Count - 1; i++ )
			{
				for ( int j = i + 1; j < allTriangleList.Count; j++ )
				{
					if ( allTriangleList[i].Equals(allTriangleList[j]) )
					{
						isSameTriangle[i] = isSameTriangle[j] = true;
					}
				}
			}

			for ( int i = 0; i < isSameTriangle.Length; i++ )
			{
				if ( !isSameTriangle[i] ) dstList.Add(allTriangleList[i]);
			}
			return dstList;
		}

		/// <summary>
		/// Get Outer Triangle List from TetraList
		/// </summary>
		/// <returns>Outer Triangle List</returns>
		private List<Triangle> TetraToTriangle(List<Tetrahedron> tetraList)
		{
			List<Triangle> triList = new List<Triangle>();

			// calculate face
			foreach ( var tetra in tetraList )
			{
				PVector v1 = tetra.P1;
				PVector v2 = tetra.P2;
				PVector v3 = tetra.P3;
				PVector v4 = tetra.P4;

				Triangle tri1 = new Triangle(v1, v2, v3);
				Triangle tri2 = new Triangle(v1, v3, v4);
				Triangle tri3 = new Triangle(v1, v4, v2);
				Triangle tri4 = new Triangle(v4, v3, v2);

				//set direction face
				PVector n;
				n = tri1.GetNormal();

				if ( n.Dot(v1) > n.Dot(v4) ) tri1.TurnBack();

				n = tri2.GetNormal();
				if ( n.Dot(v1) > n.Dot(v2) ) tri2.TurnBack();

				n = tri3.GetNormal();
				if ( n.Dot(v1) > n.Dot(v3) ) tri3.TurnBack();

				n = tri4.GetNormal();
				if ( n.Dot(v2) > n.Dot(v1) ) tri4.TurnBack();

				triList.Add(tri1);
				triList.Add(tri2);
				triList.Add(tri3);
				triList.Add(tri4);
			}

			return triList;
		}

		/// <summary>
		/// Calculate Delaunay Triangulation
		/// </summary>
		/// <param name="pVectorList">AllPoints</param>
		/// <returns>Tetrahedrone List</returns>
		private List<Tetrahedron> GetTetraList(List<PVector> pVectorList)
		{
			List<Tetrahedron> tetraList = new List<Tetrahedron>();
			tetraList.Add(this.FirstTetra);

			List<Tetrahedron> tmpTList = new List<Tetrahedron>();
			List<Tetrahedron> newTList = new List<Tetrahedron>();
			List<Tetrahedron> removeTList = new List<Tetrahedron>();

			foreach ( var point in pVectorList )
			{
				tmpTList.Clear();
				newTList.Clear();
				removeTList.Clear();

				foreach ( var t in tetraList )
				{
					if ( ( t.O != null ) &&
						( t.R > Common.GetDist(point, t.O) ) )
					{
						tmpTList.Add(t);
					}
				}

				foreach ( var t1 in tmpTList )
				{
					tetraList.Remove(t1);

					PVector v1 = t1.P1;
					PVector v2 = t1.P2;
					PVector v3 = t1.P3;
					PVector v4 = t1.P4;

					newTList.Add(new Tetrahedron(v1, v2, v3, point));
					newTList.Add(new Tetrahedron(v1, v2, v4, point));
					newTList.Add(new Tetrahedron(v1, v3, v4, point));
					newTList.Add(new Tetrahedron(v2, v3, v4, point));
				}

				bool[] isRedundancy = new bool[newTList.Count];
				for ( int i = 0; i < newTList.Count - 1; i++ )
				{
					for ( int j = i + 1; j < newTList.Count; j++ )
					{
						if ( newTList[i].Equal(newTList[j]) )
						{
							isRedundancy[i] = isRedundancy[j] = true;
						}
					}
				}

				for ( int i = 0; i < newTList.Count; i++ )
				{
					if ( !isRedundancy[i] ) tetraList.Add(newTList[i]);
				}
			}

			PVector[] outer = new PVector[] { this.FirstTetra.P1, this.FirstTetra.P2, this.FirstTetra.P3, this.FirstTetra.P4 };
			bool isOuter = false;
			var count = 0;
			for ( int i = tetraList.Count - 1; i >= 0; i-- )
			{
				isOuter = false;
				foreach ( var t4Point in tetraList[i].Vertices )
				{
					foreach ( var outerPoint in outer )
					{
						if ( t4Point.X == outerPoint.X &&
							t4Point.Y == outerPoint.Y &&
							t4Point.Z == outerPoint.Z )
						{
							isOuter = true;
						}
					}
				}
				if ( isOuter )
				{
					tetraList.RemoveAt(i);
					count++;
					Console.WriteLine(count);
				}
			}

			return tetraList;
		}

		/// <summary>
		/// calculate tetrahedrone involved all points
		/// </summary>
		/// <param name="pVectorList">xyz point list</param>
		/// <returns>tetrahedrone</returns>
		private static Tetrahedron GetFirstTetra(List<PVector> pVectorList)
		{
			float xMin = pVectorList.Min(value => value.X);
			float yMin = pVectorList.Min(value => value.Y);
			float zMin = pVectorList.Min(value => value.Z);

			float xMax = pVectorList.Max(value => value.X);
			float yMax = pVectorList.Max(value => value.Y);
			float zMax = pVectorList.Max(value => value.Z);

			//rectangular solid size
			float width = xMax - xMin;
			float height = yMax - yMin;
			float depth = zMax - zMin;

			//center of globe
			float cX = width / 2 + xMin;
			float cY = height / 2 + yMin;
			float cZ = depth / 2 + zMin;
			PVector center = new PVector(cX, cY, cZ);

			//radius
			//0.1f is addition
			float radius = Common.GetDist(new PVector(xMax, yMax, zMax), new PVector(xMin, yMin, zMin)) / 2 + 0.1f;

			PVector p1 = new PVector(center.X, center.Y + 3.0f, center.Z);
			PVector p2 = new PVector(center.X + (float)2 * (float)Math.Sqrt(2) * radius, center.Y - radius, center.Z);
			PVector p3 = new PVector(-(float)Math.Sqrt(2) * radius + center.X, -radius + center.Y, (float)Math.Sqrt(6) * radius + center.Z);
			PVector p4 = new PVector(-(float)Math.Sqrt(2) * radius + center.X, -radius + center.Y, -(float)Math.Sqrt(6) * radius + center.Z);

			return new Tetrahedron(p1, p2, p3, p4);
		}
	}
}