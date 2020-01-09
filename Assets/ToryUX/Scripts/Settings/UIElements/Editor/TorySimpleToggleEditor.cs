using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ToryUX
{
	[CustomEditor(typeof(TorySimpleToggle))]
	public class TorySimpleToggleEditor : ToggleEditor
    {
        TorySimpleToggle component;
        Vector2 toggleAreaSize;

        SerializedProperty onText;
        SerializedProperty offText;

        SerializedProperty toryBoolProperty;
        bool bindToryValueProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            component = (TorySimpleToggle) target;

            onText = serializedObject.FindProperty("onText");
            offText = serializedObject.FindProperty("offText");
            toryBoolProperty = serializedObject.FindProperty("toryBoolProperty");

            toggleAreaSize = component.untickedObject.GetComponent<RectTransform>().sizeDelta;
            component.tickedObject.GetComponent<RectTransform>().sizeDelta = toggleAreaSize;

            if (ToryValue.SecureKeysChecker.CheckSecureKeys())
            {
                if (PlayerPrefsElite.key != null)
                {
                    component.BindToryValue();
                }
            }
            bindToryValueProperty = false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (component.labelText != null)
            {
                EditorGUILayout.Space();
                Undo.RecordObject(component.labelText, "Label text change");
                component.labelText.text = EditorGUILayout.TextField("Label", component.labelText.text);
				PrefabUtility.RecordPrefabInstancePropertyModifications(component.labelText);
				EditorGUILayout.Space();
            }
            if (component.onText != null)
            {
                EditorGUILayout.PropertyField(onText);
            }
            if (component.offText != null)
            {
                EditorGUILayout.PropertyField(offText);
            }

            EditorGUI.BeginChangeCheck();
            toggleAreaSize.x = EditorGUILayout.DelayedFloatField("Toggle Area Width", toggleAreaSize.x);
            if (EditorGUI.EndChangeCheck())
            {
                component.tickedObject.GetComponent<RectTransform>().sizeDelta = toggleAreaSize;
                component.untickedObject.GetComponent<RectTransform>().sizeDelta = toggleAreaSize;
            }
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();

            bool prevValue = component.isOn;

            base.OnInspectorGUI();
            serializedObject.Update();

            if (component.isOn != prevValue)
            {
                serializedObject.ApplyModifiedProperties();
                component.UpdateValue();
                serializedObject.Update();
            }

            // ToryValue Fields

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Bind ToryBool", EditorStyles.boldLabel);

            if (bindToryValueProperty)
            {
                serializedObject.ApplyModifiedProperties();
                component.BindToryValue();
                serializedObject.Update();
                bindToryValueProperty = false;
            }

            if (component.toryBoolProperty.GetPersistentEventCount() > 1)
            {
                EditorGUILayout.HelpBox("If number of bound ToryValue exceed 1, default and current value of ToryUX component follows first bound ToryValue.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("Only \"Property\" of ToryValue in gameobject component can be bound with ToryUX component.", MessageType.Info);
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(toryBoolProperty);
            if (EditorGUI.EndChangeCheck())
            {
                bindToryValueProperty = true;
                serializedObject.ApplyModifiedProperties();
                return;
            }

            if (component.boundToryBools.Count > 0)
            {
                component.DefaultValue = EditorGUILayout.Toggle("Default Value", component.DefaultValue);
                if (GUILayout.Button("Revert To Default"))
                {
                    component.RevertToDefault();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}