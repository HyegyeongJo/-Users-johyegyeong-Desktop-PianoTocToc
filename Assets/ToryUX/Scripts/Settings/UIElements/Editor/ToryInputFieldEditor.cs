using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ToryUX
{
    [CustomEditor(typeof(ToryInputField))]
    public class ToryInputFieldEditor : InputFieldEditor
    {
        ToryInputField component;
        Vector2 toggleAreaSize;

        SerializedProperty toryStringProperty;
        bool bindToryValueProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            component = (ToryInputField) target;

            toryStringProperty = serializedObject.FindProperty("toryStringProperty");

            toggleAreaSize = component.boxRectTransform.sizeDelta;

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
                EditorGUILayout.Space();
            }

            if (component.boxRectTransform != null)
            {
                EditorGUI.BeginChangeCheck();
                toggleAreaSize.x = EditorGUILayout.DelayedFloatField("Toggle Area Width", toggleAreaSize.x);
                if (EditorGUI.EndChangeCheck())
                {
                    component.boxRectTransform.sizeDelta = toggleAreaSize;
                }
                EditorGUILayout.Space();
            }

            serializedObject.ApplyModifiedProperties();

            string prevValue = component.text;

            base.OnInspectorGUI();
            serializedObject.Update();

            if (component.text != prevValue)
            {
                serializedObject.ApplyModifiedProperties();
                component.UpdateValue(component.text);
                serializedObject.Update();
            }

            // ToryValue Fields

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Bind ToryString", EditorStyles.boldLabel);

            if (bindToryValueProperty)
            {
                serializedObject.ApplyModifiedProperties();
                component.BindToryValue();
                serializedObject.Update();
                bindToryValueProperty = false;
            }

            if (component.toryStringProperty.GetPersistentEventCount() > 1)
            {
                EditorGUILayout.HelpBox("If number of bound ToryValue exceed 1, default and current value of ToryUX component follows first bound ToryValue.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("Only \"Property\" of ToryValue in gameobject component can be bound with ToryUX component.", MessageType.Info);
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(toryStringProperty);
            if (EditorGUI.EndChangeCheck())
            {
                bindToryValueProperty = true;
                serializedObject.ApplyModifiedProperties();
                return;
            }

            if (component.boundToryStrings.Count > 0)
            {
                component.DefaultValue = EditorGUILayout.TextField("Default Value", component.DefaultValue);
                if (GUILayout.Button("Revert To Default"))
                {
                    component.RevertToDefault();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}