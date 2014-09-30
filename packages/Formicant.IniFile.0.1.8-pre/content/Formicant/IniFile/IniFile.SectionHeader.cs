using System.IO;
using System.Linq;

namespace Formicant
{
	public partial class IniFile
	{
		class SectionHeader : Setting
		{
			public SectionHeader(string name)
				: base(name, SettingTypes.SemicolonSeparated) { }

			public override void Write(TextWriter writer)
			{
				writer.WriteLine((IsNotEmpty ? "[{0}:{1}]" : "[{0}]").Fmt(Name, this.Select(_ => _.Trim()).StringJoin(";")));
			}
		}
	}
}
