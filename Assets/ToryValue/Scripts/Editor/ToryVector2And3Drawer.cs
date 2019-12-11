using UnityEngine;
using UnityEditor;

namespace ToryValue
{
	public abstract class ToryVector2And3Drawer<T> : ToryValueDrawer<T>
	{
		protected float verticalSpace;

		protected override void PrepareDrawingRelativeProperties()
		{
			base.PrepareDrawingRelativeProperties();
			SetRelativePropertyHeight();
		}

		void SetRelativePropertyHeight()
		{
			relativePropertiesPosition.height = (position.height - EditorGUIUtility.standardVerticalSpacing * 3f) * 0.25f;
			verticalSpace = relativePropertiesPosition.height + EditorGUIUtility.standardVerticalSpacing;
		}

		protected override void DrawKeyLabelField()
		{
			Rect pos = GetLabelFieldPositionAt(0);
			EditorGUI.LabelField(pos, new GUIContent("K", "Key"));
		}

		Rect GetLabelFieldPositionAt(int ithRelativeProperty)
		{
			return new Rect(relativePropertiesPosition.x,
							relativePropertiesPosition.y + verticalSpace * ithRelativeProperty,
							labelWidth,
							relativePropertiesPosition.height);
		}

		protected override void DrawKeyPropertyField()
		{
			Rect pos = GetPropertyFieldPositionAt(0);
			EditorGUI.PropertyField(pos, keyProperty, GUIContent.none);
		}

		protected Rect GetPropertyFieldPositionAt(int ithRelativeProperty)
		{
			return new Rect(relativePropertiesPosition.x + labelWidth,
							relativePropertiesPosition.y + verticalSpace * ithRelativeProperty,
							relativePropertiesPosition.width - labelWidth,
							relativePropertiesPosition.height);
		}

		protected override void DrawValueLabelField()
		{
			Rect pos = GetLabelFieldPositionAt(1);
			EditorGUI.LabelField(pos, new GUIContent("V", "Value"));
		}

		protected override void DrawValuePropertyField()
		{
			Rect pos = GetPropertyFieldPositionAt(1);
			EditorGUI.PropertyField(pos, valueProperty, GUIContent.none);
		}

		protected override void DrawDefaultValueLabelField()
		{
			Rect pos = GetLabelFieldPositionAt(2);
			EditorGUI.LabelField(pos, new GUIContent("D", "Default value"));
		}

		protected override void DrawDefaultValuePropertyField()
		{
			Rect pos = GetPropertyFieldPositionAt(2);
			EditorGUI.PropertyField(pos, defaultValueProperty, GUIContent.none);
		}

		protected override void DrawSavedValueLabelField()
		{
			Rect pos = GetLabelFieldPositionAt(3);
			string tooltip = "Saved value (" + (Saved() ? "saved" : "not saved") + ")";
			EditorGUI.LabelField(pos, new GUIContent("S", tooltip));
		}

		protected override void DrawSavedValuePropertyField()
		{
			Rect pos = GetPropertyFieldPositionAt(3);
			EditorGUI.PropertyField(pos, savedValueProperty, GUIContent.none);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) * 4f + EditorGUIUtility.standardVerticalSpacing * 3f;
		}
	}
}