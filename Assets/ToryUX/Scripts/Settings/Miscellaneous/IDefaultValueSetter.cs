using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
    public interface IDefaultValueSetter
    {
        void RevertToDefault();
    }

    public interface IDefaultValueSetter<T> : IDefaultValueSetter
    {
        T DefaultValue
        {
            get;
            set;
        }
    }
}