using System.Reflection;
using UnityEngine;
using ManUtils;

namespace ToryFramework.Behaviour
{
	[ManScriptExecutionOrder(-9998)]
	public class ToryFrameworkBehaviour : MonoBehaviour
	{
		#region FIELDS

		// Singleton

		static ToryFrameworkBehaviour instance;


		[Header("Debug")]

		[Tooltip("You can choose whether to display logs on the colsole or not.")]
		[SerializeField] bool showLog = true;

		#endregion



		#region PROPERTIES

		// Singleton

		public static ToryFrameworkBehaviour Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<ToryFrameworkBehaviour>();
				}
				return instance;
			}
		}


		// Editor

		public bool CanShowLog					{ get { return showLog; } set { showLog = value; }}

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

			// Init ToryTime.
			System.Type type = ToryTime.Instance.GetType();
			MethodInfo method = type.GetMethod("Init", (BindingFlags.NonPublic | 
			                                            BindingFlags.Public | 
			                                            BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryTime.Instance, null);
			}

			// Init ToryProgress.
			type = ToryProgress.Instance.GetType();
			method = type.GetMethod("Init", (BindingFlags.NonPublic | 
			                                 BindingFlags.Public | 
			                                 BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryProgress.Instance, null);
			}
		}

		void OnDestroy()
		{
			ResetEvents();
		}

		#endregion



		#region TORY_FRAMEWORK

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		void ResetEvents()
		{
			// Time
			System.Type type = ToryTime.Instance.GetType();
			MethodInfo method = type.GetMethod("ResetEvents", (BindingFlags.NonPublic | 
			                                                   BindingFlags.Public | 
			                                                   BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryTime.Instance, null);
			}

			// Progress
			type = ToryProgress.Instance.GetType();
			method = type.GetMethod("ResetEvents", (BindingFlags.NonPublic | 
			                                        BindingFlags.Public | 
			                                        BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryProgress.Instance, null);
			}
		}

		#endregion
	}
}