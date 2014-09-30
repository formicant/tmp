using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formicant
{
	public static partial class IntVecExtensions
	{
		#region Nullable properties

		public static int? U(this IntVec? v)
		{
			return v.HasValue ? v.Value.U : (int?)null;
		}

		public static int? V(this IntVec? v)
		{
			return v.HasValue ? v.Value.V : (int?)null;
		}

		#endregion

		#region Sum

		public static IntVec Sum(this IEnumerable<IntVec> source)
		{
			return source.Aggregate(IntVec.Zero, (u, v) => u + v);
		}

		public static IntVec Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, IntVec> selector)
		{
			return source.Select(selector).Sum();
		}

		#endregion
	}
}
