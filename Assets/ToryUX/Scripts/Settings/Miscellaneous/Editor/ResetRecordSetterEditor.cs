using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ToryUX
{
    [CustomEditor(typeof(ResetRecordSetter))]
    public class ResetRecordSetterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Reset Leaderboard Record"))
            {
                ((ResetRecordSetter) target).ResetRecord();
            }
        }
    }
}