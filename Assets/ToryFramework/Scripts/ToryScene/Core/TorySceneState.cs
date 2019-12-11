using System.Collections;
using System.Reflection;
using UnityEngine;
using ToryFramework.Behaviour;
using ToryFramework.SMB;

namespace ToryFramework.Scene
{
	public abstract partial class TorySceneState : IToryScene
	{
		#region CONSTRUCTOR

		protected TorySceneState() { }

		#endregion



		#region FIELDS

		ToryCustomSceneSMB smb;

		Coroutine updateCrt, fixedUpdateCrt;

		bool forcedEnd;

		float startTime, unscaledStartTime;

		int stepIndex = 0;

		#endregion



		#region PROPERTIES

		// Behaviours

		protected ToryFrameworkBehaviour FrameworkBehaviour 		{ get { return ToryFrameworkBehaviour.Instance; }}

		protected TorySceneBehaviour SceneBehaviour 				{ get { return TorySceneBehaviour.Instance; }}


		// Connection

		/// <summary>
		/// Gets or sets the next tory scene state.
		/// If set to null, a new tory scene will be loaded.
		/// </summary>
		/// <value>The next.</value>
		public TorySceneState Next 		{ get; set; }


		// Time Properties

		/// <summary>
		/// Gets or sets the forced stay time since this tory scene state has started.
		/// If set to 0, this value does not work.
		/// </summary>
		/// <value>The forced stay time.</value>
		public float ForcedStayTimeSinceStarted 	{ get { return smb.ForcedStayTimeSinceStarted; } set { smb.ForcedStayTimeSinceStarted = value; }}

		/// <summary>
		/// Gets or sets the maximum time that ckecks whether or not there is an interaction in this tory scene state.
		/// The <see cref="E:PlayerLeft"/> event  is triggered after this time without any interaction.
		/// If set to 0, this value does not work.
		/// </summary>
		/// <value>The continuance time.</value>
		public float InteractionCheckTime 			{ get { return smb.InteractionCheckTime; } set { smb.InteractionCheckTime = value; }}

		/// <summary>
		/// Gets or sets a value indicating whether the tory scene can be loaded automatically on player left.
		/// </summary>
		/// <value><c>true</c> if the tory scene is loaded automatically when PlayerLeft event is triggered; otherwise, <c>false</c> if only the event is triggered.</value>
		public bool CanAutoLoadOnPlayerLeft 	{ get { return smb.CanAutoLoadOnPlayerLeft; } set { smb.CanAutoLoadOnPlayerLeft = value; }}

		/// <summary>
		/// Gets or sets the transition time that this tory scene state proceeds to the next tory scene state automatically.
		/// If set to 0, thie value does not work.
		/// </summary>
		/// <value>The transition time.</value>
		public float TransitionTime 				{ get { return smb.TransitionTime; } set { smb.TransitionTime = value; }}

		/// <summary>
		/// Gets or sets a value indicating whether this tory scene state proceeds to the next tory scene state automatically on transition.
		/// </summary>
		/// <value><c>true</c> if the tory scene proceeds automatically; otherwise, <c>false</c> of only the event is triggered.</value>
		public bool CanAutoProceedOnTransition 	{ get { return smb.CanAutoProceedOnTransition; } set { smb.CanAutoProceedOnTransition = value; }}

		/// <summary>
		/// Gets the time since this tory scene state started (Read Only).
		/// </summary>
		/// <value>The time since this tory scene state started.</value>
		public float TimeSinceStarted 				{ get; private set; }

		/// <summary>
		/// Gets the timeScale-independent time since this tory scene state started (Read Only).
		/// </summary>
		/// <value>The timeScale-independent time since this tory scene state started.</value>
		public float UnscaledTimeSinceStarted 		{ get; private set; }

		/// <summary>
		/// Gets a value indicating whether the forced stay timer is timed out.
		/// </summary>
		/// <value><c>true</c> if is forced stay timer timed out; otherwise, <c>false</c>.</value>
		public bool IsForcedStayTimerTimedOut		{ get; private set; }

		/// <summary>
		/// Gets a value indicating whether the interaction check timer is timed out.
		/// timed out.
		/// </summary>
		/// <value><c>true</c> if is interaction check timer timed out; otherwise, <c>false</c>.</value>
		public bool IsInteractionCheckTimerTimedOut	{ get; private set; }

