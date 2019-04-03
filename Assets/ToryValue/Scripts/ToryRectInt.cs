namespace ToryValue
{
	[System.Serializable]
	public class ToryRectInt : ToryValue<UnityEngine.RectInt>
	{
		protected ToryRectInt()
		{
		}

		public ToryRectInt(string key) : this(key, new UnityEngine.RectInt(0, 0, 1, 1))
		{
		}

		public ToryRectInt(string key, UnityEngine.RectInt value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryRectInt(string key, UnityEngine.RectInt value, UnityEngine.RectInt defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryRectInt(string key, UnityEngine.RectInt value, UnityEngine.RectInt defaultValue, UnityEngine.RectInt savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryRectInt(string key, UnityEngine.RectInt value, System.Func<UnityEngine.RectInt, UnityEngine.RectInt> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryRectInt(string key, UnityEngine.RectInt value, System.Func<UnityEngine.RectInt, UnityEngine.RectInt> setterFunc, System.Func<UnityEngine.RectInt, UnityEngine.RectInt> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}

		protected override void SaveValueToPlayerPrefs()
		{
			PlayerPrefsElite.SetVector4(Key, RectInt2Vector4(Value));
		}

		UnityEngine.Vector4 RectInt2Vector4(UnityEngine.RectInt rect)
		{
			return new UnityEngine.Vector4(rect.x, rect.y, rect.width, rect.height);
		}

		protected override bool PlayerPrefsVerified()
		{
			return PlayerPrefsElite.VerifyVector4(Key);
		}

		protected override UnityEngine.RectInt LoadSavedValueFromPlayerPrefs()
		{
			return Vector42RectInt(PlayerPrefsElite.GetVector4(Key));
		}

		UnityEngine.RectInt Vector42RectInt(UnityEngine.Vector4 v)
		{
			return new UnityEngine.RectInt((int)v.x, (int)v.y, (int)v.z, (int)v.w);
		}
	}
}