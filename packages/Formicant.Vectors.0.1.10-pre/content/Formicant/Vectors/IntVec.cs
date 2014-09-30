using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Formicant
{
	public partial struct IntVec : IEquatable<IntVec>, IComparable<IntVec>
	{
		#region Creation

		public IntVec(int u, int v)
			: this()
		{
			U = u;
			V = v;
		}

		#endregion

		#region Properties

		public int U { [Pure] get; private set; }
		public int V { [Pure] get; private set; }

		#endregion

		#region Constants

		public static readonly IntVec Zero = new IntVec();
		public static readonly IntVec UnitU = new IntVec(1, 0);
		public static readonly IntVec UnitV = new IntVec(0, 1);
		public static readonly IntVec UnitUV = new IntVec(1, 1);
		public static readonly IntVec MinValue = new IntVec(int.MinValue, int.MinValue);
		public static readonly IntVec MaxValue = new IntVec(int.MaxValue, int.MaxValue);

		#endregion

		#region Equality

		public static bool operator ==(IntVec uv1, IntVec uv2)
		{
			return uv1.U == uv2.U && uv1.V == uv2.V;
		}

		public static bool operator !=(IntVec uv1, IntVec uv2)
		{
			return !(uv1 == uv2);
		}

		[Pure]
		public bool Equals(IntVec uv)
		{
			return U == uv.U && V == uv.V;
		}

		[Pure]
		public override bool Equals(object obj)
		{
			return obj is IntVec && Equals((IntVec)obj);
		}

		[Pure]
		public override int GetHashCode()
		{
			unchecked { return (U * 21851) ^ V; }
		}

		#endregion

		#region Inequality (by V, then by U)

		public static bool operator <=(IntVec uv1, IntVec uv2)
		{
			return uv1.V < uv2.V || uv1.V == uv2.V && uv1.U <= uv2.U;
		}

		public static bool operator >=(IntVec uv1, IntVec uv2)
		{
			return uv1.V > uv2.V || uv1.V == uv2.V && uv1.U >= uv2.U;
		}

		public static bool operator <(IntVec uv1, IntVec uv2)
		{
			return uv1.V < uv2.V || uv1.V == uv2.V && uv1.U < uv2.U;
		}

		public static bool operator >(IntVec uv1, IntVec uv2)
		{
			return uv1.V > uv2.V || uv1.V == uv2.V && uv1.U > uv2.U;
		}

		[Pure]
		public int CompareTo(IntVec other)
		{
			return V != other.V
				? V < other.V ? -1 : 1
				: U.CompareTo(other.U);
		}

		#endregion

		#region Arithmetics

		[Pure]
		public IntVec Negate()
		{
			return -this;
		}

		[Pure]
		public IntVec Add(IntVec uv)
		{
			return this + uv;
		}

		[Pure]
		public IntVec Subtract(IntVec uv)
		{
			return this - uv;
		}

		[Pure]
		public IntVec MultiplyBy(int a)
		{
			return this * a;
		}

		[Pure]
		public IntVec MultiplyBy(IntVec uv)
		{
			return this * uv;
		}

		public static IntVec operator +(IntVec uv)
		{
			return uv;
		}

		public static IntVec operator -(IntVec uv)
		{
			return new IntVec(-uv.U, -uv.V);
		}

		public static IntVec operator +(IntVec uv1, IntVec uv2)
		{
			return new IntVec(uv1.U + uv2.U, uv1.V + uv2.V);
		}

		public static IntVec operator -(IntVec uv1, IntVec uv2)
		{
			return new IntVec(uv1.U - uv2.U, uv1.V - uv2.V);
		}

		public static IntVec operator *(int a, IntVec uv)
		{
			return new IntVec(a * uv.U, a * uv.V);
		}

		public static IntVec operator *(IntVec uv, int a)
		{
			return a * uv;
		}

		public static IntVec operator *(IntVec uv1, IntVec uv2)
		{
			return new IntVec(uv1.U * uv2.U, uv1.V * uv2.V);
		}

		[Pure]
		public IntVec TurnAnticlockwise()
		{
			return new IntVec(-V, U);
		}

		[Pure]
		public IntVec TurnClockwise()
		{
			return new IntVec(V, -U);
		}

		#endregion

		#region To Vector2

		[Pure]
		public Vector2 ToVector2()
		{
			return new Vector2(U, V);
		}

		#endregion

		#region ToString

		[Pure]
		public override string ToString()
		{
			return "({0}, {1})".Fmt(U, V);
		}

		#endregion
	}
}
