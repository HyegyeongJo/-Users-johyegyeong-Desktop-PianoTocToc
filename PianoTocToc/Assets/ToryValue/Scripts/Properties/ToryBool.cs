using UnityEngine;

namespace ToryValue 
{
	/// <summary>
	/// The set of boolean values, which supports save and load functionalities.
	/// The value of this type can be serialized. (i.e., can be visualized in Unity editor.)
	/// </summary>
	[System.Serializable]
	public class ToryBool : ToryValue<bool>
	{
		#region FIELDS

		[SerializeField] string key;

		[SerializeField] bool currentValue;

		[SerializeField] bool defaultValue;

		[SerializeField] bool savedValue;

		#endregion



		#region PROPERTIES

		public override string Key 			{ get { return key; } set { key = value; }}

		public override bool Value			
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
		public override bool DefaultValue 	
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
		public override bool SavedValue 		
		{
			get
			{
				if (PlayerPrefsElite.VerifyBoolean(KeyFormatter.GetSavedKey(Key)))
				{
					savedValue = PlayerPrefsElite.GetBoolean(KeyFormatter.GetSavedKey(Key));
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
				PlayerPrefsElite.SetBoolean(KeyFormatter.GetSavedKey(Key), savedValue);
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

		protected ToryBool()
		{
		}

		public ToryBool(string key) : this(key, false, false, false)
		{
		}

		public ToryBool(string key, bool value) : this(key, value, value, value)
		{
		}

		public ToryBool(string key, bool value, bool defaultValue) : this(key, value, defaultValue, defaultValue)
		{
		}

		public ToryBool(string key, bool value, bool defaultValue, bool savedValue)
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
			if (PlayerPrefsElite.VerifyBoolean(KeyFormatter.GetSavedKey(Key)))
			{
				Value = savedValue = PlayerPrefsElite.GetBoolean(KeyFormatter.GetSavedKey(Key));
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
