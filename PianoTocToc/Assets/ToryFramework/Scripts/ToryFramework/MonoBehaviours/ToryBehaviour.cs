using UnityEngine;
using ToryFramework;

namespace ToryFramework.Behaviour
{
	public class ToryBehaviour : MonoBehaviour
	{
		//#region FIELDS

		//[Header("Use Tory Scene Events")]

		//[SerializeField, HideInInspector] protected bool useTitleStarted;
		//[SerializeField, HideInInspector] protected bool useTitleUpdated;
		//[SerializeField, HideInInspector] protected bool useTitleFixedUpdated;
		//[SerializeField, HideInInspector] protected bool useTitleLateUpdated;
		//[SerializeField, HideInInspector] protected bool useTitleEnded;

		//[SerializeField, HideInInspector] protected bool useGuideStarted;
		//[SerializeField, HideInInspector] protected bool useGuideUpdated;
		//[SerializeField, HideInInspector] protected bool useGuideFixedUpdated;
		//[SerializeField, HideInInspector] protected bool useGuideLateUpdated;
		//[SerializeField, HideInInspector] protected bool useGuideEnded;

		//[SerializeField, HideInInspector] protected bool useStageStarted;
		//[SerializeField, HideInInspector] protected bool useStageUpdated;
		//[SerializeField, HideInInspector] protected bool useStageFixedUpdated;
		//[SerializeField, HideInInspector] protected bool useStageLateUpdated;
		//[SerializeField, HideInInspector] protected bool useStageEnded;

		//[SerializeField, HideInInspector] protected bool useResultStarted;
		//[SerializeField, HideInInspector] protected bool useResultUpdated;
		//[SerializeField, HideInInspector] protected bool useResultFixedUpdated;
		//[SerializeField, HideInInspector] protected bool useResultLateUpdated;
		//[SerializeField, HideInInspector] protected bool useResultEnded;

		//[SerializeField, HideInInspector] protected bool useTransitionStarted;
		//[SerializeField, HideInInspector] protected bool useTransitionUpdated;
		//[SerializeField, HideInInspector] protected bool useTransitionFixedUpdated;
		//[SerializeField, HideInInspector] protected bool useTransitionLateUpdated;
		//[SerializeField, HideInInspector] protected bool useTransitionEnded;

		//#endregion



		//#region PROPERTIES

		//#endregion



		//#region EVENTS

		//#endregion



		//#region UNITY_FRAMEWORK

		//protected virtual void OnEnable()
		//{
		//	// Title

		//	if (useTitleStarted)
		//	{
		//		ToryScene.Instance.Title.Started += OnTitleStarted;
		//		ToryScene.Instance.Title.Started += StartTitle;
		//	}

		//	if (useTitleUpdated) 
		//	{
		//		ToryScene.Instance.Title.Updated += OnTitleUpdated;
		//		ToryScene.Instance.Title.Updated += UpdateTitle;
		//	}
		//	if (useTitleFixedUpdated) 
		//	{
		//		ToryScene.Instance.Title.FixedUpdated += OnTitleFixedUpdated;
		//		ToryScene.Instance.Title.FixedUpdated += FixedUpdateTitle;
		//	}

		//	if (useTitleLateUpdated) 
		//	{
		//		ToryScene.Instance.Title.LateUpdated += OnTitleLateUpdated;
		//		ToryScene.Instance.Title.LateUpdated += LateUpdateTitle;
		//	}

		//	if (useTitleEnded) 
		//	{
		//		ToryScene.Instance.Title.Ended += OnTitleEnded;
		//		ToryScene.Instance.Title.Ended += EndTitle;
		//	}


		//	// Guide

		//	if (useGuideStarted)
		//	{
		//		ToryScene.Instance.Guide.Started += OnGuideStarted;
		//		ToryScene.Instance.Guide.Started += StartGuide;
		//	}

		//	if (useGuideUpdated) 
		//	{
		//		ToryScene.Instance.Guide.Updated += OnGuideUpdated;
		//		ToryScene.Instance.Guide.Updated += UpdateGuide;
		//	}

		//	if (useGuideFixedUpdated) 
		//	{
		//		ToryScene.Instance.Guide.FixedUpdated += OnGuideFixedUpdated;
		//		ToryScene.Instance.Guide.FixedUpdated += FixedUpdateGuide;
		//	}

		//	if (useGuideLateUpdated) 
		//	{
		//		ToryScene.Instance.Guide.LateUpdated += OnGuideLateUpdated;
		//		ToryScene.Instance.Guide.LateUpdated += LateUpdateGuide;
		//	}

		//	if (useGuideEnded) 
		//	{
		//		ToryScene.Instance.Guide.Ended += OnGuideEnded;
		//		ToryScene.Instance.Guide.Ended += EndGuide;
		//	}


		//	// Stage

		//	if (useStageStarted)
		//	{
		//		ToryScene.Instance.Stage.Started += OnStageStarted;
		//		ToryScene.Instance.Stage.Started += StartStage;
		//	}

