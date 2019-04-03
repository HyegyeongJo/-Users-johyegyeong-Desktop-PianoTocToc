using System.Collections;
using UnityEngine;
using ToryFramework.Behaviour;

namespace ToryFramework.Input
{
	/// <summary>
	/// The base abstract class of ToryMultiInput.
	/// </summary>
	public abstract class ToryMultiInput<T> where T : struct
	{
		#region CONSTRUCTOR

		protected ToryMultiInput()
		{
		}

		protected ToryMultiInput(int id) : this()
		{
			// Set id.
			Id = id;
		}

		#endregion



		#region FIELDS

		#endregion



		#region PROPERTIES

		// Behaviour

		protected ToryFrameworkBehaviour FrameworkBehaviour		{ get { return ToryFrameworkBehaviour.Instance; }}

		protected ToryInputBehaviour InputBehaviour				{ get { return ToryInputBehaviour.Instance; }}


		// Id

		/// <summary>
		/// Gets the identifier of multi-inputs.
		/// This value is set automatically or manaully when you create an instance.
		/// If set automatically, uniqueness of this value is guaranteed.
		/// </summary>
		/// <value>The identifier.</value>
		public int Id 											{ get; protected set; }


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


		// Interaction Determination

		/// <summary>
		/// Gets or sets the interaction gauge which responds to the input and remains 0 without interaction.
		/// If the <see cref="P:InteractionType"/> is set to <see cref="T:CONTINUOUS"/>, this value is calculated by the absolute value of the change of the <see cref="P:ProcessedValue"/> over time.
		/// If the <see cref="P:InteractionType"/> is set to <see cref="T:DISCONTINUOUS"/>, this value is set to 1 with interaction, and set to 0 without interaction.
		/// </summary>
		/// <value>The interaction gauge.</value>
		public abstract float InteractionGauge 					{ get; protected set; }

		#endregion



		#region EVENTS

		// Delegates

		public delegate void ValueReceivedEventHandler(ToryMultiInput<T> input, T processedValue);

		public delegate void InteractionEventHandler(ToryMultiInput<T> input);


		// Value

		/// <summary>
		/// Occurs when value received.
		/// </summary>
		public event ValueReceivedEventHandler ValueReceived = (ToryMultiInput<T> input, T processedValue) => {};


		// Interaction Determination

		/// <summary>
		/// Occurs when a player interacts with the game.
		/// </summary>
		public event InteractionEventHandler Interacted = (ToryMultiInput<T> input) => {};


		#endregion



		#region EVENT_HANDLERS

		protected void ResetEvents()
		{
			// Value
			ValueReceived = (ToryMultiInput<T> input, T processedValue) => {};

			// Interaction Determination
			Interacted = (ToryMultiInput<T> input) => {};
		}

		/// <summary>
		/// Sets the <see cref="P:RawValue"/>.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="timeStamp">Time stamp.</param>
		public abstract void SetRawValue(T value, float timeStamp = -1f);

		protected abstract T ApplyFilter(T value, float timeStamp = -1f);


		// Event Triggers.

		protected void TriggerValueReceivedEvent(ToryMultiInput<T> input, T processedValue)
		{
			ValueReceived(input, processedValue);
		}

		protected void TriggerInteractedEvent(ToryMultiInput<T> input)
		{
			Interacted(input);
		}

		protected IEnumerator DecreaseInteractionGauge(float duration)
		{
			float initialInteractionGauge = InteractionGauge;

			while (InteractionGauge > 0f)
			{
				InteractionGauge -= Time.unscaledDeltaTime / duration * initialInteractionGauge;
				yield return null;
			}
		}

		#endregion



		#region METHODS

		#endregion
	}	
}