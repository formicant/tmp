using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formicant
{
	public static partial class IntBoundsExtensions
	{
		public static IntBounds OuterBounds(this IEnumerable<IntBounds> source)
		{
			return source.Aggregate(IntBounds.Empty, IntBounds.GetOuterBounds);
		}

		public static IntBounds Intersection(this IEnumerable<IntBounds> source)
		{
			return source.Aggregate(IntBounds.Full, IntBounds.GetIntersection);
		}
	}
}
