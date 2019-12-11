namespace ToryValue
{
	[System.Serializable]
	public class ToryVector4 : ToryValue<UnityEngine.Vector4>
	{
		protected ToryVector4()
		{
		}

		public ToryVector4(string key) : this(key, UnityEngine.Vector4.zero)
		{
		}

		public ToryVector4(string key, UnityEngine.Vector4 value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryVector4(string key, UnityEngine.Vector4 value, UnityEngine.Vector4 defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryVector4(string key, UnityEngine.Vector4 value, UnityEngine.Vector4 defaultValue, UnityEngine.Vector4 savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryVector4(string key, UnityEngine.Vector4 value, System.Func<UnityEngine.Vector4, UnityEngine.Vector4> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryVector4(string key, UnityEngine.Vector4 value, System.Func<UnityEngine.Vector4, UnityEngine.Vector4> setterFunc, System.Func<UnityEngine.Vector4, UnityEngine.Vector4> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetVector4(Key, Value);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyVector4(Key);
		}

		protected override UnityEngine.Vector4 LoadSavedValueFromPlayerPrefs()
		{
			return PlayerPrefsElite.GetVector4(Key);
		}
	}
}