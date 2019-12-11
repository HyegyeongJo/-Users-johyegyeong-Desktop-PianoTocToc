using System.Collections.Generic;
using UnityEngine;
using ToryValue;

namespace ToryFramework.Input
{
	/// <summary>
	/// The concrete class of generic ToryInput.
	/// </summary>
	public class ToryQuaternionInput : ToryInput<Quaternion>
	{
		#region CONSTRUCTOR

		protected ToryQuaternionInput() { }

		#endregion



		#region FIELDS

		// Filters

		OneEuroFilter[] oefs;
		Queue<Quaternion> ensemble;
		Quaternion prevProcessedValue;
		float prevTime, curTime;


		// Interaction Determination

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
		public override Quaternion RawValue 					{ get; protected set; }

		/// <summary>
		/// Gets or sets the processed value.
		/// This value is calculated by applying the <see cref="P:FilterType"/> and multiplying the <see cref="P:Gain"/> to the <see cref="P:RawValue"/>.
		/// </summary>
		/// <value>The processed value.</value>
		public override Quaternion ProcessedValue 				{ get; protected set; }


		// Filters

		/// <summary>
		/// Gets or sets the frequency of 1€ filter.
		/// The value cannot be smaller than 0.
		/// </summary>
		/// <value>The frequency of 1€ filter.</value>
		public override ToryFloat OEFFrequency 					{ get { return base.OEFFrequency; } set { base.OEFFrequency = value; }}


		// Multi-input

