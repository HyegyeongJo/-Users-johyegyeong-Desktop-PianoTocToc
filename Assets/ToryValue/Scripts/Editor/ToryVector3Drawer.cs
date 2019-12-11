using System.Reflection;

namespace ToryValue 
{
	[UnityEditor.CustomPropertyDrawer(typeof(ToryVector3))]
	public class ToryVector3Drawer : ToryVector2And3Drawer<UnityEngine.Vector3>
	{
		protected override ToryValue<UnityEngine.Vector3> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryVector3>(inspectedObject, serializedProperty);
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<UnityEngine.Vector3> toryValue)
		{
			toryValue.Value = valueProperty.vector3Value;
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<UnityEngine.Vector3> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.vector3Value);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		void SyncSerializedPropertyWithValue(UnityEditor.SerializedProperty serializedProperty, UnityEngine.Vector3 inspectedToryValue)
		{
			serializedProperty.vector3Value = inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<UnityEngine.Vector3> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.vector3Value);
			SyncSerializedPropertyWithValue(savedValueProperty, (UnityEngine.Vector3)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        PlayerPrefsElite.GetVector3(keyProperty.stringValue).Equals(savedValueProperty.vector3Value));
		}

		protected override void SaveInspectedToryValue(ToryValue<UnityEngine.Vector3> toryValue)
		{
			PlayerPrefsElite.SetVector3(toryValue.Key, savedValueProperty.vector3Value);
		}

		protected override bool SavedButInconsistent()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        !PlayerPrefsElite.GetVector3(keyProperty.stringValue).Equals(savedValueProperty.vector3Value));
		}

		protected override void FetchInspectedToryValueSavedValue()
		{
			savedValueProperty.vector3Value = PlayerPrefsElite.GetVector3(keyProperty.stringValue);
		}
	}
}
