using ToryFramework.Scene;

namespace ToryFramework
{
	public class ToryTitleScene : TorySceneState
	{
		#region SINGLETON

		static volatile ToryTitleScene instance;
		static readonly object syncRoot = new object();

		ToryTitleScene() { }

		public static ToryTitleScene Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ToryTitleScene();
						}
					}
				}
				return instance;
			}
		}

		#endregion
	}
}