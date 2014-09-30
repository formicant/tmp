using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formicant;

namespace TestConsole
{
	public static class SymRoman
	{
		public static string ToSymRoman(this int n, int b)
		{
			return ToSymRoman(n, b, 0);
		}

		static string ToSymRoman(int n, int b, int i)
		{
			if(n == 0)
				return "";
			var s = (b - 1).Div(2);
			var q = (n + s).Mod(b) - s;
			var p = (n + s).Div(b);
			return q > 0
				? ToSymRoman(p, b, i + 1) + new string(Digits[i], q)
				: new string(Digits[i], -q) + ToSymRoman(p, b, i + 1);
		}

		const string Digits = "IVXLCDMivxlcdm";
	}
}