		//	if (useStageUpdated) 
		//	{
		//		ToryScene.Instance.Stage.Updated += OnStageUpdated;
		//		ToryScene.Instance.Stage.Updated += UpdateStage;
		//	}

		//	if (useStageFixedUpdated) 
		//	{
		//		ToryScene.Instance.Stage.FixedUpdated += OnStageFixedUpdated;
		//		ToryScene.Instance.Stage.FixedUpdated += FixedUpdateStage;
		//	}

		//	if (useStageLateUpdated) 
		//	{
		//		ToryScene.Instance.Stage.LateUpdated += OnStageLateUpdated;
		//		ToryScene.Instance.Stage.LateUpdated += LateUpdateStage;
		//	}

		//	if (useStageEnded) 
		//	{
		//		ToryScene.Instance.Stage.Ended += OnStageEnded;
		//		ToryScene.Instance.Stage.Ended += EndStage;
		//	}


		//	// Result

		//	if (useResultStarted)
		//	{
		//		ToryScene.Instance.Result.Started += OnResultStarted;
		//		ToryScene.Instance.Result.Started += StartResult;
		//	}

		//	if (useResultUpdated) 
		//	{
		//		ToryScene.Instance.Result.Updated += OnResultUpdated;
		//		ToryScene.Instance.Result.Updated += UpdateResult;
		//	}

		//	if (useResultFixedUpdated) 
		//	{
		//		ToryScene.Instance.Result.FixedUpdated += OnResultFixedUpdated;
		//		ToryScene.Instance.Result.FixedUpdated += FixedUpdateResult;
		//	}

		//	if (useResultLateUpdated) 
		//	{
		//		ToryScene.Instance.Result.LateUpdated += OnResultLateUpdated;
		//		ToryScene.Instance.Result.LateUpdated += LateUpdateResult;
		//	}

		//	if (useResultEnded) 
		//	{
		//		ToryScene.Instance.Result.Ended += OnResultEnded;
		//		ToryScene.Instance.Result.Ended += EndResult;
		//	}


		//	// Transition

		//	if (useTransitionStarted)
		//	{
		//		ToryScene.Instance.Transition.Started += OnTransitionStarted;
		//		ToryScene.Instance.Transition.Started += StartTransition;
		//	}

		//	if (useTransitionUpdated) 
		//	{
		//		ToryScene.Instance.Transition.Updated += OnTransitionUpdated;
		//		ToryScene.Instance.Transition.Updated += UpdateTransition;
		//	}

		//	if (useTransitionFixedUpdated) 
		//	{
		//		ToryScene.Instance.Transition.FixedUpdated += OnTransitionFixedUpdated;
		//		ToryScene.Instance.Transition.FixedUpdated += FixedUpdateTransition;
		//	}

		//	if (useTransitionLateUpdated) 
		//	{
		//		ToryScene.Instance.Transition.LateUpdated += OnTransitionLateUpdated;
		//		ToryScene.Instance.Transition.LateUpdated += LateUpdateTransition;
		//	}

		//	if (useTransitionEnded) 
		//	{
		//		ToryScene.Instance.Transition.Ended += OnTransitionEnded;
		//		ToryScene.Instance.Transition.Ended += EndTransition;
		//	}
		//}

		//protected virtual void OnDisable()
		//{
		//	// Title
		//	ToryScene.Instance.Title.Started -= OnTitleStarted;
		//	ToryScene.Instance.Title.Updated -= OnTitleUpdated;
		//	ToryScene.Instance.Title.FixedUpdated -= OnTitleFixedUpdated;
		//	ToryScene.Instance.Title.LateUpdated -= OnTitleLateUpdated;
		//	ToryScene.Instance.Title.Ended -= OnTitleEnded;

		//	ToryScene.Instance.Title.Started -= StartTitle;
		//	ToryScene.Instance.Title.Updated -= UpdateTitle;
		//	ToryScene.Instance.Title.FixedUpdated -= FixedUpdateTitle;
		//	ToryScene.Instance.Title.LateUpdated -= LateUpdateTitle;
		//	ToryScene.Instance.Title.Ended -= EndTitle;

		//	// Guide
		//	ToryScene.Instance.Guide.Started -= OnGuideStarted;
		//	ToryScene.Instance.Guide.Updated -= OnGuideUpdated;
		//	ToryScene.Instance.Guide.FixedUpdated -= OnGuideFixedUpdated;
		//	ToryScene.Instance.Guide.LateUpdated -= OnGuideLateUpdated;
		//	ToryScene.Instance.Guide.Ended -= OnGuideEnded;

		//	ToryScene.Instance.Guide.Started -= StartGuide;
		//	ToryScene.Instance.Guide.Updated -= UpdateGuide;
		//	ToryScene.Instance.Guide.FixedUpdated -= FixedUpdateGuide;
		//	ToryScene.Instance.Guide.LateUpdated -= LateUpdateGuide;
		//	ToryScene.Instance.Guide.Ended -= EndGuide;

		//	// Stage
		//	ToryScene.Instance.Stage.Started -= OnStageStarted;
		//	ToryScene.Instance.Stage.Updated -= OnStageUpdated;
		//	ToryScene.Instance.Stage.FixedUpdated -= OnStageFixedUpdated;
		//	ToryScene.Instance.Stage.LateUpdated -= OnStageLateUpdated;
		//	ToryScene.Instance.Stage.Ended -= OnStageEnded;

