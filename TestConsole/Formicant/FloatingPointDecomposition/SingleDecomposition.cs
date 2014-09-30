using System;
using System.Globalization;

namespace Formicant
{
	public class SingleDecomposition
	{
		public SingleDecomposition(float value)
		{
			Value = value;
		}

		public SingleDecomposition(bool isNegative, int mantissa, int exponent)
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

		public float Value
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

		public int Mantissa
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
		public bool IsZero { get { return Exponent == ExpZero && Mantissa == 0; } }
		public bool IsSubNormal { get { return Exponent == ExpZero && Mantissa != 0; } }
		public bool IsInfinity { get { return Exponent == ExpNaN && Mantissa == 0; } }
		public bool IsNaN { get { return Exponent == ExpNaN && Mantissa != 0; } }

		public static readonly float Epsilon0 = new SingleDecomposition(false, 0, 1 + ExpZero).Value;
		public static readonly float Epsilon1 = new SingleDecomposition(false, 0, -MantissaBits).Value;

		public const int MantissaBits = 23;
		public const int ExponentBits = 8;

		public const int ExpZero = -0x7F;
		public const int ExpNaN = 0x80;

		#region Equality

		public static bool operator ==(SingleDecomposition d1, SingleDecomposition d2)
		{
			return ReferenceEquals(d1, null)
				? ReferenceEquals(d2, null)
				: !ReferenceEquals(d2, null) &&
					(d1.IsNegative == d2.IsNegative && d1.Mantissa == d2.Mantissa && d1.Exponent == d2.Exponent);
		}

		public static bool operator !=(SingleDecomposition d1, SingleDecomposition d2)
		{
			return !(d1 == d2);
		}

		public override bool Equals(object obj)
		{
			var decomposition = obj as SingleDecomposition;
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
					? IsZero ? "Zer" : Math.Abs(Exponent + (IsSubNormal ? 1 : 0)).ToString("D3", CultureInfo.InvariantCulture)
					: IsNaN ? "NaN" : "Inf"
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
			int bits = BitConverter.ToInt32(BitConverter.GetBytes(_val), 0);
			_isNegative = bits < 0;
			_mantissa = bits & MaskMantissa;
			_exponent = ExpZero + ((bits & MaskExponent) >> MantissaBits);
		}

		void Compose()
		{
			int l = (_isNegative ? MaskSign : 0) | ((_exponent - ExpZero) << MantissaBits) | _mantissa;
			_val = BitConverter.ToSingle(BitConverter.GetBytes(l), 0);
		}

		float _val;
		bool _isNegative;
		int _mantissa;
		int _exponent;

		const int MaskSign =  /* 0x80000000 */ int.MinValue;
		const int MaskExponent = 0x7F800000;
		const int MaskMantissa = 0x007FFFFF;

		static readonly string MessageMantissaOutOfRange =
			string.Format(CultureInfo.InvariantCulture, "Mantissa must be between {0} and {1}.", 0, MaskMantissa);
		static readonly string MessageExponentOutOfRange =
			string.Format(CultureInfo.InvariantCulture, "Exponent must be between {0} and {1}.", ExpZero, ExpNaN);

		#endregion
	}
}
