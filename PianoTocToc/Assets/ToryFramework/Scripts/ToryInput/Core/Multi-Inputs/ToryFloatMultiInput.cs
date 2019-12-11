using System.Collections.Generic;
using UnityEngine;

namespace ToryFramework.Input
{
	/// <summary>
	/// The concrete class of generic ToryMultiInput.
	/// </summary>
	public class ToryFloatMultiInput : ToryMultiInput<float>
	{
		#region CONSTRUCTOR

		public ToryFloatMultiInput() : base()
		{
			// Set a new id.
			if (latestId == int.MaxValue)
			{
				latestId = 0;
			}
			Id = latestId++;

			// Filters
			ensemble = new Queue<float>();
			oef = new OneEuroFilter(InputBehaviour.OEFFrequency.Value);
			prevProcessedValue = ProcessedValue = RawValue = 0f;
			prevTime = curTime = Time.unscaledTime;

			// Events
			ToryInput.Instance.OEFFrequency.ValueChanged += OEFFrequency_ValueChanged;
		}

		public ToryFloatMultiInput(int id) : base(id)
		{
			// Filters
			ensemble = new Queue<float>();
			oef = new OneEuroFilter(InputBehaviour.OEFFrequency.Value);
			prevProcessedValue = ProcessedValue = RawValue = 0f;
			prevTime = curTime = Time.unscaledTime;

			// Events
			ToryInput.Instance.OEFFrequency.ValueChanged += OEFFrequency_ValueChanged;
		}

		#endregion



		#region FIELDS

		static int latestId = 0;

		// Filters

		OneEuroFilter oef;
		Queue<float> ensemble;
		float prevProcessedValue;
		float prevTime, curTime;

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
		public override float RawValue 							{ get; protected set; }

		/// <summary>
		/// Gets or sets the processed value.
		/// This value is calculated by applying the <see cref="P:FilterType"/> and multiplying the <see cref="P:Gain"/> to the <see cref="P:RawValue"/>.
		/// </summary>
		/// <value>The processed value.</value>
		public override float ProcessedValue 					{ get; protected set; }


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

		void OEFFrequency_ValueChanged(float value)
		{
			if (value > 0f)
			{
				oef.UpdateParams(value, 1f, 0f, 1f);
			}
		}

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		/// <summary>
		/// Sets the <see cref="P:RawValue"/>.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="timeStamp">Time stamp.</param>
		public override void SetRawValue(float value, float timeStamp = -1f)
		{
			// Set the InteractionGauge.
			switch (InputBehaviour.InteractionType.Value)
			{
				case InteractionType.CONTINUOUS:
					// Update current variables.
					curTime = Time.unscaledTime;

					// Set the raw and processed values.
					RawValue = value;
					ProcessedValue = ApplyFilter(value, timeStamp) * InputBehaviour.Gain.Value;

					// Trigger an event.
					TriggerValueReceivedEvent(this, ProcessedValue);

					// Calc. the change of the processed value over time.
					float d = Mathf.Abs((ProcessedValue - prevProcessedValue) / (curTime - prevTime));

					// Set the InteractionGauge.
					InteractionGauge = d;

					// Trigger the interaction event.
					if (InteractionGauge >= InputBehaviour.MinimumInteraction.Value)
					{
						TriggerInteractedEvent(this);
					}

					// Update previous variables.
					prevProcessedValue = ProcessedValue;
					prevTime = curTime;

					break;

				case InteractionType.DISCONTINUOUS:
					// Reset the values coroutine.
					if (resetValuesCrt != null)
					{
						InputBehaviour.StopCoroutine(resetValuesCrt);
					}

					// Set the raw and processed values.
					RawValue = value;
					ProcessedValue = ApplyFilter(value, timeStamp) * InputBehaviour.Gain.Value;

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
					resetValuesCrt = InputBehaviour.StartCoroutine(ManUtils.ManCoroutine.WaitAndAction(0.1f, () => RawValue = prevProcessedValue = ProcessedValue = 0f));

					// Decrease the InteractionGauge.
					if (decreaseInteractionGaugeCrt != null)
					{
						InputBehaviour.StopCoroutine(decreaseInteractionGaugeCrt);
					}
					decreaseInteractionGaugeCrt = InputBehaviour.StartCoroutine(DecreaseInteractionGauge(1f));

					break;
			}
		}

		protected override float ApplyFilter(float value, float timeStamp = -1f)
		{
			float oefResult = 0f, eaResult = 0f;

			// Ensemble average - Enqueue or dequeue the recent value to the ensemble array.
			while (ensemble.Count >= InputBehaviour.EnsembleSize.Value)
			{
				ensemble.Dequeue();
			}
			ensemble.Enqueue(value);

			// Calc. the ensemble average.
			Queue<float>.Enumerator e = ensemble.GetEnumerator();
			while (e.MoveNext())
			{
				eaResult += e.Current;
			}
			eaResult /= ensemble.Count;

			// Calc. One Euro filter.
			oefResult = oef.Filter(value, timeStamp);

			// Return the result.
			float result = value;
			switch (InputBehaviour.FilterType.Value)
			{
				case FilterType.NONE:
					break;

				case FilterType.ONE_EURO_FILTER:
					result = oefResult;
					break;

				case FilterType.ENSEMBLE_AVERAGE:
					result = eaResult;
					break;
			}
			return result;
		}

		#endregion
	}
}