using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	[CustomEditor(typeof(ProgressUI))]
	public class ProgressUIEditor : Editor
	{
		Transform targetTransform;

		SerializedProperty shouldBeHiddenOnStart;

		bool isUIReferenceResolutionValid = false;
		RectTransform uiWrapperTransform;
		float wrapperTransformBottom;
		float wrapperTransformTop;
		float editorValueOfWrapperTransformBottom;
		float editorValueOfWrapperTransformTop;

		Image fillBackgroundColorImage;
		Image fillForegroundColorImage;
		Image goalIconImage;
		IdleBasedAnimationPlayer goalIconAnimationPlayer;

		Image indicatorIconImage;
		IdleBasedAnimationPlayer indicatorIconAnimationPlayer;

		ProgressUIWrapperAnimationPlayer progressWrapperAnimationPlayer;

		void OnEnable()
		{
			targetTransform = ((ProgressUI) target).transform;
			shouldBeHiddenOnStart = serializedObject.FindProperty("shouldBeHiddenOnStart");

			if (!UIOrientationSetter.ReferenceResolution.Equals(Vector2.zero))
			{
				uiWrapperTransform = targetTransform.GetComponentInChildren<TagClasses.ProgressUITransformWrapperObject>(true).GetComponent<RectTransform>();
				wrapperTransformBottom = uiWrapperTransform.offsetMin.y;
				wrapperTransformTop = UIOrientationSetter.ReferenceResolution.y + uiWrapperTransform.offsetMax.y;
				isUIReferenceResolutionValid = true;
			}

			fillBackgroundColorImage = targetTransform.GetComponentInChildren<TagClasses.ProgressUIFillBackgroundObject>(true).GetComponent<Image>();
			fillForegroundColorImage = targetTransform.GetComponentInChildren<TagClasses.ProgressUIFillForegroundObject>(true).GetComponent<Image>();

			goalIconImage = targetTransform.GetComponentInChildren<TagClasses.ProgressUIGoalIconObject>(true).GetComponent<Image>();
			goalIconAnimationPlayer = goalIconImage.GetComponent<IdleBasedAnimationPlayer>();

			indicatorIconImage = targetTransform.GetComponentInChildren<TagClasses.ProgressUIIndicatorIconObject>(true).GetComponent<Image>();
			indicatorIconAnimationPlayer = indicatorIconImage.GetComponent<IdleBasedAnimationPlayer>();

			progressWrapperAnimationPlayer = targetTransform.GetComponentInChildren<ProgressUIWrapperAnimationPlayer>(true);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Hide on start toggle.
			EditorGUILayout.Space();
			shouldBeHiddenOnStart.boolValue = EditorGUILayout.Toggle("Hide on start", shouldBeHiddenOnStart.boolValue);

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Inspector values may not be shown as updated after performing undo/redo. Deselecting/reselecting the GameObject should solve the problem.", MessageType.Info);

			// Progress bar size.
			if (!isUIReferenceResolutionValid)
			{
				Debug.Log("ReferenceResolution is not valid.");
				EditorGUILayout.Space();
				EditorGUILayout.HelpBox("Failed to check UI orientation.\nDeselecting and reslecting this GameObject should solve the problem.", MessageType.Warning);

				if (!UIOrientationSetter.ReferenceResolution.Equals(Vector2.zero))
				{
					uiWrapperTransform = targetTransform.GetComponentInChildren<TagClasses.ProgressUITransformWrapperObject>().GetComponent<RectTransform>();
					wrapperTransformBottom = uiWrapperTransform.offsetMin.y;
					wrapperTransformTop = UIOrientationSetter.ReferenceResolution.y + uiWrapperTransform.offsetMax.y;
					isUIReferenceResolutionValid = true;
				}
			}
			else
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Progress Bar Size", EditorStyles.boldLabel);
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.MinMaxSlider(ref wrapperTransformBottom, ref wrapperTransformTop, 0, UIOrientationSetter.ReferenceResolution.y);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Bottom", GUILayout.MaxWidth(50));
				wrapperTransformBottom = EditorGUILayout.FloatField(wrapperTransformBottom, GUILayout.MaxWidth(50));
				EditorGUILayout.LabelField(string.Format("⇤ {0} ⇥", (wrapperTransformTop - wrapperTransformBottom).ToString()), EditorStyles.centeredGreyMiniLabel, GUILayout.MinWidth(0));
				EditorGUILayout.LabelField("Top", GUILayout.MaxWidth(50));
				wrapperTransformTop = UIOrientationSetter.ReferenceResolution.y - EditorGUILayout.FloatField(UIOrientationSetter.ReferenceResolution.y - wrapperTransformTop, GUILayout.MaxWidth(50));
				EditorGUILayout.EndHorizontal();
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(uiWrapperTransform, "Progess bar size change");
					uiWrapperTransform.offsetMin = new Vector2(0, wrapperTransformBottom);
					Undo.RecordObject(uiWrapperTransform, "Progess bar size change");
					uiWrapperTransform.offsetMax = new Vector2(0, wrapperTransformTop - UIOrientationSetter.ReferenceResolution.y);
				}
			}

			// Background/foreground color.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Color Scheme", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Bottom and top color of progress fill.");
			EditorGUILayout.BeginHorizontal();
			Undo.RecordObject(fillBackgroundColorImage, "Color scheme change");
			fillBackgroundColorImage.color = EditorGUILayout.ColorField(fillBackgroundColorImage.color);
			Undo.RecordObject(fillForegroundColorImage, "Color scheme change");
			fillForegroundColorImage.color = EditorGUILayout.ColorField(fillForegroundColorImage.color);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.HelpBox("When the color does not seem to apply, clicking the color field once more to open color wheel window should solve the problem.", MessageType.None);

			// Goal icon sprite and animation.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Goal Icon", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("An image on goal point of progress bar.", EditorStyles.wordWrappedLabel);
			EditorGUI.indentLevel += 1;
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Idle Animation", EditorStyles.boldLabel);
			Undo.RecordObject(goalIconAnimationPlayer, "Goal icon animation clip change");
			goalIconAnimationPlayer.idleAnimation.animationClip = EditorGUILayout.ObjectField("Animation Clip", goalIconAnimationPlayer.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(goalIconAnimationPlayer, "Goal icon animation speed change");
			goalIconAnimationPlayer.idleAnimation.playSpeed = EditorGUILayout.FloatField("Play Speed", goalIconAnimationPlayer.idleAnimation.playSpeed);
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel -= 1;
			EditorGUILayout.EndVertical();
			Undo.RecordObject(goalIconImage, "Goal icon image change");
			goalIconImage.sprite = EditorGUILayout.ObjectField(goalIconImage.sprite, typeof(Sprite), false, GUILayout.MaxWidth(100f), GUILayout.MaxHeight(100f)) as Sprite;
			EditorGUILayout.EndHorizontal();

			// Indicator icon sprite and animation.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Indicator Icon", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("An image over indicator marker.", EditorStyles.wordWrappedLabel);
			EditorGUI.indentLevel += 1;
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Idle Animation", EditorStyles.boldLabel);
			Undo.RecordObject(indicatorIconAnimationPlayer, "Indicator icon animation clip change");
			indicatorIconAnimationPlayer.idleAnimation.animationClip = EditorGUILayout.ObjectField("Animation Clip", indicatorIconAnimationPlayer.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(indicatorIconAnimationPlayer, "Indicator icon animation speed change");
			indicatorIconAnimationPlayer.idleAnimation.playSpeed = EditorGUILayout.FloatField("Play Speed", indicatorIconAnimationPlayer.idleAnimation.playSpeed);
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel -= 1;
			EditorGUILayout.EndVertical();
			Undo.RecordObject(indicatorIconImage, "Indicator icon image change");
			indicatorIconImage.sprite = EditorGUILayout.ObjectField(indicatorIconImage.sprite, typeof(Sprite), false, GUILayout.MaxWidth(100f), GUILayout.MaxHeight(100f)) as Sprite;
			EditorGUILayout.EndHorizontal();

			// UI show/hide animation.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("UI Show Animation", EditorStyles.boldLabel);
			Undo.RecordObject(progressWrapperAnimationPlayer, "ProgressUI show animation clip change");
			progressWrapperAnimationPlayer.showAnimation.animationClip = EditorGUILayout.ObjectField("Animation Clip", progressWrapperAnimationPlayer.showAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(progressWrapperAnimationPlayer, "ProgressUI show animation speed change");
			progressWrapperAnimationPlayer.showAnimation.playSpeed = EditorGUILayout.FloatField("Play Speed", progressWrapperAnimationPlayer.showAnimation.playSpeed);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("UI Hide Animation", EditorStyles.boldLabel);
			Undo.RecordObject(progressWrapperAnimationPlayer, "ProgressUI hide animation clip change");
			progressWrapperAnimationPlayer.hideAnimation.animationClip = EditorGUILayout.ObjectField("Animation Clip", progressWrapperAnimationPlayer.hideAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(progressWrapperAnimationPlayer, "ProgressUI hide animation speed change");
			progressWrapperAnimationPlayer.hideAnimation.playSpeed = EditorGUILayout.FloatField("Play Speed", progressWrapperAnimationPlayer.hideAnimation.playSpeed);

			serializedObject.ApplyModifiedProperties();
		}
	}
}