using System.Collections.Generic;
using UnityEngine;
using ToryFramework.Behaviour;

namespace ToryFramework.Input
{
	/// <summary>
	/// The concrete class of generic ToryMultiInput.
	/// </summary>
	public class ToryKeyCodeMultiInput : ToryMultiInput<KeyCode>
	{
		#region CONSTRUCTOR

		public ToryKeyCodeMultiInput() : base()
		{
			// Set a new id.
			if (latestId == int.MaxValue)
			{
				latestId = 0;
			}
			Id = latestId++;
		}

		public ToryKeyCodeMultiInput(int id) : base(id)
		{
		}

		#endregion



		#region FIELDS

		static int latestId = 0;

		// Interaction Determination

		float interactionGauge;
		Coroutine decreaseInteractionGaugeCrt;
		Coroutine resetValuesCrt;

		#endregion



		#region PROPERTIES

		// Values

		/// <summary>
		/// Gets or sets the raw value.
		/// You can set this value using <see cref="M:SetRawValue"/> method when receiving the data from <see cref="T:Gemstone"/>, <see cref="T:ReactZ"/>, etc. 
		/// </summary>
		/// <value>The raw value.</value>
		public override KeyCode RawValue 						{ get; protected set; }

		/// <summary>
		/// Gets or sets the processed value.
		/// This value is calculated by applying the <see cref="P:FilterType"/> and multiplying the <see cref="P:Gain"/> to the <see cref="P:RawValue"/>.
		/// </summary>
		/// <value>The processed value.</value>
		public override KeyCode ProcessedValue 					{ get; protected set; }


		// Interaction Determination

		/// <summary>
		/// Gets or sets the interaction gauge which responds to the input and remains 0 without interaction.
		/// If the <see cref="P:InteractionType"/> is set to <see cref="T:CONTINUOUS"/>, this value is calculated by the absolute value of the gradient of the <see cref="P:ProcessedValue"/> over time.
		/// If the <see cref="P:InteractionType"/> is set to <see cref="T:DISCONTINUOUS"/>, this value is set to 1 with interaction, and set to 0 without interaction.
		/// </summary>
		/// <value>The interaction gauge.</value>
		public override float InteractionGauge 					{ get { return interactionGauge; }
			protected set
			{
				interactionGauge = Mathf.Max(0f, value);
			}
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
		/// Sets the <see cref="P:RawValue"/>.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="timeStamp">Time stamp.</param>
		public override void SetRawValue(KeyCode value, float timeStamp = -1f)
		{
			// Set the InteractionGauge.
			switch (InputBehaviour.InteractionType.Value)
			{
				case InteractionType.CONTINUOUS:
					// Set the raw and processed values.
					RawValue = value;
					ProcessedValue = RawValue;

					// Trigger an event.
					TriggerValueReceivedEvent(this, ProcessedValue);

					// Set the InteractionGauge.
					InteractionGauge = 1f;

					// Trigger the interaction event.
					if (InteractionGauge >= InputBehaviour.MinimumInteraction.Value)
					{
						TriggerInteractedEvent(this);
					}

					// Decrease the InteractionGauge.
					if (decreaseInteractionGaugeCrt != null)
					{
						InputBehaviour.StopCoroutine(decreaseInteractionGaugeCrt);
					}
					decreaseInteractionGaugeCrt = InputBehaviour.StartCoroutine(DecreaseInteractionGauge(1f));

					break;

				case InteractionType.DISCONTINUOUS:
					// Reset the values coroutine.
					if (resetValuesCrt != null)
					{
						InputBehaviour.StopCoroutine(resetValuesCrt);
					}

					// Set the raw and processed values.
					RawValue = value;
					ProcessedValue = RawValue;

					// Trigger an event.
					TriggerValueReceivedEvent(this, ProcessedValue);

					// Set the interaction gauge to 1.
					InteractionGauge = 1f;

					// Trigger the interaction event.
					TriggerInteractedEvent(this);

					// Set the raw and processed values back to its initial value after 0.1 second.
					if (resetValuesCrt != null)
					{
						InputBehaviour.StopCoroutine(resetValuesCrt);
					}
					resetValuesCrt = InputBehaviour.StartCoroutine(ManUtils.ManCoroutine.WaitAndAction(0.1f, () => RawValue = ProcessedValue = KeyCode.None));

					// Decrease the InteractionGauge.
					if (decreaseInteractionGaugeCrt != null)
					{
						InputBehaviour.StopCoroutine(decreaseInteractionGaugeCrt);
					}
					decreaseInteractionGaugeCrt = InputBehaviour.StartCoroutine(DecreaseInteractionGauge(1f));

					break;
			}
		}

		protected override KeyCode ApplyFilter(KeyCode value, float timeStamp = -1f)
		{
			// Do nth.
			return value;
		}

		#endregion
	}
}