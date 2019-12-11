using System.Reflection;
using UnityEngine;
using ToryFramework.Input;
using ManUtils;
using ToryValue;

namespace ToryFramework.Behaviour
{
	[ManScriptExecutionOrder(-9998)]
	public class ToryInputBehaviour : MonoBehaviour
	{
		#region FIELDS

		// Singleton

		static ToryInputBehaviour instance;


		[Header("Data Properties")]

		// Warning disable: https://answers.unity.com/questions/21796/disable-warning-messages.html
		#pragma warning disable 0414 // private field assigned but not used.
		[Tooltip("The type of the data. " +
		         "Choose one according to the type of input signal. " +
		         "The type of ToryInput is modified in the specified type here, " +
		         "and then you can access ToryInput with this type from other script." +
		         "\nNOTE: You can only change this type here in the editor before starting the game.")]
		[SerializeField, ManReadOnlyOnPlaying] DataType dataType = DataType.Float;
		

		[Header("Filters")]

		[Tooltip("The type of the filter.")]
		[SerializeField] ToryFilterTypeEnum filterType = new ToryFilterTypeEnum("Filter Type",
		                                                                        Input.FilterType.ONE_EURO_FILTER,
		                                                                        Input.FilterType.ONE_EURO_FILTER, 
		                                                                        Input.FilterType.ONE_EURO_FILTER);
		
		[Tooltip("The frequency of 1€ filter. " +
		         "The value cannot be smaller than 0.")]
		[SerializeField] ToryFloat oefFrequency = new ToryFloat("OEF Frequency", 120f, 120f, 120f);

		[Tooltip("The window size of the ensemble average filter. " +
		         "The value cannot be smaller than 1.")]
		[SerializeField] ToryInt ensembleSize = new ToryInt("Ensemble Size", 10, 10, 10);


		[Header("Amplifier")]

		[Tooltip("The value to amplify the raw value.")]
		[SerializeField] ToryFloat gain = new ToryFloat("Gain", 1f, 1f, 1f);


		[Header("Interaction Test")]

		[Tooltip("The type of the interaction over time. " +
		         "If you receive data every frame or regular interval, choose the CONTINUOUS type. " +
		         "If not, choose the DISCONTINUOUS type.")]
		[SerializeField] ToryInteractionTypeEnum interactionType = new ToryInteractionTypeEnum("Interaction Type",
		                                                                                       Input.InteractionType.CONTINUOUS,
		                                                                                       Input.InteractionType.CONTINUOUS,
		                                                                                       Input.InteractionType.CONTINUOUS);

		[Tooltip("The value determining if there is interaction, or not. " +
		         "If the InteractionGauge is less than this value, it is considered that there is no interaction. " +
		         "The value cannot be smaller than 0.")]
		[SerializeField] ToryFloat minimumInteraction = new ToryFloat("Minimum Interaction", 0.1f, 0.1f, 0.1f);


		[Header("Multi-Input")]

		[Tooltip("The maximum number of the multi-input. " +
		         "If set to 0, the multi-input is not used.")]
		[SerializeField] ToryInt maxMultiInputCount;

		#endregion



		#region PROPERTIES

