using ToryFramework.Scene;

namespace ToryFramework
{
	public class ToryResultScene : TorySceneState
	{
		#region SINGLETON

		static volatile ToryResultScene instance;
		static readonly object syncRoot = new object();

		ToryResultScene() { }

		public static ToryResultScene Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ToryResultScene();
						}
					}
				}
				return instance;
			}
		}

		#endregion
	}
}