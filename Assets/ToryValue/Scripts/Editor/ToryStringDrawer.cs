using System.Reflection;

namespace ToryValue 
{
	[UnityEditor.CustomPropertyDrawer(typeof(ToryString))]
	public class ToryStringDrawer : ToryValueDrawer<string>
	{
		protected override ToryValue<string> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryString>(inspectedObject, serializedProperty);
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<string> toryValue)
		{
			toryValue.Value = valueProperty.stringValue;
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		void SyncSerializedPropertyWithValue(UnityEditor.SerializedProperty serializedProperty, string inspectedToryValue)
		{
			serializedProperty.stringValue = inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<string> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.stringValue);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<string> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.stringValue);
			SyncSerializedPropertyWithValue(savedValueProperty, (string)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        PlayerPrefsElite.GetString(keyProperty.stringValue).Equals(savedValueProperty.stringValue));
		}

		protected override void SaveInspectedToryValue(ToryValue<string> toryValue)
		{
			PlayerPrefsElite.SetString(toryValue.Key, savedValueProperty.stringValue);
		}

		protected override bool SavedButInconsistent()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        !PlayerPrefsElite.GetString(keyProperty.stringValue).Equals(savedValueProperty.stringValue));
		}

		protected override void FetchInspectedToryValueSavedValue()
		{
			savedValueProperty.stringValue = PlayerPrefsElite.GetString(keyProperty.stringValue);
		}
	}
}
