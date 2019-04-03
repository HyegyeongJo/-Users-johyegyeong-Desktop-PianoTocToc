namespace ToryValue
{
	[System.Serializable]
	public class ToryVector2 : ToryValue<UnityEngine.Vector2>
	{
		protected ToryVector2()
		{
		}

		public ToryVector2(string key) : this(key, UnityEngine.Vector2.zero)
		{
		}

		public ToryVector2(string key, UnityEngine.Vector2 value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryVector2(string key, UnityEngine.Vector2 value, UnityEngine.Vector2 defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryVector2(string key, UnityEngine.Vector2 value, UnityEngine.Vector2 defaultValue, UnityEngine.Vector2 savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryVector2(string key, UnityEngine.Vector2 value, System.Func<UnityEngine.Vector2, UnityEngine.Vector2> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryVector2(string key, UnityEngine.Vector2 value, System.Func<UnityEngine.Vector2, UnityEngine.Vector2> setterFunc, System.Func<UnityEngine.Vector2, UnityEngine.Vector2> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetVector2(Key, Value);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyVector2(Key);
		}

		protected override UnityEngine.Vector2 LoadSavedValueFromPlayerPrefs()
		{
			return PlayerPrefsElite.GetVector2(Key);
		}
	}
}