		/// <summary>
		/// Gets a value indicating whether the transition timer is timed out.
		/// </summary>
		/// <value><c>true</c> if is transition timer timed out; otherwise, <c>false</c>.</value>
		public bool IsTransitionTimerTimedOut 		{ get; private set; }


		// Input

		/// <summary>
		/// Gets a value indicating whether a player left from the scene.
		/// </summary>
		/// <value><c>true</c> if is player left; otherwise, <c>false</c>.</value>
		public bool IsPlayerLeft 					{ get; private set; } 


		// Steps

		/// <summary>
		/// Gets or sets the step count.
		/// Cannot be smaller than 1.
		/// </summary>
		/// <value>The step count.</value>
		public int StepCount 						{ get { return smb.StepCount; } set { smb.StepCount = value; }}

		/// <summary>
		/// Gets or sets the index of the step.
		/// Cannot be smaller than 0 and greater than or equal to <see cref="P:StepCount"/>.
		/// </summary>
		/// <value>The index of the step.</value>
		public int StepIndex 						{ get { return stepIndex; }
			set
			{
				stepIndex = Mathf.Clamp(value, 0, StepCount - 1);
			}
		}


		// Play

		/// <summary>
		/// Gets a value indicating whether this tory scene is playing (Read Only).
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying 						{ get; private set; }


		#endregion



		#region EVENTS

		// Update

		/// <summary>
		/// Occurs when this tory scene state is started.
		/// </summary>
		public event UpdateEventHandler Started = () => {};

		/// <summary>
		/// Occurs every frame after <see cref="E:Started"/> if this tory scene state is playing. 
		/// </summary>
		public event UpdateEventHandler Updated = () => {};

		/// <summary>
		/// Occurs after <see cref="E:Updated"/> if this tory scene state is playing.
		/// </summary>
		public event UpdateEventHandler LateUpdated = () => {};

		/// <summary>
		/// Occurs every fixed frame after <see cref="E:Started"/> if this tory scene state is playing.
		/// </summary>
		public event UpdateEventHandler FixedUpdated = () => {};

		/// <summary>
		/// Occurs when this tory scene state is ended.
		/// </summary>
		public event UpdateEventHandler Ended = () => {};


		// Time

		/// <summary>
		/// Occurs when forced stay timer timed out.
		/// </summary>
		public event ToryTime.TimerEventHandler ForcedStayTimerTimedOut = () => {};

		/// <summary>
		/// Occurs when interaction check timer timed out.
		/// </summary>
		public event ToryTime.TimerEventHandler InteractionCheckTimerTimedOut = () => {};

		/// <summary>
		/// Occurs when transition timer timed out.
		/// </summary>
		public event ToryTime.TimerEventHandler TransitionTimerTimedOut = () => {};


		// Input

		/// <summary>
		/// Occurs when a player interacts with the game.
		/// </summary>
		public event ToryInput.InteractionEventHandler Interacted = () => {};

		/// <summary>
		/// Occurs when a player left.
		/// </summary>
		public event ToryInput.InteractionEventHandler PlayerLeft = () => {};

		#endregion



		#region EVENT_HANDLERS

		protected void OnForcedStayTimerTimedOut()
		{
			IsForcedStayTimerTimedOut = true;
			ForcedStayTimerTimedOut();
		}

		protected void OnInteractionCheckTimerTimedOut()
		{
			IsInteractionCheckTimerTimedOut = true;
			InteractionCheckTimerTimedOut();
			if (CanAutoLoadOnPlayerLeft)
			{
				LoadToryScene();
			}
		}

		protected void OnTransitionTimerTimedOut()
		{
			IsTransitionTimerTimedOut = true;
			TransitionTimerTimedOut();
			if (CanAutoProceedOnTransition)
			{
				Proceed();
			}
		}

		protected void OnSceneLoaded()
		{
			stepIndex = 1;
		}

		void OnSceneEnded()
		{
			
		}

		void OnPlayerLeft()
		{
			IsPlayerLeft = true;
			PlayerLeft();
		}

		#endregion



		#region METHODS

