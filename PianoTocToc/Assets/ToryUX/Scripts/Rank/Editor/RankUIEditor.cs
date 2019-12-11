using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [CustomEditor(typeof(RankUI))]
    public class RankUIEditor : Editor
    {
        Transform targetTransform;

        SerializedProperty shouldBeHiddenOnStart;

        #pragma warning disable 0414
        Image rankIconImage;
        Image[] decorationImages;

        Dictionary<string, List<Graphic>> colorSchemeSet;

        SerializedProperty textPostfixScale;
        SerializedProperty useSeparatedRankingText;

        SerializedProperty rankingRiseSfx;
        SerializedProperty rankingFallSfx;

        ParticleSystem rankingRiseVfx;
        ParticleSystem rankingFallVfx;

        protected static bool showAnimationField = false;
        RankUIWrapperAnimationPlayer[] rankUIWrapperAnimationPlayers;
        RankAnimationPlayer[] rankingAnimations;

        void OnEnable()
        {
            targetTransform = ((RankUI) target).transform;
            shouldBeHiddenOnStart = serializedObject.FindProperty("shouldBeHiddenOnStart");

            rankIconImage = targetTransform.GetComponentInChildren<TagClasses.UIIconObject>(true).GetComponent<Image>();
            var decorationImageObjects = targetTransform.GetComponentsInChildren<TagClasses.UIDecorationImageObject>(true);
            decorationImages = new Image[decorationImageObjects.Length];
            for (int i = 0; i < decorationImageObjects.Length; i++)
            {
                decorationImages[i] = decorationImageObjects[i].GetComponent<Image>();
            }

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

            ((RankUI) target).rankingText = targetTransform.GetComponentInChildren<TagClasses.RankUIRankingTextObject>(true).GetComponent<Text>();
            ((RankUI) target).rankingPostfixText = targetTransform.GetComponentInChildren<TagClasses.RankUIRankingPostfixTextObject>(true).GetComponent<Text>();

            textPostfixScale = serializedObject.FindProperty("textPostfixScale");
            useSeparatedRankingText = serializedObject.FindProperty("useSeparatedRankingText");

            rankUIWrapperAnimationPlayers = targetTransform.GetComponentsInChildren<RankUIWrapperAnimationPlayer>(true);
            rankingAnimations = targetTransform.GetComponentsInChildren<RankAnimationPlayer>(true);

            rankingRiseSfx = serializedObject.FindProperty("rankingRiseSfx");
            rankingFallSfx = serializedObject.FindProperty("rankingFallSfx");

            if (targetTransform.GetComponentInChildren<TagClasses.RankUIRankingRiseVfxObject>() != null)
            {
                rankingRiseVfx = targetTransform.GetComponentInChildren<TagClasses.RankUIRankingRiseVfxObject>(true).GetComponent<ParticleSystem>();
                ((RankUI) target).rankingRiseVfx = rankingRiseVfx;
            }
            if (targetTransform.GetComponentInChildren<TagClasses.RankUIRankingFallVfxObject>() != null)
            {
                rankingFallVfx = targetTransform.GetComponentInChildren<TagClasses.RankUIRankingFallVfxObject>(true).GetComponent<ParticleSystem>();
                ((RankUI) target).rankingFallVfx = rankingFallVfx;
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

            /*
            // Rank icon sprite.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Rank Icon", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("An image representing Rank or character.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.EndVertical();
            Undo.RecordObject(rankIconImage, "Goal icon image change");
            rankIconImage.sprite = EditorGUILayout.ObjectField(rankIconImage.sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80)) as Sprite;
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
                }
                EditorGUILayout.EndHorizontal();
            }

            // Ranking text postfix color scheme.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ranking Text Postfix", EditorStyles.wordWrappedLabel);
            Undo.RecordObject(target, "rankingPostfixTextColor change");
            ((RankUI) target).textPostfixTextColor = EditorGUILayout.ColorField(((RankUI) target).textPostfixTextColor, GUILayout.MaxWidth(65f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("When the color does not seem to apply, clicking the color field once more to open color wheel window should solve the problem.", MessageType.None);

            // Ranking Text Size
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Ranking Text", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(useSeparatedRankingText);
            if (!useSeparatedRankingText.boolValue)
            {
                EditorGUILayout.PropertyField(textPostfixScale);
            }
            ((RankUI) target).UpdateRank();

            // SFXs.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("SFXs", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(rankingRiseSfx);
            EditorGUILayout.PropertyField(rankingFallSfx);

            // VFXs.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("VFXs", EditorStyles.boldLabel);
            if (!(EditorApplication.isPlaying || EditorApplication.isPaused) || ((RankUI) target).IsShown)
            {
                EditorGUILayout.HelpBox("Read only fields below; click it to select and then edit it if neccessary.", MessageType.None);
                EditorGUILayout.ObjectField("Ranking Rise Vfx", rankingRiseVfx, typeof(ParticleSystem), true);
                EditorGUILayout.ObjectField("Ranking Fall Vfx", rankingFallVfx, typeof(ParticleSystem), true);
            }
            else
            {
                EditorGUILayout.HelpBox("VFXs only show when those GameObjects are active in the scene.", MessageType.Warning);
            }

            // Rank Board animations.
            if (rankUIWrapperAnimationPlayers.Length > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Rank Board Animations", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Show/Hide animation of rank UI.\nPlays when Rank.ShowUI() and Rank.HideUI() call.", EditorStyles.wordWrappedLabel);
                EditorGUI.indentLevel += 1;
                for (int i = 0; i < rankUIWrapperAnimationPlayers.Length; i++)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(rankUIWrapperAnimationPlayers[i].name, EditorStyles.boldLabel);
                    if (GUILayout.Button("Ping Object", EditorStyles.miniButtonRight))
                    {
                        EditorGUIUtility.PingObject(rankUIWrapperAnimationPlayers[i].gameObject);
                    }
                    EditorGUILayout.EndHorizontal();
                    Undo.RecordObject(rankUIWrapperAnimationPlayers[i], "Rank show animation clip change");
                    rankUIWrapperAnimationPlayers[i].showAnimation.animationClip = EditorGUILayout.ObjectField("Show Anim. Clip", rankUIWrapperAnimationPlayers[i].showAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(rankUIWrapperAnimationPlayers[i], "Rank show animation speed change");
                    rankUIWrapperAnimationPlayers[i].showAnimation.playSpeed = EditorGUILayout.FloatField("Show Anim. Speed", rankUIWrapperAnimationPlayers[i].showAnimation.playSpeed);
                    Undo.RecordObject(rankUIWrapperAnimationPlayers[i], "Rank hide animation clip change");
                    rankUIWrapperAnimationPlayers[i].hideAnimation.animationClip = EditorGUILayout.ObjectField("Hide Anim. Clip", rankUIWrapperAnimationPlayers[i].hideAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(rankUIWrapperAnimationPlayers[i], "Rank hide animation speed change");
                    rankUIWrapperAnimationPlayers[i].hideAnimation.playSpeed = EditorGUILayout.FloatField("Hide Anim. Speed", rankUIWrapperAnimationPlayers[i].hideAnimation.playSpeed);
                }
                EditorGUI.indentLevel -= 1;
            }

            // Ranking rise/fall animations.
            if (rankingAnimations.Length > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Ranking Rise/Fall Animations", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Animations to play when ranking is rising/falling.", EditorStyles.wordWrappedLabel);
                EditorGUI.indentLevel += 1;
                for (int i = 0; i < rankingAnimations.Length; i++)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(rankingAnimations[i].name, EditorStyles.boldLabel);
                    if (GUILayout.Button("Ping Object", EditorStyles.miniButtonRight))
                    {
                        EditorGUIUtility.PingObject(rankingAnimations[i].gameObject);
                    }
                    EditorGUILayout.EndHorizontal();
                    Undo.RecordObject(rankingAnimations[i], "Ranking idle animation clip change");
                    rankingAnimations[i].idleAnimation.animationClip = EditorGUILayout.ObjectField("Idle Anim. Clip", rankingAnimations[i].idleAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(rankingAnimations[i], "Ranking idle animation speed change");
                    rankingAnimations[i].idleAnimation.playSpeed = EditorGUILayout.FloatField("Idle Anim. Speed", rankingAnimations[i].idleAnimation.playSpeed);
                    EditorGUILayout.Space();
                    Undo.RecordObject(rankingAnimations[i], "Ranking rise animation clip change");
                    rankingAnimations[i].riseAnimation.animationClip = EditorGUILayout.ObjectField("Rise Anim. Clip", rankingAnimations[i].riseAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(rankingAnimations[i], "Ranking rise animation speed change");
                    rankingAnimations[i].riseAnimation.playSpeed = EditorGUILayout.FloatField("Rise Anim. Speed", rankingAnimations[i].riseAnimation.playSpeed);
                    EditorGUILayout.Space();
                    Undo.RecordObject(rankingAnimations[i], "Ranking fall animation clip change");
                    rankingAnimations[i].fallAnimation.animationClip = EditorGUILayout.ObjectField("Fall Anim. Clip", rankingAnimations[i].fallAnimation.animationClip, typeof(AnimationClip), false) as AnimationClip;
                    Undo.RecordObject(rankingAnimations[i], "Ranking fall animation speed change");
                    rankingAnimations[i].fallAnimation.playSpeed = EditorGUILayout.FloatField("Fall Anim. Speed", rankingAnimations[i].fallAnimation.playSpeed);
                }
                EditorGUI.indentLevel -= 1;
            }

            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}