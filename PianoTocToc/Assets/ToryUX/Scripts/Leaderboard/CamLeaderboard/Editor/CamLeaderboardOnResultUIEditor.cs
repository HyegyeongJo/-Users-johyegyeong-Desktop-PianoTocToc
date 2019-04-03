using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [CustomEditor(typeof(CamLeaderboardOnResultUI))]
    public class CamLeaderboardOnResultUIEditor : Editor
    {
        Transform targetTransform;

        SerializedProperty minRankToTakePicture;

        Dictionary<string, List<Graphic>> colorSchemeSet;

        SerializedProperty goodJobSfx;
        SerializedProperty cameraSfx;

        void OnEnable()
        {
            targetTransform = ((CamLeaderboardOnResultUI) target).transform;

            minRankToTakePicture = serializedObject.FindProperty("minRankToTakePicture");

            var customazableColorObjects = targetTransform.GetComponentsInChildren<TagClasses.CamLeaderboardUICustomazableColorObject>(true);
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

            goodJobSfx = serializedObject.FindProperty("goodJobSfx");
            cameraSfx = serializedObject.FindProperty("cameraSfx");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Settings.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(minRankToTakePicture);

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
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.HelpBox("When the color does not seem to apply, clicking the color field once more to open color wheel window should solve the problem.", MessageType.None);

            // Best score/countdown sfx.
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sfxs", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(goodJobSfx);
            EditorGUILayout.PropertyField(cameraSfx);

            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}