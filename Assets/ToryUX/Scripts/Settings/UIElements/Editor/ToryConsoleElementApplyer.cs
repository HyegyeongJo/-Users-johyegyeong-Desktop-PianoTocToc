using UnityEditor;
using UnityEngine;

namespace ToryUX
{
	public class ToryConsoleElementApplyer : Editor
	{
		/// <summary>
		/// Apply ToryConsole instance to ToryConsoleSetup component in UI Root.
		/// </summary>
		[InitializeOnLoadMethod]
		static void ApplyToryConsoleSetup()
		{
			ToryConsoleSetup uiRootComponent = FindObjectOfType<ToryConsoleSetup>();

			if (uiRootComponent != null && uiRootComponent.uiConsoleElement == null)
			{
				foreach (ToryConsole t in Resources.FindObjectsOfTypeAll<ToryConsole>())
				{
					if (EditorUtility.IsPersistent(t.transform.root.gameObject))
					{
						continue;
					}
					else
					{
						uiRootComponent.uiConsoleElement = t;
						EditorUtility.SetDirty(uiRootComponent);
						return;
					}
				}
			}
		}
	}
}