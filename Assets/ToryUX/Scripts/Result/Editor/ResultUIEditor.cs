using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	[CustomEditor(typeof(ResultUI))]
	public class ResultUIEditor : Editor
	{
		Transform targetTransform;

		SerializedProperty shouldBeHiddenOnStart;

        Image scoreIconImage;
        IdleBasedAnimationPlayer scoreIconAnimationPlayer;

        Image heroImageOnFail;
        Image heroImageOnSuccess;
        IdleBasedAnimationPlayer heroAnimationPlayerOnFail;
        IdleBasedAnimationPlayer heroAnimationPlayerOnSuccess;

        SerializedProperty sfxOnSuccess;
        SerializedProperty sfxOnFail;

        ParticleSystem vfxOnSuccess;
        ParticleSystem vfxOnFail;

        Dictionary<string, List<Graphic>> colorSchemeSet;

        Image backdropImage;
        ShowAndHideAnimationPlayer backdropAnimationPlayer;

		SerializedProperty toggleBlurBackground;
        SerializedProperty toggleVignettes;
        SerializedProperty toggleBloomFlash;

        SerializedProperty bestScoreSfx;
        SerializedProperty countDownSfx;

		void OnEnable()
		{
			targetTransform = ((ResultUI)target).transform;
			shouldBeHiddenOnStart = serializedObject.FindProperty("shouldBeHiddenOnStart");

            scoreIconImage = targetTransform.GetComponentInChildren<TagClasses.ResultUIIconImageObject>(true).GetComponent<Image>();
            scoreIconAnimationPlayer = scoreIconImage.GetComponent<IdleBasedAnimationPlayer>();

            heroImageOnFail = targetTransform.GetComponentInChildren<TagClasses.ResultUIHeroImageOnFailObject>(true).GetComponent<Image>();
            heroImageOnSuccess = targetTransform.GetComponentInChildren<TagClasses.ResultUIHeroImageOnSuccessObject>(true).GetComponent<Image>();
            heroAnimationPlayerOnFail = heroImageOnFail.GetComponent<IdleBasedAnimationPlayer>();
            heroAnimationPlayerOnSuccess = heroImageOnSuccess.GetComponent<IdleBasedAnimationPlayer>();

            sfxOnSuccess = serializedObject.FindProperty("sfxOnSuccess");
            sfxOnFail = serializedObject.FindProperty("sfxOnFail");

            if (targetTransform.GetComponentInChildren<TagClasses.ResultUIFailVfxObject>(true) != null)
            {
                vfxOnFail = targetTransform.GetComponentInChildren<TagClasses.ResultUIFailVfxObject>(true).gameObject.GetComponent<ParticleSystem>();
            }
            if (targetTransform.GetComponentInChildren<TagClasses.ResultUISuccessVfxObject>(true) != null)
            {
                vfxOnSuccess = targetTransform.GetComponentInChildren<TagClasses.ResultUISuccessVfxObject>(true).gameObject.GetComponent<ParticleSystem>();
            }

            var customazableColorObjects = targetTransform.GetComponentsInChildren<ResultUICustomazableColorObject>(true);
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

            backdropImage = targetTransform.GetComponentInChildren<TagClasses.ResultUIBackdropObject>(true).GetComponent<Image>();
            backdropAnimationPlayer = backdropImage.GetComponent<ShowAndHideAnimationPlayer>();

			toggleBlurBackground = serializedObject.FindProperty("toggleBlurBackground");
            toggleVignettes = serializedObject.FindProperty("toggleVignettes");
            toggleBloomFlash = serializedObject.FindProperty("toggleBloomFlash");

            bestScoreSfx = serializedObject.FindProperty("bestScoreSfx");
            countDownSfx = serializedObject.FindProperty("countDownSfx");
        }

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Hide on start toggle.
			EditorGUILayout.Space();
			shouldBeHiddenOnStart.boolValue = EditorGUILayout.Toggle("Hide on start", shouldBeHiddenOnStart.boolValue);

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Inspector values may not be shown as updated after performing undo/redo. Deselecting/reselecting the GameObject should solve the problem.", MessageType.Info);

            // Score icon image and animation.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Score Icon", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            Undo.RecordObject(scoreIconImage, "Score icon sprite change");
            scoreIconImage.sprite = EditorGUILayout.ObjectField(scoreIconImage.sprite, typeof(Sprite), false, GUILayout.Width(80f), GUILayout.Height(80f)) as Sprite;
			PrefabUtility.RecordPrefabInstancePropertyModifications(scoreIconImage);
			EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Animation Clip/Speed");
            Undo.RecordObject(scoreIconAnimationPlayer, "Score icon animation clip change");
            scoreIconAnimationPlayer.idleAnimation.animationClip = EditorGUILayout.ObjectField(scoreIconAnimationPlayer.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
            Undo.RecordObject(scoreIconAnimationPlayer, "Score icon animation speed change");
            scoreIconAnimationPlayer.idleAnimation.playSpeed = EditorGUILayout.FloatField(scoreIconAnimationPlayer.idleAnimation.playSpeed);
			PrefabUtility.RecordPrefabInstancePropertyModifications(scoreIconAnimationPlayer);
			EditorGUILayout.Space();
            if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
            {
                EditorGUIUtility.PingObject(scoreIconImage.gameObject);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            // Success result.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Success Result", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.LabelField("Hero Image");
            EditorGUILayout.BeginHorizontal();
            Undo.RecordObject(heroImageOnSuccess, "Hero image on success change");
            heroImageOnSuccess.sprite = EditorGUILayout.ObjectField(heroImageOnSuccess.sprite, typeof(Sprite), false, GUILayout.Width(80f), GUILayout.Height(80f)) as Sprite;
			PrefabUtility.RecordPrefabInstancePropertyModifications(heroImageOnSuccess);
			EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Animation Clip/Speed");
            Undo.RecordObject(heroAnimationPlayerOnSuccess, "Hero image animation clip on success change");
            heroAnimationPlayerOnSuccess.idleAnimation.animationClip = EditorGUILayout.ObjectField(heroAnimationPlayerOnSuccess.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
            Undo.RecordObject(heroAnimationPlayerOnSuccess, "Hero image animation speed on success change");
            heroAnimationPlayerOnSuccess.idleAnimation.playSpeed = EditorGUILayout.FloatField(heroAnimationPlayerOnSuccess.idleAnimation.playSpeed);
			PrefabUtility.RecordPrefabInstancePropertyModifications(heroAnimationPlayerOnSuccess);
			EditorGUILayout.Space();
            if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
            {
                EditorGUIUtility.PingObject(heroImageOnSuccess.gameObject);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sfxOnSuccess);

            EditorGUILayout.Space();
            EditorGUILayout.ObjectField("Vfx", vfxOnSuccess, typeof(ParticleSystem), true);
            EditorGUILayout.HelpBox("Vfx field is read-only. Click it to select/edit the particle system.", MessageType.None);
            EditorGUI.indentLevel -= 1;

            // Fail result.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Fail Result", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.LabelField("Hero Image");
            EditorGUILayout.BeginHorizontal();
            Undo.RecordObject(heroImageOnFail, "Hero image on fail change");
            heroImageOnFail.sprite = EditorGUILayout.ObjectField(heroImageOnFail.sprite, typeof(Sprite), false, GUILayout.Width(80f), GUILayout.Height(80f)) as Sprite;
			PrefabUtility.RecordPrefabInstancePropertyModifications(heroImageOnFail);
			EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Animation Clip/Speed");
            Undo.RecordObject(heroAnimationPlayerOnFail, "Hero image animation clip on fail change");
            heroAnimationPlayerOnFail.idleAnimation.animationClip = EditorGUILayout.ObjectField(heroAnimationPlayerOnFail.idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
            Undo.RecordObject(heroAnimationPlayerOnFail, "Hero image animation speed on fail change");
            heroAnimationPlayerOnFail.idleAnimation.playSpeed = EditorGUILayout.FloatField(heroAnimationPlayerOnFail.idleAnimation.playSpeed);
			PrefabUtility.RecordPrefabInstancePropertyModifications(heroAnimationPlayerOnFail);
			EditorGUILayout.Space();
            if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
            {
                EditorGUIUtility.PingObject(heroImageOnFail.gameObject);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sfxOnFail);

            EditorGUILayout.Space();
            EditorGUILayout.ObjectField("Vfx", vfxOnFail, typeof(ParticleSystem), true);
            EditorGUILayout.HelpBox("Vfx field is read-only. Click it to select/edit the particle system.", MessageType.None);
            EditorGUI.indentLevel -= 1;

            // Color Scheme.
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

            // Backdrop image and animation.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Backdrop Image And Show Animation", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            Undo.RecordObject(backdropImage, "Backdrop image change");
            backdropImage.sprite = EditorGUILayout.ObjectField(backdropImage.sprite, typeof(Sprite), false, GUILayout.Width(80f), GUILayout.Height(80f)) as Sprite;
			PrefabUtility.RecordPrefabInstancePropertyModifications(backdropImage);
			EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Animation Clip/Speed");
            Undo.RecordObject(backdropAnimationPlayer, "Backdrop show animation clip change");
            backdropAnimationPlayer.showAnimation.animationClip = EditorGUILayout.ObjectField(backdropAnimationPlayer.showAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
            Undo.RecordObject(backdropAnimationPlayer, "Backdrop show animation speed change");
            backdropAnimationPlayer.showAnimation.playSpeed = EditorGUILayout.FloatField(backdropAnimationPlayer.showAnimation.playSpeed);
			PrefabUtility.RecordPrefabInstancePropertyModifications(backdropAnimationPlayer);
			EditorGUILayout.Space();
            if (GUILayout.Button("Ping Object", EditorStyles.miniButton))
            {
                EditorGUIUtility.PingObject(backdropImage.gameObject);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

			// Toggle effects.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Screen Effects", EditorStyles.boldLabel);
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

            // Best score/countdown sfx.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Best Score/Countdown Sfx", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(bestScoreSfx);
            EditorGUILayout.PropertyField(countDownSfx);

			EditorGUILayout.Space();
			serializedObject.ApplyModifiedProperties();
		}
	}
}