		protected void ResetEvents()
		{
			// Update
			Started = () => {};
			Updated = () => {};
			LateUpdated = () => {};
			FixedUpdated = () => {};
			Ended = () => {};

			// Time
			ForcedStayTimerTimedOut = () => {};
			InteractionCheckTimerTimedOut = () => {};
			TransitionTimerTimedOut = () => {};

			// Input
			Interacted = () => {};
			PlayerLeft = () => {};
		}

		protected void Init(ToryCustomSceneSMB smb)
		{
			if (smb == null)
			{
				this.smb = ScriptableObject.CreateInstance<ToryNullSceneSMB>();
			}
			else
			{
				this.smb = smb;
			}

			// Set the Next.
			SetNext(); // Declared in the partial class.

			// Tory events.
			ToryScene.Instance.Loaded += OnSceneLoaded;
		}

		protected void Start()
		{
			// Check if this scene is already started.
			if (IsPlaying)
			{
				return;
			}

			// Initialzie
			IsPlaying = true;
			forcedEnd = false;
			startTime = Time.time;
			unscaledStartTime = Time.unscaledTime;
			IsForcedStayTimerTimedOut = (ForcedStayTimeSinceStarted > 0f) ? false : true;
			IsInteractionCheckTimerTimedOut = false;
			IsTransitionTimerTimedOut = false;
			IsPlayerLeft = false;

			// Set the current tory scene.
			System.Type type = ToryScene.Instance.GetType();
			PropertyInfo prop = type.GetProperty("Current", (BindingFlags.NonPublic | 
			                                                 BindingFlags.Public | 
			                                                 BindingFlags.Instance));
			if (prop != null)
			{
				prop.SetValue(ToryScene.Instance, this, null);
			}

			// Time - reset and add listener to the ForcedStayTimerTimedOut event.
			type = ToryTime.Instance.GetType();
			MethodInfo method = type.GetMethod("ResetForcedStayTimer", (BindingFlags.NonPublic | 
			                                                            BindingFlags.Public | 
			                                                            BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryTime.Instance, null);
			}
			ToryTime.Instance.ForcedStayTimerTimedOut += OnForcedStayTimerTimedOut;

			// Time - reset and add listener to the InteractionCheckTimerTimedOut event.
			method = type.GetMethod("ResetInteractionCheckTimer", (BindingFlags.NonPublic | 
			                                                       BindingFlags.Public | 
			                                                       BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryTime.Instance, null);
			}
			ToryTime.Instance.InteractionCheckTimerTimedOut += OnInteractionCheckTimerTimedOut;

			// Time - reset and add listener to the TransitionTimerTimedOut event.
			method = type.GetMethod("ResetTransitionTimer", (BindingFlags.NonPublic | 
			                                                 BindingFlags.Public | 
			                                                 BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryTime.Instance, null);
			}
			ToryTime.Instance.TransitionTimerTimedOut += OnTransitionTimerTimedOut;

			// Input
			ToryInput.Instance.Interacted += Interacted;
			ToryInput.Instance.PlayerLeft += OnPlayerLeft;

			// Time - Start the timer methods.
			MethodInfo StartForcedStayTimer = type.GetMethod("StartForcedStayTimer", (BindingFlags.NonPublic | 
			                                                                          BindingFlags.Public | 
			                                                                          BindingFlags.Instance));
			MethodInfo InteractionCheckTimer = type.GetMethod("StartInteractionCheckTimer", (BindingFlags.NonPublic | 
			                                                                                 BindingFlags.Public | 
			                                                                                 BindingFlags.Instance));
			MethodInfo TransitionTimer = type.GetMethod("StartTransitionTimer", (BindingFlags.NonPublic | 
			                                                                     BindingFlags.Public | 
			                                                                     BindingFlags.Instance));
			if (StartForcedStayTimer != null)
			{
				StartForcedStayTimer.Invoke(ToryTime.Instance, null);
			}
			if (InteractionCheckTimer != null)
			{
				InteractionCheckTimer.Invoke(ToryTime.Instance, null);
			}
			if (TransitionTimer != null)
			{
				TransitionTimer.Invoke(ToryTime.Instance, null);
			}

			// Start
			if (FrameworkBehaviour.CanShowLog)
			{
				Debug.Log("[ToryScene] A new tory scene state (" + smb.Name + ") started.");
			}
			Started();

			// Start update coroutines.
			updateCrt = SceneBehaviour.StartCoroutine(Update());
			fixedUpdateCrt = SceneBehaviour.StartCoroutine(FixedUpdate());
		}

