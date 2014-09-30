using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Formicant
{
	public partial class IniFile : IEnumerable<IniFile.Section>
	{
		public IniFile(SettingTypes settingType = SettingTypes.SemicolonSeparated)
		{
			SettingType = settingType;
			_sections = new Dictionary<string, Section>();
		}

		public IniFile(TextReader reader, SettingTypes settingType = SettingTypes.SemicolonSeparated)
			: this(settingType)
		{
			Read(reader);
		}

		public IniFile(Stream stream, Encoding encoding, SettingTypes settingType = SettingTypes.SemicolonSeparated)
			: this(settingType)
		{
			using(var reader = new StreamReader(stream, encoding))
				Read(reader);
		}

		public IniFile(Stream stream, SettingTypes settingType = SettingTypes.SemicolonSeparated)
			: this(stream, Encoding.UTF8, settingType) { }

		public IniFile(string path, Encoding encoding, SettingTypes settingType = SettingTypes.SemicolonSeparated)
			: this(settingType)
		{
			using(var reader = new StreamReader(path, encoding))
				Read(reader);
		}

		public IniFile(string path, SettingTypes settingType = SettingTypes.SemicolonSeparated)
			: this(path, Encoding.UTF8, settingType) { }

		public SettingTypes SettingType { get; private set; }

		public void Read(TextReader reader)
		{
			string currentSection = "";
			string currentSetting = "";
			string line;
			while((line = reader.ReadLine()) != null)
			{
				line = line.Trim();
				var detach = line.DetachFirstChar();
				if(detach.Key == '[')
				{
					var par = detach.Value.KeyValueSplit(']', trim: true);
					currentSection = par.Key;
					currentSetting = "";
				}
				else if(detach.Key != default(char) && detach.Key != ';' && detach.Key != '#')
				{
					var par = line.KeyValueSplit('=', trim: true);
					if(par.Key.IsNotEmpty())
						currentSetting = par.Key;
					if(par.Value.IsNotEmpty())
					{
						if(SettingType == SettingTypes.Multiline)
							this[currentSection][currentSetting].Add(par.Value);
						else
							this[currentSection][currentSetting].AddSeparated(par.Value);
					}
				}
			}
		}

		public void Write(TextWriter writer)
		{
			bool first = true;
			foreach(var section in _sections.Values)
			{
				if(first)
					first = false;
				else
					writer.WriteLine();
				section.Write(writer);
			}
		}

		public void Write(Stream stream, Encoding encoding)
		{
			using(var writer = new StreamWriter(stream, encoding))
				Write(writer);
		}

		public void Write(Stream stream)
		{
			Write(stream, Encoding.UTF8);
		}

		public void Write(string path, Encoding encoding)
		{
			using(var writer = new StreamWriter(path, false, encoding))
				Write(writer);
		}

		public void Write(string path)
		{
			Write(path, Encoding.UTF8);
		}

		public Section this[string sectionName]
		{
			get
			{
				_sections.AddIfAbsent(sectionName, () => new Section(sectionName, SettingType));
				return _sections[sectionName];
			}
		}

		public void Clear()
		{
			_sections.Clear();
		}

		public IEnumerator<Section> GetEnumerator()
		{
			return _sections.Values.Where(section => section.IsNotEmpty).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		readonly Dictionary<string, Section> _sections;
	}
}
