namespace ToryValue 
{
	[System.Serializable]
	public class ToryFloat : ToryValue<float>
	{
		protected ToryFloat()
		{
		}

		public ToryFloat(string key) : this(key, 0f)
		{
		}

		public ToryFloat(string key, float value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryFloat(string key, float value, float defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryFloat(string key, float value, float defaultValue, float savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryFloat(string key, float value, System.Func<float, float> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryFloat(string key, float value, System.Func<float, float> setterFunc, System.Func<float, float> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetFloat(Key, Value);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyFloat(Key);
		}

		protected override float LoadSavedValueFromPlayerPrefs()
		{
			return PlayerPrefsElite.GetFloat(Key);
		}
	}
}