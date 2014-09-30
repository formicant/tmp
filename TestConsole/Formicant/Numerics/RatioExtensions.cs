using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formicant
{
	public static partial class RatioExtensions
	{
		#region Sum

		public static Ratio Sum(this IEnumerable<Ratio> source)
		{
			return source.Aggregate(Ratio.Zero, (p, q) => p + q);
		}

		public static Ratio Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Ratio> selector)
		{
			return source.Select(selector).Sum();
		}

		#endregion

		#region Product

		public static Ratio Product(this IEnumerable<Ratio> source)
		{
			return source.Aggregate(Ratio.Unit, (p, q) => p * q);
		}

		public static Ratio Product<TSource>(this IEnumerable<TSource> source, Func<TSource, Ratio> selector)
		{
			return source.Select(selector).Product();
		}

		#endregion

		#region Average

		public static Ratio Average(this IEnumerable<Ratio> source)
		{
			if(source == null) throw new ArgumentNullException("source");
			var sum = Ratio.Zero;
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

		public static Ratio Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Ratio> selector)
		{
			return source.Select(selector).Average();
		}

		public static Ratio? Average(this IEnumerable<Ratio?> source)
		{
			if(source == null) throw new ArgumentNullException("source");
			var sum = Ratio.Zero;
			int count = 0;
			foreach(var p in source.OfType<Ratio>())
			{
				sum += p;
				count++;
			}
			return count > 0
				? sum / count
				: (Ratio?) null;
		}

		public static Ratio? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Ratio?> selector)
		{
			return source.Select(selector).Average();
		}

		#endregion
	}
}
