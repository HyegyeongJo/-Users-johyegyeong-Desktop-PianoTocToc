using UnityEngine;
using ManUtils;

namespace ToryFramework.Behaviour
{
	[ManScriptExecutionOrder(-9998)]
	public partial class ToryValueBehaviour : MonoBehaviour
	{
		#region FIELDS

		// Singleton

		static ToryValueBehaviour instance;

		#endregion



		#region PROPERTIES

		// Singleton

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static ToryValueBehaviour Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<ToryValueBehaviour>();
				}
				return instance;
			}
		}

		#endregion



		#region EVENTS

		#endregion



		#region UNITY_FRAMEWORK

		void Awake()
		{
			// Singleton
			if (Instance != this)
			{
				Destroy(this);
				Debug.LogError("The duplicated singleton instance of " + name + " removed.");
			}
		}

		#endregion



		#region CUSTOM_FRAMEWORK

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		#endregion
	}
}