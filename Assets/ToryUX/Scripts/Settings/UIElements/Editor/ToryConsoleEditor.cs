using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ToryUX
{
	[CustomEditor(typeof(ToryConsole))]
	public class ToryConsoleEditor : Editor
	{
		void OnEnable()
		{
			ToryConsoleSetup uiRootComponent = ((ToryConsole) target).gameObject.transform.root.gameObject.GetComponentInChildren<ToryConsoleSetup>();

			if (uiRootComponent != null && uiRootComponent.uiConsoleElement == null)
			{
				uiRootComponent.uiConsoleElement = (ToryConsole) target;
				EditorUtility.SetDirty(uiRootComponent);
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			base.OnInspectorGUI();
			serializedObject.ApplyModifiedProperties();
		}
	}
}