using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using ToryFramework.Scene;
using ToryFramework.Behaviour;
using ToryFramework.SMB;

namespace ToryFramework
{
	/// <summary>
	/// Tory scene class that provides states and connections about the scene flow.
	/// You can manage a Unity scene by dividing it into the multiple tory scene states.
	/// </summary>
	public partial class ToryScene : IToryScene
	{
		#region SINGLETON

		static volatile ToryScene instance;
		static readonly object syncRoot = new object();

		ToryScene() { }

		public static ToryScene Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ToryScene();
						}
					}
				}
				return instance;
			}
		}

		#endregion



		#region FIELDS

		TorySceneSMB smb;

		Coroutine updateCrt, fixedUpdateCrt;

		bool playing;

		float startTime, unscaledStartTime;

		int stageIndex = 0;

		#endregion



		#region PROPERTIES

		// Behaviours

		public ToryFrameworkBehaviour FrameworkBehaviour 		{ get { return ToryFrameworkBehaviour.Instance; }}

		public TorySceneBehaviour SceneBehaviour 				{ get { return TorySceneBehaviour.Instance; }}


		// States

		/// <summary>
		/// Gets the current tory scene state (Read Only).
		/// </summary>
		/// <value>The current tory scene state.</value>
		public TorySceneState Current 			{ get; private set; }

		/// <summary>
		/// Gets or sets the first tory scene state.
		/// </summary>
		/// <value>The first tory scene state.</value>
		public TorySceneState First 			{ get; set; }


		// Unity scene management

		/// <summary>
		/// Gets or sets a value indicating whether the tory scene can load the Unity scene when it is loaded.
		/// </summary>
		/// <value><c>true</c> if the tory scene can load the Unity scene when it is loaded; otherwise, <c>false</c>.</value>
		public bool CanLoadUnityScene 			{ get { return smb.CanLoadUnityScene; } set { smb.CanLoadUnityScene = value; }}


		/// <summary>
		/// Gets or sets the build index of the Unity scene listed on the Build Settings.
		/// If the <see cref="P:CanLoadUnityScene"/> is set to true, this will load the Unity scene of this index.
		/// For example, you can set this value to 1 if you use the Crytonite scene as index 0.
		/// </summary>
		/// <value>The index of the Unity scene.</value>
		public int UnitySceneIndex 					{ get { return smb.UnitySceneIndex; } set { smb.UnitySceneIndex = Mathf.Max(0, value); }}


		// Time properties

		/// <summary>
		/// Gets the time since this tory scene laoded (Read Only).
		/// </summary>
		/// <value>The time since this tory scene loaded.</value>
		public float TimeSinceLoaded 				{ get; private set; }

		/// <summary>
		/// Gets the timeScale-independent time since this tory scene loaded (Read Only).
		/// </summary>
		/// <value>The timeScale-independent time since this tory scene loaded.</value>
		public float UnscaledTimeSinceLoaded 		{ get; private set; }


		// Stages

		/// <summary>
		/// Gets or sets the stage count.
		/// Cannot be smaller than 1.
		/// </summary>
		/// <value>The stage count.</value>
		public int StageCount 					{ get { return smb.StageCount; } set { smb.StageCount = value; }}

		/// <summary>
		/// Gets or sets the index of the stage.
		/// Cannot be smaller than 0 and greater than or equal to <see cref="P:StageCount"/>.
		/// </summary>
		/// <value>The index of the stage.</value>
		public int StageIndex 					{ get { return stageIndex; }
			set
			{
				stageIndex = Mathf.Clamp(value, 0, StageCount - 1);
			}
		}


		// Clear

		/// <summary>
		/// Gets a value indicating whether this tory scene is cleared or not (Read Only).
		/// </summary>
		/// <value><c>true</c> if is cleared; otherwise, <c>false</c>.</value>
		public bool IsCleared 					{ get; private set; }


		// Input

		/// <summary>
		/// Gets a value indicating whether a player left from the scene.
		/// </summary>
		/// <value><c>true</c> if is player left; otherwise, <c>false</c>.</value>
		public bool IsPlayerLeft 				{ get { return ToryInput.Instance.IsPlayerLeft; }}

		#endregion



		#region EVENTS

		// Update

		/// <summary>
		/// Occurs when this tory scene is loaded.
		/// </summary>
		public event UpdateEventHandler Loaded = () => {};

		/// <summary>
		/// Occurs when this tory scene is started.
		/// </summary>
		public event UpdateEventHandler Started = () => {};

		/// <summary>
		/// Occurs every frame after <see cref="E:Started"/> if this tory scene is playing. 
		/// </summary>
		public event UpdateEventHandler Updated = () => {};

		/// <summary>
		/// Occurs after <see cref="E:Updated"/> if this tory scene is playing.
		/// </summary>
		public event UpdateEventHandler LateUpdated = () => {};

		/// <summary>
		/// Occurs every fixed frame after <see cref="E:Started"/> if this tory scene is playing.
		/// </summary>
		public event UpdateEventHandler FixedUpdated = () => {};

		/// <summary>
		/// Occurs when this tory scene is ended.
		/// </summary>
		public event UpdateEventHandler Ended = () => {};


		// Clear

		/// <summary>
		/// Occurs when this tory scene is cleared.
		/// </summary>
		public event ClearEventHandler Cleared = (bool cleared) => {};


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

		#endregion



		#region METHODS

		void ResetEvents()
		{
			// Update
			Loaded = () => {};
			Started = () => {};
			Updated = () => {};
			LateUpdated = () => {};
			FixedUpdated = () => {};
			Ended = () => {};

			// Clear
			Cleared = (bool cleared) => {};

			// Time
			ForcedStayTimerTimedOut = () => {};
			InteractionCheckTimerTimedOut = () => {};
			TransitionTimerTimedOut = () => {};

			// Input
			Interacted = () => {};
			PlayerLeft = () => {};
		}

		void Init(TorySceneSMB smb)
		{
			this.smb = smb;
			if (smb == null)
			{
				Debug.LogError("[ToryScene] Please confirm the ToryScene in the Animator. " +
				                   "There is the invalid TorySceneSMB component in the ToryScene layer.");
				return;
			}

			SetFirst();

			// Set the current tory scene.
			Current = First;
		}

		void Start()
		{
			// Check if this scene is already started.
			if (playing)
			{
				return;	
			}

			// Initialize
			playing = true;
			startTime = Time.time;
			unscaledStartTime = Time.unscaledTime;
			IsCleared = false;

			// Time
			ToryTime.Instance.ForcedStayTimerTimedOut += ForcedStayTimerTimedOut;
			ToryTime.Instance.InteractionCheckTimerTimedOut += InteractionCheckTimerTimedOut;
			ToryTime.Instance.TransitionTimerTimedOut += TransitionTimerTimedOut;

			// Input
			ToryInput.Instance.Interacted += Interacted;
			ToryInput.Instance.PlayerLeft += PlayerLeft;

			// Start
			if (FrameworkBehaviour.CanShowLog)
			{
				Debug.Log("[ToryScene] A new tory scene loaded.");	
			}
			Loaded();

			// Start
			if (FrameworkBehaviour.CanShowLog)
			{
				Debug.Log("[ToryScene] A new tory scene started.");	
			}
			Started();

			// Start the first tory scene.
			System.Type type = First.GetType();
			MethodInfo method = type.GetMethod("Start", (BindingFlags.NonPublic | 
			                                             BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(First, null);
			}

			// Start update coroutines.
			updateCrt = SceneBehaviour.StartCoroutine(Update());
			fixedUpdateCrt = SceneBehaviour.StartCoroutine(FixedUpdate());
		}

		IEnumerator Update()
		{
			while (true)
			{
				yield return null;

				// Update the time.
				TimeSinceLoaded = startTime - Time.time;
				UnscaledTimeSinceLoaded = unscaledStartTime - Time.unscaledTime;

				// Updates
				Updated();
				LateUpdated();
			}
		}

		IEnumerator FixedUpdate()
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

		void End()
		{
			SceneBehaviour.StartCoroutine(EndCoroutine());
		}

		IEnumerator EndCoroutine()
		{
			// Check if this scene is already ended.
			if (!playing)
			{
				yield break;	
			}
			playing = false;

			// End the updates
			if (updateCrt != null)
			{
				SceneBehaviour.StopCoroutine(updateCrt);	
			}
			if (fixedUpdateCrt != null)
			{
				SceneBehaviour.StopCoroutine(fixedUpdateCrt);	
			}

			// Time
			ToryTime.Instance.ForcedStayTimerTimedOut -= ForcedStayTimerTimedOut;
			ToryTime.Instance.InteractionCheckTimerTimedOut -= InteractionCheckTimerTimedOut;
			ToryTime.Instance.TransitionTimerTimedOut -= TransitionTimerTimedOut;

			// Input
			ToryInput.Instance.Interacted -= Interacted;
			ToryInput.Instance.PlayerLeft -= PlayerLeft;

			// End
			if (FrameworkBehaviour.CanShowLog)
			{
				Debug.Log("[ToryScene] The tory scene ended.");	
			}
			Ended();

			// Wait a frame.
			yield return null;

			// Load a new scene or restart the current scene
			if (CanLoadUnityScene)
			{
				SceneManager.LoadScene(UnitySceneIndex);
			}
			else
			{
				Start();
			}
		}

		/// <summary>
		/// Proceed to the next tory scene state.
		/// </summary>
		public void Proceed()
		{
			Current.Proceed();
		}

		/// <summary>
		/// Proceed to the next tory scene state after the <c>delay</c> in seconds.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		public void Proceed(float delay)
		{
			Current.Proceed(delay);
		}

		/// <summary>
		/// Proceed to the next tory scene state after the <c>delay</c> in seconds.
		/// You can determine when to end the current tory scene via the <c>endNow</c> parameter.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		/// <param name="endNow">If set to <c>true</c>, end the current tory scene immediately, and then wait the <c>delay</c> in seconds to start the next tory scene. If set to <c>false</c>, wait the <c>delay</c> in seconds before ending the current tory scene.</param>
		public void Proceed(float delay, bool endNow)
		{
			Current.Proceed(delay, endNow);
		}

		/// <summary>
		/// Start the specified tory scene state.
		/// </summary>
		/// <param name="scene">The tory scene state to start.</param>
		public void Start(IToryScene scene)
		{
			Current.Start(scene);
		}

		/// <summary>
		/// Start the specified tory scene state after the <c>delay</c> in seconds.
		/// </summary>
		/// <returns>The tory scene state to start.</returns>
		/// <param name="scene">Scene.</param>
		/// <param name="delay">Delay in seconds.</param>
		public void Start(IToryScene scene, float delay)
		{
			Current.Start(scene, delay);
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
			Current.Start(scene, delay, endNow);
		}

		/// <summary>
		/// Loads a new tory scene.
		/// </summary>
		public void LoadToryScene()
		{
			Current.LoadToryScene();
		}

		/// <summary>
		/// Loads a new tory scene after the <c>delay</c> in seconds.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		public void LoadToryScene(float delay)
		{
			Current.LoadToryScene(delay);
		}

		/// <summary>
		/// Loads a new tory scene after the <c>delay</c> in seconds.
		/// You can determine when to end the current tory scene via the <c>endNow</c> parameter.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		/// <param name="endNow">If set to <c>true</c>, end the current tory scene immediately, and then wait the <c>delay</c> in seconds to load a new tory scene. If set to <c>false</c>, wait the <c>delay</c> in seconds before loading a new tory scene.</param>
		public void LoadToryScene(float delay, bool endNow)
		{
			Current.LoadToryScene(delay, endNow);
		}

		public void Clear(bool cleared = true)
		{
			IsCleared = cleared;
			Cleared(cleared);
		}

		#endregion
	}
}