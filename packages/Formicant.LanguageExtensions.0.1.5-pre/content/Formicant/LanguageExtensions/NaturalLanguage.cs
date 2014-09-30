using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Formicant
{
	public abstract class NaturalLanguage
	{
		public static readonly NaturalLanguage English = new EnglishLanguage();
		public static readonly NaturalLanguage Russian = new RussianLanguage();


		public abstract string GetNounFormByNumeral(string[] forms, int number);


		private class EnglishLanguage : NaturalLanguage
		{
			public override string GetNounFormByNumeral(string[] forms, int number)
			{
				return number.Abs() == 1
					? forms.Length > 2 ? forms[1] : ""
					: forms.Length > 1 ? forms.Length > 2 ? forms[2] : forms[1] : "";
			}
		}

		private class RussianLanguage : NaturalLanguage
		{
			public override string GetNounFormByNumeral(string[] forms, int number)
			{
				int units = number.Abs().Mod(10);
				int tens = number.Abs().Div(10).Mod(10);
				return tens != 1 && units != 0 && units < 5 ? units == 1
					? forms.Length > 1 ? forms[1] : ""
					: forms.Length > 2 ? forms[2] : ""
					: forms.Length > 3 ? forms[3] : "";
			}
		}
	}
}
