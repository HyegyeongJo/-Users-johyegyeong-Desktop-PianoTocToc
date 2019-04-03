using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToryUX
{
    public class AllSettingsReverter : MonoBehaviour
    {
        public void RevertAllSettings()
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            var logMessage = new StringBuilder();
            logMessage.Append("Reverting all settings to defaults..");
            #endif

            foreach (Selectable s in Resources.FindObjectsOfTypeAll<Selectable>().Where(s => s is IDefaultValueSetter))
            {
                #if UNITY_EDITOR
                if (EditorUtility.IsPersistent(s.transform.root.gameObject))
                {
                    continue;
                }
                #endif

                ((IDefaultValueSetter) s).RevertToDefault();

                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                logMessage.AppendFormat("\n  {0} reverted to default value", s.name);
                #endif
            }

            foreach (DefaultValueSetterMonoBehaviour s in Resources.FindObjectsOfTypeAll<DefaultValueSetterMonoBehaviour>())
            {
                #if UNITY_EDITOR
                if (EditorUtility.IsPersistent(s.transform.root.gameObject))
                {
                    continue;
                }
                #endif

                ((IDefaultValueSetter) s).RevertToDefault();

                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                logMessage.AppendFormat("\n  {0} reverted to default value", s.name);
                #endif
            }

            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(logMessage.ToString());
            #endif
        }
    }
}