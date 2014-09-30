﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formicant
{
	public class FreeDic<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>
	{
		#region Constructors

		public FreeDic()
			: this(new Dictionary<TKey, TValue>())
		{
		}

		public FreeDic(IEqualityComparer<TKey> comparer)
			: this(new Dictionary<TKey, TValue>(comparer))
		{
		}

		public FreeDic(IDictionary<TKey, TValue> dictionary)
		{
			if(dictionary == null)
				throw new ArgumentNullException("dictionary");
			_dic = dictionary;
		}

		#endregion

		#region IReadOnlyDictionary

		public bool ContainsKey(TKey key)
		{
			return key != null && _dic.ContainsKey(key);
		}

		public IEnumerable<TKey> Keys
		{
			get { return _dic.Keys; }
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if(key != null)
				return _dic.TryGetValue(key, out value);
			else
			{
				value = default(TValue);
				return false;
			}
		}

		public IEnumerable<TValue> Values
		{
			get { return _dic.Values; }
		}

		public int Count
		{
			get { return _dic.Count; }
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dic.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _dic.GetEnumerator();
		}

		#endregion

		#region IDictionary

		public void Add(TKey key, TValue value)
		{
			this[key] = value;
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get { return _dic.Keys; }
		}

		public bool Remove(TKey key)
		{
			return key != null
				? _dic.Remove(key)
				: false;
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get { return _dic.Values; }
		}

		public TValue this[TKey key]
		{
			get
			{
				return ContainsKey(key)
					? _dic[key]
					: default(TValue);
			}
			set
			{
				if(value != null)
					_dic[key] = value;
			}
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this[item.Key] = item.Value;
		}

		public void Clear()
		{
			_dic.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return _dic.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			_dic.CopyTo(array, arrayIndex);
		}

		public bool IsReadOnly
		{
			get { return _dic.IsReadOnly; }
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return _dic.Remove(item);
		}

		#endregion

		#region private

		readonly IDictionary<TKey, TValue> _dic;

		#endregion
	}
}
