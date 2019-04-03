using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	[CustomEditor(typeof(TitleUI))]
	public class TitleUIEditor : Editor
	{
		Transform targetTransform;

		SerializedProperty shouldBeHiddenOnStart;

		Image heroImage;
		IdleBasedAnimationPlayer heroImageAnimationPlayer;

		Image wordmarkImage;
		IdleBasedAnimationPlayer wordmarkAnimationPlayer;

		Image instructionIconImage;
		Image instructionWordsImage;
		IdleBasedAnimationPlayer instructionIconAnimationPlayer;
		IdleBasedAnimationPlayer instructionWordsAnimationPlayer;

		Image funtoryLogoImage;
		IdleBasedAnimationPlayer funtoryLogoAnimationPlayer;

		SerializedProperty gameStartSfx;
		SerializedProperty toggleBlurBackground;
		SerializedProperty toggleVignettes;
		SerializedProperty toggleBloomFlash;

		void OnEnable()
		{
			targetTransform = ((TitleUI) target).transform;
			shouldBeHiddenOnStart = serializedObject.FindProperty("shouldBeHiddenOnStart");

			heroImage = targetTransform.GetComponentInChildren<TagClasses.TitleUIHeroImageObject>(true).GetComponent<Image>();
			heroImageAnimationPlayer = heroImage.GetComponent<IdleBasedAnimationPlayer>();

			wordmarkImage = targetTransform.GetComponentInChildren<TagClasses.TitleUIWordmarkImageObject>(true).GetComponent<Image>();
			wordmarkAnimationPlayer = wordmarkImage.transform.GetComponent<IdleBasedAnimationPlayer>();

			instructionIconImage = targetTransform.GetComponentInChildren<TagClasses.TitleUIInstructionIconObject>(true).GetComponent<Image>();
			instructionWordsImage = targetTransform.GetComponentInChildren<TagClasses.TitleUIInstructionWordsObject>(true).GetComponent<Image>();
			instructionIconAnimationPlayer = instructionIconImage.GetComponent<IdleBasedAnimationPlayer>();
			instructionWordsAnimationPlayer = instructionWordsImage.GetComponent<IdleBasedAnimationPlayer>();

			funtoryLogoImage = targetTransform.GetComponentInChildren<TagClasses.TitleUIFuntoryLogoObject>(true).GetComponent<Image>();
			funtoryLogoAnimationPlayer = funtoryLogoImage.GetComponent<IdleBasedAnimationPlayer>();

			gameStartSfx = serializedObject.FindProperty("gameStartSfx");
			toggleBlurBackground = serializedObject.FindProperty("toggleBlurBackground");
			toggleVignettes = serializedObject.FindProperty("toggleVignettes");
			toggleBloomFlash = serializedObject.FindProperty("toggleBloomFlash");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Hide on start toggle.
			EditorGUILayout.Space();
			shouldBeHiddenOnStart.boolValue = EditorGUILayout.Toggle("Hide on start", shouldBeHiddenOnStart.boolValue);

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Inspector values may not be shown as updated after performing undo/redo. Deselecting/reselecting the GameObject should solve the problem.", MessageType.Info);

			// Hero image.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Hero Image", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			Undo.RecordObject(heroImage, "Hero image change");
			heroImage.sprite = EditorGUILayout.ObjectField(heroImage.sprite, typeof(Sprite), false, GUILayout.Width(100f), GUILayout.Height(70f)) as Sprite;
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Animation Clip/Speed");
			Undo.RecordObject(heroImageAnimationPlayer, "Hero image animation clip change");
			heroImageAnimationPlayer.idleAnimation.animationClip = EditorGUILayout.ObjectField(heroImageAnimationPlayer.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(heroImageAnimationPlayer, "Hero image animation speed change");
			heroImageAnimationPlayer.idleAnimation.playSpeed = EditorGUILayout.FloatField(heroImageAnimationPlayer.idleAnimation.playSpeed);
			EditorGUILayout.Space();
			if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
			{
				EditorGUIUtility.PingObject(heroImage.gameObject);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			// Wordmark.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Wordmark", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			Undo.RecordObject(wordmarkImage, "Wordmark image change");
			wordmarkImage.sprite = EditorGUILayout.ObjectField(wordmarkImage.sprite, typeof(Sprite), false, GUILayout.Width(100f), GUILayout.Height(70f)) as Sprite;
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Animation Clip/Speed");
			Undo.RecordObject(wordmarkAnimationPlayer, "Wordmark animation clip change");
			wordmarkAnimationPlayer.idleAnimation.animationClip = EditorGUILayout.ObjectField(wordmarkAnimationPlayer.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(heroImageAnimationPlayer, "Wordmark animation speed change");
			wordmarkAnimationPlayer.idleAnimation.playSpeed = EditorGUILayout.FloatField(wordmarkAnimationPlayer.idleAnimation.playSpeed);
			EditorGUILayout.Space();
			if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
			{
				EditorGUIUtility.PingObject(wordmarkImage.gameObject);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			// Instructions.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Instruction Images", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			Undo.RecordObject(instructionIconImage, "Instruction icon image change");
			instructionIconImage.sprite = EditorGUILayout.ObjectField(instructionIconImage.sprite, typeof(Sprite), false, GUILayout.Width(100f), GUILayout.Height(70f)) as Sprite;
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Animation Clip/Speed");
			Undo.RecordObject(instructionIconAnimationPlayer, "Instruction icon animation clip change");
			instructionIconAnimationPlayer.idleAnimation.animationClip = EditorGUILayout.ObjectField(instructionIconAnimationPlayer.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(instructionIconAnimationPlayer, "Instruction icon animation speed change");
			instructionIconAnimationPlayer.idleAnimation.playSpeed = EditorGUILayout.FloatField(instructionIconAnimationPlayer.idleAnimation.playSpeed);
			EditorGUILayout.Space();
			if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
			{
				EditorGUIUtility.PingObject(instructionIconImage.gameObject);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			Undo.RecordObject(instructionWordsImage, "Instruction words image change");
			instructionWordsImage.sprite = EditorGUILayout.ObjectField(instructionWordsImage.sprite, typeof(Sprite), false, GUILayout.Width(100f), GUILayout.Height(70f)) as Sprite;
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Animation Clip/Speed");
			Undo.RecordObject(instructionWordsAnimationPlayer, "Instruction words animation clip change");
			instructionWordsAnimationPlayer.idleAnimation.animationClip = EditorGUILayout.ObjectField(instructionWordsAnimationPlayer.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(instructionWordsAnimationPlayer, "Instruction words animation speed change");
			instructionWordsAnimationPlayer.idleAnimation.playSpeed = EditorGUILayout.FloatField(instructionWordsAnimationPlayer.idleAnimation.playSpeed);
			EditorGUILayout.Space();
			if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
			{
				EditorGUIUtility.PingObject(instructionWordsImage.gameObject);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			// Funtory logo.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Funtory Icon", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			Undo.RecordObject(funtoryLogoImage, "Funtory icon sprite change");
			funtoryLogoImage.sprite = EditorGUILayout.ObjectField(funtoryLogoImage.sprite, typeof(Sprite), false, GUILayout.Width(80f), GUILayout.Height(80f)) as Sprite;
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Animation Clip/Speed");
			Undo.RecordObject(funtoryLogoAnimationPlayer, "Funtory icon animation clip change");
			funtoryLogoAnimationPlayer.idleAnimation.animationClip = EditorGUILayout.ObjectField(funtoryLogoAnimationPlayer.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
			Undo.RecordObject(funtoryLogoAnimationPlayer, "Funtory icon animation speed change");
			funtoryLogoAnimationPlayer.idleAnimation.playSpeed = EditorGUILayout.FloatField(funtoryLogoAnimationPlayer.idleAnimation.playSpeed);
			EditorGUILayout.Space();
			if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
			{
				EditorGUIUtility.PingObject(funtoryLogoAnimationPlayer.gameObject);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			// Effects.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Game Start Effects", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(gameStartSfx, new GUIContent("Sound Effects"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(toggleBlurBackground);
			EditorGUILayout.PropertyField(toggleVignettes);
			EditorGUILayout.PropertyField(toggleBloomFlash);
			if (GUILayout.Button("Select CameraEffect Object", GUILayout.ExpandWidth(true)))
			{
				try
				{
					Selection.activeGameObject = GameObject.FindObjectOfType<CameraEffects>().gameObject;
				}
				catch
				{
					Debug.LogError("CameraEffect component cannot be found in the scene.");
				}
			}
			EditorGUILayout.HelpBox("Drag and drop your Post Processing Profile to CameraEffect component if you have one. Otherwise, leave it null.", MessageType.None);

			EditorGUILayout.Space();
			serializedObject.ApplyModifiedProperties();
		}
	}
}