using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
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
			Test6();
		}

		static void Test6()
		{
			var passwords = GetPasswoeds().ToList();
			var groupedPasswords = passwords.ToLookup(m => m).Select(g => new { g.Key, Count = g.Count() }).OrderByDescending(g => g.Count);
			foreach(var g in groupedPasswords.Take(200))
				Console.WriteLine("{0:F3}%\t{1}", 100D * g.Count / passwords.Count(), g.Key);
		}

		static IEnumerable<string> GetPasswoeds()
		{
			using(var reader = new StreamReader("Mail.txt"))
				while(!reader.EndOfStream)
					yield return reader.ReadLine().KeyValueSplit(':').Value;
		}

		// -----------------------

		static void Test5()
		{
			using(var stream = new StreamWriter("output.txt", false, new UTF8Encoding(false)))
			{
				for(int i = 0; i < 0x200; i++)
					stream.Write("${0:X}, ", i);
			}
		}

		static void GenFF()
		{
			using(var stream = new StreamWriter("None.sfd", false, new UTF8Encoding(false)))
			{
				WriteHeader(stream);
				int counter = 0;
				for(int i = 0x000000; i < 0x020000; i += BlockSize)
				//for(int i = 0x000000; i < 0x110000; i += BlockSize)
					WriteBlock(stream, i / BlockSize, ref counter);
			}
		}

		const int BlockSize = 0x0001;

		static void WriteHeader(StreamWriter writer)
		{
			writer.WriteLine(@"SplineFontDB: 3.0
FontName: None
FullName: None
FamilyName: None
Weight: Regular
Copyright: Copyright (c) 2014, Anonymous
UComments: ""2014-9-1: Created with FontForge (http://fontforge.org)"" 
Version: 001.000
ItalicAngle: 0
UnderlinePosition: -100
UnderlineWidth: 50
Ascent: 819
Descent: 205
LayerCount: 2
Layer: 0 0 ""Back""  1
Layer: 1 0 ""Fore""  0
XUID: [1021 794 -1203387176 14434]
FSType: 0
OS2Version: 0
OS2_WeightWidthSlopeOnly: 0
OS2_UseTypoMetrics: 1
CreationTime: 1409546186
ModificationTime: 1409549344
PfmFamily: 49
TTFWeight: 400
TTFWidth: 5
LineGap: 90
VLineGap: 0
OS2TypoAscent: 0
OS2TypoAOffset: 1
OS2TypoDescent: 0
OS2TypoDOffset: 1
OS2TypoLinegap: 90
OS2WinAscent: 0
OS2WinAOffset: 1
OS2WinDescent: 0
OS2WinDOffset: 1
HheadAscent: 0
HheadAOffset: 1
HheadDescent: 0
HheadDOffset: 1
OS2Vendor: 'PfEd'
MarkAttachClasses: 1
DEI: 91125
LangName: 1033 
Encoding: ISO8859-1
UnicodeInterp: none
NameList: AGL For New Fonts
DisplaySize: -48
AntiAlias: 1
FitToEm: 1
WinInfo: 64 16 4
BeginPrivate: 0
EndPrivate
TeXData: 1 0 0 346030 173015 115343 0 1048576 115343 783286 444596 497025 792723 393216 433062 380633 303038 157286 324010 404750 52429 2506097 1059062 262144
BeginChars: 1114112 1114112");
		}

		static void WriteBlock(StreamWriter writer, int block, ref int counter)
		{
			var codePoints = GetBlockCodePoints(block).ToArray();
			if(codePoints.Any())
			{
				var first = codePoints.First();
				var last = codePoints.Last();
				var altUni = new StringBuilder();
				foreach(var i in codePoints.Skip(1))
					altUni.AppendFormat(" {0:X6}.ffffffff.0", i);
				writer.Write(@"
StartChar: u{2}
Encoding: {0} {0} {2}
Width: 400
Flags: HW", first, last, counter, altUni);
				if(first == 0)
					writer.Write(@"
HStem: 50 50<100 300> 600 50<100 300>
VStem: 50 50<100 600> 300 50<100 600>");
				writer.Write(@"
LayerCount: 2
Fore");
				if(first == 0)
					writer.Write(@"
SplineSet
100 30 m 1
 320 30 l 1
 320 550 l 1
 100 30 l 1
80 50 m 1
 300 570 l 1
 80 570 l 1
 80 50 l 1
50 0 m 1
 50 600 l 1
 350 600 l 1
 350 0 l 1
 50 0 l 1
EndSplineSet");
				else
					writer.Write(@"
Refer: 0 0 N 1 0 0 1 0 0 1");
				writer.Write(@"
EndChar
");
				counter++;
			}
		}

		static IEnumerable<int> GetBlockCodePoints(int block)
		{
			return Enumerable.Range(block * BlockSize, BlockSize)
				.Where(IsValidCodePoint);
		}

		static bool IsValidCodePoint(int i)
		{
			return
				i >= 0x000000 && i < 0x00D800 ||
				i >= 0x000001 && i < 0x00D800 ||
				i >= 0x00E000 && i < 0x00FFFE ||
				i >= 0x010000 && i < 0x030000 ||
				i >= 0x030000 && i < 0x0E0000 ||
				i >= 0x0E0000 && i < 0x110000;
		}

		// ---------------------------------

		static void Test4()
		{
			decimal amount = 2;
			string str = amount.ToString("C", CultureInfo.InvariantCulture);
			str.Print();
		}

		static void Test3()
		{
			Enumerable.Range(2, 30).Select(n => "{0}\t{1}".Fmt(n, 7141.ToSymRoman(n))).Print();
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
