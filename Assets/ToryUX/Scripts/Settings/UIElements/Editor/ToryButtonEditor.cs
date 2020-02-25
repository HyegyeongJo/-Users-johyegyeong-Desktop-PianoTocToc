using UnityEditor;
using UnityEditor.UI;

namespace ToryUX
{
	[CustomEditor(typeof(ToryButton))]
	public class ToryButtonEditor : ButtonEditor
	{
		ToryButton component;

		protected override void OnEnable()
		{
			base.OnEnable();
            component = (ToryButton)target;
		}

		public override void OnInspectorGUI()
		{
			if (component.text != null)
			{
				EditorGUILayout.Space();
				Undo.RecordObject(component.text, "Button text change");
				component.text.text = EditorGUILayout.TextField("Button Text", component.text.text);
				PrefabUtility.RecordPrefabInstancePropertyModifications(component.text);
				EditorGUILayout.Space();
			}
			base.OnInspectorGUI();
		}
	}
}