		protected IEnumerator Update()
		{
			while (true)
			{
				yield return null;

				// Update the time.
				TimeSinceStarted = Time.time - startTime;
				UnscaledTimeSinceStarted = Time.unscaledTime - unscaledStartTime;

				// Updates
				Updated();
				LateUpdated();
			}
		}

		protected IEnumerator FixedUpdate()
		{
			while (true)
			{
				yield return new WaitForFixedUpdate();

				// Pause
				//if (Time.timeScale.Equals(0f))
				//{
				//	yield return new WaitWhile(() => Time.timeScale.Equals(0f));
				//}

				// Fixed update
				FixedUpdated();
			}
		}


		protected IEnumerator End(IToryScene toScene = null, float delay = 0f, bool endNow = false)
		{
			// Check if this scene is under forced stay or not.
			if (!IsForcedStayTimerTimedOut)
			{
				if (FrameworkBehaviour.CanShowLog) 
				{
					Debug.Log("[ToryScene] The tory scene (" + smb.Name + ") lasts for " 
					          + (ToryTime.Instance.ForcedStayTimer).ToString("F1") 
					          + " seconds.");
				}
				yield break;
			}

			// Check if this scene is already ended.
			if (!IsPlaying)
			{
				yield break;
			}
			IsPlaying = false;

			// Delay.
			if (!endNow && delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}

			// End the updates
			if (updateCrt != null)
			{
				SceneBehaviour.StopCoroutine(updateCrt);
			}
			if (fixedUpdateCrt != null)
			{
				SceneBehaviour.StopCoroutine(fixedUpdateCrt);
			}

			// Time - end the timer methods.
			System.Type type = ToryTime.Instance.GetType();
			MethodInfo StopForcedStayTimer = type.GetMethod("StopForcedStayTimer", (BindingFlags.NonPublic | 
			                                                                        BindingFlags.Public | 
			                                                                        BindingFlags.Instance));
			MethodInfo StopInteractionCheckTimer = type.GetMethod("StopInteractionCheckTimer", (BindingFlags.NonPublic | 
			                                                                                    BindingFlags.Public | 
			                                                                                    BindingFlags.Instance));
			MethodInfo StopTransitionTimer = type.GetMethod("StopTransitionTimer", (BindingFlags.NonPublic | 
			                                                                        BindingFlags.Public | 
			                                                                        BindingFlags.Instance));
			if (StopForcedStayTimer != null)
			{
				StopForcedStayTimer.Invoke(ToryTime.Instance, null);
			}
			if (StopInteractionCheckTimer != null)
			{
				StopInteractionCheckTimer.Invoke(ToryTime.Instance, null);
			}
			if (StopTransitionTimer != null)
			{
				StopTransitionTimer.Invoke(ToryTime.Instance, null);
			}

			// Time - remove listeners.
			ToryTime.Instance.ForcedStayTimerTimedOut -= OnForcedStayTimerTimedOut;
			ToryTime.Instance.InteractionCheckTimerTimedOut -= OnInteractionCheckTimerTimedOut;
			ToryTime.Instance.TransitionTimerTimedOut -= OnTransitionTimerTimedOut;

			// Input
			ToryInput.Instance.Interacted -= Interacted;
			ToryInput.Instance.PlayerLeft -= OnPlayerLeft;

			// End
			if (FrameworkBehaviour.CanShowLog)
			{
				Debug.Log("[ToryScene] The current tory scene state (" + smb.Name + ") ended.");	
			}
			Ended();

			// Reset fields
			IsForcedStayTimerTimedOut = false;
			IsInteractionCheckTimerTimedOut = false;
			IsTransitionTimerTimedOut = false;
			IsPlayerLeft = false;

			// Delay.
			if (endNow && delay > 0f)
			{
				yield return new WaitForSeconds(delay);	
			}

			// If set to the forced end, end the ToryScene.
			if (forcedEnd)
			{
				// Go to Scene.End.
				type = ToryScene.Instance.GetType();
				MethodInfo method = type.GetMethod("End", (BindingFlags.NonPublic | 
				                                           BindingFlags.Public | 
				                                           BindingFlags.Instance));
				if (method != null)
				{
					method.Invoke(ToryScene.Instance, null);
				}
			}
			else if (toScene != null)
			{
				// Go to the specified scene.
				type = toScene.GetType();
				MethodInfo method = type.GetMethod("Start", (BindingFlags.NonPublic | 
				                                             BindingFlags.Instance));
				if (method != null)
				{
					method.Invoke(toScene, null);
				}
			}
			else
			{
				// Go to the Next.Start.
				if (Next != null)
				{
					type = Next.GetType();
					MethodInfo method = type.GetMethod("Start", (BindingFlags.NonPublic | 
					                                             BindingFlags.Instance));
					if (method != null)
					{
						method.Invoke(Next, null);
					}
				}
				else
				{
					// Go to the Scene.End.
					type = ToryScene.Instance.GetType();
					MethodInfo method = type.GetMethod("End", (BindingFlags.NonPublic | 
					                                           BindingFlags.Public | 
					                                           BindingFlags.Instance));
					if (method != null)
					{
						method.Invoke(ToryScene.Instance, null);
					}
				}
			}
		}


