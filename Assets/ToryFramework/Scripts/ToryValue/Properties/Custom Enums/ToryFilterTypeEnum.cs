using ToryValue;

namespace ToryFramework.Input
{
	[System.Serializable]
	public class ToryFilterTypeEnum : ToryEnum<FilterType>
	{
		protected ToryFilterTypeEnum()
		{
		}

		public ToryFilterTypeEnum(string key) : this(key, default(FilterType))
		{
		}

		public ToryFilterTypeEnum(string key, FilterType value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryFilterTypeEnum(string key, FilterType value, FilterType defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryFilterTypeEnum(string key, FilterType value, FilterType defaultValue, FilterType savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryFilterTypeEnum(string key, FilterType value, System.Func<FilterType, FilterType> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryFilterTypeEnum(string key, FilterType value, System.Func<FilterType, FilterType> setterFunc, System.Func<FilterType, FilterType> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}
	}
}
