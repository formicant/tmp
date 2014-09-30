using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Formicant
{
	public partial class IniFile
	{
		public class Section : IEnumerable<Setting>
		{
			public Section(string name, SettingTypes settingType)
			{
				SettingType = settingType;
				Name = name;
				var par = name.KeyValueSplit(':', trim: true);
				Header = new SectionHeader(par.Key);
				Header.AddSeparated(par.Value);
				_settings = new Dictionary<string, Setting>();
			}

			public SettingTypes SettingType { get; private set; }

			public string Name { get; private set; }

			public Setting Header { get; private set; }

			public bool IsNotEmpty
			{
				get { return _settings.Count > 0; }
			}

			public bool IsEmpty
			{
				get { return !IsNotEmpty; }
			}

			public Setting this[string settingName]
			{
				get
				{
					_settings.AddIfAbsent(settingName, () => new Setting(settingName, SettingType));
					return _settings[settingName];
				}
			}

			public void Clear()
			{
				_settings.Clear();
			}

			public IEnumerator<Setting> GetEnumerator()
			{
				return _settings.Values.Where(_ => _.IsNotEmpty).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void Write(TextWriter writer)
			{
				Header.Write(writer);
				foreach(var setting in _settings.Values)
					setting.Write(writer);
			}

			readonly Dictionary<string, Setting> _settings;
		}
	}
}
