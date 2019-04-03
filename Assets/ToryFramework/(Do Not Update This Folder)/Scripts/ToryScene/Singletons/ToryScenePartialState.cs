using UnityEngine;
using ToryFramework.Scene;

namespace ToryFramework
{
	/// <summary>
	/// Tory scene class that provides states and connections about the scene flow.
	/// You can manage a Unity scene by dividing it to multiple tory scenes.
	/// </summary>
	public partial class ToryScene : IToryScene
	{
		void SetFirst()
		{
			switch (smb.First)
			{
				case State.TITLE:
					First = ToryTitleScene.Instance;
					break;

				case State.PLAY:
					First = ToryPlayScene.Instance;
					break;

				case State.RESULT:
					First = ToryResultScene.Instance;
					break;

				case State.NULL:
				case State.EXIT:
				default:
					First = null;
					Debug.LogError("[ToryScene] Please confirm the connection of ToryScene in the Animator. " +
					               "There is the invalid first state in the ToryScene layer.");
					break;
			}
		}

		public ToryTitleScene Title { get { return ToryTitleScene.Instance; }}

		public ToryPlayScene Play { get { return ToryPlayScene.Instance; }}

		public ToryResultScene Result { get { return ToryResultScene.Instance; }}
	}
}