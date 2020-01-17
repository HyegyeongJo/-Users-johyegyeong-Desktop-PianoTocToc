using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ToryUX
{
    [CustomEditor(typeof(ToryToggle))]
    public class ToryToggleEditor : ButtonEditor
    {
        ToryToggle component;

        SerializedProperty options;
        SerializedProperty onToggle;

        SerializedProperty playerPrefsKeyString;

        SerializedProperty toryToggleIntProperty;
        bool bindToryValueProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            component = (ToryToggle) target;

            options = serializedObject.FindProperty("options");
            onToggle = serializedObject.FindProperty("onToggle");
            toryToggleIntProperty = serializedObject.FindProperty("toryToggleIntProperty");

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

            EditorGUILayout.LabelField("Options To Toggle", EditorStyles.boldLabel);
            ArrayGUI(options);
            EditorGUILayout.HelpBox(@"Options above will toggle around on each toggle click.
Values can be accesed via properties such as 'options', 'CurrentOptionIndex', 'SelectedOption'.
'OnToggle(currentIndex, selectedOption)' event will fire to deal with those more conveniently.", MessageType.Info);

            EditorGUILayout.PropertyField(onToggle);
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();

            int prevValue = component.CurrentOptionIndex;

            base.OnInspectorGUI();
            serializedObject.Update();

            if (component.CurrentOptionIndex != prevValue)
            {
                serializedObject.ApplyModifiedProperties();
                component.UpdateValue();
                serializedObject.Update();
            }

            // ToryValue Fields

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Bind ToryInt", EditorStyles.boldLabel);

            if (bindToryValueProperty)
            {
                serializedObject.ApplyModifiedProperties();
                component.BindToryValue();
                serializedObject.Update();
                bindToryValueProperty = false;
            }

            if (component.toryToggleIntProperty.GetPersistentEventCount() > 1)
            {
                EditorGUILayout.HelpBox("If number of bound ToryValue exceed 1, default and current value of ToryUX component follows first bound ToryValue.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("Only \"Property\" of ToryValue in gameobject component can be bound with ToryUX component.", MessageType.Info);
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(toryToggleIntProperty);
            if (EditorGUI.EndChangeCheck())
            {
                bindToryValueProperty = true;
                serializedObject.ApplyModifiedProperties();
                return;
            }

            if (component.boundToryInts.Count > 0)
            {
                component.DefaultValue = EditorGUILayout.Popup("Default Value", component.DefaultValue, component.options);
                if (GUILayout.Button("Revert To Default"))
                {
                    component.RevertToDefault();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        void ArrayGUI(SerializedProperty property)
        {
            SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
            EditorGUILayout.PropertyField(arraySizeProp);

            EditorGUI.indentLevel++;

            for (int i = 0; i < arraySizeProp.intValue; i++)
            {
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i));
            }

            EditorGUI.indentLevel--;
        }
    }
}