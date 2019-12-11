using UnityEngine;

namespace ToryValue 
{
	/// <summary>
	/// The set of integer values, which supports save and load functionalities.
	/// The value of this type can be serialized. (i.e., can be visualized in Unity editor.)
	/// </summary>
	[System.Serializable]
	public class ToryInt : ToryValue<int>
	{
		#region FIELDS

		[SerializeField] string key;

		[SerializeField] int currentValue;

		[SerializeField] int defaultValue;

		[SerializeField] int savedValue;

		#endregion
		
		

		#region PROPERTIES

		public override string Key 			{ get { return key; } set { key = value; }}

		public override int Value			
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
		public override int DefaultValue
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
		public override int SavedValue
		{
			get
			{
				if (PlayerPrefsElite.VerifyInt(KeyFormatter.GetSavedKey(Key)))
				{
					savedValue = PlayerPrefsElite.GetInt(KeyFormatter.GetSavedKey(Key));
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
				PlayerPrefsElite.SetInt(KeyFormatter.GetSavedKey(Key), savedValue);
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

		protected ToryInt()
		{
		}

		public ToryInt(string key) : this(key, 0, 0, 0)
		{
		}

		public ToryInt(string key, int value) : this(key, value, value, value)
		{
		}

		public ToryInt(string key, int value, int defaultValue) : this(key, value, defaultValue, defaultValue)
		{
		}

		public ToryInt(string key, int value, int defaultValue, int savedValue)
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
			if (PlayerPrefsElite.VerifyInt(KeyFormatter.GetSavedKey(Key)))
			{
				Value = savedValue = PlayerPrefsElite.GetInt(KeyFormatter.GetSavedKey(Key));
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
