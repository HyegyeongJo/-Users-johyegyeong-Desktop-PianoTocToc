using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ToryFramework.Behaviour;
using ToryValue;

namespace ToryFramework.Input
{
	#region ENUMS

	// Data Properties

	/// <summary>
	/// The type of the data.
	/// Choose one of them according to the type of input signal.
	/// The type of ToryInput is modified in the type specified here,
	/// and then you can access ToryInput with this type from other script.
	/// </summary>
	public enum DataType 		{ Int, Float, Vector2, Vector3, Vector4, Quaternion, KeyCode }

	/// <summary>
	/// The type of the interaction over time.
	/// If you receive data every frame or regular interval, choose the CONTINUOUS type.
	/// If not, choose the DISCONTINUOUS type.
	/// </summary>
	public enum InteractionType 	{ CONTINUOUS, DISCONTINUOUS }


	// Filters

	/// <summary>
	/// The type of the filter.
	/// </summary>
	public enum FilterType 		{ NONE, ONE_EURO_FILTER, ENSEMBLE_AVERAGE }

	#endregion


	/// <summary>
	/// The base abstarct class of generic ToryInput.
	/// </summary>
	public abstract class ToryInput<T> where T : struct
	{
		#region CONSTRUCTOR

		protected ToryInput() { }

		#endregion



		#region FIELDS

		protected List<ToryMultiInput<T>> multiInputList;

		float interactionGauge;

		#endregion



		#region PROPERTIES

		// Behaviours

		protected ToryFrameworkBehaviour FrameworkBehaviour 	{ get { return ToryFrameworkBehaviour.Instance; }}

		protected ToryInputBehaviour InputBehaviour 			{ get { return ToryInputBehaviour.Instance; }}

		// Data Properties

		/// <summary>
		/// Gets the type of the data (Read Only).
		/// You can only change this value in the inspector of ToryInput gameObject before starting the game.
		/// </summary>
		/// <value>The type of the data.</value>
		public DataType DataType 								{ get { return InputBehaviour.DataType; }}

		/// <summary>
		/// Gets or sets the type of the interaction over time.
		/// </summary>
		/// <value>The type of the interaction.</value>
		public ToryInteractionTypeEnum InteractionType 			{ get { return InputBehaviour.InteractionType; } set { InputBehaviour.InteractionType = value; }}


		// Values

		/// <summary>
		/// Gets or sets the raw value.
		/// You can set this value using <see cref="M:SetRawValue"/> method when receiving the data from <see cref="T:Gemstone"/>, <see cref="T:ReactZ"/>, etc. 
		/// </summary>
		/// <value>The raw value.</value>
		public abstract T RawValue 								{ get; protected set; }

		/// <summary>
		/// Gets or sets the processed value.
		/// This value is calculated by applying the <see cref="P:FilterType"/> and multiplying the <see cref="P:Gain"/> to the <see cref="P:RawValue"/>.
		/// </summary>
		/// <value>The processed value.</value>
		public abstract T ProcessedValue 						{ get; protected set; }

		/// <summary>
		/// Gets or sets the value to amplify the raw value.
		/// </summary>
		/// <value>The gain.</value>
		public ToryFloat Gain 									{ get { return InputBehaviour.Gain; } set { InputBehaviour.Gain = value; }}


		// Filters

		/// <summary>
		/// Gets or sets the type of the filter.
		/// </summary>
		/// <value>The type of the filter.</value>
		public ToryFilterTypeEnum FilterType 					{ get { return InputBehaviour.FilterType; } set { InputBehaviour.FilterType = value; }}

		/// <summary>
		/// Gets or sets the frequency of 1€ filter.
		/// The value cannot be smaller than 0.
		/// </summary>
		/// <value>The frequency of 1€ filter.</value>
		public virtual ToryFloat OEFFrequency 					{ get { return InputBehaviour.OEFFrequency; } set { InputBehaviour.OEFFrequency = value; }}

		/// <summary>
		/// Gets or sets the window size of the ensemble average filter.
		/// The value cannot be smaller than 1.
		/// </summary>
		/// <value>The window size of the ensemble average filter.</value>
		public ToryInt EnsembleSize								{ get { return InputBehaviour.EnsembleSize; } set { InputBehaviour.EnsembleSize = value; }}


		// Interaction Determination

		/// <summary>
		/// Gets or sets the interaction gauge which responds to the input and remains 0 without interaction.
		/// If the <see cref="P:InteractionType"/> is set to <see cref="T:CONTINUOUS"/>, this value is calculated by the absolute value of the change of the <see cref="P:ProcessedValue"/> over time.
		/// If the <see cref="P:InteractionType"/> is set to <see cref="T:DISCONTINUOUS"/>, this value is set to 1 with interaction, and set to 0 without interaction.
		/// </summary>
		/// <value>The interaction gauge.</value>
		public float InteractionGauge 							{ get { return interactionGauge; }
			protected set
			{
				interactionGauge = Mathf.Max(0f, value);
			}
		}