		// Singleton

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static ToryInputBehaviour Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<ToryInputBehaviour>();
				}
				return instance;
			}
		}

		// Data Properties

		/// <summary>
		/// Gets the type of the data (Read Only).
		/// You can only change this value in the inspector of ToryInput gameObject before starting the game.
		/// </summary>
		/// <value>The type of the data.</value>
		public DataType DataType 						{ get { return dataType; }}

		/// <summary>
		/// Gets or sets the type of the contiuity of data over time.
		/// </summary>
		/// <value>The type of the contituity.</value>
		public ToryInteractionTypeEnum InteractionType 	{ get { return interactionType; } set { interactionType = value; }}


		// Filters

		/// <summary>
		/// Gets or sets the type of the filter.
		/// </summary>
		/// <value>The type of the filter.</value>
		public ToryFilterTypeEnum FilterType 			{ get { return filterType; } set { filterType = value; }}

		/// <summary>
		/// Gets or sets the frequency of 1€ filter.
		/// The value cannot be smaller than 0.
		/// </summary>
		/// <value>The requency of 1€ filter.</value>
		public ToryFloat OEFFrequency 					{ get { return oefFrequency; } set { oefFrequency = value; }}

		/// <summary>
		/// Gets or sets the window size of the ensemble average filter.
		/// The value cannot be smaller than 1
		/// </summary>
		/// <value>The window size of the ensemble average filter.</value>
		public ToryInt EnsembleSize						{ get { return ensembleSize; } set { ensembleSize = value; }}


		// Interaction Determination

		/// <summary>
		/// Gets of sets the value determining if there is interaction, or not.
		/// If the <see cref="P:InteractionGauge"/> is less than this value, it is considered that there is no interaction.
		/// The value cannot be smaller than 0.
		/// </summary>
		/// <value>The minimum interaction.</value>
		public ToryFloat MinimumInteraction				{ get { return minimumInteraction; } set { minimumInteraction = value; }}


		// Amplifier

		/// <summary>
		/// Gets or sets the value to amplify the raw value.
		/// </summary>
		/// <value>The gain.</value>
		public ToryFloat Gain 							{ get { return gain; } set { gain = value; }}


		// Multi-Input

		/// <summary>
		/// Gets or sets the maximum number of multi-inputs.
		/// If set to 0, the multi-input is not used.
		/// </summary>
		/// <value>The maximum multi input count.</value>
		public ToryInt MaxMultiInputCount 				{ get { return maxMultiInputCount; } set { maxMultiInputCount = value; }}

		#endregion



		#region EVENTS

		#endregion



		#region UNITY_FRAMEWORK

		void Awake()
		{
			// Singleton
			if (Instance != this)
			{
				Destroy(this);
				Debug.LogError("The duplicated singleton instance of " + name + " removed.");
			}

			// Init ToryInput.
			System.Type type = ToryInput.Instance.GetType();
			MethodInfo method = type.GetMethod("Init", (BindingFlags.NonPublic |
														BindingFlags.Public |
														BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryInput.Instance, null);
			}
		}

		void OnEnable()
		{
			ToryInput.Instance.OEFFrequency.ValueChanged += OEFFrequency_ValueChanged;
			ToryInput.Instance.OEFFrequency.DefaultValueChanged += OEFFrequency_DefaultValueChanged;;
			ToryInput.Instance.OEFFrequency.SavedValueChanged += OEFFrequency_SavedValueChanged;

			ToryInput.Instance.EnsembleSize.ValueChanged += EnsembleSize_ValueChanged;
			ToryInput.Instance.EnsembleSize.DefaultValueChanged += EnsembleSize_DefaultValueChanged;;
			ToryInput.Instance.EnsembleSize.SavedValueChanged += EnsembleSize_SavedValueChanged;

			ToryInput.Instance.MinimumInteraction.ValueChanged += MinimumInteraction_ValueChanged;
			ToryInput.Instance.MinimumInteraction.DefaultValueChanged += MinimumInteraction_DefaultValueChanged;;
			ToryInput.Instance.MinimumInteraction.SavedValueChanged += MinimumInteraction_SavedValueChanged;

			ToryInput.Instance.MaxMultiInputCount.ValueChanged += MaxMultiInputCount_ValueChanged;
			ToryInput.Instance.MaxMultiInputCount.DefaultValueChanged += MaxMultiInputCount_DefaultValueChanged;;
			ToryInput.Instance.MaxMultiInputCount.SavedValueChanged += MaxMultiInputCount_SavedValueChanged;
		}

		void OnDisable()
		{
			ToryInput.Instance.OEFFrequency.ValueChanged -= OEFFrequency_ValueChanged;
			ToryInput.Instance.OEFFrequency.DefaultValueChanged -= OEFFrequency_DefaultValueChanged;;
			ToryInput.Instance.OEFFrequency.SavedValueChanged -= OEFFrequency_SavedValueChanged;

			ToryInput.Instance.EnsembleSize.ValueChanged -= EnsembleSize_ValueChanged;
			ToryInput.Instance.EnsembleSize.DefaultValueChanged -= EnsembleSize_DefaultValueChanged;;
			ToryInput.Instance.EnsembleSize.SavedValueChanged -= EnsembleSize_SavedValueChanged;

			ToryInput.Instance.MinimumInteraction.ValueChanged -= MinimumInteraction_ValueChanged;
			ToryInput.Instance.MinimumInteraction.DefaultValueChanged -= MinimumInteraction_DefaultValueChanged;;
			ToryInput.Instance.MinimumInteraction.SavedValueChanged -= MinimumInteraction_SavedValueChanged;

			ToryInput.Instance.MaxMultiInputCount.ValueChanged -= MaxMultiInputCount_ValueChanged;
			ToryInput.Instance.MaxMultiInputCount.DefaultValueChanged -= MaxMultiInputCount_DefaultValueChanged;;
			ToryInput.Instance.MaxMultiInputCount.SavedValueChanged -= MaxMultiInputCount_SavedValueChanged;
		}

		void OnDestroy()
		{
			ResetEvents();
		}

		#endregion



		#region CUSTOM_FRAMEWORK

		// OEF Frequency

		void OEFFrequency_ValueChanged(float value)
		{
			if (value < 0f)
			{
				OEFFrequency.Value = Mathf.Max(0f, value);
			}
		}

		void OEFFrequency_DefaultValueChanged(float value)
		{
			if (value < 0f)
			{
				OEFFrequency.DefaultValue = Mathf.Max(0f, value);
			}
		}

		void OEFFrequency_SavedValueChanged(float value)
		{
			if (value < 0f)
			{
				OEFFrequency.SavedValue = Mathf.Max(0f, value);
			}
		}

		// Ensemble Size

		void EnsembleSize_ValueChanged(int value)
		{
			if (value < 1)
			{
				EnsembleSize.Value = Mathf.Max(1, value);
			}
		}

		void EnsembleSize_DefaultValueChanged(int value)
		{
			if (value < 1)
			{
				EnsembleSize.DefaultValue = Mathf.Max(1, value);
			}
		}

		void EnsembleSize_SavedValueChanged(int value)
		{
			if (value < 1)
			{
				EnsembleSize.SavedValue = Mathf.Max(1, value);
			}
		}

		// Minimum Interaction

		void MinimumInteraction_ValueChanged(float value)
		{
			if (value < 0f)
			{
				MinimumInteraction.Value = Mathf.Max(0f, value);
			}
		}

		void MinimumInteraction_DefaultValueChanged(float value)
		{
			if (value < 0f)
			{
				MinimumInteraction.DefaultValue = Mathf.Max(0f, value);
			}
		}

		void MinimumInteraction_SavedValueChanged(float value)
		{
			if (value < 0f)
			{
				MinimumInteraction.SavedValue = Mathf.Max(0f, value);
			}
		}

		// Max Multi Input Count

		void MaxMultiInputCount_ValueChanged(int value)
		{
			if (value < 0)
			{
				MaxMultiInputCount.Value = Mathf.Max(0, value);
			}
		}

		void MaxMultiInputCount_DefaultValueChanged(int value)
		{
			if (value < 0)
			{
				MaxMultiInputCount.DefaultValue = Mathf.Max(0, value);
			}
		}

		void MaxMultiInputCount_SavedValueChanged(int value)
		{
			if (value < 0)
			{
				MaxMultiInputCount.SavedValue = Mathf.Max(0, value);
			}
		}

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		void ResetEvents()
		{
			// Input
			System.Type type = ToryInput.Instance.GetType();
			MethodInfo method = type.GetMethod("ResetEvents", (BindingFlags.NonPublic | 
			                                                   BindingFlags.Public | 
			                                                   BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryInput.Instance, null);
			}
		}

		#endregion
	}
}
