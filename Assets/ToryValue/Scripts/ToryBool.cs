namespace ToryValue 
{
	[System.Serializable]
	public class ToryBool : ToryValue<bool>
	{
		protected ToryBool()
		{
		}

		public ToryBool(string key) : this(key, false)
		{
		}

		public ToryBool(string key, bool value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryBool(string key, bool value, bool defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryBool(string key, bool value, bool defaultValue, bool savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryBool(string key, bool value, System.Func<bool, bool> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryBool(string key, bool value, System.Func<bool, bool> setterFunc, System.Func<bool, bool> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetBoolean(Key, Value);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyBoolean(Key);
		}

		protected override bool LoadSavedValueFromPlayerPrefs()
		{
			return PlayerPrefsElite.GetBoolean(Key);
		}
	}	
}