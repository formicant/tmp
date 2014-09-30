using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Formicant
{
	public abstract partial class QSvg : QXml
	{
		protected QSvg(int decimalPlaces = 6)
			: base(Xmlns)
		{
			DecimalPlaces = decimalPlaces;
		}

		public int DecimalPlaces { get; private set; }

		#region Element shorthand methods

		protected QSvgDocument SvgDoc(double width, double height, params object[] content)
		{
			return new QSvgDocument(this, new Vector2(width, height), content);
		}
		protected QSvgDocument SvgDoc(Vector2 size, params object[] content)
		{
			return new QSvgDocument(this, size, content);
		}
		protected QSvgDocument SvgDoc(params object[] content)
		{
			return new QSvgDocument(this, content);
		}

		protected QElement Svg(double width, double height, params object[] content)
		{
			return Svg(content.Prepend(A("height", height)).Prepend(A("width", width)));
		}
		protected QElement Svg(Vector2 size, params object[] content)
		{
			return Svg(size.X, size.Y, content);
		}
		protected QElement Svg(params object[] content)
		{
			return E("svg", content)
				[A("version", 1.1M)]
			;
		}

		protected QElement G
		{
			get { return E("g"); }
		}

		protected QAttribute ViewBox(Vector2 origin, Vector2 size)
		{
			return ViewBox(origin.X, origin.Y, size.X, size.Y);
		}
		protected QAttribute ViewBox(double x, double y, double width, double height)
		{
			return A("viewBox", new[] { x, y, width, height });
		}

		protected QElement Title(string title)
		{
			return E("title", title);
		}
		protected QElement Desc(string description)
		{
			return E("desc", description);
		}

		#endregion

		#region QSvgDocument class

		protected class QSvgDocument : XDocument
		{
			public QSvgDocument(QSvg qSvg, params object[] content)
				: base(DocType("svg", PublicId, SystemId))
			{
				Add(qSvg.Svg(content));
			}
			public QSvgDocument(QSvg qSvg, Vector2 size, params object[] content)
				: base(DocType("svg", PublicId, SystemId))
			{
				Add(qSvg.Svg(size, content));
			}

			public QSvgDocument this[object child]
			{
				get
				{
					if(Root != null && child != null)
						Root.Add(child);
					return this;
				}
			}

			const string SystemId = "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd";
			const string PublicId = "-//W3C//DTD SVG 1.1//EN";
		}

		#endregion

		#region ValueConverter

		protected override object ValueConverter(object value)
		{
			if(value is double)
				return ((double)value).Round(DecimalPlaces);

			if(value is Vector2)
				return "{0} {1}".Fmt(ValueConverter(((Vector2)value).X), ValueConverter(((Vector2)value).Y));

			if(value is IEnumerable<double>)
				return ((IEnumerable<double>)value).Select(p => ValueConverter(p)).StringJoin(" ");

			if(value is IEnumerable<Vector2>)
				return ((IEnumerable<Vector2>)value).Select(p => ValueConverter(p)).StringJoin(", ");

			if(value is Color)
				return ((Color)value).A == byte.MaxValue
					? "#{0:X2}{1:X2}{2:X2}".Fmt(((Color)value).R, ((Color)value).G, ((Color)value).B)
					: "rgba({0},{1},{2},{3})".Fmt(((Color)value).R, ((Color)value).G, ((Color)value).B, ValueConverter((double)((Color)value).A / byte.MaxValue));

			if(value is QPath)
				return ((QPath)value).Nodes
					.Select(n => "{0} {1}".Fmt(n.Absolute ? char.ToUpperInvariant(n.Char) : n.Char, ValueConverter(n.Values)))
					.StringJoin(" ");

			return base.ValueConverter(value);
		}

		#endregion

		#region Namespases

		static readonly XNamespace Xmlns = "http://www.w3.org/2000/svg";
		static readonly XNamespace Xlink = "http://www.w3.org/1999/xlink";

		#endregion
	}
}
