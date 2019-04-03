using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ToryUX
{
    [CustomEditor(typeof(VolumeLevelSetter))]
    public class VolumeLevelSetterEditor : Editor
    {
        VolumeLevelSetter component;

        SerializedProperty defaultValue;
        SerializedProperty targetParameter;

        float prevValue;

        protected void OnEnable()
        {
            component = (VolumeLevelSetter) target;
            component.OnEnable();

            targetParameter = serializedObject.FindProperty("targetParameter");
            defaultValue = serializedObject.FindProperty("defaultValue");

            prevValue = component.SliderComponent.value;

            ToryValue.SecureKeysChecker.CheckSecureKeys();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (component.SliderComponent.value != prevValue)
            {
                component.SetVolume(component.SliderComponent.value);
                prevValue = component.SliderComponent.value;

                EditorUtility.SetDirty(UISound.Instance);
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(targetParameter);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                OnEnable();
                return;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(defaultValue);
            if (EditorGUI.EndChangeCheck())
            {
                if (UISound.Instance != null)
                {
                    switch (component.targetParameter)
                    {
                        case VolumeLevelSetter.DefaultAudioMixerParameters.MasterVolume:
                            UISound.Instance.MasterVolume.DefaultValue = defaultValue.floatValue;
                            break;

                        case VolumeLevelSetter.DefaultAudioMixerParameters.BgmVolume:
                            UISound.Instance.BgmVolume.DefaultValue = defaultValue.floatValue;
                            break;

                        case VolumeLevelSetter.DefaultAudioMixerParameters.SfxVolume:
                            UISound.Instance.SfxVolume.DefaultValue = defaultValue.floatValue;
                            break;

                        default:
                            break;
                    }

                    EditorUtility.SetDirty(UISound.Instance);
                }
            }

            if (GUILayout.Button("Revert to Default Settings"))
            {
                ((IDefaultValueSetter) target).RevertToDefault();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}