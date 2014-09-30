using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Formicant
{
	public partial struct Line2
	{
		#region Creation

		public Line2(double a, double b, double c)
			: this()
		{
			A = a;
			B = b;
			C = c;
		}

		public Line2(double a, double b, Vector2 p)
			: this(a, b, a * p.X + b * p.Y) { }

		public Line2(Vector2 p1, Vector2 p2)
			: this(p2.Y - p1.Y, p1.X - p2.X, p1) { }

		public Line2(Vector2 p, Line2 parallel)
			: this(parallel.A, parallel.B, p) { }

		#endregion

		#region Properties

		// A * x + B * y = C
		public double A { [Pure] get; private set; }
		public double B { [Pure] get; private set; }
		public double C { [Pure] get; private set; }

		// y = Kx * x + Cx
		public double Kx { [Pure] get { return -A / B; } }
		public double Cx { [Pure] get { return C / B; } }

		// x = Ky * y + Cy
		public double Ky { [Pure] get { return -B / A; } }
		public double Cy { [Pure] get { return C / A; } }

		#endregion

		#region Operations

		public static Vector2 Intersection(Line2 l1, Line2 l2)
		{
			double Δ  = l1.A * l2.B - l1.B * l2.A;
			double ΔX = l1.C * l2.B - l1.B * l2.C;
			double ΔY = l1.A * l2.C - l1.C * l2.A;
			return new Vector2(ΔX / Δ, ΔY / Δ);
		}

		[Pure]
		public Vector2 IntersectionWith(Line2 l)
		{
			return Intersection(this, l);
		}

		[Pure]
		public Vector2 ProjectionOf(Vector2 p)
		{
			return IntersectionWith(new Line2(B, A, p));
		}

		#endregion
	}
}
