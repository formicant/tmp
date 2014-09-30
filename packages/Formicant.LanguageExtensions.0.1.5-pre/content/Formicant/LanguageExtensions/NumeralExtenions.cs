using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Formicant
{
	public static partial class NumeralExtenions
	{
		public static string NumeralWithNoun(this int number, NaturalLanguage language, string nounTemplate, bool showNumber = true)
		{
			var forms = nounTemplate.Split('|');
			return (showNumber ? "{0} {1}{2}" : "{1}{2}").Fmt(number, forms[0], language.GetNounFormByNumeral(forms, number));
		}

		/// <summary>
		///	Returns the string consisting of a number and an English noun in the corresponding form.
		/// </summary>
		/// <param name="number">Number</param>
		/// <param name="nounTemplate">String of the form "‹stem›[|‹pl›]" or "‹stem›|‹sg›|‹pl›"</param>
		/// <param name="showNumber">Whether to include the number into the string</param>
		/// <returns>String of the corresponding form.</returns>
		/// <example>
		///		<code>i.NumeralWithEnglishNoun("vector|s")</code>
		///		<code>i.NumeralWithEnglishNoun("matri|x|ces")</code>
		///		<code>i.NumeralWithEnglishNoun("sheep")</code>
		///		<code>i.NumeralWithEnglishNoun("|foot|feet")</code>
		/// </example>
		public static string NumeralWithEnglishNoun(this int number, string nounTemplate, bool showNumber = true)
		{
			return number.NumeralWithNoun(NaturalLanguage.English, nounTemplate, showNumber);
		}

		/// <summary>
		///	Returns the string consisting of a number and a Russian noun in the corresponding form.
		/// </summary>
		/// <param name="number">Number</param>
		/// <param name="nounTemplate">String of the form "‹stem›[|‹sg›[|‹du›[|‹pl›]]]"</param>
		/// <param name="showNumber">Whether to include the number into the string</param>
		/// <returns>String of the corresponding form.</returns>
		/// <example>
		///		<code>i.NumeralWithRussianNoun("кош|ка|ки|ек")</code>
		///		<code>i.NumeralWithRussianNoun("кот||а|ов")</code>
		///		<code>i.NumeralWithRussianNoun("собак|а|и")</code>
		///		<code>i.NumeralWithRussianNoun("кенгуру")</code>
		///		<code>i.NumeralWithRussianNoun("|ребёнок|ребёнка|детей")</code>
		/// </example>
		public static string NumeralWithRussianNoun(this int number, string nounTemplate, bool showNumber = true)
		{
			return number.NumeralWithNoun(NaturalLanguage.Russian, nounTemplate, showNumber);
		}


		public static string TextWithNumerals(this string textTemplate, NaturalLanguage language, params int[] numbers)
		{
			return TextWithNumeralsRegex.Replace(textTemplate,
				match => match.Groups[1].Success
					? numbers[match.Groups[2].Value.ParseInt()].NumeralWithNoun(language, match.Groups[3].Value, false)
					: match.Value)
				.Fmt(numbers.Cast<object>().ToArray());
		}

		/// <summary>
		///	Returns the string with numbers and English words in the corresponding forms.
		/// </summary>
		/// <param name="textTemplate">Template string</param>
		/// <param name="numbers">Numbers to insert</param>
		/// <returns>String of the corresponding form.</returns>
		/// <example>
		///		<code>"{0} vector{0|s}, {1} matri{1|x|ces}, {2} sheep{2|}, and {3} {3|foot|feet}".TextWithEnglishNumerals(a, b, c, d)</code>
		///		<code>"{0} process{0|es} run{0|s|}".TextWithEnglishNumerals(i)</code>
		/// </example>
		public static string TextWithEnglishNumerals(this string textTemplate, params int[] numbers)
		{
			return textTemplate.TextWithNumerals(NaturalLanguage.English, numbers);
		}

		/// <summary>
		///	Returns the string with numbers and Russian words in the corresponding forms.
		/// </summary>
		/// <param name="textTemplate">Template string</param>
		/// <param name="numbers">Numbers to insert</param>
		/// <returns>String of the corresponding form.</returns>
		/// <example>
		///		<code>"{0} кош{0|ка|ки|ек}, {1} кот{1||а|ов}, {2} собак{2|а|и}, {3} кенгуру{3|} и {4} {4|ребёнок|ребёнка|детей}".TextWithRussianNumerals(a, b, c, d, e)</code>
		///		<code>"{0} мыш{0|ь|и|ей} пищ{0|и|а|а}т".TextWithRussianNumerals(i)</code>
		/// </example>
		public static string TextWithRussianNumerals(this string textTemplate, params int[] numbers)
		{
			return textTemplate.TextWithNumerals(NaturalLanguage.Russian, numbers);
		}

		static readonly Regex TextWithNumeralsRegex = new Regex(@"\{\{|(\{(\d+)(\|(?:\}\}|[^\}])*)\})");
	}
}