		/// <summary>
		/// Gets the multi-input array.
		/// </summary>
		/// <value>The multi inputs.</value>
		public override ToryMultiInput<Quaternion>[] MultiInputs { get; protected set; }

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
				for (int i = 0; i < oefs.Length; i++)
				{
					oefs[i].UpdateParams(value, 1f, 0f, 1f);
				}
			}
		}

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		protected override void Init()
		{
			base.Init();

			// Filters
			ensemble = new Queue<Quaternion>();
			oefs = new OneEuroFilter[4];
			for (int i = 0; i < oefs.Length; i++)
			{
				oefs[i] = new OneEuroFilter(OEFFrequency.Value);
			}
			prevProcessedValue = ProcessedValue = RawValue = Quaternion.identity;
			prevTime = curTime = Time.unscaledTime;

			// Events
			ToryInput.Instance.OEFFrequency.ValueChanged += OEFFrequency_ValueChanged;
		}

		/// <summary>
		/// Sets the <see cref="P:RawValue"/>.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="timeStamp">Time stamp.</param>
		public override void SetRawValue(Quaternion value, float timeStamp = -1f)
		{
			// Set the InteractionGauge.
			switch (InteractionType.Value)
			{
				case Input.InteractionType.CONTINUOUS:
					// Update current variables.
					curTime = Time.unscaledTime;

					// Set the raw and processed values.
					RawValue = value;
					ProcessedValue = ApplyFilter(value, timeStamp);

					// Trigger an event.
					TriggerValueReceivedEvent(ProcessedValue);

					// Calc. the change of the processed value over time.
					float d = Quaternion.Angle(prevProcessedValue, ProcessedValue) * Gain.Value / (curTime - prevTime);

					// Set the InteractionGauge.
					InteractionGauge = d;

					// Trigger the interaction event.
					if (InteractionGauge >= MinimumInteraction.Value)
					{
						TriggerInteractedEvent();
					}

					// Update previous variables.
					prevProcessedValue = ProcessedValue;
					prevTime = curTime;

					break;

				case Input.InteractionType.DISCONTINUOUS:
					// Reset the values coroutine.
					if (resetValuesCrt != null)
					{
						InputBehaviour.StopCoroutine(resetValuesCrt);
					}

					// Set the raw and processed values.
					RawValue = value;
					ProcessedValue = ApplyFilter(value, timeStamp);

					// Trigger an event.
					TriggerValueReceivedEvent(ProcessedValue);

					// Set the interaction gauge to 1.
					InteractionGauge = 1f;

					// Trigger the interaction event.
					TriggerInteractedEvent();

					// Set the raw and processed values back to its initial value after 0.1 second.
					if (resetValuesCrt != null)
					{
						InputBehaviour.StopCoroutine(resetValuesCrt);
					}
					resetValuesCrt = InputBehaviour.StartCoroutine(ManUtils.ManCoroutine.WaitAndAction(0.1f, () => RawValue = prevProcessedValue = ProcessedValue = Quaternion.identity));

					// Decrease the InteractionGauge.
					if (decreaseInteractionGaugeCrt != null)
					{
						InputBehaviour.StopCoroutine(decreaseInteractionGaugeCrt);
					}
					decreaseInteractionGaugeCrt = InputBehaviour.StartCoroutine(DecreaseInteractionGauge(1f));

					break;
			}
		}

		protected override Quaternion ApplyFilter(Quaternion value, float timeStamp = -1f)
		{
			Quaternion oefResult = Quaternion.identity, eaResult = new Quaternion(0f, 0f, 0f, 0f);

			// Ensemble average - Enqueue or dequeue the recent value to the ensemble array.
			while (ensemble.Count >= EnsembleSize.Value)
			{
				ensemble.Dequeue();
			}
			ensemble.Enqueue(value);

			// Calc. the ensemble average.
			Queue<Quaternion>.Enumerator e = ensemble.GetEnumerator();
			while (e.MoveNext())
			{
				for (int i = 0; i < oefs.Length; i++)
				{
					eaResult[i] += e.Current[i];
				}
			}
			for (int i = 0; i < oefs.Length; i++)
			{
				eaResult[i] /= ensemble.Count;
			}

			// Calc. One Euro filter.
			for (int i = 0; i < oefs.Length; i++)
			{
				oefResult[i] = oefs[i].Filter(value[i], timeStamp);
			}

			// Return the result.
			Quaternion result = value;
			switch (FilterType.Value)
			{
				case Input.FilterType.NONE:
					break;

				case Input.FilterType.ONE_EURO_FILTER:
					result = oefResult;
					break;

				case Input.FilterType.ENSEMBLE_AVERAGE:
					result = eaResult;
					break;
			}
			return result;
		}


		// Multi-input

		/// <summary>
		/// Adds a multi-input to the <see cref="P:MultiInputs"/> array.
		/// The <see cref="P:Id"/> is set automatically not to be duplicated.
		/// If the length of <see cref="P:MultiInputs"/> array reaches to the <see cref="P:MaxMultiInputCount"/>, 
		/// this method stops to work and returns <see cref="T:false"/>.
		/// </summary>
		/// <returns><c>true</c>, if a multi-input was added, <c>false</c> otherwise.</returns>
		public override bool AddMultiInput()
		{
			// Check the length.
			if (MultiInputs.Length >= MaxMultiInputCount.Value)
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] The maximum number of multi-inputs (" + MaxMultiInputCount + ") reached.");
				}
				return false;
			}

			// Add a new multi-input.
			ToryMultiInput<Quaternion> mi = new ToryQuaternionMultiInput();
			multiInputList.Add(mi);
			MultiInputs = multiInputList.ToArray();
			mi.ValueReceived += TriggerMultiInputValueReceivedEvent;
			mi.Interacted += TriggerMultiInputInteractedEvent;

			if (FrameworkBehaviour.CanShowLog)
			{
				Debug.Log("[ToryInput] A new multi-input (ID: " + mi.Id + ") added.");
			}

			// Trigger an event.
			TriggerMultiInputAdded(mi);

			return true;
		}

		/// <summary>
		/// Adds a multi-input to the <see cref="P:MultiInputs"/> array.
		/// The <see cref="P:Id"/> is set manually.
		/// If the length of <see cref="P:MultiInputs"/> array reaches to the <see cref="P:MaxMultiInputCount"/>, 
		/// this method stops to work and returns <see cref="T:false"/>.
		/// </summary>
		/// <returns><c>true</c>, if a multi-input was added, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		public override bool AddMultiInput(int id)
		{
			// Check the length.
			if (MultiInputs.Length >= MaxMultiInputCount.Value)
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] The maximum number of multi-inputs (" + MaxMultiInputCount + ") reached.");
				}	
				return false;
			}

			//// Check if the id already exists.
			//ToryMultiInput<float> found = multiInputList.Find( (obj) => { return obj.Id.Equals(id); } );
			//if (found != null)
			//{
			//	if (FrameworkBehaviour.CanShowLog)
			//	{
			//		Debug.Log("[ToryInput] The id (" + id + ") passed by a parameter already exists in the MultiInputs array.");
			//	}
			//	return false;
			//}

			// Add a new multi-input.
			ToryMultiInput<Quaternion> mi = new ToryQuaternionMultiInput(id);
			multiInputList.Add(mi);
			MultiInputs = multiInputList.ToArray();
			mi.ValueReceived += TriggerMultiInputValueReceivedEvent;
			mi.Interacted += TriggerMultiInputInteractedEvent;

			if (FrameworkBehaviour.CanShowLog)
			{
				Debug.Log("[ToryInput] A new multi-input (ID: " + mi.Id + ") added.");
			}

			// Trigger an event.
			TriggerMultiInputAdded(mi);

			return true;
		}

		/// <summary>
		/// Removes the specified multi-input if it is in the <see cref="P:MultiInputs"/> array.
		/// </summary>
		/// <returns><c>true</c>, if the specified multi-input was removed, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		public override bool RemoveMultiInput(ToryMultiInput<Quaternion> input)
		{
			// Check the length.
			if (MultiInputs.Length == 0)
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] No multi-input left in the MultiInputs array.");
				}
				return false;
			}

			// Remove the multi-input.
			if (multiInputList.Remove(input))
			{
				MultiInputs = multiInputList.ToArray();
				input.ValueReceived -= TriggerMultiInputValueReceivedEvent;
				input.Interacted -= TriggerMultiInputInteractedEvent;

				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] The multi-input of ID " + input.Id + " removed.");
				}

				// Trigger an event.
				TriggerMultiInputRemoved(input);

				return true;
			}
			// If not exist in the list.
			else
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] No multi-input of ID " + input.Id + " in the MultiInputs array");
				}
				return false;
			}
		}

		/// <summary>
		/// Removes the multi-inputs of <see cref="P:Id"/>  if they are in the <see cref="P:MultiInputs"/> array.
		/// </summary>
		/// <returns><c>true</c>, if the multi-inputs were removed, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		public override bool RemoveMultiInput(int id)
		{
			// Check the length.
			if (MultiInputs.Length == 0)
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] No multi-input left in the MultiInputs array.");
				}
				return false;
			}

			// Remove the multi-input of the id.
			ToryMultiInput<Quaternion> mi = multiInputList.Find( (obj) => { return obj.Id.Equals(id); } );
			if (mi != null && multiInputList.Remove(mi))
			{
				MultiInputs = multiInputList.ToArray();
				mi.ValueReceived -= TriggerMultiInputValueReceivedEvent;
				mi.Interacted -= TriggerMultiInputInteractedEvent;

				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] The multi-input of ID " + mi.Id + " removed.");
				}

				// Trigger an event.
				TriggerMultiInputRemoved(mi);

				return true;
			}
			// If not exist int the list.
			else
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] No multi-input of ID " + mi.Id + " in the MultiInputs array");
				}
				return false;
			}
		}

		/// <summary>
		/// Removes the multi-input at <see cref="P:ith"/> index if it is in the <see cref="P:MultiInputs"/> array.
		/// </summary>
		/// <returns><c>true</c>, if the multi-input was removed, <c>false</c> otherwise.</returns>
		/// <param name="i">The index.</param>
		public override bool RemoveMultiInputAt(int i)
		{
			// Check the i.
			if (i < 0)
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] The parameter, i, cannot be lower than 0.");
				}
				return false;
			}
			else if (i >= MultiInputs.Length)
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] The parameter, i, cannot be greater than or equal to the length of the MultiInputs.");
				}
				return false;
			}

			// Check the length.
			if (MultiInputs.Length == 0)
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] No multi-input left in the MultiInputs array.");
				}
				return false;
			}

			// Remove the multi-input at i.
			ToryMultiInput<Quaternion> mi = multiInputList.Find( (obj) => { return obj.Id.Equals(MultiInputs[i].Id); } );
			if (mi != null && multiInputList.Remove(mi))
			{
				MultiInputs = multiInputList.ToArray();
				mi.ValueReceived -= TriggerMultiInputValueReceivedEvent;
				mi.Interacted -= TriggerMultiInputInteractedEvent;

				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] The multi-input of ID " + mi.Id + " removed.");
				}

				// Trigger an event.
				TriggerMultiInputRemoved(mi);

				return true;
			}
			else
			{
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryInput] No multi-input of ID " + mi.Id + " in the MultiInputs array");
				}
				return false;
			}
		}

		#endregion
	}
}