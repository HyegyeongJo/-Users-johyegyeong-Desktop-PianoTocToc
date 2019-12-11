namespace ToryValue 
{
	public class ToryEnum<T> : ToryValue<T>
	{
		protected ToryEnum()
		{
		}

		public ToryEnum(string key) : this(key, default(T))
		{
		}

		public ToryEnum(string key, T value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryEnum(string key, T value, T defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryEnum(string key, T value, T defaultValue, T savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryEnum(string key, T value, System.Func<T, T> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryEnum(string key, T value, System.Func<T, T> setterFunc, System.Func<T, T> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetInt(Key, (int)(object)Value);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyInt(Key);
		}

		protected override T LoadSavedValueFromPlayerPrefs()
		{
			int i = PlayerPrefsElite.GetInt(Key);
			return (T)System.Enum.ToObject(typeof(T), i);
		}
	}	
}