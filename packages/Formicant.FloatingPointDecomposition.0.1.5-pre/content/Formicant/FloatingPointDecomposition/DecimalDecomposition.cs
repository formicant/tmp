using System;
using System.Globalization;

namespace Formicant
{
	public class DecimalDecomposition
	{
		public DecimalDecomposition(decimal value)
		{
			Value = value;
		}

		public DecimalDecomposition(bool isNegative, decimal mantissa, int exponent)
		{
			if(mantissa < 0)
				throw new ArgumentOutOfRangeException("mantissa", mantissa, MessageMantissaOutOfRange);
			if(exponent < 0 || exponent > ExpMax)
				throw new ArgumentOutOfRangeException("exponent", exponent, MessageExponentOutOfRange);

			_bits = decimal.GetBits(decimal.Truncate(mantissa));
			_bits[3] = (isNegative ? MaskSign : 0) | (exponent << UnusedLoBits);

			Compose();
		}

		public decimal Value
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
			get { return _bits[3] < 0; }
			set
			{
				_bits[3] = _bits[3] & ~MaskSign | (value ? MaskSign : 0);
				Compose();
			}
		}

		public decimal Mantissa
		{
			get { return new decimal(_bits[0], _bits[1], _bits[2], false, 0); }
			set
			{
				if(value < 0)
					throw new ArgumentOutOfRangeException("value", value, MessageMantissaOutOfRange);
				var bits = decimal.GetBits(decimal.Truncate(value));
				_bits[0] = bits[0];
				_bits[1] = bits[1];
				_bits[2] = bits[2];
				Compose();
			}
		}

		public int Exponent
		{
			get { return (_bits[3] & MaskExponent) >> UnusedLoBits; }
			set
			{
				if(value < 0 || value > ExpMax)
					throw new ArgumentOutOfRangeException("value", value, MessageExponentOutOfRange);
				_bits[3] = _bits[3] & ~MaskExponent | (value << UnusedLoBits);
				Compose();
			}
		}

		public static readonly decimal Epsilon0 = new DecimalDecomposition(false, 1, ExpMax).Value;

		public const int MantissaBits = 96;
		public const int UnusedHiBits = 10;
		public const int ExponentBits = 5;
		public const int UnusedLoBits = 16;

		public const int ExpMax = 28;

		#region Equality

		public static bool operator ==(DecimalDecomposition d1, DecimalDecomposition d2)
		{
			return ReferenceEquals(d1, null)
				? ReferenceEquals(d2, null)
				: !ReferenceEquals(d2, null) &&
					(d1.IsNegative == d2.IsNegative && d1.Mantissa == d2.Mantissa && d1.Exponent == d2.Exponent);
		}

		public static bool operator !=(DecimalDecomposition d1, DecimalDecomposition d2)
		{
			return !(d1 == d2);
		}

		public override bool Equals(object obj)
		{
			var decomposition = obj as DecimalDecomposition;
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
			return string.Format("{0}{1}·10^−{2}",
				IsNegative ? '−' : '+',
				Mantissa.ToString(CultureInfo.InvariantCulture),
				Exponent);
		}

		public string ToStringBinary()
		{
			return string.Format("[{0}{1}{2}|{3}|{4}|{5}|{6}]",
				_bits[2].ToBinary(),
				_bits[1].ToBinary(),
				_bits[0].ToBinary(),
				IsNegative.ToBinary(),
				((_bits[3] & MaskUnusedHi) >> (ExponentBits + UnusedLoBits)).ToBinary(UnusedHiBits),
				Exponent.ToBinary(ExponentBits),
				(_bits[3] & MaskUnusedLo).ToBinary(UnusedLoBits));
		}

		#endregion

		#region private

		void Decompose()
		{
			_bits = decimal.GetBits(_val);
		}

		void Compose()
		{
			_val = new decimal(_bits);
		}

		decimal _val;
		int[] _bits = new int[4];

		const int MaskSign =  /* 0x80000000 */ int.MinValue;
		const int MaskUnusedHi = 0x7FE00000;
		const int MaskExponent = 0x001F0000;
		const int MaskUnusedLo = 0x0000FFFF;

		static readonly string MessageMantissaOutOfRange = "Mantissa must be nonnegative.";
		static readonly string MessageExponentOutOfRange
			= string.Format(CultureInfo.InvariantCulture, "Exponent must be between {0} and {1}.", 0, ExpMax);

		#endregion
	}
}
