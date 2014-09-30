using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formicant;
using JetBrains.Annotations;

namespace TestConsole
{
	public static class ConsoleExtensions
	{
		public static void Print(this string str, int indent = 0)
		{
			Console.WriteLine(new string(' ', indent) + str);
		}

		[StringFormatMethod("format")]
		public static void Print(this string format, object[] args, int indent = 0)
		{
			format.Fmt(args).Print(indent);
		}

		public static void Print(this IFormattable obj, string format, int indent = 0)
		{
			obj.ToInvariantString(format).Print(indent);
		}

		public static void Print(this object obj, int indent = 0)
		{
			var str = obj as string;
			if(str.IsNotNull())
			{
				str.Print(indent);
				return;
			}
			var seq = obj as IEnumerable;
			if(seq.IsNotNull())
			{
				"[".Print(indent);
				foreach(var item in seq)
					item.Print(indent + 1);
				"]".Print(indent);
				return;
			}
			obj.ToInvariantString().Print(indent);
		}
	}
}
