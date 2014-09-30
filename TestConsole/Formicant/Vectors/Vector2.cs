using System;
using JetBrains.Annotations;

namespace Formicant
{
	public partial struct Vector2 : IEquatable<Vector2>
	{
		#region Creation

		public Vector2(double x, double y)
			: this()
		{
			X = x;
			Y = y;
		}

		public static Vector2 Polar(double length, double argument)
		{
			return new Vector2(length * argument.Cos(), length * argument.Sin());
		}

		#endregion

		#region Properties

		public double X { [Pure] get; private set; }

		public double Y { [Pure] get; private set; }

		public double Length
		{
			[Pure] get { return (X.Sq() + Y.Sq()).Sqrt(); }
		}

		public double Argument
		{
			[Pure] get { return Y.AtgTo(X); }
		}

		#endregion

		#region Constants

		public static readonly Vector2 Zero = new Vector2(0, 0);

		public static readonly Vector2 UnitX = new Vector2(1, 0);

		public static readonly Vector2 UnitY = new Vector2(0, 1);

		public static readonly Vector2 UnitXY = new Vector2(1, 1);

		#endregion

		#region Equality

		public static bool operator ==(Vector2 u, Vector2 v)
		{
			return u.X == v.X && u.Y == v.Y;
		}

		public static bool operator !=(Vector2 u, Vector2 v)
		{
			return !(u == v);
		}

		[Pure]
		public bool Equals(Vector2 other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y);
		}

		[Pure]
		public override bool Equals(object obj)
		{
			return obj is Vector2 && Equals((Vector2)obj);
		}

		[Pure]
		public override int GetHashCode()
		{
			unchecked { return (X.GetHashCode() * 21851) ^ Y.GetHashCode(); }
		}

		#endregion

		#region Relations

		[Pure]
		public static bool AreAlmostEqual(Vector2 u, Vector2 v, double tolerance = Number.ε)
		{
			return Distance(u, v) <= tolerance;
		}

		[Pure]
		public bool AlmostEquals(Vector2 v, double tolerance = Number.ε)
		{
			return AreAlmostEqual(this, v, tolerance);
		}

		#endregion

		#region Arithmetics

		public static Vector2 operator +(Vector2 v)
		{
			return v;
		}

		public static Vector2 operator -(Vector2 v)
		{
			return new Vector2(-v.X, -v.Y);
		}

		public static Vector2 operator +(Vector2 u, Vector2 v)
		{
			return new Vector2(u.X + v.X, u.Y + v.Y);
		}

		public static Vector2 operator -(Vector2 u, Vector2 v)
		{
			return new Vector2(u.X - v.X, u.Y - v.Y);
		}

		public static Vector2 operator *(double a, Vector2 v)
		{
			return new Vector2(a * v.X, a * v.Y);
		}

		public static Vector2 operator *(Vector2 v, double a)
		{
			return new Vector2(v.X * a, v.Y * a);
		}

		public static Vector2 operator /(Vector2 v, double a)
		{
			return new Vector2(v.X / a, v.Y / a);
		}

		public static Vector2 operator *(Vector2 u, Vector2 v)
		{
			return new Vector2(u.X * v.X, u.Y * v.Y);
		}

		public static Vector2 operator /(Vector2 u, Vector2 v)
		{
			return new Vector2(u.X / v.X, u.Y / v.Y);
		}

		public static double ScalarProduct(Vector2 u, Vector2 v)
		{
			return u.X * v.X + u.Y * v.Y;
		}

		public static double ExteriorProduct(Vector2 u, Vector2 v)
		{
			return u.X * v.Y - u.Y * v.X;
		}

		public static double Distance(Vector2 u, Vector2 v)
		{
			return (v - u).Length;
		}

		public static double AngleBetween(Vector2 u, Vector2 v)
		{
			return (v.Argument - u.Argument).WrapAngle();
		}

		public static Vector2 Middle(Vector2 u, Vector2 v)
		{
			return (u + v) / 2;
		}

		[Pure]
		public Vector2 Add(Vector2 v)
		{
			return this + v;
		}

		[Pure]
		public Vector2 Subtract(Vector2 v)
		{
			return this - v;
		}

		[Pure]
		public Vector2 MultiplyBy(double a)
		{
			return this * a;
		}

		[Pure]
		public Vector2 DivideBy(double a)
		{
			return this / a;
		}

		[Pure]
		public Vector2 MultiplyBy(Vector2 v)
		{
			return this * v;
		}

		[Pure]
		public Vector2 DivideBy(Vector2 v)
		{
			return this / v;
		}

		[Pure]
		public double ScalarProductWith(Vector2 v)
		{
			return ScalarProduct(this, v);
		}

		[Pure]
		public double ExteriorProductWith(Vector2 v)
		{
			return ExteriorProduct(this, v);
		}

		[Pure]
		public Vector2 Normalize()
		{
			return this / Length;
		}

		[Pure]
		public double DistanceTo(Vector2 v)
		{
			return Distance(this, v);
		}

		[Pure]
		public double AngleTo(Vector2 v)
		{
			return AngleBetween(this, v);
		}

		[Pure]
		public Vector2 Turn(double angle)
		{
			return Polar(Length, Argument + angle);
		}

		[Pure]
		public Vector2 FlipX()
		{
			return new Vector2(-X, Y);
		}

		[Pure]
		public Vector2 FlipY()
		{
			return new Vector2(X, -Y);
		}

		[Pure]
		public Vector2 MiddleTo(Vector2 v)
		{
			return Middle(this, v);
		}

		#endregion

		#region Coordinate-wise

		[Pure]
		public Vector2 CoordWise(Func<double, double> func)
		{
			return new Vector2(func(X), func(Y));
		}

		[Pure]
		public Vector2 CoordWiseTo(Vector2 v, Func<double, double, double> func)
		{
			return new Vector2(func(X, v.X), func(Y, v.Y));
		}

		#endregion

		#region To IntVec

		[Pure]
		public IntVec ToIntVec()
		{
			return new IntVec(X.RoundToInt(), Y.RoundToInt());
		}

		#endregion

		#region ToString

		[Pure]
		public override string ToString()
		{
			return "({0}, {1})".Fmt(X, Y);
		}

		#endregion
	}
}