		// Interface

		/// <summary>
		/// Proceed to the next tory scene state.
		/// </summary>
		public void Proceed()
		{
			Proceed(0f);
		}

		/// <summary>
		/// Proceed to the next tory scene state after the <c>delay</c> in seconds.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		public void Proceed(float delay)
		{
			Proceed(delay, false);
		}

		/// <summary>
		/// Proceed to the next tory scene state after the <c>delay</c> in seconds.
		/// You can determine when to end the current tory scene via the <c>endNow</c> parameter.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		/// <param name="endNow">If set to <c>true</c>, end the current tory scene immediately, and then wait the <c>delay</c> in seconds to start the next tory scene. If set to <c>false</c>, wait the <c>delay</c> in seconds before ending the current tory scene.</param>
		public void Proceed(float delay, bool endNow)
		{
			SceneBehaviour.StartCoroutine(End(null, delay, endNow));
		}

		/// <summary>
		/// Start the specified tory scene state.
		/// </summary>
		/// <param name="scene">The tory scene state to start.</param>
		public void Start(IToryScene scene)
		{
			Start(scene, 0f);
		}


		/// <summary>
		/// Start the specified tory scene state after the <c>delay</c> in seconds.
		/// </summary>
		/// <returns>The tory scene state to start.</returns>
		/// <param name="scene">Scene.</param>
		/// <param name="delay">Delay in seconds.</param>
		public void Start(IToryScene scene, float delay)
		{
			Start(scene, delay, false);
		}

		/// <summary>
		/// Start the specified tory scene state after the <c>delay</c> in seconds.
		/// You can determine when to end the current tory scene via the <c>endNow</c> parameter.
		/// </summary>
		/// <returns>The start.</returns>
		/// <param name="scene">The tory scene state to start.</param>
		/// <param name="delay">Delay in seconds.</param>
		/// <param name="endNow">If set to <c>true</c>, end the current tory scene immediately, and then wait the <c>delay</c> in seconds to start the next tory scene. If set to <c>false</c>, wait the <c>delay</c> in seconds before ending the current tory scene.</param>
		public void Start(IToryScene scene, float delay, bool endNow)
		{
			SceneBehaviour.StartCoroutine(End(scene, delay, endNow));
		}

		/// <summary>
		/// Loads a new tory scene.
		/// </summary>
		public void LoadToryScene()
		{
			LoadToryScene(0f);
		}

		/// <summary>
		/// Loads a new tory scene after the <c>delay</c> in seconds.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		public void LoadToryScene(float delay)
		{
			LoadToryScene(delay, false);
		}

		/// <summary>
		/// Loads a new tory scene after the <c>delay</c> in seconds.
		/// You can determine when to end the current tory scene via the <c>endNow</c> parameter.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		/// <param name="endNow">If set to <c>true</c>, end the current tory scene immediately, and then wait the <c>delay</c> in seconds to load a new tory scene. If set to <c>false</c>, wait the <c>delay</c> in seconds before loading a new tory scene.</param>
		public void LoadToryScene(float delay, bool endNow)
		{
			forcedEnd = true;
			SceneBehaviour.StartCoroutine(End(null, delay, endNow));
		}

		#endregion
	}
}