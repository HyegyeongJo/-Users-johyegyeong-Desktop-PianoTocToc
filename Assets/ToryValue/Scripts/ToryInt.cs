namespace ToryValue 
{
	[System.Serializable]
	public class ToryInt : ToryValue<int>
	{
		protected ToryInt()
		{
		}

		public ToryInt(string key) : this(key, 0)
		{
		}

		public ToryInt(string key, int value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryInt(string key, int value, int defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryInt(string key, int value, int defaultValue, int savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryInt(string key, int value, System.Func<int, int> setterFunc) 
			: base(key, value, setterFunc)
		{
		}

		public ToryInt(string key, int value, System.Func<int, int> setterFunc, System.Func<int, int> getterFunc) 
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetInt(Key, Value);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyInt(Key);
		}

		protected override int LoadSavedValueFromPlayerPrefs()
		{
			return PlayerPrefsElite.GetInt(Key);
		}
	}
}