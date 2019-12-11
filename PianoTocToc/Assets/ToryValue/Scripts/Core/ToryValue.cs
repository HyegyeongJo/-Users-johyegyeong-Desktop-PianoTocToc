namespace ToryValue 
{
	public abstract class ToryValue<T>
	{
		#region FIELDS

		#endregion



		#region PROPERTIES

		public abstract string Key 			{ get; set; }

		public abstract T Value 			{ get; set; }

		/// <summary>
		/// Gets or sets the default value from or to the PlayerPrefs.
		/// </summary>
		/// <value>The default value.</value>
		public abstract T DefaultValue 		{ get; set; }

		/// <summary>
		/// Gets or sets the saved value from or to the PlayerPrefs.
		/// </summary>
		/// <value>The saved value.</value>
		public abstract T SavedValue		{ get; set; }

		#endregion



		#region CONSTRUCTOR

		protected ToryValue()
		{
		}

		#endregion



		#region EVENTS

		public delegate void ValueEventHandler(T value);

		// Changed

		public event ValueEventHandler ValueChanged = (T value) => {};

		public event ValueEventHandler DefaultValueChanged = (T value) => {};

		public event ValueEventHandler SavedValueChanged = (T value) => {};

		// Saved

		public event ValueEventHandler ValueSaved = (T value) => {};

		// Loaded

		public event ValueEventHandler DefaultValueLoaded = (T value) => {};

		public event ValueEventHandler SavedValueLoaded = (T value) => {};


		#endregion



		#region CUSTOM_FRAMEWORK

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		/// <summary>
		/// Saves the current value to the saved value.
		/// </summary>
		public void Save()
		{
			SavedValue = Value;
		}

		/// <summary>
		/// Sets the current value from the default value.
		/// </summary>
		public void LoadDefaultValue()
		{
			Value = DefaultValue;
			DefaultValueLoaded(DefaultValue);
		}

		/// <summary>
		/// Loads the saved value from the PlayerPrefs to the value. Returns <c>true</c>, if the saved value was verified and loaded correctly, and <c>false</c> if not.
		/// </summary>
		/// <returns><c>true</c>, if the saved value was verified and loaded correctly, <c>false</c> otherwise.</returns>
		public abstract bool LoadSavedValue();


		// Event triggers

		protected void TriggerValueChangedEvent(T value)
		{
			ValueChanged(value);
		}

		protected void TriggerDefaultValueChangedEvent(T value)
		{
			DefaultValueChanged(value);
		}

		protected void TriggerSavedValueChangedEvent(T value)
		{
			SavedValueChanged(value);
		}

		protected void TriggerValueSavedEvent(T value)
		{
			ValueSaved(value);
		}

		protected void TriggerDefaultValueLoadedEvent(T value)
		{
			DefaultValueLoaded(value);
		}

		protected void TriggerSavedValueLoadedEvent(T value)
		{
			SavedValueLoaded(value);
		}

		#endregion
	}
}
