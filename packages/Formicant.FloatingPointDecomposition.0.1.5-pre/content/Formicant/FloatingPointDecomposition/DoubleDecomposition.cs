using System;
using System.Globalization;

namespace Formicant
{
	public class DoubleDecomposition
	{
		public DoubleDecomposition(double value)
		{
			Value = value;
		}

		public DoubleDecomposition(bool isNegative, long mantissa, int exponent)
		{
			if(mantissa < 0 || mantissa > MaskMantissa)
				throw new ArgumentOutOfRangeException("mantissa", mantissa, MessageMantissaOutOfRange);
			if(exponent < ExpZero || exponent > ExpNaN)
				throw new ArgumentOutOfRangeException("exponent", exponent, MessageExponentOutOfRange);

			_isNegative = isNegative;
			_mantissa = mantissa;
			_exponent = exponent;

			Compose();
		}

		public double Value
		{
			get { return _val; }
			set
			{
				_val = value;
				Decompose();
			}
		}

		public bool IsNegative
		{
			get { return _isNegative; }
			set
			{
				_isNegative = value;
				Compose();
			}
		}

		public long Mantissa
		{
			get { return _mantissa; }
			set
			{
				if(value < 0 || value > MaskMantissa)
					throw new ArgumentOutOfRangeException("value", value, MessageMantissaOutOfRange);
				_mantissa = value;
				Compose();
			}
		}

		public int Exponent
		{
			get { return _exponent; }
			set
			{
				if(value < ExpZero || value > ExpNaN)
					throw new ArgumentOutOfRangeException("value", value, MessageExponentOutOfRange);
				_exponent = value;
				Compose();
			}
		}

		public bool IsReal { get { return Exponent != ExpNaN; } }
		public bool IsNormal { get { return Exponent > ExpZero && Exponent < ExpNaN; } }
		public bool IsZero { get { return Exponent == ExpZero && Mantissa == 0L; } }
		public bool IsSubNormal { get { return Exponent == ExpZero && Mantissa != 0L; } }
		public bool IsInfinity { get { return Exponent == ExpNaN && Mantissa == 0L; } }
		public bool IsNaN { get { return Exponent == ExpNaN && Mantissa != 0L; } }

		public static readonly double Epsilon0 = new DoubleDecomposition(false, 0, 1 + ExpZero).Value;
		public static readonly double Epsilon1 = new DoubleDecomposition(false, 0, -MantissaBits).Value;

		public const int MantissaBits = 52;
		public const int ExponentBits = 11;

		public const int ExpZero = -0x3FF;
		public const int ExpNaN = 0x400;

		#region Equality

		public static bool operator ==(DoubleDecomposition d1, DoubleDecomposition d2)
		{
			return ReferenceEquals(d1, null)
				? ReferenceEquals(d2, null)
				: !ReferenceEquals(d2, null) &&
					(d1.IsNegative == d2.IsNegative && d1.Mantissa == d2.Mantissa && d1.Exponent == d2.Exponent);
		}

		public static bool operator !=(DoubleDecomposition d1, DoubleDecomposition d2)
		{
			return !(d1 == d2);
		}

		public override bool Equals(object obj)
		{
			var decomposition = obj as DoubleDecomposition;
			return !ReferenceEquals(decomposition, null) && this == decomposition;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		#endregion

		#region ToString

		public override string ToString()
		{
			return string.Format("{0}{1}.{2}₂·2^{3}{4}",
				IsNegative ? '−' : '+',
				IsReal
					? IsZero || IsSubNormal ? '0' : '1'
					: IsInfinity ? '∞' : '?',
				Mantissa.ToBinary(MantissaBits),
				IsReal && !IsZero
					? Exponent < 0 ? '−' : '+'
					: '?',
				IsReal
					? IsZero ? "Zer " : Math.Abs(Exponent + (IsSubNormal ? 1 : 0)).ToString("D4", CultureInfo.InvariantCulture)
					: IsNaN ? "NaN " : "Inf "
				);
		}

		public string ToStringBinary()
		{
			return string.Format("[{0}|{1}|{2}]",
				IsNegative.ToBinary(),
				(Exponent - ExpZero).ToBinary(ExponentBits),
				Mantissa.ToBinary(MantissaBits));
		}

		#endregion

		#region private

		void Decompose()
		{
			long bits = BitConverter.DoubleToInt64Bits(_val);
			_isNegative = bits < 0;
			_mantissa = bits & MaskMantissa;
			_exponent = ExpZero + (int)((bits & MaskExponent) >> MantissaBits);
		}

		void Compose()
		{
			long l = (_isNegative ? MaskSign : 0L) | ((long)(_exponent - ExpZero) << MantissaBits) | _mantissa;
			_val = BitConverter.Int64BitsToDouble(l);
		}

		double _val;
		bool _isNegative;
		long _mantissa;
		int _exponent;

		const long MaskSign =  /* 0x8000000000000000L */ long.MinValue;
		const long MaskExponent = 0x7FF0000000000000L;
		const long MaskMantissa = 0x000FFFFFFFFFFFFFL;

		static readonly string MessageMantissaOutOfRange =
			string.Format(CultureInfo.InvariantCulture, "Mantissa must be between {0} and {1}.", 0, MaskMantissa);
		static readonly string MessageExponentOutOfRange =
			string.Format(CultureInfo.InvariantCulture, "Exponent must be between {0} and {1}.", ExpZero, ExpNaN);

		#endregion
	}
}
