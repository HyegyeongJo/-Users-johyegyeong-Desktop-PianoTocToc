namespace ToryValue
{
	[System.Serializable]
	public class ToryVector3 : ToryValue<UnityEngine.Vector3>
	{
		protected ToryVector3()
		{
		}

		public ToryVector3(string key) : this(key, UnityEngine.Vector3.zero)
		{
		}

		public ToryVector3(string key, UnityEngine.Vector3 value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryVector3(string key, UnityEngine.Vector3 value, UnityEngine.Vector3 defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryVector3(string key, UnityEngine.Vector3 value, UnityEngine.Vector3 defaultValue, UnityEngine.Vector3 savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryVector3(string key, UnityEngine.Vector3 value, System.Func<UnityEngine.Vector3, UnityEngine.Vector3> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryVector3(string key, UnityEngine.Vector3 value, System.Func<UnityEngine.Vector3, UnityEngine.Vector3> setterFunc, System.Func<UnityEngine.Vector3, UnityEngine.Vector3> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetVector3(Key, Value);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyVector3(Key);
		}

		protected override UnityEngine.Vector3 LoadSavedValueFromPlayerPrefs()
		{
			return PlayerPrefsElite.GetVector3(Key);
		}
	}
}