		/// <summary>
		/// Gets a value indicating whether a player left from the game.
		/// </summary>
		/// <value><c>true</c> if is player left; otherwise, <c>false</c>.</value>
		public bool IsPlayerLeft 								{ get; private set; }

		/// <summary>
		/// Gets of sets the value determining if there is interaction, or not.
		/// If the <see cref="P:InteractionGauge"/> is less than this value, it is considered that there is no interaction.
		/// The value cannot be smaller than 0.
		/// </summary>
		/// <value>The minimum interaction.</value>
		public ToryFloat MinimumInteraction 					{ get { return InputBehaviour.MinimumInteraction; } set { InputBehaviour.MinimumInteraction = value; }}


		// Multi-input

		/// <summary>
		/// Gets the multi-input array.
		/// </summary>
		/// <value>The multi inputs.</value>
		public abstract ToryMultiInput<T>[] MultiInputs 		{ get; protected set; }

		/// <summary>
		/// Gets or sets the maximum number of multi-inputs.
		/// If set to 0, the multi-input is not used.
		/// </summary>
		/// <value>The maximum multi input count.</value>
		public ToryInt MaxMultiInputCount						{ get { return InputBehaviour.MaxMultiInputCount; } set { InputBehaviour.MaxMultiInputCount = value; }}


		#endregion



		#region EVENTS

		// Delegates

		public delegate void ValueReceivedEventHandler(T processedValue);

		public delegate void InteractionEventHandler();

		public delegate void MultiInputsEventHandler(ToryMultiInput<T> input);

		public delegate void MultiInputValueReceivedEventHandler(ToryMultiInput<T> input, T processedValue);

		public delegate void MultiInputInteractionEventHandler(ToryMultiInput<T> input);


		// Value

		/// <summary>
		/// Occurs when a value received.
		/// </summary>
		public event ValueReceivedEventHandler ValueReceived = (T processedValue) => {};


		// Interaction Determination

		/// <summary>
		/// Occurs when a player interacts with the game.
		/// </summary>
		public event InteractionEventHandler Interacted = () => {};

		/// <summary>
		/// Occurs when a player left.
		/// </summary>
		public event InteractionEventHandler PlayerLeft = () => {};



		// Multi-input

		/// <summary>
		/// Occurs when a multi-input added.
		/// The parameter refers to the added multi-input.
		/// </summary>
		public event MultiInputsEventHandler MultiInputAdded = (ToryMultiInput<T> input) => {};

		/// <summary>
		/// Occurs when a multi-input removed.
		/// The parameter refers to the removed multi-input.
		/// </summary>
		public event MultiInputsEventHandler MultiInputRemoved = (ToryMultiInput<T> input) => {};

		/// <summary>
		/// Occurs when a multi-input receives a value.
		/// The first parameter refers to the multi-input instance which receives the value.
		/// </summary>
		public event MultiInputValueReceivedEventHandler MultiInputValueReceived = (ToryMultiInput<T> input, T processedValue) => {};

		/// <summary>
		/// Occurs when a player interacts with a multi-input.
		/// The parameter refers to the multi-input instance with which a player intearcts.
		/// </summary>
		public event MultiInputInteractionEventHandler MultiInputInteracted = (ToryMultiInput<T> input) => {};


		#endregion



		#region EVENT_HANDLERS

		void OnSceneEnded()
		{
			InputBehaviour.StartCoroutine(ManUtils.ManCoroutine.WaitAndAction(1, () => 
			{ 
				IsPlayerLeft = false;
			}));
		}

		void OnInteractionCheckTimerTimedOut()
		{
			IsPlayerLeft = true;
			PlayerLeft();
		}

		#endregion



		#region METHODS

		protected void ResetEvents()
		{
			// Value
			ValueReceived = (T processedValue) => {};

			// Interaction Determination
			Interacted = () => {};
			PlayerLeft = () => {};

			// Multi-input
			MultiInputAdded = (ToryMultiInput<T> input) => {};
			MultiInputRemoved = (ToryMultiInput<T> input) => {};
			MultiInputValueReceived = (ToryMultiInput<T> input, T processedValue) => {};
			MultiInputInteracted = (ToryMultiInput<T> input) => {};

			// Reset multi-input events.
			for (int i = 0; i < MultiInputs.Length ; i++)
			{
				System.Type type = MultiInputs[i].GetType();
				MethodInfo method = type.GetMethod("ResetEvents", (BindingFlags.NonPublic | 
				                                                   BindingFlags.Public | 
				                                                   BindingFlags.Instance));
				if (method != null)
				{
					method.Invoke(MultiInputs[i], null);
				}
			}
		}

