using System;

namespace Formicant
{
	public static partial class Number
	{
		public static bool IsBetween<T>(this T value, T lowest, T highest)
			where T : IComparable<T>
		{
			return value.CompareTo(lowest) >= 0 && value.CompareTo(highest) <= 0;
		}
	}
}
