namespace ToryValue
{
	[System.Serializable]
	public class ToryRect : ToryValue<UnityEngine.Rect>
	{
		protected ToryRect()
		{
		}

		public ToryRect(string key) : this(key, UnityEngine.Rect.zero)
		{
		}

		public ToryRect(string key, UnityEngine.Rect value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryRect(string key, UnityEngine.Rect value, UnityEngine.Rect defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryRect(string key, UnityEngine.Rect value, UnityEngine.Rect defaultValue, UnityEngine.Rect savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryRect(string key, UnityEngine.Rect value, System.Func<UnityEngine.Rect, UnityEngine.Rect> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryRect(string key, UnityEngine.Rect value, System.Func<UnityEngine.Rect, UnityEngine.Rect> setterFunc, System.Func<UnityEngine.Rect, UnityEngine.Rect> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetVector4(Key, Rect2Vector4(Value));
		}

		UnityEngine.Vector4 Rect2Vector4(UnityEngine.Rect rect)
		{
			return new UnityEngine.Vector4(rect.x, rect.y, rect.width, rect.height);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyVector4(Key);
		}

		protected override UnityEngine.Rect LoadSavedValueFromPlayerPrefs()
		{
			return Vector42Rect(PlayerPrefsElite.GetVector4(Key));
		}

		UnityEngine.Rect Vector42Rect(UnityEngine.Vector4 v)
		{
			return new UnityEngine.Rect(v.x, v.y, v.z, v.w);
		}
	}
}