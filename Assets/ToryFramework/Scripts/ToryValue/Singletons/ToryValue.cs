using ToryFramework.Behaviour;

namespace ToryFramework
{
	public partial class ToryValue
	{
		#region SINGLETON

		static volatile ToryValue instance;
		static readonly object syncRoot = new object();

		ToryValue() { }

		public static ToryValue Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ToryValue();
						}
					}
				}
				return instance;
			}
		}

		#endregion



		#region PROPERTIES

		// Behaviours

		protected ToryFrameworkBehaviour FrameworkBehaviour 	{ get { return ToryFrameworkBehaviour.Instance; }}

		ToryValueBehaviour ValueBehaviour 						{ get { return ToryValueBehaviour.Instance; }}

		#endregion
	}
}
