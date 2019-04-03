using UnityEngine;

namespace ToryFramework.Scene
{
	public abstract partial class TorySceneState : IToryScene
	{
		protected void SetNext()
		{
			switch (smb.Next)
			{
				case State.TITLE:
					Next = ToryTitleScene.Instance;
					break;

				case State.PLAY:
					Next = ToryPlayScene.Instance;
					break;

				case State.RESULT:
					Next = ToryResultScene.Instance;
					break;

				case State.EXIT:
				case State.NULL:
					break;

				default:
					Next = null;
					Debug.LogError("[ToryScene] Please confirm the connection of ToryScene in the Animator. " +
					               "There is no valid next state in the " + smb.Name + " state.");
					break;
			}
		}
	}
}