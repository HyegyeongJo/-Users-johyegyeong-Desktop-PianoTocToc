using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ToryUX
{
    [CustomEditor(typeof(AllSettingsReverter))]
    public class AllSettingsReverterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Revert to Default Settings"))
            {
                ((AllSettingsReverter) target).RevertAllSettings();
            }
        }
    }
}