		//	ToryScene.Instance.Stage.Started -= StartStage;
		//	ToryScene.Instance.Stage.Updated -= UpdateStage;
		//	ToryScene.Instance.Stage.FixedUpdated -= FixedUpdateStage;
		//	ToryScene.Instance.Stage.LateUpdated -= LateUpdateStage;
		//	ToryScene.Instance.Stage.Ended -= EndStage;

		//	// Result
		//	ToryScene.Instance.Result.Started -= OnResultStarted;
		//	ToryScene.Instance.Result.Updated -= OnResultUpdated;
		//	ToryScene.Instance.Result.FixedUpdated -= OnResultFixedUpdated;
		//	ToryScene.Instance.Result.LateUpdated -= OnResultLateUpdated;
		//	ToryScene.Instance.Result.Ended -= OnResultEnded;

		//	ToryScene.Instance.Result.Started -= StartResult;
		//	ToryScene.Instance.Result.Updated -= UpdateResult;
		//	ToryScene.Instance.Result.FixedUpdated -= FixedUpdateResult;
		//	ToryScene.Instance.Result.LateUpdated -= LateUpdateResult;
		//	ToryScene.Instance.Result.Ended -= EndResult;

		//	// Transition
		//	ToryScene.Instance.Transition.Started -= OnTransitionStarted;
		//	ToryScene.Instance.Transition.Updated -= OnTransitionUpdated;
		//	ToryScene.Instance.Transition.FixedUpdated -= OnTransitionFixedUpdated;
		//	ToryScene.Instance.Transition.LateUpdated -= OnTransitionLateUpdated;
		//	ToryScene.Instance.Transition.Ended -= OnTransitionEnded;

		//	ToryScene.Instance.Transition.Started -= StartTransition;
		//	ToryScene.Instance.Transition.Updated -= UpdateTransition;
		//	ToryScene.Instance.Transition.FixedUpdated -= FixedUpdateTransition;
		//	ToryScene.Instance.Transition.LateUpdated -= LateUpdateTransition;
		//	ToryScene.Instance.Transition.Ended -= EndTransition;
		//}

		//#endregion



		//#region CUSTOM_FRAMEWORK

		//// Title

		//protected virtual void OnTitleStarted() { }

		//protected virtual void OnTitleUpdated() { }

		//protected virtual void OnTitleFixedUpdated() { }

		//protected virtual void OnTitleLateUpdated() { }

		//protected virtual void OnTitleEnded() { }


		//protected virtual void StartTitle() { }

		//protected virtual void UpdateTitle() { }

		//protected virtual void FixedUpdateTitle() { }

		//protected virtual void LateUpdateTitle() { }

		//protected virtual void EndTitle() { }

		//// Guide

		//protected virtual void OnGuideStarted() { }

		//protected virtual void OnGuideUpdated() { }

		//protected virtual void OnGuideFixedUpdated() { }

		//protected virtual void OnGuideLateUpdated() { }

		//protected virtual void OnGuideEnded() { }


		//protected virtual void StartGuide() { }

		//protected virtual void UpdateGuide() { }

		//protected virtual void FixedUpdateGuide() { }

		//protected virtual void LateUpdateGuide() { }

		//protected virtual void EndGuide() { }

		//// Stage

		//protected virtual void OnStageStarted() { }

		//protected virtual void OnStageUpdated() { }

		//protected virtual void OnStageFixedUpdated() { }

		//protected virtual void OnStageLateUpdated() { }

		//protected virtual void OnStageEnded() { }


		//protected virtual void StartStage() { }

		//protected virtual void UpdateStage() { }

		//protected virtual void FixedUpdateStage() { }

		//protected virtual void LateUpdateStage() { }

		//protected virtual void EndStage() { }

		//// Result

		//protected virtual void OnResultStarted() { }

		//protected virtual void OnResultUpdated() { }

		//protected virtual void OnResultFixedUpdated() { }

		//protected virtual void OnResultLateUpdated() { }

		//protected virtual void OnResultEnded() { }


		//protected virtual void StartResult() { }

		//protected virtual void UpdateResult() { }

		//protected virtual void FixedUpdateResult() { }

		//protected virtual void LateUpdateResult() { }

		//protected virtual void EndResult() { }

		//// Transition

		//protected virtual void OnTransitionStarted() { }

		//protected virtual void OnTransitionUpdated() { }

		//protected virtual void OnTransitionFixedUpdated() { }

		//protected virtual void OnTransitionLateUpdated() { }

		//protected virtual void OnTransitionEnded() { }


		//protected virtual void StartTransition() { }

		//protected virtual void UpdateTransition() { }

		//protected virtual void FixedUpdateTransition() { }

		//protected virtual void LateUpdateTransition() { }

		//protected virtual void EndTransition() { }

		//#endregion



		//#region EVENT_HANDLERS

		//#endregion



		//#region METHODS

		//#endregion
	}
}