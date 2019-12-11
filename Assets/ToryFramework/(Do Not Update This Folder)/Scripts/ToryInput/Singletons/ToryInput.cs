using ToryFramework.Input;

namespace ToryFramework
{
	public class ToryInput : ToryFloatInput
	{
		#region SINGLETON

		static volatile ToryInput instance;
		static readonly object syncRoot = new object();

		ToryInput() { }

		public static ToryInput Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ToryInput();
						}
					}
				}
				return instance;
			}
		}

		#endregion
	}	
}