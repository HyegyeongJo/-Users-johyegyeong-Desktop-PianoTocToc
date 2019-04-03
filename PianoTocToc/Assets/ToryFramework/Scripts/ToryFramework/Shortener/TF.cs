using ToryFramework;

/// <summary>
/// The shortener of ToryFramework.
/// </summary>
public static class TF
{
	#region PROPERTIES

	/// <summary>
	/// Gets the tory scene that provides states and connections about the scene flow.
	/// You can manage a Unity scene by dividing it into the multiple tory scene states.
	/// </summary>
	/// <value>The tory scene.</value>
	public static ToryScene Scene 					{ get { return ToryScene.Instance; }}

	/// <summary>
	/// Gets the tory input that supports varius types and provides signal processing and interaction gauge. 
	/// </summary>
	/// <value>The tory input.</value>
	public static ToryInput Input 					{ get { return ToryInput.Instance; }}

	/// <summary>
	/// Gets the tory time that provides timer-related events.
	/// </summary>
	/// <value>The time.</value>
	public static ToryTime Time 					{ get { return ToryTime.Instance; }}

	/// <summary>
	/// Gets the custom tory value that supports save and load functionalities of a value.
	/// </summary>
	/// <value>The custom tory value.</value>
	public static ToryFramework.ToryValue Value 	{ get { return ToryFramework.ToryValue.Instance; }}

	/// <summary>
	/// Gets the tory progress that handles progress points of the game.
	/// </summary>
	/// <value>The tory progress.</value>
	public static ToryProgress Progress 			{ get { return ToryProgress.Instance; }}

	#endregion
}
