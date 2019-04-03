using UnityEngine;
using ToryFramework.Scene;
using ManUtils;

namespace ToryFramework.SMB
{
	public class TorySceneSMB : StateMachineBehaviour
	{
		#region FIELDS

		[Header("Connection Info.")]

		[Tooltip("The first tory scene state.")]
		[SerializeField, ManReadOnly] State first;


		[Header("Unity Scene Management")]

		[Tooltip("If set to true, the Unity scene of Unity Scene Index will be loaded when the tory scene is laoded.")]
		[SerializeField, ManReadOnlyOnPlaying] bool loadUnityScene = true;

		[Tooltip("The current scene index listed on the Build Settings. " +
		         "If the Load Unity Scene is set to true, this will load the Unity scene of this index. " +
		         "For example, you can set this value to 1 if you use the Crytonite scene as index 0.")]
		[SerializeField, ManReadOnlyOnPlaying] int unitySceneIndex = 0;


		[Header("Multi-Stage")]

		[Tooltip("Use this value freely." +
		         "Cannot be smaller than 1.")]
		[SerializeField] int stageCount = 1;

		#endregion



		#region PROPERTIES

		// Connection info.

		public State First 			{ get { return first; } set { first = value; }}


		// Scene management

		public bool CanLoadUnityScene 	{ get { return loadUnityScene; } set { loadUnityScene = value; }}

		public int UnitySceneIndex 	 { get { return unitySceneIndex; }
			set
			{
				unitySceneIndex = Mathf.Max(0, value);
			}
		}


		// Steps

		public int StageCount				{ get { return stageCount; }
			set
			{
				stageCount = Mathf.Max(1, value);
			}
		}

		#endregion



		#region EVENTS

		#endregion



		#region UNITY_FRAMEWORK

		#endregion



		#region CUSTOM_FRAMEWORK

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		#endregion
	}	
}