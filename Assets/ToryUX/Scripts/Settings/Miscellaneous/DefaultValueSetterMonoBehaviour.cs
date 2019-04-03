using System.Collections;
using UnityEngine;

namespace ToryUX
{
    public class DefaultValueSetterMonoBehaviour : MonoBehaviour, IDefaultValueSetter
    {
        public virtual void RevertToDefault()
        {}
    }
}