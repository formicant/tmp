using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Formicant
{
	public struct IntBounds : IEnumerable<IntVec>, IEquatable<IntBounds>
	{
		#region Creation

		public IntBounds(IntVec topLeft, IntVec bottomRight)
			: this()
		{
			TopLeft = topLeft;
			BottomRight = bottomRight;
		}

		public IntBounds(int left, int right, int top, int bottom)
			: this(new IntVec(left, top), new IntVec(right, bottom)) { }

		public IntBounds(IntVec size)
			: this(IntVec.Zero, size - IntVec.UnitUV) { }

		public IntBounds(int width, int height)
			: this(IntVec.Zero, new IntVec(width - 1, height - 1)) { }

		public static IntBounds GetBounds(IEnumerable<IntVec> uvs)
		{
			return uvs.Aggregate(Empty, (current, uv) => current.ExpandTo(uv));
		}

		#endregion

		#region Constants

		public static readonly IntBounds Empty = new IntBounds(IntVec.MaxValue, IntVec.MinValue);
		public static readonly IntBounds Full = new IntBounds(IntVec.MinValue, IntVec.MaxValue);

		#endregion

		#region Properties

		public IntVec TopLeft { [Pure] get; private set; }

		public IntVec BottomRight { [Pure] get; private set; }

		public IntVec TopRight
		{
			[Pure] get { return new IntVec(Right, Top); }
		}

		public IntVec BottomLeft
		{
			[Pure] get { return new IntVec(Left, Bottom); }
		}

		public int Left
		{
			[Pure] get { return TopLeft.U; }
		}

		public int Right
		{
			[Pure] get { return BottomRight.U; }
		}

		public int Top
		{
			[Pure] get { return TopLeft.V; }
		}

		public int Bottom
		{
			[Pure] get { return BottomRight.V; }
		}

		public int Width
		{
			[Pure] get { return 1 + Right - Left; }
		}

		public int Height
		{
			[Pure] get { return 1 + Bottom - Top; }
		}

		public bool IsNotEmpty
		{
			[Pure] get { return Left <= Right && Top <= Bottom; }
		}

		public bool IsEmpty
		{
			[Pure] get { return !IsNotEmpty; }
		}

		public bool IsNotFull
		{
			[Pure] get { return !IsFull; }
		}

		public bool IsFull
		{
			[Pure] get { return TopLeft == IntVec.MinValue && TopRight == IntVec.MaxValue; }
		}

		#endregion

		#region Equality

		public static bool operator ==(IntBounds a, IntBounds b)
		{
			return
				a.IsEmpty && b.IsEmpty ||
				a.TopLeft == b.TopLeft && a.BottomRight == b.BottomRight;
		}

		public static bool operator !=(IntBounds a, IntBounds b)
		{
			return !(a == b);
		}

		[Pure]
		public bool Equals(IntBounds other)
		{
			return TopLeft.Equals(other.TopLeft) && BottomRight.Equals(other.BottomRight);
		}

		[Pure]
		public override bool Equals(object obj)
		{
			return obj is IntBounds && Equals((IntBounds)obj);
		}

		[Pure]
		public override int GetHashCode()
		{
			unchecked
			{
				return IsNotEmpty
					? 1 + ((TopLeft.GetHashCode() * 397) ^ BottomRight.GetHashCode())
					: 0;
			}
		}

		#endregion

		#region Partial order by inclusion

		public static bool operator <=(IntBounds a, IntBounds b)
		{
			return
				a.IsEmpty && b.IsEmpty ||
				a.Top >= b.Top && a.Left >= b.Left && a.Bottom <= b.Bottom && a.Right <= b.Right;
		}

		public static bool operator >=(IntBounds a, IntBounds b)
		{
			return
				a.IsEmpty && b.IsEmpty ||
				a.Top <= b.Top && a.Left <= b.Left && a.Bottom >= b.Bottom && a.Right >= b.Right;
		}

		public static bool operator <(IntBounds a, IntBounds b)
		{
			return a <= b && a != b;
		}

		public static bool operator >(IntBounds a, IntBounds b)
		{
			return a >= b && a != b;
		}

		#endregion

		#region Contains

		[Pure]
		public bool Contains(IntVec uv)
		{
			return uv.U >= Left && uv.U <= Right && uv.V >= Top && uv.V <= Bottom;
		}

		[Pure]
		public bool EdgeContains(IntVec uv)
		{
			return IsNotEmpty && uv.U == Left || uv.U == Right || uv.V == Top || uv.V == Bottom;
		}

		#endregion

		#region Operations

		public static IntBounds GetOuterBounds(IntBounds a, IntBounds b)
		{
			return new IntBounds(
				Math.Min(a.Left, b.Left), Math.Max(a.Right, b.Right),
				Math.Min(a.Top, b.Top), Math.Max(a.Bottom, b.Bottom));
		}

		public static IntBounds GetIntersection(IntBounds a, IntBounds b)
		{
			return new IntBounds(
				Math.Max(a.Left, b.Left), Math.Min(a.Right, b.Right),
				Math.Max(a.Top, b.Top), Math.Min(a.Bottom, b.Bottom));
		}

		[Pure]
		public IntBounds MoveBy(IntVec uv)
		{
			return IsNotEmpty
				? new IntBounds(TopLeft + uv, BottomRight + uv)
				: Empty;
		}

		[Pure]
		public IntBounds ExpandTo(IntVec uv)
		{
			return IsEmpty
				? new IntBounds(uv, uv)
				: Contains(uv)
					? this
					: new IntBounds(Math.Min(Left, uv.U), Math.Max(Right, uv.U), Math.Min(Top, uv.V), Math.Max(Bottom, uv.V));
		}

		[Pure]
		public IntBounds ExpandTo(IntBounds b)
		{
			return GetOuterBounds(this, b);
		}

		[Pure]
		public IntBounds IntersectWith(IntBounds b)
		{
			return GetIntersection(this, b);
		}

		#endregion

		#region Enumeration

		[Pure]
		public IEnumerator<IntVec> GetEnumerator()
		{
			for(int v = Top; v <= Bottom; v++)
				for(int u = Left; u <= Right; u++)
					yield return new IntVec(u, v);
		}

		[Pure]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region ToString

		[Pure]
		public override string ToString()
		{
			return IsNotEmpty ? "{0} — {1}".Fmt(TopLeft, BottomRight) : "Empty";
		}

		#endregion
	}
}
