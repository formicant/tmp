using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formicant
{
	public static partial class Vector2Extensions
	{
		#region Nullable properties

		public static double? X(this Vector2? v)
		{
			return v.HasValue ? v.Value.X : (double?) null;
		}

		public static double? Y(this Vector2? v)
		{
			return v.HasValue ? v.Value.Y : (double?) null;
		}

		#endregion

		#region Sum

		public static Vector2 Sum(this IEnumerable<Vector2> source)
		{
			return source.Aggregate(Vector2.Zero, (u, v) => u + v);
		}

		public static Vector2 Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector2> selector)
		{
			return source.Select(selector).Sum();
		}

		#endregion

		#region Average

		public static Vector2 Average(this IEnumerable<Vector2> source)
		{
			if(source == null) throw new ArgumentNullException("source");
			var sum = Vector2.Zero;
			int count = 0;
			foreach(var p in source)
			{
				sum += p;
				count++;
			}
			if(count > 0)
				return sum / count;
			throw new InvalidOperationException("Sequence contains no elements");
		}

		public static Vector2 Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector2> selector)
		{
			return source.Select(selector).Average();
		}

		public static Vector2? Average(this IEnumerable<Vector2?> source)
		{
			if(source == null) throw new ArgumentNullException("source");
			var sum = Vector2.Zero;
			int count = 0;
			foreach(var p in source.OfType<Vector2>())
			{
				sum += p;
				count++;
			}
			return count > 0
				? sum / count
				: (Vector2?)null;
		}

		public static Vector2? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector2?> selector)
		{
			return source.Select(selector).Average();
		}

		#endregion
	}
}
