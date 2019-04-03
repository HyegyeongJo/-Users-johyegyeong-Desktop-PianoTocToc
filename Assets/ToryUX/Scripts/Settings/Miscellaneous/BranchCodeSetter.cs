using System.Collections;
using System.Collections.Generic;
using ToryValue;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [RequireComponent(typeof(ToryInputField))]
    public class BranchCodeSetter : MonoBehaviour
    {
        ToryInputField inputFieldComponent;
        ToryInputField InputFieldComponent
        {
            get
            {
                if (inputFieldComponent == null)
                {
                    inputFieldComponent = GetComponent<ToryInputField>();
                }
                return inputFieldComponent;
            }
        }
        
        void OnEnable()
        {
            InputFieldComponent.UpdateValue(ToryCare.Config.BranchCode);
        }

        public void SetBranchCode(string text)
        {
            ToryCare.Config.BranchCode = text;
            ToryUX.SettingsUI.Instance.saveSharedSettingsJson = true;
        }
    }
}