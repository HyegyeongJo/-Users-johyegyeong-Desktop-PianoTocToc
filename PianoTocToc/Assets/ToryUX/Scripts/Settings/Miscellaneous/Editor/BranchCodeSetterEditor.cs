using UnityEditor;
using UnityEngine;

namespace ToryUX
{
    [CustomEditor(typeof(BranchCodeSetter))]
    public class BranchCodeSetterEditor : Editor
    {
        void OnEnable()
        {
            ToryValue.SecureKeysChecker.CheckSecureKeys();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Revert to Default Settings"))
            {
                ((IDefaultValueSetter) target).RevertToDefault();
            }
        }
    }
}