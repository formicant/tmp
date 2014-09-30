using System.Drawing;

namespace Formicant
{
	public partial class QSvg
	{
		protected QAttribute Id(string id)
		{
			return A("id", id);
		}

		protected QAttribute Opacity(double? value)
		{
			return A("opacity", value);
		}

		protected QAttribute Fill(Color? color = null)
		{
			return A("fill", color ?? (object)"none");
		}

		protected QAttribute[] Stroke(Color? color = null, double? width = null)
		{
			return new []
			{
				A("stroke", color ?? (object)"none"),
				A("stroke-width", width)
			};
		}

		protected QAttribute Stroke(double width)
		{
			return A("stroke-width", width);
		}
	}
}
