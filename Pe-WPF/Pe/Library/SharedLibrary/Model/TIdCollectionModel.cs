﻿namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using System.Xml.Serialization;
	using System.Reflection;

	[Serializable]
	public class TIdCollectionModel<TKey, TValue>: ObservableCollection<TValue>, IIsDisposed, IModel
		where TValue: ITId<TKey>
		where TKey: IComparable
	{
		#region variable

		protected Dictionary<TKey, TValue> _map = new Dictionary<TKey, TValue>();
		//protected bool _isReadOnly = false;

		[IgnoreDataMember, XmlIgnore]
		IEnumerable<PropertyInfo> _propertyInfos = null;

		#endregion

		public TIdCollectionModel()
			: base()
		{
			//Items = new List<TValue>();
			IsDisposed = false;
		}

		~TIdCollectionModel()
		{
			Dispose(false);
		}

		#region property

		//[DataMember]
		//public List<TValue> Items { get; set; }

		public IEnumerable<TKey> Keys { get { return this.Select(i => i.Id); } }

		#endregion

		#region IIsDisposed

		[IgnoreDataMember, XmlIgnore]
		public bool IsDisposed { get; private set; }

		protected virtual void Dispose(bool disposing)
		{
			if(IsDisposed) {
				return;
			}

			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		#region IModel

		[IgnoreDataMember, XmlIgnore]
		public virtual string DisplayText
		{
			get { return GetType().FullName; }
		}

		[IgnoreDataMember, XmlIgnore]
		public IEnumerable<PropertyInfo> PropertyInfos
		{
			get
			{
				if(this._propertyInfos == null) {
					this._propertyInfos = GetType().GetProperties();
				}

				return this._propertyInfos;
			}
		}

		public IEnumerable<string> GetNameValueList()
		{
			var nameValueMap = ReflectionUtility.GetMembers(this, PropertyInfos);
			return ReflectionUtility.GetNameValueStrings(nameValueMap);
		}

		public virtual void Correction()
		{ }

		#endregion

		#region ObservableCollection

		/// <summary>
		/// 要素を追加する。
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException">valueがnull</exception>
		/// <exception cref="ArgumentException">value.Idがすでに存在する</exception>
		public new void Add(TValue value)
		{
			CheckId(value);

			Add(value, true);
		}

		public new void Clear()
		{
			base.Clear();
			this._map.Clear();
		}

		public new void ClearItems()
		{
			base.ClearItems();
			this._map.Clear();
		}

		public new bool Remove(TValue item)
		{
			if(base.Remove(item)) {
				this._map.Remove(item.Id);
				return true;
			}

			return false;
		}

		public bool Remove(TKey key)
		{
			var index = FindIndex(key);
			if (index != -1) {
				Items.RemoveAt(index);
				this._map.Remove(key);

				return true;
			}

			return false;
		}

		#region indexer

		public TValue this[TKey key]
		{
			get
			{
				TValue result;
				if (this._map.TryGetValue(key, out result)) {
					return result;
				}

				result = Items.FirstOrDefault(i => key.CompareTo(i.Id) == 0);
				if (result != null) {
					this._map[result.Id] = result;
					return result;
				}

				throw new IndexOutOfRangeException(GetIdString(key));
			}
		}

		#endregion

		#endregion

		#region function

		static string GetIdString(TKey id)
		{
			return string.Format("Id = {0}", id);
		}
		static string GetIdString(ITId<TKey> id)
		{
			return GetIdString(id);
		}
		static bool IsEqual(TKey a, TKey b)
		{
			return a.CompareTo(b) == 0;
		}
		static bool IsEqual(TValue a, TValue b)
		{
			return IsEqual(a.Id, b.Id);
		}
		
		void CheckId(TValue item)
		{
			CheckUtility.Enforce<ArgumentException>(item.IsSafeId(item.Id));
		}

		void Add(TValue value, bool check)
		{
			if (check) {
				if (value == null) {
					throw new ArgumentNullException("value");
				}

				if(this.Any(i => IsEqual(value, i))) {
					throw new ArgumentException(GetIdString(value));
				}

				CheckId(value);
			}

			base.Add(value);
			this._map[value.Id] = value;
		}

		public int FindIndex(TKey key)
		{
			return Items.FindIndex(i => IsEqual(key, i.Id));
		}

		/// <summary>
		/// 要素を設定する。
		/// <para>既に存在する場合は上書きされる。</para>
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException">valueがnull</exception>
		public void Set(TValue value)
		{
			if (value == null) {
				throw new ArgumentNullException("value");
			}

			var index = FindIndex(value.Id);
			if(index != -1) {
				Items[index] = value;
				this._map[value.Id] = value;
			} else {
				CheckId(value);
				Add(value, false);
			}
		}

		/// <summary>
		/// IDの入れ替え。
		/// <para>オブジェクトの入れ替えでないことに注意。</para>
		/// </summary>
		/// <param name="keyA"></param>
		/// <param name="keyB"></param>
		public void SwapId(TKey keyA, TKey keyB)
		{
			var valueA = this[keyA];
			var valueB = this[keyB];
			valueA.Id = keyB;
			valueB.Id = keyA;
			this._map[keyA] = valueB;
			this._map[keyB] = valueA;
		}

		public void SwapObject(TValue a, TValue b)
		{
			var indexA = IndexOf(a);
			var indexB = IndexOf(b);

			RemoveAt(indexA);
			RemoveAt(indexB);

			Insert(indexA, b);
			Insert(indexB, a);
		}

		public bool TryGetValue(TKey key, out TValue result)
		{
			if (this._map.TryGetValue(key, out result)) {
				return true;
			}

			var item = Items.FirstOrDefault(i => IsEqual(key, i.Id));
			if (item != null) {
				result = item;
				return true;
			}
			result = default(TValue);
			return false;
		}

		public bool Contains(TKey key)
		{
			TValue temp;
			if (this._map.TryGetValue(key, out temp)) {
				return true;
			}

			return Items.Any(i => IsEqual(key, i.Id));
		}

		void ChangeId(TKey src, TKey dst)
		{
			TValue tempValue;
			if (TryGetValue(dst, out tempValue)) {
				throw new ArgumentException(string.Format("exists key({0})", dst));
			}

			var srcValue = this[src];
			srcValue.Id = dst;
			this._map.Remove(src);
			this._map[dst] = srcValue;
		}

		#endregion
	}
}
