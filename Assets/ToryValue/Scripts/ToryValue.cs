namespace ToryValue
{
	public abstract class ToryValue<T>
	{
		// Implicit operator references:
		// https://stackoverflow.com/questions/4769379/automatic-type-conversion-in-c-sharp
		// https://docs.microsoft.com/ko-kr/dotnet/csharp/language-reference/keywords/implicit
		// http://crystalcube.co.kr/158
		public static implicit operator T(ToryValue<T> value) { return value.Value; }

		[UnityEngine.SerializeField] string key;
		[UnityEngine.SerializeField] T value;
		[UnityEngine.SerializeField] T defaultValue;
		[UnityEngine.SerializeField] T savedValue;

		public string Key { get { return key; } set { key = value; } }
		public T Value { get { return getterFunc(value); } set { SetValue(value); } }
		[System.Obsolete("This property is obsolete. Use Save or GetSavedValue methods instead.")]
		public T SavedValue
		{
			get { return getterFunc(savedValue); }
			set
			{
				value = setterFunc(value);
				bool changed = !savedValue.Equals(value);
				savedValue = value;
				if (changed)
				{
					SavedValueChanged(savedValue);
				}
			}
		}
		[System.Obsolete("This property is obsolete. Use GetDefaultValue and SetDefaultValue methods instead.")]
		public T DefaultValue
		{
			get { return getterFunc(defaultValue); }
			set
			{
				value = setterFunc(value);
				bool changed = !defaultValue.Equals(value);
				defaultValue = value;
				if (changed)
				{
					DefaultValueChanged(defaultValue);
				}
			}
		}

		System.Func<T, T> setterFunc = (arg) => { return arg; };
		System.Func<T, T> getterFunc = (arg) => { return arg; };

		public System.Func<T, T> SetterFunc { get { return setterFunc; } set { SetSetterFunc(value); } }
		public System.Func<T, T> GetterFunc { get { return getterFunc; } set { SetGetterFunc(value); } }

		void SetSetterFunc(System.Func<T, T> func)
		{
			AssertGetterAndSetterFuncsShouldNotBeNull(func);
			setterFunc = func;
		}

		void AssertGetterAndSetterFuncsShouldNotBeNull(System.Func<T, T> func)
		{
			if (func == null)
			{
				throw new System.ArgumentException("The getter and setter function should not be null.");
			}
		}

		void SetGetterFunc(System.Func<T, T> func)
		{
			AssertGetterAndSetterFuncsShouldNotBeNull(func);
			getterFunc = func;
		}

		void SetValue(T value)
		{
			value = setterFunc(value);
			if (!this.value.Equals(value))
			{
				this.value = value;
				Changed(value);
				ValueChanged(value);
			}
		}

		/// <summary>
		/// Occurs when the <see cref="Value"/> is changed.
		/// </summary>
		public event System.Action<T> Changed = (T value) => { };

		[System.Obsolete]
		public delegate void ValueEventHandler(T value);
		[System.Obsolete("This event is obsolete. Use Changed event instead.")]
		public event ValueEventHandler ValueChanged = (T value) => { };
		[System.Obsolete("This event is obsolete.")]
		public event ValueEventHandler DefaultValueChanged = (T value) => { };
		[System.Obsolete("This event is obsolete.")]
		public event ValueEventHandler SavedValueChanged = (T value) => { };
		[System.Obsolete("This event is obsolete. Use Saved event instead.")]
		public event ValueEventHandler ValueSaved = (T value) => { };
		[System.Obsolete("This event is obsolete. Use Loaded event instead.")]
		public event ValueEventHandler DefaultValueLoaded = (T value) => { };
		[System.Obsolete("This event is obsolete. Use Loaded event instead.")]
		public event ValueEventHandler SavedValueLoaded = (T value) => { };

		// The following constructor is nessesary to use Unity's serialization.
		protected ToryValue()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToryValue{T}"/> class.
		/// </summary>
		/// <param name="key">Key to save the <see cref="Value"/> to the <see cref="UnityEngine.PlayerPrefs"/>. This value should be unique and consistent.</param>
		/// <param name="value">Value.</param>
		protected ToryValue(string key, T value) : this(key, value, (arg) => { return arg; })
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		protected ToryValue(string key, T value, T defaultValue) : this(key, value, (arg) => { return arg; })
		{
			this.defaultValue = defaultValue;
		}

		[System.Obsolete("This constructor is obsolete.")]
		protected ToryValue(string key, T value, T defaultValue, T savedValue) : this(key, value, (arg) => { return arg; })
		{
			this.defaultValue = defaultValue;
			this.savedValue = savedValue;
		}

		protected ToryValue(string key, T value, System.Func<T, T> setterFunc) : this(key, value, setterFunc, (arg) => { return arg; })
		{
		}

		protected ToryValue(string key, T value, System.Func<T, T> setterFunc, System.Func<T, T> getterFunc)
		{
			AssertKeyIsNotBothNullAndEmpty(key);
			SetSetterFunc(setterFunc);
			SetGetterFunc(getterFunc);
			value = setterFunc(value);
			this.key = key;
			this.value = value;
			this.defaultValue = value;
			this.savedValue = value;
		}

		void AssertKeyIsNotBothNullAndEmpty(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new System.ArgumentException("The key of the ToryValue should not be null or empty.");
			}
		}

		/// <summary>
		/// Saves the <see cref="Value"/> to the <see cref="UnityEngine.PlayerPrefs"/> with the <see cref="Key"/>.
		/// </summary>
		public void Save()
		{
			SavedValue = Value;
			SaveValueToPlayerPrefs();
			Saved(Value);
			ValueSaved(Value);
		}

		protected abstract void SaveValueToPlayerPrefs();

		/// <summary>
		/// Occurs when the <see cref="Value"/> is saved to the <see cref="UnityEngine.PlayerPrefs"/>.
		/// </summary>
		public event System.Action<T> Saved = (T value) => { };

		/// <summary>
		/// Loas the value saved in the <see cref="UnityEngine.PlayerPrefs"/> into the <see cref="Value"/>. 
		/// </summary>
		public void Load()
		{
			bool verified = PlayerPrefsVerified();
			Value = LoadSavedValueIf(verified);
			if (verified)
			{
				Loaded(Value);
				SavedValueLoaded(Value);
			}
		}

		[System.Obsolete("This method is obsolete. Use Load method instead.")]
		public void LoadSavedValue()
		{
			Load();
		}

		protected abstract bool PlayerPrefsVerified();

		/// <summary>
		/// Loads the value saved in the <see cref="UnityEngine.PlayerPrefs"/>.
		/// </summary>
		/// <returns>The saved value.</returns>
		T LoadSavedValueIf(bool verified)
		{
			T savedValue = SavedValue;
			if (verified)
			{
				savedValue = LoadSavedValueFromPlayerPrefs();
			}
			return savedValue;
		}

		protected abstract T LoadSavedValueFromPlayerPrefs();

		/// <summary>
		/// Occurs when the saved value is successfully verified and loaded from the <see cref="UnityEngine.PlayerPrefs"/> into the <see cref="Value"/>.
		/// </summary>
		public event System.Action<T> Loaded = (T value) => { };

		/// <summary>
		/// Reverts the <see cref="Value"/> to the default.
		/// </summary>
		public void Revert()
		{
			Value = DefaultValue;
			Reverted(Value);
			DefaultValueLoaded(Value);
		}

		[System.Obsolete("This method is obsolete. Use Revert method instead.")]
		public void LoadDefaultValue()
		{
			Revert();
		}

		/// <summary>
		/// Occurs when the <see cref="Value"/> is reverted to the default.
		/// </summary>
		public event System.Action<T> Reverted = (T value) => { };

		/// <summary>
		/// 초기값을 반환합니다. 초기값을 확인하고 싶을 경우에만 사용하세요. 만약 값을 초기값으로 되돌리고 싶을 경우에는 <see cref="Revert"/> 메소드를 사용하세요.
		/// </summary>
		/// <returns>The default value.</returns>
		public T GetDefaultValue()
		{
			return DefaultValue;
		}

		public void SetDefaultValue(T value)
		{
			DefaultValue = value;
		}

		/// <summary>
		/// 저장값을 반환합니다. 저장값을 확인하고 싶을 경우에만 사용하세요. 만약 값에 저장값을 불러오고 싶을 경우에는 <see cref="Load"/> 메소드를 사용하세요.
		/// </summary>
		/// <returns>The saved value.</returns>
		public T GetSavedValue()
		{
			bool verified = PlayerPrefsVerified();
			return LoadSavedValueIf(verified);
		}
	}
}