		protected virtual void Init()
		{
			// Multi-input
			multiInputList = new List<ToryMultiInput<T>>();
			MultiInputs = new ToryMultiInput<T>[0];

			// Add an time event handler.
			ToryTime.Instance.InteractionCheckTimerTimedOut += OnInteractionCheckTimerTimedOut;

			// Add an scene event handler.
			ToryScene.Instance.Ended += OnSceneEnded;

			// Load saved values.
			InteractionType.LoadSavedValue();
			Gain.LoadSavedValue();
			FilterType.LoadSavedValue();
			OEFFrequency.LoadSavedValue();
			EnsembleSize.LoadSavedValue();
			MinimumInteraction.LoadSavedValue();
			MaxMultiInputCount.LoadSavedValue();
		}

		/// <summary>
		/// Sets the <see cref="P:RawValue"/>.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="timeStamp">Time stamp.</param>
		public abstract void SetRawValue(T value, float timeStamp = -1f);

		protected abstract T ApplyFilter(T value, float timeStamp = -1f);


		// Multi-input

		/// <summary>
		/// Adds the a multi-input to the <see cref="P:MultiInputs"/> array.
		/// The <see cref="P:Id"/> is set automatically not to be duplicated.
		/// If the length of <see cref="P:MultiInputs"/> array reaches to the <see cref="P:MaxMultiInputCount"/>, 
		/// this method stops to work and returns <see cref="T:false"/>.
		/// </summary>
		/// <returns><c>true</c>, if a multi-input was added, <c>false</c> otherwise.</returns>
		public abstract bool AddMultiInput();

		/// <summary>
		/// Adds the a multi-input to the <see cref="P:MultiInputs"/> array.
		/// The <see cref="P:Id"/> is set manually.
		/// If the length of <see cref="P:MultiInputs"/> array reaches to the <see cref="P:MaxMultiInputCount"/>, 
		/// this method stops to work and returns <see cref="T:false"/>.
		/// </summary>
		/// <returns><c>true</c>, if a multi-input was added, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		public abstract bool AddMultiInput(int id);

		/// <summary>
		/// Removes the specified multi-input if it is in the <see cref="P:MultiInputs"/> array.
		/// </summary>
		/// <returns><c>true</c>, if the specified multi-input was removed, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		public abstract bool RemoveMultiInput(ToryMultiInput<T> input);

		/// <summary>
		/// Removes the multi-inputs of <see cref="P:Id"/>  if they are in the <see cref="P:MultiInputs"/> array.
		/// </summary>
		/// <returns><c>true</c>, if the multi-inputs were removed, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		public abstract bool RemoveMultiInput(int id);

		/// <summary>
		/// Removes the multi-input at <see cref="P:ith"/> index if it is in the <see cref="P:MultiInputs"/> array.
		/// </summary>
		/// <returns><c>true</c>, if the multi-input was removed, <c>false</c> otherwise.</returns>
		/// <param name="i">The index.</param>
		public abstract bool RemoveMultiInputAt(int i);


		// Interaction Gauge.

		protected IEnumerator DecreaseInteractionGauge(float duration)
		{
			float initialInteractionGauge = InteractionGauge;
			
			while (InteractionGauge > 0f)
			{
				InteractionGauge -= Time.unscaledDeltaTime / duration * initialInteractionGauge;
				yield return null;
			}
		}


		// Event Triggers.

		protected void TriggerValueReceivedEvent(T processedValue)
		{
			ValueReceived(processedValue);
		}

		protected void TriggerInteractedEvent()
		{
			IsPlayerLeft = false;
			Interacted();
		}

		protected void TriggerMultiInputAdded(ToryMultiInput<T> input)
		{
			MultiInputAdded(input);
			TriggerMultiInputInteractedEvent(input);
		}

		protected void TriggerMultiInputRemoved(ToryMultiInput<T> input)
		{
			MultiInputRemoved(input);
			TriggerMultiInputInteractedEvent(input);
		}

		protected void TriggerMultiInputValueReceivedEvent(ToryMultiInput<T> input, T processedValue)
		{
			MultiInputValueReceived(input, processedValue);
		}

		protected void TriggerMultiInputInteractedEvent(ToryMultiInput<T> input)
		{
			MultiInputInteracted(input);
			TriggerInteractedEvent();
		}

		#endregion
	}
}
