using UnityEngine;

namespace ToryValue 
{
	/// <summary>
	/// The set of Vector4 values, which supports save and load functionalities.
	/// The value of this type can be serialized. (i.e., can be visualized in Unity editor.)
	/// </summary>
	[System.Serializable]
	public class ToryVector4 : ToryValue<Vector4>
	{
		#region FIELDS

		[SerializeField] string key;

		[SerializeField] Vector4 currentValue;

		[SerializeField] Vector4 defaultValue;

		[SerializeField] Vector4 savedValue;

		#endregion



		#region PROPERTIES

		public override string Key 			{ get { return key; } set { key = value; }}

		public override Vector4 Value			
		{ 	
			get
			{
				return currentValue;
			}
			set
			{
				if (!currentValue.Equals(value))
				{
					currentValue = value;
#if UNITY_EDITOR
					if (Application.isPlaying)
					{
#endif
						TriggerValueChangedEvent(value);
#if UNITY_EDITOR
					}
#endif
				}
			}
		}

		/// <summary>
		/// Gets or sets the default value from or to the PlayerPrefs.
		/// </summary>
		/// <value>The default value.</value>
		public override Vector4 DefaultValue
		{
			get
			{
				return defaultValue;
			}
			set
			{
				if (!defaultValue.Equals(value))
				{
					defaultValue = value;
#if UNITY_EDITOR
					if (Application.isPlaying)
					{
#endif
						TriggerDefaultValueChangedEvent(value);
#if UNITY_EDITOR
					}
#endif
				}
			}
		}

		/// <summary>
		/// Gets or sets the saved value from or to the PlayerPrefs.
		/// </summary>
		/// <value>The saved value.</value>
		public override Vector4 SavedValue
		{
			get
			{
				if (PlayerPrefsElite.VerifyVector4(KeyFormatter.GetSavedKey(Key)))
				{
					savedValue = PlayerPrefsElite.GetVector4(KeyFormatter.GetSavedKey(Key));
				}
				return savedValue;
			}
			set
			{
				// Change the value.
				if (!savedValue.Equals(value))
				{
					savedValue = value;
#if UNITY_EDITOR
					if (Application.isPlaying)
					{
#endif
						TriggerSavedValueChangedEvent(value);
#if UNITY_EDITOR
					}
#endif
				}

				// Check the key.
				if (Key.Equals(""))
				{
					Debug.LogWarning("Please set the key of ToryValue before using it. " +
									 "You are trying to save a ToryValue with an empty(\"\") key.");
				}

				// Save the value.
				PlayerPrefsElite.SetVector4(KeyFormatter.GetSavedKey(Key), savedValue);
#if UNITY_EDITOR
				if (Application.isPlaying)
				{
#endif
					TriggerValueSavedEvent(savedValue);
#if UNITY_EDITOR
				}
#endif
			}
		}

		#endregion



		#region CONSTRUCTOR

		protected ToryVector4()
		{
		}

		public ToryVector4(string key) : this(key, Vector4.zero, Vector4.zero, Vector4.zero)
		{
		}

		public ToryVector4(string key, Vector4 value) : this(key, value, value, value)
		{
		}

		public ToryVector4(string key, Vector4 value, Vector4 defaultValue) : this(key, value, defaultValue, defaultValue)
		{
		}

		public ToryVector4(string key, Vector4 value, Vector4 defaultValue, Vector4 savedValue)
		{
			this.key = key;
			this.currentValue = value;
			this.defaultValue = defaultValue;
			this.savedValue = savedValue;
		}

		#endregion



		#region EVENTS

		#endregion



		#region UNITY_FRAMEWORK

		#endregion



		#region CUSTOM_FRAMEWORK

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		/// <summary>
		/// Loads the saved value from the PlayerPrefs to the current value. Returns <c>true</c>, if the saved value was verified and loaded correctly, and <c>false</c> if not.
		/// </summary>
		/// <returns><c>true</c>, if the saved value was verified and loaded correctly, <c>false</c> otherwise.</returns>
		public override bool LoadSavedValue()
		{
			if (PlayerPrefsElite.VerifyVector4(KeyFormatter.GetSavedKey(Key)))
			{
				Value = savedValue = PlayerPrefsElite.GetVector4(KeyFormatter.GetSavedKey(Key));
				TriggerSavedValueLoadedEvent(savedValue);
				return true;
			}
			Value = savedValue;
			SavedValue = savedValue;
			return false;
		}

		#endregion
	}	
}
