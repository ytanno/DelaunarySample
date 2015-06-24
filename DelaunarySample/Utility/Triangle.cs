using System;

namespace Utility
{
	public class Triangle
	{
		public PVector V1 { get; set; }

		public PVector V2 { get; set; }

		public PVector V3 { get; set; }

		public PVector[] Vertics { get; set; }

		public Triangle(PVector v1, PVector v2, PVector v3)
		{
			this.V1 = v1;
			this.V2 = v2;
			this.V3 = v3;

			Vertics = new PVector[3];
			Vertics[0] = v1;
			Vertics[1] = v2;
			Vertics[2] = v3;
		}

		public PVector GetNormal()
		{
			PVector edge1 = new PVector(V2.X - V1.X, V2.Y - V1.Y, V2.Z - V1.Z);
			PVector edge2 = new PVector(V3.X - V1.X, V3.Y - V1.Y, V3.Z - V1.Z);

			PVector normal = edge1.Cross(edge2);

			return normal.GetNormalization();
		}

		public void TurnBack()
		{
			PVector tmp = this.V3;
			this.V3 = this.V1;
			this.V1 = tmp;

			this.Vertics[0] = this.V1;
			this.Vertics[2] = this.V3;
		}

		public bool Equals(Triangle src)
		{
			foreach ( var my in this.Vertics )
			{
				bool match = false;
				foreach ( var t in src.Vertics )
				{
					if ( my.Equals(t) ) match = true;
				}
				if ( !match ) return false;
			}
			return true;
		}

		public float GetArea()
		{
			PVector AB = this.V2.Sub(this.V1);
			PVector AC = this.V3.Sub(this.V1);
			PVector cross = AB.Cross(AC);
			double result = Math.Sqrt(cross.X * cross.X + cross.Y * cross.Y + cross.Z * cross.Z) / 2.0;
			return (float)result;
		}
	}
}