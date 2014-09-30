using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using JetBrains.Annotations;

namespace Formicant
{
	public struct Ratio : IEquatable<Ratio>, IComparable<Ratio>, IComparable
	{
		#region Creation

		public Ratio(int numerator)
			: this()
		{
			Numerator = numerator;
		}

		public Ratio(int numerator, int denominator)
			: this()
		{
			int gcd = numerator.Gcd(denominator);
			Numerator = gcd != 0 ? numerator.Div(gcd) : 0;
			Denominator = gcd != 0 ? denominator.Div(gcd) : 0;
		}

		#endregion

		#region Properties

		public int Numerator { [Pure] get; private set; }

		public int Denominator
		{
			[Pure] get { unchecked { return _denominatorMinusOne + 1; } }
			private set { unchecked { _denominatorMinusOne = value - 1; } }
		}

		int _denominatorMinusOne;

		public double Value
		{
			[Pure] get { return (double)Numerator / (double)Denominator; }
		}

		public bool IsFinite
		{
			[Pure] get { return Denominator != 0; }
		}

		#endregion

		#region Constants

		public static readonly Ratio Zero = new Ratio(0);

		public static readonly Ratio Unit = new Ratio(1);

		#endregion

		#region Conversion

		public static implicit operator Ratio(int n)
		{
			return new Ratio(n);
		}

		public static explicit operator double(Ratio r)
		{
			return r.Value;
		}

		#endregion

		#region Equality

		public static bool operator ==(Ratio p, Ratio q)
		{
			return p.Numerator == q.Numerator && p.Denominator == q.Denominator;
		}

		public static bool operator !=(Ratio p, Ratio q)
		{
			return !(p == q);
		}

		[Pure]
		public bool Equals(Ratio other)
		{
			return this == other;
		}

		[Pure]
		public override bool Equals(object obj)
		{
			return obj is Ratio && Equals((Ratio)obj);
		}

		[Pure]
		public override int GetHashCode()
		{
			unchecked { return (Numerator * 397) ^ Denominator; }
		}

		#endregion

		#region Inequality

		public static bool operator <=(Ratio p, Ratio q)
		{
			return p.Numerator * q.Denominator <= q.Numerator * p.Denominator;
		}

		public static bool operator >=(Ratio p, Ratio q)
		{
			return p.Numerator * q.Denominator >= q.Numerator * p.Denominator;
		}

		public static bool operator <(Ratio p, Ratio q)
		{
			return p.Numerator * q.Denominator < q.Numerator * p.Denominator;
		}

		public static bool operator >(Ratio p, Ratio q)
		{
			return p.Numerator * q.Denominator > q.Numerator * p.Denominator;
		}

		[Pure]
		public int CompareTo(Ratio other)
		{
			return (this - other).Sign();
		}

		[Pure]
		public int CompareTo(object obj)
		{
			if(obj is Ratio)
				return CompareTo((Ratio)obj);
			else throw new ArgumentException();
		}

		#endregion

		#region Arithmetics

		public static Ratio operator +(Ratio p, Ratio q)
		{
			return new Ratio(p.Numerator * q.Denominator + q.Numerator * p.Denominator, q.Denominator * p.Denominator);
		}

		public static Ratio operator -(Ratio p, Ratio q)
		{
			return new Ratio(p.Numerator * q.Denominator - q.Numerator * p.Denominator, q.Denominator * p.Denominator);
		}

		public static Ratio operator *(Ratio p, Ratio q)
		{
			return new Ratio(p.Numerator * q.Numerator, p.Denominator * q.Denominator);
		}

		public static Ratio operator /(Ratio p, Ratio q)
		{
			return new Ratio(p.Numerator * q.Denominator, p.Denominator * q.Numerator);
		}

		[Pure]
		public Ratio Abs()
		{
			return new Ratio(Numerator.Abs(), Denominator.Abs());
		}

		[Pure]
		public int Sign()
		{
			return Numerator.Sign() * Denominator.Sign();
		}

		[Pure]
		public Ratio Inv()
		{
			return new Ratio(Denominator, Numerator);
		}

		[Pure]
		public Ratio Sq()
		{
			return new Ratio(Numerator.Sq(), Denominator.Sq());
		}

		[Pure]
		public Ratio Cube()
		{
			return new Ratio(Numerator.Cube(), Denominator.Cube());
		}

		[Pure]
		public Ratio Pow(int n)
		{
			return n >= 0
				? new Ratio(Numerator.Pow(n), Denominator.Pow(n))
				: new Ratio(Denominator.Pow(-n), Numerator.Pow(-n));
		}

		#endregion

		#region Rounding

		[Pure]
		public Ratio Floor()
		{
			return new Ratio(Numerator.Div(Denominator));
		}
		[Pure]
		public int FloorToInt()
		{
			return Numerator.Div(Denominator);
		}

		[Pure]
		public Ratio Celing()
		{
			return new Ratio(-Numerator.Div(-Denominator));
		}
		[Pure]
		public int CelingToInt()
		{
			return -Numerator.Div(-Denominator);
		}

		[Pure]
		public Ratio Round()
		{
			return (this + new Ratio(1, 2)).Floor();
		}
		[Pure]
		public int RoundToInt()
		{
			return (this + new Ratio(1, 2)).FloorToInt();
		}

		#endregion

		#region String

		[Pure]
		public override string ToString()
		{
			return "{0} : {1}".Fmt(Numerator, Denominator);
		}

		public static Ratio? Parse(string str)
		{
			var parts = str.KeyValueSplit(':', trim: true);
			int? numerator = parts.Key.ParseNullableInt();
			int? denominator = parts.Value.ParseNullableInt();
			return numerator.HasValue && (denominator.HasValue || parts.Value.IsEmpty())
				? new Ratio(numerator.Value, denominator ?? 1)
				: (Ratio?)null;
		}

		#endregion
	}
}
