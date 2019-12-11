namespace ToryValue 
{
	[System.Serializable]
	public class ToryString : ToryValue<string>
	{
		protected ToryString()
		{
		}

		public ToryString(string key) : this(key, "")
		{
		}

		public ToryString(string key, string value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryString(string key, string value, string defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryString(string key, string value, string defaultValue, string savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryString(string key, string value, System.Func<string, string> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryString(string key, string value, System.Func<string, string> setterFunc, System.Func<string, string> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetString(Key, Value);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyString(Key);
		}

		protected override string LoadSavedValueFromPlayerPrefs()
		{
			return PlayerPrefsElite.GetString(Key);
		}
	}
}