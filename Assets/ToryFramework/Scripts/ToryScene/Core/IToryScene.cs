namespace ToryFramework.Scene
{
	public delegate void UpdateEventHandler();

	public delegate void ClearEventHandler(bool cleared);

	public interface IToryScene
	{
		#region PROPERTIES

		#endregion



		#region EVENTS

		// Update

		/// <summary>
		/// Occurs when this tory scene state is started.
		/// </summary>
		event UpdateEventHandler Started;

		/// <summary>
		/// Occurs every frame after <see cref="E:Started"/> if this tory scene state is playing. 
		/// </summary>
		event UpdateEventHandler Updated;

		/// <summary>
		/// Occurs after <see cref="E:Updated"/> if this tory scene state is playing.
		/// </summary>
		event UpdateEventHandler LateUpdated;

		/// <summary>
		/// Occurs every fixed frame after <see cref="E:Started"/> if this tory scene state is playing.
		/// </summary>
		event UpdateEventHandler FixedUpdated;

		/// <summary>
		/// Occurs when this tory scene state is ended.
		/// </summary>
		event UpdateEventHandler Ended;


		// Time

		/// <summary>
		/// Occurs when forced stay timer timed out.
		/// </summary>
		event ToryTime.TimerEventHandler ForcedStayTimerTimedOut;

		/// <summary>
		/// Occurs when interaction check timer timed out.
		/// </summary>
		event ToryTime.TimerEventHandler InteractionCheckTimerTimedOut;

		/// <summary>
		/// Occurs when transition timer timed out.
		/// </summary>
		event ToryTime.TimerEventHandler TransitionTimerTimedOut;


		// Input

		/// <summary>
		/// Occurs when a player interacts with the game.
		/// </summary>
		event ToryInput.InteractionEventHandler Interacted;

		/// <summary>
		/// Occurs when a player left.
		/// </summary>
		event ToryInput.InteractionEventHandler PlayerLeft;

		#endregion



		#region METHODS

		/// <summary>
		/// Proceed to the next tory scene state.
		/// </summary>
		void Proceed();

		/// <summary>
		/// Proceed to the next tory scene state after the <c>delay</c> in seconds.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		void Proceed(float delay);

		/// <summary>
		/// Proceed to the next tory scene state after the <c>delay</c> in seconds.
		/// You can determine when to end the current tory scene via the <c>endNow</c> parameter.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		/// <param name="endNow">If set to <c>true</c>, end the current tory scene immediately, and then wait the <c>delay</c> in seconds to start the next tory scene. If set to <c>false</c>, wait the <c>delay</c> in seconds before ending the current tory scene.</param>
		void Proceed(float delay, bool endNow);

		/// <summary>
		/// Start the specified tory scene state.
		/// </summary>
		/// <param name="state">The tory scene state to start.</param>
		void Start(IToryScene state);

		/// <summary>
		/// Start the specified tory scene state after the <c>delay</c> in seconds.
		/// </summary>
		/// <returns>The tory scene state to start.</returns>
		/// <param name="state">Scene.</param>
		/// <param name="delay">Delay in seconds.</param>
		void Start(IToryScene state, float delay);

		/// <summary>
		/// Start the specified tory scene state after the <c>delay</c> in seconds.
		/// You can determine when to end the current tory scene via the <c>endNow</c> parameter.
		/// </summary>
		/// <returns>The start.</returns>
		/// <param name="state">The tory scene state to start.</param>
		/// <param name="delay">Delay in seconds.</param>
		/// <param name="endNow">If set to <c>true</c>, end the current tory scene immediately, and then wait the <c>delay</c> in seconds to start the next tory scene. If set to <c>false</c>, wait the <c>delay</c> in seconds before ending the current tory scene.</param>
		void Start(IToryScene state, float delay, bool endNow);

		/// <summary>
		/// Loads a new tory scene.
		/// </summary>
		void LoadToryScene();

		/// <summary>
		/// Loads a new tory scene after the <c>delay</c> in seconds.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		void LoadToryScene(float delay);

		/// <summary>
		/// Loads a new tory scene after the <c>delay</c> in seconds.
		/// You can determine when to end the current tory scene via the <c>endNow</c> parameter.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		/// <param name="endNow">If set to <c>true</c>, end the current tory scene immediately, and then wait the <c>delay</c> in seconds to load a new tory scene. If set to <c>false</c>, wait the <c>delay</c> in seconds before loading a new tory scene.</param>
		void LoadToryScene(float delay, bool endNow);

		#endregion
	}
}