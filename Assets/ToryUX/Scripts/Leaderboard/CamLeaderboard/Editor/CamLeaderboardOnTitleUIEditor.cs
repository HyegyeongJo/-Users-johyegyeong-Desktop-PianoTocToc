using System.Collections;
using System.Collections.Generic;
using ToryUX.TagClasses;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [CustomEditor(typeof(CamLeaderboardOnTitleUI))]
    public class CamLeaderboardOnTitleUIEditor : Editor
    {
        CamLeaderboardOnTitleUI self;

        CamLeaderboardUITodaySolidColorObject[] todaySolidColorObjects;
        CamLeaderboardUITodayGradientColorObject[] todayGradientColorObjects;
        CamLeaderboardUITodayTextColorObject[] todayTextColorObjects;
        CamLeaderboardUIWeeklySolidColorObject[] weeklySolidColorObjects;
        CamLeaderboardUIWeeklyGradientColorObject[] weeklyGradientColorObjects;
        CamLeaderboardUIWeeklyTextColorObject[] weeklyTextColorObjects;

        SerializedProperty recordBarMinWidth;

        void OnEnable()
        {
            self = (CamLeaderboardOnTitleUI) target;

            todaySolidColorObjects = self.GetComponentsInChildren<CamLeaderboardUITodaySolidColorObject>(true);
            todayGradientColorObjects = self.GetComponentsInChildren<CamLeaderboardUITodayGradientColorObject>(true);
            todayTextColorObjects = self.GetComponentsInChildren<CamLeaderboardUITodayTextColorObject>(true);
            weeklySolidColorObjects = self.GetComponentsInChildren<CamLeaderboardUIWeeklySolidColorObject>(true);
            weeklyGradientColorObjects = self.GetComponentsInChildren<CamLeaderboardUIWeeklyGradientColorObject>(true);
            weeklyTextColorObjects = self.GetComponentsInChildren<CamLeaderboardUIWeeklyTextColorObject>(true);

            recordBarMinWidth = serializedObject.FindProperty("recordBarMinWidth");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Minimum bar size.
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(recordBarMinWidth);

            // Color Scheme.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Color Scheme", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Today Graphics");
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(self, "todaySolidColor change");
            self.todaySolidColor = EditorGUILayout.ColorField(self.todaySolidColor, GUILayout.MaxWidth(65f));
            Undo.RecordObject(self, "todayGradientColor change");
            self.todayGradientColor = EditorGUILayout.ColorField(self.todayGradientColor, GUILayout.MaxWidth(65f));
            Undo.RecordObject(self, "todayTextColor change");
            self.todayTextColor = EditorGUILayout.ColorField(self.todayTextColor, GUILayout.MaxWidth(65f));
			PrefabUtility.RecordPrefabInstancePropertyModifications(self);
			if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < todaySolidColorObjects.Length; i++)
                {
                    todaySolidColorObjects[i].Color = self.todaySolidColor;
                }
                for (int i = 0; i < todayGradientColorObjects.Length; i++)
                {
                    todayGradientColorObjects[i].Color = self.todayGradientColor;
                }
                for (int i = 0; i < todayTextColorObjects.Length; i++)
                {
                    todayTextColorObjects[i].Color = self.todayTextColor;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Weekly Graphics");
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(self, "weeklySolidColor change");
            self.weeklySolidColor = EditorGUILayout.ColorField(self.weeklySolidColor, GUILayout.MaxWidth(65f));
            Undo.RecordObject(self, "weeklyGradientColor change");
            self.weeklyGradientColor = EditorGUILayout.ColorField(self.weeklyGradientColor, GUILayout.MaxWidth(65f));
            Undo.RecordObject(self, "weeklyTextColor change");
            self.weeklyTextColor = EditorGUILayout.ColorField(self.weeklyTextColor, GUILayout.MaxWidth(65f));
			PrefabUtility.RecordPrefabInstancePropertyModifications(self);
			if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < weeklySolidColorObjects.Length; i++)
                {
                    weeklySolidColorObjects[i].Color = self.weeklySolidColor;
                }
                for (int i = 0; i < weeklyGradientColorObjects.Length; i++)
                {
                    weeklyGradientColorObjects[i].Color = self.weeklyGradientColor;
                }
                for (int i = 0; i < weeklyTextColorObjects.Length; i++)
                {
                    weeklyTextColorObjects[i].Color = self.weeklyTextColor;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("When the color does not seem to apply, clicking the color field once more to open color wheel window should solve the problem.", MessageType.None);

            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}