using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
    public class GameTimeFreezer : MonoBehaviour
    {
        public void FreezeGameTime(bool freeze)
        {
            Debug.Log(freeze ? "Freeze!" : "Unfreeze..");
            Time.timeScale = (freeze ? 0 : 1f);
        }
    }
}