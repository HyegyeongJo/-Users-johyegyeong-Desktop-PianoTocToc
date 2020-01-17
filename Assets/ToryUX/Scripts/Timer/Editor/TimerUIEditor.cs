using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [CustomEditor(typeof(TimerUI))]
    public class TimerUIEditor : Editor
    {
        Transform targetTransform;

        SerializedProperty shouldBeHiddenOnStart;

        Image timerIconImage;
        Image[] decorationImages;

        Dictionary<string, List<Graphic>> colorSchemeSet;

        SerializedProperty mode;
        SerializedProperty countdownStartTime;
        SerializedProperty showMilliseconds;
        SerializedProperty millisecondsTextScale;

        SerializedProperty lapCountPostfixScale;

        SerializedProperty timerTickSfx;
        SerializedProperty timerTockSfx;
        SerializedProperty timerAlertSfx;

        ParticleSystem timerTickVfx;
        ParticleSystem timerTockVfx;
        ParticleSystem timerAlertVfx;

        protected static bool showAnimationField = false;
        TimerUIWrapperAnimationPlayer[] timerUIWrapperAnimationPlayers;
        TimerAnimationPlayer[] timerAnimations;

        void OnEnable()
        {
            targetTransform = ((TimerUI) target).transform;
            shouldBeHiddenOnStart = serializedObject.FindProperty("shouldBeHiddenOnStart");

            mode = serializedObject.FindProperty("mode");
            countdownStartTime = serializedObject.FindProperty("countdownStartTime");
            showMilliseconds = serializedObject.FindProperty("showMilliseconds");
            millisecondsTextScale = serializedObject.FindProperty("millisecondsTextScale");

            lapCountPostfixScale = serializedObject.FindProperty("lapCountPostfixScale");

            /*
            timerIconImage = targetTransform.GetComponentInChildren<TagClasses.UIIconObject>(true).GetComponent<Image>();
            var decorationImageObjects = targetTransform.GetComponentsInChildren<TagClasses.UIDecorationImageObject>(true);
            decorationImages = new Image[decorationImageObjects.Length];
            for (int i = 0; i < decorationImageObjects.Length; i++)
            {
                decorationImages[i] = decorationImageObjects[i].GetComponent<Image>();
            }
            */

            var customazableColorObjects = targetTransform.GetComponentsInChildren<TagClasses.UICustomazableColorObject>(true);
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

            ((TimerUI) target).timerText = targetTransform.GetComponentInChildren<TagClasses.TimerUITimeTextObject>(true).GetComponent<Text>();

            ((TimerUI) target).containingLapCountUI = (targetTransform.GetComponentInChildren<TagClasses.TimerUILapTextObject>(true) != null);
            if (((TimerUI) target).containingLapCountUI)
            {
                ((TimerUI) target).lapText = targetTransform.GetComponentInChildren<TagClasses.TimerUILapTextObject>(true).GetComponent<Text>();
            }

            timerUIWrapperAnimationPlayers = targetTransform.GetComponentsInChildren<TimerUIWrapperAnimationPlayer>(true);
            timerAnimations = targetTransform.GetComponentsInChildren<TimerAnimationPlayer>(true);

            timerTickSfx = serializedObject.FindProperty("timerTickSfx");
            timerTockSfx = serializedObject.FindProperty("timerTockSfx");
            timerAlertSfx = serializedObject.FindProperty("timerAlertSfx");

            if (targetTransform.GetComponentInChildren<TagClasses.TimerUITimerTickVfxObject>() != null)
            {
                timerTickVfx = targetTransform.GetComponentInChildren<TagClasses.TimerUITimerTickVfxObject>(true).GetComponent<ParticleSystem>();
                ((TimerUI) target).timerTickVfx = timerTickVfx;
            }
            if (targetTransform.GetComponentInChildren<TagClasses.TimerUITimerTockVfxObject>() != null)
            {
                timerTockVfx = targetTransform.GetComponentInChildren<TagClasses.TimerUITimerTockVfxObject>(true).GetComponent<ParticleSystem>();
                ((TimerUI) target).timerTockVfx = timerTockVfx;
            }
            if (targetTransform.GetComponentInChildren<TagClasses.TimerUITimerAlertVfxObject>() != null)
            {
                timerAlertVfx = targetTransform.GetComponentInChildren<TagClasses.TimerUITimerAlertVfxObject>(true).GetComponent<ParticleSystem>();
                ((TimerUI) target).timerAlertVfx = timerAlertVfx;
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

            // Timer setting.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Timer Setting", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(mode);
            if (((TimerUI) target).mode == Timer.TimerMode.Countdown)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(countdownStartTime);
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.PropertyField(showMilliseconds);
            if (showMilliseconds.boolValue)
            {
                EditorGUILayout.PropertyField(millisecondsTextScale);
            }
            ((TimerUI) target).UpdateTimer();

            // Lap setting.
            if (((TimerUI) target).containingLapCountUI)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Lap Count Setting", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(lapCountPostfixScale);
                ((TimerUI) target).UpdateLapCount();
            }

            /*
            // Timer icon sprite.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Timer Icon", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("An image representing timer or character.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.EndVertical();
            Undo.RecordObject(timerIconImage, "Goal icon image change");
            timerIconImage.sprite = EditorGUILayout.ObjectField(timerIconImage.sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80)) as Sprite;
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
                }
                EditorGUILayout.EndHorizontal();
            }
            */

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

            // Lap count text color scheme.
            if (((TimerUI) target).containingLapCountUI)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Lap Count Text", EditorStyles.wordWrappedLabel);
                Undo.RecordObject(target, "labCountColor change");
                ((TimerUI) target).lapCountColor = EditorGUILayout.ColorField(((TimerUI) target).lapCountColor, GUILayout.MaxWidth(65f));
                Undo.RecordObject(target, "labCountPostfixColor change");
                ((TimerUI) target).lapCountPostfixColor = EditorGUILayout.ColorField(((TimerUI) target).lapCountPostfixColor, GUILayout.MaxWidth(65f));
				PrefabUtility.RecordPrefabInstancePropertyModifications(target);
				EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.HelpBox("When the color does not seem to apply, clicking the color field once more to open color wheel window should solve the problem.", MessageType.None);

            // SFXs.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("SFXs", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(timerTickSfx);
            EditorGUILayout.PropertyField(timerTockSfx);
            EditorGUILayout.PropertyField(timerAlertSfx);

            // VFXs.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("VFXs", EditorStyles.boldLabel);
            if (!(EditorApplication.isPlaying || EditorApplication.isPaused) || ((TimerUI) target).IsShown)
            {
                EditorGUILayout.HelpBox("Read only fields below; click it to select and then edit it if neccessary.", MessageType.None);
                EditorGUILayout.ObjectField("Timer Tick Vfx", timerTickVfx, typeof(ParticleSystem), true);
                EditorGUILayout.ObjectField("Timer Tock Vfx", timerTockVfx, typeof(ParticleSystem), true);
                EditorGUILayout.ObjectField("Timer Alert Vfx", timerAlertVfx, typeof(ParticleSystem), true);
            }
            else
            {
                EditorGUILayout.HelpBox("VFXs only show when those GameObjects are active in the scene.", MessageType.Warning);
            }

            // Timer Board animations.
            if (timerUIWrapperAnimationPlayers.Length > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Timer Board Animations", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Show/Hide animation of timer UI.\nPlays when Timer.ShowUI() and Timer.HideUI() call.", EditorStyles.wordWrappedLabel);
                EditorGUI.indentLevel += 1;
                for (int i = 0; i < timerUIWrapperAnimationPlayers.Length; i++)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(timerUIWrapperAnimationPlayers[i].name, EditorStyles.boldLabel);
                    if (GUILayout.Button("Ping Object", EditorStyles.miniButtonRight))
                    {
                        EditorGUIUtility.PingObject(timerUIWrapperAnimationPlayers[i].gameObject);
                    }
                    EditorGUILayout.EndHorizontal();
                    Undo.RecordObject(timerUIWrapperAnimationPlayers[i], "Timer show animation clip change");
                    timerUIWrapperAnimationPlayers[i].showAnimation.animationClip = EditorGUILayout.ObjectField("Show Anim. Clip", timerUIWrapperAnimationPlayers[i].showAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(timerUIWrapperAnimationPlayers[i], "Timer show animation speed change");
                    timerUIWrapperAnimationPlayers[i].showAnimation.playSpeed = EditorGUILayout.FloatField("Show Anim. Speed", timerUIWrapperAnimationPlayers[i].showAnimation.playSpeed);
                    Undo.RecordObject(timerUIWrapperAnimationPlayers[i], "Timer hide animation clip change");
                    timerUIWrapperAnimationPlayers[i].hideAnimation.animationClip = EditorGUILayout.ObjectField("Hide Anim. Clip", timerUIWrapperAnimationPlayers[i].hideAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(timerUIWrapperAnimationPlayers[i], "Timer hide animation speed change");
                    timerUIWrapperAnimationPlayers[i].hideAnimation.playSpeed = EditorGUILayout.FloatField("Hide Anim. Speed", timerUIWrapperAnimationPlayers[i].hideAnimation.playSpeed);
					PrefabUtility.RecordPrefabInstancePropertyModifications(timerUIWrapperAnimationPlayers[i]);
				}
				EditorGUI.indentLevel -= 1;
            }

            // Timer tick/tock/alert animations.
            if (timerAnimations.Length > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Timer tick/tock/alert Animations", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Animations to play when timer tick, tock or alerts.", EditorStyles.wordWrappedLabel);
                EditorGUI.indentLevel += 1;
                for (int i = 0; i < timerAnimations.Length; i++)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(timerAnimations[i].name, EditorStyles.boldLabel);
                    if (GUILayout.Button("Ping Object", EditorStyles.miniButtonRight))
                    {
                        EditorGUIUtility.PingObject(timerAnimations[i].gameObject);
                    }
                    EditorGUILayout.EndHorizontal();
                    Undo.RecordObject(timerAnimations[i], "Timer idle animation clip change");
                    timerAnimations[i].idleAnimation.animationClip = EditorGUILayout.ObjectField("Idle Anim. Clip", timerAnimations[i].idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(timerAnimations[i], "Timer idle animation speed change");
                    timerAnimations[i].idleAnimation.playSpeed = EditorGUILayout.FloatField("Idle Anim. Speed", timerAnimations[i].idleAnimation.playSpeed);
                    EditorGUILayout.Space();
                    Undo.RecordObject(timerAnimations[i], "Timer tick animation clip change");
                    timerAnimations[i].tickAnimation.animationClip = EditorGUILayout.ObjectField("Tick Anim. Clip", timerAnimations[i].tickAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(timerAnimations[i], "Timer tick animation speed change");
                    timerAnimations[i].tickAnimation.playSpeed = EditorGUILayout.FloatField("Tick Anim. Speed", timerAnimations[i].tickAnimation.playSpeed);
                    EditorGUILayout.Space();
                    Undo.RecordObject(timerAnimations[i], "Timer tock animation clip change");
                    timerAnimations[i].tockAnimation.animationClip = EditorGUILayout.ObjectField("Tock Anim. Clip", timerAnimations[i].tockAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(timerAnimations[i], "Timer tock animation speed change");
                    timerAnimations[i].tockAnimation.playSpeed = EditorGUILayout.FloatField("Tock Anim. Speed", timerAnimations[i].tockAnimation.playSpeed);
                    EditorGUILayout.Space();
                    Undo.RecordObject(timerAnimations[i], "Timer alert animation clip change");
                    timerAnimations[i].alertAnimation.animationClip = EditorGUILayout.ObjectField("Alert Anim. Clip", timerAnimations[i].alertAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(timerAnimations[i], "Timer alert animation speed change");
                    timerAnimations[i].alertAnimation.playSpeed = EditorGUILayout.FloatField("Alert Anim. Speed", timerAnimations[i].alertAnimation.playSpeed);
					PrefabUtility.RecordPrefabInstancePropertyModifications(timerAnimations[i]);
				}
				EditorGUI.indentLevel -= 1;
            }

            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}