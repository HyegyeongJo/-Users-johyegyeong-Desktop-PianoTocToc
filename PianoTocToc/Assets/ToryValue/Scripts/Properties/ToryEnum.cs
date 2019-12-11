using UnityEngine;

namespace ToryValue 
{
	/// <summary>
	/// The set of custom enumerations, which supports save and load functionalities.
	/// Note that the value of this type cannot be serialized as it is generic type. (i.e., cannot be visualized in Unity editor.)
	/// </summary>
	public class ToryEnum<T> : ToryValue<T>
	{
		#region FIELDS

		protected string key;

		protected T currentValue;

		protected T defaultValue;

		protected T savedValue;

		#endregion



		#region PROPERTIES

		public override string Key 			{ get { return key; } set { key = value; }}

		public override T Value			
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
		public override T DefaultValue 	
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
		public override T SavedValue 		
		{
			get
			{ 
				if (PlayerPrefsElite.VerifyInt(KeyFormatter.GetSavedKey(Key)))
				{
					int s = PlayerPrefsElite.GetInt(KeyFormatter.GetSavedKey(Key));
					savedValue = (T)System.Enum.ToObject(typeof(T), s);
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
				int s = (int)(object)savedValue;
				PlayerPrefsElite.SetInt(KeyFormatter.GetSavedKey(Key), s);
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

		protected ToryEnum()
		{
		}

		public ToryEnum(string key) : this(key, default(T), default(T), default(T))
		{
		}

		public ToryEnum(string key, T value) : this(key, value, value, value)
		{
		}

		public ToryEnum(string key, T value, T defaultValue) : this(key, value, defaultValue, defaultValue)
		{
		}

		public ToryEnum(string key, T value, T defaultValue, T savedValue)
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
				int s = PlayerPrefsElite.GetInt(KeyFormatter.GetSavedKey(Key));
				Value = savedValue = (T)System.Enum.ToObject(typeof(T), s);
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
