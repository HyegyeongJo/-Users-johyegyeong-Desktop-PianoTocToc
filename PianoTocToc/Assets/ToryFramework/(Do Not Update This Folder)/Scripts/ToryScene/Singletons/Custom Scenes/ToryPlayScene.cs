using ToryFramework.Scene;

namespace ToryFramework
{
	public class ToryPlayScene : TorySceneState
	{
		#region SINGLETON

		static volatile ToryPlayScene instance;
		static readonly object syncRoot = new object();

		ToryPlayScene() { }

		public static ToryPlayScene Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ToryPlayScene();
						}
					}
				}
				return instance;
			}
		}

		#endregion
	}
}