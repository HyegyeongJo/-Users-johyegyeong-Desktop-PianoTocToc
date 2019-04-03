using System.Reflection;
using UnityEngine;
using ManUtils;

namespace ToryFramework.Behaviour
{
	[RequireComponent(typeof(Animator))]
	[ManScriptExecutionOrder(-9998)]
	public partial class TorySceneBehaviour : MonoBehaviour
	{
		#region FIELDS

		// Singleton
		static TorySceneBehaviour instance;

		// Components
		Animator animator;

		#endregion



		#region PROPERTIES

		// Singleton
		public static TorySceneBehaviour Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<TorySceneBehaviour>();
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

			// Components
			animator = GetComponent<Animator>();

			// Init tory scene and states.
			InitStates();

			// Turn off the aniamtor.
			animator.enabled = false;
		}

		void OnDestroy()
		{
			ResetEvents();
		}

		void Start()
		{
			// Start ToryScene.
			System.Type type = ToryScene.Instance.GetType();
			MethodInfo method = type.GetMethod("Start", (BindingFlags.NonPublic |
														 BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryScene.Instance, null);
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