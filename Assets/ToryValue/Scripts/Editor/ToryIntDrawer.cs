using System.Reflection;

namespace ToryValue 
{
	[UnityEditor.CustomPropertyDrawer(typeof(ToryInt))]
	public class ToryIntDrawer : ToryValueDrawer<int>
	{
		protected override ToryValue<int> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryInt>(inspectedObject, serializedProperty);
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<int> toryValue)
		{
			toryValue.Value = valueProperty.intValue;
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		void SyncSerializedPropertyWithValue(UnityEditor.SerializedProperty serializedProperty, int inspectedToryValue)
		{
			serializedProperty.intValue = inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<int> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.intValue);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<int> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.intValue);
			SyncSerializedPropertyWithValue(savedValueProperty, (int)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        PlayerPrefsElite.GetInt(keyProperty.stringValue).Equals(savedValueProperty.intValue));
		}

		protected override void SaveInspectedToryValue(ToryValue<int> toryValue)
		{
			PlayerPrefsElite.SetInt(toryValue.Key, savedValueProperty.intValue);
		}

		protected override bool SavedButInconsistent()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        !PlayerPrefsElite.GetInt(keyProperty.stringValue).Equals(savedValueProperty.intValue));
		}

		protected override void FetchInspectedToryValueSavedValue()
		{
			savedValueProperty.intValue = PlayerPrefsElite.GetInt(keyProperty.stringValue);
		}
	}
}
