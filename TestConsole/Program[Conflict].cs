using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Formicant;

namespace TestConsole
{
	class Program
	{
		static void Main()
		{
			Console.InputEncoding = Console.OutputEncoding = Encoding.Unicode;
			Test3();
		}

		static void Test3()
		{
			Enumerable.Range(2, 33).Select(n => "{0}\t{1}".Fmt(n, 7137.ToSymRoman(n))).Print();
		}

		static void Test2()
		{
			string ids = "5, 21 ,,rt,3";
			var pcSalesAccountIds = ids
				.Split(',')
				.Select(id => id.Trim().ParseNullableInt())
				.OfType<int>();
			pcSalesAccountIds.Print();
		}


		static void Test1()
		{
			Enumerable.Range(0, 8)
				.Select(i => i.NumeralWithRussianNoun("тем|а|ы"))
				.Print();
			Enumerable.Range(0, 8)
				.Select(i => "{0} тем{0|а|ы}".TextWithRussianNumerals(i))
				.Print();
		}
	}
}
