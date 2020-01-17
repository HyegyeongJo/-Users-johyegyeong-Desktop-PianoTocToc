using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	[CustomEditor(typeof(ScoreUI))]
	public class ScoreUIEditor : Editor
	{
		Transform targetTransform;

		SerializedProperty shouldBeHiddenOnStart;

		Image scoreIconImage;
		Image[] decorationImages;

		Dictionary<string, List<Graphic>> colorSchemeSet;

		SerializedProperty scoreGainSfx;
		SerializedProperty scoreLoseSfx;
		SerializedProperty comboGainSfx;
		SerializedProperty comboFailSfx;

		ParticleSystem scoreGainVfx;
		ParticleSystem scoreLoseVfx;
		ParticleSystem comboStartVfx;
		ParticleSystem comboGainVfx;
		ParticleSystem comboFailVfx;

		protected static bool showAnimationField = false;
		ScoreUIWrapperAnimationPlayer[] scoreUIWrapperAnimationPlayers;
		ComboUIWrapperAnimationPlayer[] comboUIWrapperAnimationPlayers;
		ScorePointAnimationPlayer[] scorePointAnimations;
		ComboPointAnimationPlayer[] comboPointAnimations;

		void OnEnable()
		{
			targetTransform = ((ScoreUI) target).transform;
			shouldBeHiddenOnStart = serializedObject.FindProperty("shouldBeHiddenOnStart");

			scoreIconImage = targetTransform.GetComponentInChildren<TagClasses.ScoreUIIconObject>(true).GetComponent<Image>();
			var decorationImageObjects = targetTransform.GetComponentsInChildren<TagClasses.ScoreUIDecorationImageObject>(true);
			decorationImages = new Image[decorationImageObjects.Length];
			for (int i = 0; i < decorationImageObjects.Length; i++)
			{
				decorationImages[i] = decorationImageObjects[i].GetComponent<Image>();
			}

			var customazableColorObjects = targetTransform.GetComponentsInChildren<ScoreUICustomazableColorObject>(true);
			colorSchemeSet = new Dictionary<string, List<Graphic>>();
			for (int i = 0; i < customazableColorObjects.Length; i++)
			{
				try
				{
					colorSchemeSet[customazableColorObjects[i].colorSchemeName].Add(customazableColorObjects[i].GetComponent<Graphic>());
				}
				catch
				{
					colorSchemeSet[customazableColorObjects[i].colorSchemeName] = new List<Graphic>();
					colorSchemeSet[customazableColorObjects[i].colorSchemeName].Add(customazableColorObjects[i].GetComponent<Graphic>());
				}
			}

			((ScoreUI) target).scorePointText = targetTransform.GetComponentInChildren<TagClasses.ScoreUIScoreTextObject>(true).GetComponent<Text>();
			((ScoreUI) target).comboPointText = targetTransform.GetComponentInChildren<TagClasses.ScoreUIComboTextObject>(true).GetComponent<Text>();

			scoreUIWrapperAnimationPlayers = targetTransform.GetComponentsInChildren<ScoreUIWrapperAnimationPlayer>(true);
			comboUIWrapperAnimationPlayers = targetTransform.GetComponentsInChildren<ComboUIWrapperAnimationPlayer>(true);
			scorePointAnimations = targetTransform.GetComponentsInChildren<ScorePointAnimationPlayer>(true);
			comboPointAnimations = targetTransform.GetComponentsInChildren<ComboPointAnimationPlayer>(true);

			scoreGainSfx = serializedObject.FindProperty("scoreGainSfx");
			scoreLoseSfx = serializedObject.FindProperty("scoreLoseSfx");
			comboGainSfx = serializedObject.FindProperty("comboGainSfx");
			comboFailSfx = serializedObject.FindProperty("comboFailSfx");

			if (targetTransform.GetComponentInChildren<TagClasses.ScoreUIScoreGainVfxObject>() != null)
			{
				scoreGainVfx = targetTransform.GetComponentInChildren<TagClasses.ScoreUIScoreGainVfxObject>(true).GetComponent<ParticleSystem>();
				((ScoreUI) target).scoreGainVfx = scoreGainVfx;
			}
			if (targetTransform.GetComponentInChildren<TagClasses.ScoreUIScoreLoseVfxObject>() != null)
			{
				scoreLoseVfx = targetTransform.GetComponentInChildren<TagClasses.ScoreUIScoreLoseVfxObject>(true).GetComponent<ParticleSystem>();
				((ScoreUI) target).scoreLoseVfx = scoreLoseVfx;
			}
			if (targetTransform.GetComponentInChildren<TagClasses.ScoreUIComboStartVfxObject>() != null)
			{
				comboStartVfx = targetTransform.GetComponentInChildren<TagClasses.ScoreUIComboStartVfxObject>(true).GetComponent<ParticleSystem>();
				((ScoreUI) target).comboStartVfx = comboStartVfx;
			}
			if (targetTransform.GetComponentInChildren<TagClasses.ScoreUIComboGainVfxObject>() != null)
			{
				comboGainVfx = targetTransform.GetComponentInChildren<TagClasses.ScoreUIComboGainVfxObject>(true).GetComponent<ParticleSystem>();
				((ScoreUI) target).comboGainVfx = comboGainVfx;
			}
			if (targetTransform.GetComponentInChildren<TagClasses.ScoreUIComboLoseVfxObject>() != null)
			{
				comboFailVfx = targetTransform.GetComponentInChildren<TagClasses.ScoreUIComboLoseVfxObject>(true).GetComponent<ParticleSystem>();
				((ScoreUI) target).comboFailVfx = comboFailVfx;
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Hide on start toggle.
			EditorGUILayout.Space();
			shouldBeHiddenOnStart.boolValue = EditorGUILayout.Toggle("Hide on start", shouldBeHiddenOnStart.boolValue);

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Inspector values may not be shown as updated after performing undo/redo. Deselecting/reselecting the GameObject should solve the problem.", MessageType.Info);

			// Score icon sprite.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Score Icon", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("An image representing score point.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.EndVertical();
			Undo.RecordObject(scoreIconImage, "Goal icon image change");
			scoreIconImage.sprite = EditorGUILayout.ObjectField(scoreIconImage.sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80)) as Sprite;
			PrefabUtility.RecordPrefabInstancePropertyModifications(scoreIconImage);
			EditorGUILayout.EndHorizontal();

			// Decoration images.
			if (decorationImages.Length > 0)
			{
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField("Decoration Images", EditorStyles.boldLabel);
				EditorGUILayout.LabelField("Decorative images can be changed to fit theme.", EditorStyles.wordWrappedLabel);
				EditorGUILayout.EndVertical();
				for (int i = 0; i < decorationImages.Length; i++)
				{
					Undo.RecordObject(decorationImages[i], "Decoration image change");
					decorationImages[i].sprite = EditorGUILayout.ObjectField(decorationImages[i].sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80)) as Sprite;
					PrefabUtility.RecordPrefabInstancePropertyModifications(decorationImages[i]);
				}
				EditorGUILayout.EndHorizontal();
			}

			// Color scheme.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Color Scheme", EditorStyles.boldLabel);
			foreach (var colorScheme in colorSchemeSet)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(colorScheme.Key, EditorStyles.wordWrappedLabel);
				Undo.RecordObjects(colorScheme.Value.ToArray(), "Color scheme change");
				foreach (var graphic in colorScheme.Value)
				{
					graphic.color = EditorGUILayout.ColorField(graphic.color, GUILayout.MaxWidth(65f));
					PrefabUtility.RecordPrefabInstancePropertyModifications(graphic);
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.HelpBox("When the color does not seem to apply, clicking the color field once more to open color wheel window should solve the problem.", MessageType.None);

			// SFXs.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("SFXs", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(scoreGainSfx);
			EditorGUILayout.PropertyField(scoreLoseSfx);
			EditorGUILayout.PropertyField(comboGainSfx);
			EditorGUILayout.PropertyField(comboFailSfx);

			// VFXs.
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("VFXs", EditorStyles.boldLabel);
			if (!(EditorApplication.isPlaying || EditorApplication.isPaused) || ((ScoreUI) target).IsShown)
			{
				EditorGUILayout.HelpBox("Read only fields below; click it to select and then edit it if neccessary.", MessageType.None);
				EditorGUILayout.ObjectField("Score Gain Vfx", scoreGainVfx, typeof(ParticleSystem), true);
				EditorGUILayout.ObjectField("Score Lose Vfx", scoreLoseVfx, typeof(ParticleSystem), true);
				EditorGUILayout.ObjectField("Combo Start Vfx", comboStartVfx, typeof(ParticleSystem), true);
				EditorGUILayout.ObjectField("Combo Gain Vfx", comboGainVfx, typeof(ParticleSystem), true);
				EditorGUILayout.ObjectField("Combo Fail Vfx", comboFailVfx, typeof(ParticleSystem), true);
			}
			else
			{
				EditorGUILayout.HelpBox("VFXs only show when those GameObjects are active in the scene.", MessageType.Warning);
			}

			// Scoreboard animations.
			if (scoreUIWrapperAnimationPlayers.Length > 0)
			{
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Scoreboard Animations", EditorStyles.boldLabel);
				EditorGUILayout.LabelField("Show/Hide animation of scoreboard UI.\nPlays when Score.ShowUI() and Score.HideUI() call.", EditorStyles.wordWrappedLabel);
				EditorGUI.indentLevel += 1;
				for (int i = 0; i < scoreUIWrapperAnimationPlayers.Length; i++)
				{
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(scoreUIWrapperAnimationPlayers[i].name, EditorStyles.boldLabel);
					if (GUILayout.Button("Ping Object", EditorStyles.miniButtonRight))
					{
						EditorGUIUtility.PingObject(scoreUIWrapperAnimationPlayers[i].gameObject);
					}
					EditorGUILayout.EndHorizontal();
					Undo.RecordObject(scoreUIWrapperAnimationPlayers[i], "Scoreboard show animation clip change");
					scoreUIWrapperAnimationPlayers[i].showAnimation.animationClip = EditorGUILayout.ObjectField("Show Anim. Clip", scoreUIWrapperAnimationPlayers[i].showAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(scoreUIWrapperAnimationPlayers[i], "Scoreboard show animation speed change");
					scoreUIWrapperAnimationPlayers[i].showAnimation.playSpeed = EditorGUILayout.FloatField("Show Anim. Speed", scoreUIWrapperAnimationPlayers[i].showAnimation.playSpeed);
					Undo.RecordObject(scoreUIWrapperAnimationPlayers[i], "Scoreboard hide animation clip change");
					scoreUIWrapperAnimationPlayers[i].hideAnimation.animationClip = EditorGUILayout.ObjectField("Hide Anim. Clip", scoreUIWrapperAnimationPlayers[i].hideAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(scoreUIWrapperAnimationPlayers[i], "Scoreboard hide animation speed change");
					scoreUIWrapperAnimationPlayers[i].hideAnimation.playSpeed = EditorGUILayout.FloatField("Hide Anim. Speed", scoreUIWrapperAnimationPlayers[i].hideAnimation.playSpeed);
					PrefabUtility.RecordPrefabInstancePropertyModifications(scoreUIWrapperAnimationPlayers[i]);
				}
				EditorGUI.indentLevel -= 1;
			}

			// Combo pane animations.
			if (comboUIWrapperAnimationPlayers.Length > 0)
			{
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Combo Pane Animations", EditorStyles.boldLabel);
				EditorGUILayout.LabelField("Show/Hide animation of combo pane UI.\nPlays when combo counting starts and combo fails.", EditorStyles.wordWrappedLabel);
				EditorGUI.indentLevel += 1;
				for (int i = 0; i < comboUIWrapperAnimationPlayers.Length; i++)
				{
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(comboUIWrapperAnimationPlayers[i].name, EditorStyles.boldLabel);
					if (GUILayout.Button("Ping Object", EditorStyles.miniButtonRight))
					{
						EditorGUIUtility.PingObject(comboUIWrapperAnimationPlayers[i].gameObject);
					}
					EditorGUILayout.EndHorizontal();
					Undo.RecordObject(comboUIWrapperAnimationPlayers[i], "Combo pane show animation clip change");
					comboUIWrapperAnimationPlayers[i].showAnimation.animationClip = EditorGUILayout.ObjectField("Show Anim. Clip", scoreUIWrapperAnimationPlayers[i].showAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(comboUIWrapperAnimationPlayers[i], "Combo pane show animation speed change");
					comboUIWrapperAnimationPlayers[i].showAnimation.playSpeed = EditorGUILayout.FloatField("Show Anim. Speed", scoreUIWrapperAnimationPlayers[i].showAnimation.playSpeed);
					EditorGUILayout.Space();
					Undo.RecordObject(comboUIWrapperAnimationPlayers[i], "Combo pane hide animation clip change");
					comboUIWrapperAnimationPlayers[i].hideAnimation.animationClip = EditorGUILayout.ObjectField("Hide Anim. Clip", scoreUIWrapperAnimationPlayers[i].hideAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(comboUIWrapperAnimationPlayers[i], "Combo pane hide animation speed change");
					scoreUIWrapperAnimationPlayers[i].hideAnimation.playSpeed = EditorGUILayout.FloatField("Hide Anim. Speed", scoreUIWrapperAnimationPlayers[i].hideAnimation.playSpeed);
					PrefabUtility.RecordPrefabInstancePropertyModifications(comboUIWrapperAnimationPlayers[i]);
				}
				EditorGUI.indentLevel -= 1;
			}

			// Score gain/lose animations.
			if (scorePointAnimations.Length > 0)
			{
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Score Gain/Lose Animations", EditorStyles.boldLabel);
				EditorGUILayout.LabelField("Animations to play when gaining/losing score points.", EditorStyles.wordWrappedLabel);
				EditorGUI.indentLevel += 1;
				for (int i = 0; i < scorePointAnimations.Length; i++)
				{
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(scorePointAnimations[i].name, EditorStyles.boldLabel);
					if (GUILayout.Button("Ping Object", EditorStyles.miniButtonRight))
					{
						EditorGUIUtility.PingObject(scorePointAnimations[i].gameObject);
					}
					EditorGUILayout.EndHorizontal();
					Undo.RecordObject(scorePointAnimations[i], "Score idle animation clip change");
					scorePointAnimations[i].idleAnimation.animationClip = EditorGUILayout.ObjectField("Idle Anim. Clip", scorePointAnimations[i].idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(scorePointAnimations[i], "Score idle animation speed change");
					scorePointAnimations[i].idleAnimation.playSpeed = EditorGUILayout.FloatField("Idle Anim. Speed", scorePointAnimations[i].idleAnimation.playSpeed);
					EditorGUILayout.Space();
					Undo.RecordObject(scorePointAnimations[i], "Score gain animation clip change");
					scorePointAnimations[i].gainAnimation.animationClip = EditorGUILayout.ObjectField("Gain Anim. Clip", scorePointAnimations[i].gainAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(scorePointAnimations[i], "Score gain animation speed change");
					scorePointAnimations[i].gainAnimation.playSpeed = EditorGUILayout.FloatField("Gain Anim. Speed", scorePointAnimations[i].gainAnimation.playSpeed);
					EditorGUILayout.Space();
					Undo.RecordObject(scorePointAnimations[i], "Score lose animation clip change");
					scorePointAnimations[i].loseAnimation.animationClip = EditorGUILayout.ObjectField("Lose Anim. Clip", scorePointAnimations[i].loseAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(scorePointAnimations[i], "Score lose animation speed change");
					scorePointAnimations[i].loseAnimation.playSpeed = EditorGUILayout.FloatField("Lose Anim. Speed", scorePointAnimations[i].loseAnimation.playSpeed);
					PrefabUtility.RecordPrefabInstancePropertyModifications(scorePointAnimations[i]);
				}
				EditorGUI.indentLevel -= 1;
			}

			// Combo gain/fail animations.
			if (comboPointAnimations.Length > 0)
			{
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Combo Gain/Fail Animations", EditorStyles.boldLabel);
				EditorGUILayout.LabelField("Animations to play when gaining/failing combo.", EditorStyles.wordWrappedLabel);
				EditorGUI.indentLevel += 1;
				for (int i = 0; i < comboPointAnimations.Length; i++)
				{
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(comboPointAnimations[i].name, EditorStyles.boldLabel);
					if (GUILayout.Button("Ping Object", EditorStyles.miniButtonRight))
					{
						EditorGUIUtility.PingObject(comboPointAnimations[i].gameObject);
					}
					EditorGUILayout.EndHorizontal();
					Undo.RecordObject(comboPointAnimations[i], "Combo idle animation clip change");
					comboPointAnimations[i].idleAnimation.animationClip = EditorGUILayout.ObjectField("Idle Anim. Clip", comboPointAnimations[i].idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(comboPointAnimations[i], "Combo idle animation speed change");
					comboPointAnimations[i].idleAnimation.playSpeed = EditorGUILayout.FloatField("Idle Anim. Speed", comboPointAnimations[i].idleAnimation.playSpeed);
					EditorGUILayout.Space();
					Undo.RecordObject(comboPointAnimations[i], "Combo gain animation clip change");
					comboPointAnimations[i].gainAnimation.animationClip = EditorGUILayout.ObjectField("Gain Anim. Clip", comboPointAnimations[i].gainAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(comboPointAnimations[i], "Combo gain animation speed change");
					comboPointAnimations[i].gainAnimation.playSpeed = EditorGUILayout.FloatField("Gain Anim. Speed", comboPointAnimations[i].gainAnimation.playSpeed);
					EditorGUILayout.Space();
					Undo.RecordObject(comboPointAnimations[i], "Combo fail animation clip change");
					comboPointAnimations[i].failAnimation.animationClip = EditorGUILayout.ObjectField("Fail Anim. Clip", comboPointAnimations[i].failAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
					Undo.RecordObject(comboPointAnimations[i], "Combo fail animation speed change");
					comboPointAnimations[i].failAnimation.playSpeed = EditorGUILayout.FloatField("Fail Anim. Speed", comboPointAnimations[i].failAnimation.playSpeed);
					PrefabUtility.RecordPrefabInstancePropertyModifications(comboPointAnimations[i]);
				}
				EditorGUI.indentLevel -= 1;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}