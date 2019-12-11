using UnityEngine;
using ToryValue;

namespace ToryFramework.Input
{
	[System.Serializable]
	public class ToryFilterTypeEnum : ToryValue<FilterType>
	{
		#region FIELDS

		[SerializeField] string key;

		[SerializeField] FilterType currentValue;

		[SerializeField] FilterType defaultValue;

		[SerializeField] FilterType savedValue;

		#endregion



		#region PROPERTIES

		public override string Key 			{ get { return key; } set { key = value; }}

		public override FilterType Value			
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
		public override FilterType DefaultValue 	
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
		public override FilterType SavedValue 		
		{
			get
			{ 
				if (PlayerPrefsElite.VerifyInt(KeyFormatter.GetSavedKey(Key)))
				{
					savedValue = (FilterType)PlayerPrefsElite.GetInt(KeyFormatter.GetSavedKey(Key));
				}
				return savedValue; 
			} 
			set
			{
				// Change the value.
				bool changed = false;
				if (!savedValue.Equals(value))
				{
					savedValue = value;
					changed = true;
				}

				// Check the key.
				if (Key.Equals(""))
				{
					Debug.LogWarning("Please set the key of ToryValue before using it. " +
					                 "You are trying to save a ToryValue with an empty(\"\") key.");
				}

				// Save the value.
				PlayerPrefsElite.SetInt(KeyFormatter.GetSavedKey(Key), (int)savedValue);
#if UNITY_EDITOR
				if (Application.isPlaying)
				{
#endif
					TriggerValueSavedEvent(savedValue);
					if (changed)
					{
						TriggerSavedValueChangedEvent(value);
					}
#if UNITY_EDITOR
				}
#endif
			}
		}

		#endregion



		#region CONSTRUCTOR

		public ToryFilterTypeEnum()
		{
		}

		public ToryFilterTypeEnum(string key) : this()
		{
			this.key = key;
		}

		public ToryFilterTypeEnum(string key, FilterType value) : this(key)
		{
			this.currentValue = value;
		}

		public ToryFilterTypeEnum(string key, FilterType value, FilterType defaultValue) : this(key, value)
		{
			this.defaultValue = defaultValue;
		}

		public ToryFilterTypeEnum(string key, FilterType value, FilterType defaultValue, FilterType savedValue) : this(key, value, defaultValue)
		{
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
				Value = savedValue = (FilterType)PlayerPrefsElite.GetInt(KeyFormatter.GetSavedKey(Key));
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
