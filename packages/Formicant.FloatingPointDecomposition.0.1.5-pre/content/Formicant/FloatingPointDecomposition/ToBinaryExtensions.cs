
namespace Formicant
{
	public static partial class ToBinaryExtensions
	{
		// byte
		public static string ToBinary(this byte a, int bits = 8, char charZero = '0', char charOne = '1')
		{
			string binary = string.Empty;
			for(int i = 0; i < bits; i++)
			{
				binary = ((a & (byte)1) != (byte)0 ? charOne : charZero) + binary;
				a >>= 1;
			}
			return binary;
		}

		// ushort
		public static string ToBinary(this ushort a, int bits = 16, char charZero = '0', char charOne = '1')
		{
			string binary = string.Empty;
			for(int i = 0; i < bits; i++)
			{
				binary = ((a & (ushort)1) != (ushort)0 ? charOne : charZero) + binary;
				a >>= 1;
			}
			return binary;
		}

		// uint
		public static string ToBinary(this uint a, int bits = 32, char charZero = '0', char charOne = '1')
		{
			string binary = string.Empty;
			for(int i = 0; i < bits; i++)
			{
				binary = ((a & (uint)1) != (uint)0 ? charOne : charZero) + binary;
				a >>= 1;
			}
			return binary;
		}

		// ulong
		public static string ToBinary(this ulong a, int bits = 64, char charZero = '0', char charOne = '1')
		{
			string binary = string.Empty;
			for(int i = 0; i < bits; i++)
			{
				binary = ((a & (ulong)1) != (ulong)0 ? charOne : charZero) + binary;
				a >>= 1;
			}
			return binary;
		}

		// sbyte
		public static string ToBinary(this sbyte a, int bits = 8, char charZero = '0', char charOne = '1')
		{
			string binary = string.Empty;
			for(int i = 0; i < bits; i++)
			{
				binary = ((a & (sbyte)1) != (sbyte)0 ? charOne : charZero) + binary;
				a >>= 1;
			}
			return binary;
		}

		// short
		public static string ToBinary(this short a, int bits = 16, char charZero = '0', char charOne = '1')
		{
			string binary = string.Empty;
			for(int i = 0; i < bits; i++)
			{
				binary = ((a & (short)1) != (short)0 ? charOne : charZero) + binary;
				a >>= 1;
			}
			return binary;
		}

		// int
		public static string ToBinary(this int a, int bits = 32, char charZero = '0', char charOne = '1')
		{
			string binary = string.Empty;
			for(int i = 0; i < bits; i++)
			{
				binary = ((a & (int)1) != (int)0 ? charOne : charZero) + binary;
				a >>= 1;
			}
			return binary;
		}

		// long
		public static string ToBinary(this long a, int bits = 64, char charZero = '0', char charOne = '1')
		{
			string binary = string.Empty;
			for(int i = 0; i < bits; i++)
			{
				binary = ((a & (long)1) != (long)0 ? charOne : charZero) + binary;
				a >>= 1;
			}
			return binary;
		}

		// bool
		public static string ToBinary(this bool a, int bits = 1, char charZero = '0', char charOne = '1')
		{
			return new string(a ? charOne : charZero, bits);
		}
	}
}