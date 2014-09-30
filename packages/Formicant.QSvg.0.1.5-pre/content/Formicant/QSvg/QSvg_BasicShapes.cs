using System.Collections.Generic;

namespace Formicant
{
	public partial class QSvg
	{
		protected QElement Rect(Vector2? origin = null, Vector2? size = null, Vector2? cornerRadii = null)
		{
			return Rect(origin.X(), origin.Y(), size.X(), size.Y(), cornerRadii.X(), cornerRadii.Y());
		}
		protected QElement Rect(double? x = null, double? y = null, double? width = null, double? height = null, double? rx = null, double? ry = null)
		{
			return E("rect")
				[A("x", x)]
				[A("y", y)]
				[A("width", width)]
				[A("height", height)]
				[A("rx", rx)]
				[A("ry", ry)]
			;
		}

		protected QElement Circle(Vector2? center = null, double? radius = null)
		{
			return Circle(center.X(), center.Y(), radius);
		}
		protected QElement Circle(double? cx = null, double? cy = null, double? r = null)
		{
			return E("circle")
				[A("cx", cx)]
				[A("cy", cy)]
				[A("r", r)]
			;
		}

		protected QElement Ellipse(Vector2? center = null, Vector2? axes = null)
		{
			return Ellipse(center.X(), center.Y(), axes.X(), axes.Y());
		}
		protected QElement Ellipse(double? cx = null, double? cy = null, double? rx = null, double? ry = null)
		{
			return E("ellipse")
				[A("cx", cx)]
				[A("cy", cy)]
				[A("rx", rx)]
				[A("ry", ry)]
			;
		}

		protected QElement Line(Vector2? start = null, Vector2? end = null)
		{
			return Line(start.X(), start.Y(), end.X(), end.Y());
		}
		protected QElement Line(double? x1 = null, double? y1 = null, double? x2 = null, double? y2 = null)
		{
			return E("line")
				[A("x1", x1)]
				[A("y1", y1)]
				[A("x2", x2)]
				[A("y2", y2)]
			;
		}

		protected QElement PolyLine(params Vector2[] points)
		{
			return PolyLine(points as IEnumerable<Vector2>);
		}
		protected QElement PolyLine(IEnumerable<Vector2> points)
		{
			return E("polyline")
				[A("points", points)]
			;
		}

		protected QElement Polygon(params Vector2[] points)
		{
			return Polygon(points as IEnumerable<Vector2>);
		}
		protected QElement Polygon(IEnumerable<Vector2> points)
		{
			return E("polygon")
				[A("points", points)]
			;
		}
	}
}
