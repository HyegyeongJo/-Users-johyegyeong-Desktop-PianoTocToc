using System.Collections;
using UnityEngine;
using ToryFramework.Behaviour;

namespace ToryFramework
{
	public class ToryTime
	{
		public delegate void TimerEventHandler();

		#region SINGLETON

		static volatile ToryTime instance;
		static readonly object syncRoot = new object();

		ToryTime() { }

		public static ToryTime Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ToryTime();
						}
					}
				}
				return instance;
			}
		}

		#endregion


		#region FIELDS

		Coroutine forcedStayTimerCrt, interactionCheckTimerCrt, transitionTimerCrt;

		float forcedStayTime, interactionCheckTime, transitionTime;

		bool forcedStayTimerTimedUp;

		#endregion



		#region PROPERTIES

		// Behaviours

		protected ToryFrameworkBehaviour FrameworkBehaviour 		{ get { return ToryFrameworkBehaviour.Instance; }}


		// Timer

		/// <summary>
		/// Gets the forced stay timer of the current tory scene state (Read Only).
		/// </summary>
		/// <value>The forced stay timer.</value>
		public float ForcedStayTimer 			{ get; private set; }

		/// <summary>
		/// Gets the interaction check timer of the current tory scene state (Read Only).
		/// </summary>
		/// <value>The interaction check timer.</value>
		public float InteractionCheckTimer 		{ get; private set; }

		/// <summary>
		/// Gets the transition timer of the current tory scene state (Read Only).
		/// </summary>
		/// <value>The transition timer.</value>
		public float TransitionTimer 			{ get; private set; }

		#endregion



		#region EVENTS

		/// <summary>
		/// Occurs when forced stay timer timed out.
		/// </summary>
		public event TimerEventHandler ForcedStayTimerTimedOut = () => {};

		/// <summary>
		/// Occurs when interaction check timer timed out.
		/// </summary>
		public event TimerEventHandler InteractionCheckTimerTimedOut = () => {};

		/// <summary>
		/// Occurs when transition timer timed out.
		/// </summary>
		public event TimerEventHandler TransitionTimerTimedOut = () => {};

		#endregion



		#region TORY_FRAMEWORK

		void OnInteracted()
		{
			InteractionCheckTimer = interactionCheckTime;
		}

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		void ResetEvents()
		{
			ForcedStayTimerTimedOut = () => {};
			InteractionCheckTimerTimedOut = () => {};
			TransitionTimerTimedOut = () => {};
		}

		void Init()
		{
			// Input events
			ToryInput.Instance.Interacted += OnInteracted;
		}


		// Forced stay time.

		void StartForcedStayTimer()
		{
			if (forcedStayTimerCrt != null) 
			{
				FrameworkBehaviour.StopCoroutine(forcedStayTimerCrt);
			}
			forcedStayTimerCrt = FrameworkBehaviour.StartCoroutine(UpdateForcedStayTimer());
		}

		void StopForcedStayTimer()
		{
			if (forcedStayTimerCrt != null)
			{
				FrameworkBehaviour.StopCoroutine(forcedStayTimerCrt);
			}
		}

		IEnumerator UpdateForcedStayTimer()
		{
			if (forcedStayTime > 0f)
			{
				while (ForcedStayTimer > 0f)
				{
					ForcedStayTimer -= Time.deltaTime;
					ForcedStayTimer = Mathf.Max(0f, ForcedStayTimer);

					yield return null;
				}

				forcedStayTimerTimedUp = true;
				if (FrameworkBehaviour.CanShowLog)
				{
					Debug.Log("[ToryTime] ForcedStayTimer times up.");
				}
				ForcedStayTimerTimedOut();
			}
		}

		void ResetForcedStayTimer()
		{
			ForcedStayTimer = forcedStayTime = ToryScene.Instance.Current.ForcedStayTimeSinceStarted;
			if (forcedStayTime > 0f)
			{
				forcedStayTimerTimedUp = false;
			}
			else
			{
				forcedStayTimerTimedUp = true;
			}
		}


		// Interaction check time.

		void StartInteractionCheckTimer()
		{
			if (interactionCheckTimerCrt != null)
			{
				FrameworkBehaviour.StopCoroutine(interactionCheckTimerCrt);
			}
			interactionCheckTimerCrt = FrameworkBehaviour.StartCoroutine(UpdateInteractionCheckTimer());
		}

		void StopInteractionCheckTimer()
		{
			if (interactionCheckTimerCrt != null)
			{
				FrameworkBehaviour.StopCoroutine(interactionCheckTimerCrt);
			}
		}

		IEnumerator UpdateInteractionCheckTimer()
		{
			if (interactionCheckTime > 0f)
			{
				while (InteractionCheckTimer > 0f || !forcedStayTimerTimedUp)
				{
					InteractionCheckTimer -= Time.deltaTime;
					InteractionCheckTimer = Mathf.Max(0f, InteractionCheckTimer);

					yield return null;
				}

				if (FrameworkBehaviour.CanShowLog) 
				{
					Debug.Log("[ToryTime] InteractionCheckTimer times up.");
				}
				InteractionCheckTimerTimedOut();
			}
		}

		void ResetInteractionCheckTimer()
		{
			InteractionCheckTimer = interactionCheckTime = ToryScene.Instance.Current.InteractionCheckTime;
		}


		// Transition time.

		void StartTransitionTimer()
		{
			if (transitionTimerCrt != null)
			{
				FrameworkBehaviour.StopCoroutine(transitionTimerCrt);
			}
			transitionTimerCrt = FrameworkBehaviour.StartCoroutine(UpdateTransitionTimer());
		}

		void StopTransitionTimer()
		{
			if (transitionTimerCrt != null)
			{
				FrameworkBehaviour.StopCoroutine(transitionTimerCrt);
			}
		}

		IEnumerator UpdateTransitionTimer()
		{
			if (transitionTime > 0f)
			{
				while (TransitionTimer > 0f || !forcedStayTimerTimedUp)
				{
					TransitionTimer -= Time.deltaTime;
					TransitionTimer = Mathf.Max(0f, TransitionTimer);

					yield return null;
				}

				if (FrameworkBehaviour.CanShowLog) 
				{
					Debug.Log("[ToryTime] TransitionTimer times up.");
				}
				TransitionTimerTimedOut();
			}
		}

		void ResetTransitionTimer()
		{
			TransitionTimer = transitionTime = ToryScene.Instance.Current.TransitionTime;
		}

		#endregion
	}
}