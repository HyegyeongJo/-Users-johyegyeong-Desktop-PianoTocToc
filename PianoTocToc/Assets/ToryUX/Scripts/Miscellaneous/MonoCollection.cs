// Pattern reference: http://answers.unity3d.com/questions/251765/invoking-all-instances-of-a-non-static-function.html

using System.Collections.Generic;
using UnityEngine;

public class MonoCollection<T> : MonoBehaviour where T : MonoCollection<T>
{
	public static List<T> allEnabledIntances = new List<T>();

	protected virtual void Awake()
	{
		if (!allEnabledIntances.Contains(this as T))
		{
			allEnabledIntances.Add(this as T);
		}
	}

	protected virtual void OnDestroy()
	{
		if (allEnabledIntances.Contains(this as T))
		{
			allEnabledIntances.Remove(this as T);
		}
	}
}