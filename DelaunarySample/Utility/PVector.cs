using System;

namespace Utility
{
	public class PVector
	{
		/// <summary>
		/// X Point
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// Y Point
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		/// Z Point
		/// </summary>
		public float Z { get; set; }

		public PVector(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public PVector Cross(PVector src)
		{
			float crossX = this.Y * src.Z - this.Z * src.Y;
			float crossY = this.Z * src.X - this.X * src.Z;
			float crossZ = this.X * src.Y - this.Y * src.X;

			return new PVector(crossX, crossY, crossZ);
		}

		public float Dot(PVector src)
		{
			return X * ( src.X ) + Y * src.Y + Z * src.Z;
		}

		public PVector Sub(PVector src)
		{
			PVector dst = new PVector(this.X - src.X, this.Y - src.Y, this.Z - src.Z);
			return dst;
		}

		public PVector GetNormalization()
		{
			double length = Math.Sqrt(( X * X ) + ( Y * Y ) + ( Z * Z ));

			PVector dst = new PVector(this.X / (float)length,
									  this.Y / (float)length,
									  this.Z / (float)length);

			return dst;
		}

		public bool Equals(PVector src)
		{
			if ( this.X == src.X && this.Y == src.Y && this.Z == src.Z ) return true;
			else return false;
		}
	}
}