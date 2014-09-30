using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Formicant
{
	public partial class IniFile
	{
		public class Setting : List<string>
		{
			public Setting(string name, SettingTypes settingType)
			{
				Name = name;
				SettingType = settingType;
			}

			public SettingTypes SettingType { get; private set; }

			public string Name { get; private set; }

			public bool IsNotEmpty { get { return Count > 0; } }

			public bool IsEmpty { get { return !IsNotEmpty; } }

			public bool IsSingleValued { get { return Count <= 1; } }

			public static implicit operator string(Setting setting)
			{
				return setting.GetString();
			}

			public string GetString()
			{
				return this.SingleOrDefaultEvenIfMany();
			}

			public int? GetNullableInt()
			{
				return GetString().ParseNullableInt();
			}

			public decimal? GetNullableDecimal()
			{
				return GetString().ParseNullableDecimal();
			}

			public bool? GetNullableBool()
			{
				return ParseNullableBool(GetString());
			}

			public string GetString(int index)
			{
				return index >= 0 && index < Count
					? this[index]
					: null;
			}

			public int? GetNullableInt(int index)
			{
				return GetString(index).ParseNullableInt();
			}

			public decimal? GetNullableDecimal(int index)
			{
				return GetString(index).ParseNullableDecimal();
			}

			public bool? GetNullableBool(int index)
			{
				return ParseNullableBool(GetString(index));
			}

			public void Set(string val)
			{
				Clear();
				Add(val);
			}

			public void Set(object val)
			{
				Clear();
				Add(val);
			}

			public void Set(IEnumerable<object> vals)
			{
				Clear();
				Add(vals);
			}

			public void Set(int index, object val)
			{
				this[index] = val.ToInvariantString();
			}

			public void Add(object val)
			{
				if(val != null)
					base.Add(val.ToInvariantString());
			}

			public void Add(IEnumerable<object> vals)
			{
				AddRange(vals.Select(_ => _.ToInvariantString()));
			}

			public void AddSeparated(string separatedVals)
			{
				AddRange(separatedVals.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
			}

			public void Insert(int index, object val)
			{
				base.Insert(index, val.ToInvariantString());
			}

			public virtual void Write(TextWriter writer)
			{
				switch(SettingType)
				{
					case SettingTypes.SemicolonSeparated:
						writer.WriteLine("{0}={1}".Fmt(Name, this.Select(_ => _.Trim()).StringJoin(";")));
						break;
					case SettingTypes.Multiline:
						if(IsNotEmpty && IsSingleValued)
							writer.WriteLine("{0} = {1}".Fmt(Name, GetString().Trim()));
						else
						{
							writer.WriteLine(Name);
							foreach(var val in this)
								writer.WriteLine(" = {0}".Fmt(val).Trim());
						}
						break;
				}
			}

			static bool? ParseNullableBool(string val)
			{
				string s = val.IsNotEmpty() ? val.ToLowerInvariant() : null;
				if(s == "true" || s == "yes" || s == "1")
					return true;
				else if(s == "false" || s == "no" || s == "0")
					return false;
				else return null;
			}
		}
	}
}
