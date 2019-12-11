using UnityEngine;
using ToryFramework.Scene;
using ManUtils;

namespace ToryFramework.SMB
{
	public abstract class ToryCustomSceneSMB : StateMachineBehaviour
	{
		#region FIELDS

		[Header("Connection Info.")]

		[Tooltip("The next tory scene state. " +
		         "If set to Exit, a new tory scene will be loaded.")]
		[SerializeField, ManReadOnly] protected State next;


		[Header("Forced Stay")]

		[Tooltip("The forced stay time since this tory scene state has started. " +
		         "If set to 0, this value does not work.")]
		[SerializeField, ManReadOnlyOnPlaying] protected float forcedStayTimeSinceStarted = 0.5f;


		[Header("Interaction")]

		[Tooltip("The maximum time that ckecks whether or not there is an interaction in this tory scene state. " +
		         "The PlayerLeft event is triggered after this time without any interaction. " +
		         "If set to 0, this value does not work.")]
		[SerializeField, ManReadOnlyOnPlaying] protected float interactionCheckTime = 30f;

		[Tooltip("The value indicating whether the tory scene can be loaded automatically on player left. " +
		         "If set to true, the tory scene is loaded automatically when PlayerLeft event is triggered. " +
		         "If set to false, only the event is triggered.")]
		[SerializeField, ManReadOnlyOnPlaying] protected bool autoLoadOnPlayerLeft = false;


		[Header("Transition")]

		[Tooltip("The transition time that this tory scene state proceeds to the next tory scene state. " +
		         "If set to 0, thie value does not work.")]
		[SerializeField, ManReadOnlyOnPlaying] protected float transitionTime = 0f;

		[Tooltip("The value indicating whether this tory scene state proceeds to the next tory scene state automatically on transition." +
		         "If set to true, the tory scene proceeds automatically." +
		         "If set to false, only the event is triggered.")]
		[SerializeField, ManReadOnlyOnPlaying] protected bool autoProceedOnTransition = false;


		[Header("Multi-Step")]

		[Tooltip("Use this value freely." +
		         "Cannot be smaller than 1.")]
		[SerializeField, ManReadOnlyOnPlaying] protected int stepCount = 1;

		#endregion



		#region PROPERTIES

		// State info.

		public abstract string Name 	{ get; }


		// Connection info.

		public State Next 				{ get { return next; } set { next = value; }}


		// Times

		public float ForcedStayTimeSinceStarted 	{ get { return forcedStayTimeSinceStarted; }
			set
			{
				forcedStayTimeSinceStarted = Mathf.Max(0f, value);
			}
		}

		public float InteractionCheckTime 	 { get { return interactionCheckTime; }
			set
			{
				interactionCheckTime = Mathf.Max(0f, value);
			}
		}

		public bool CanAutoLoadOnPlayerLeft 	{ get { return autoLoadOnPlayerLeft; } set { autoLoadOnPlayerLeft = value; }}

		public float TransitionTime 	 { get { return transitionTime; }
			set
			{
				transitionTime = Mathf.Max(0f, value);
			}
		}

		public bool CanAutoProceedOnTransition 	{ get { return autoProceedOnTransition; } set { autoProceedOnTransition = value; }}


		// Steps

		public int StepCount			{ get { return stepCount; }
			set
			{
				stepCount = Mathf.Max(1, value);
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

		#endregion
	}
}

