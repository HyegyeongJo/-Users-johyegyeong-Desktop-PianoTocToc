using System.Reflection;

namespace ToryValue
{
	public abstract class ToryEnumDrawer<T> : ToryValueDrawer<T>
	{
		protected override ToryValue<T> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryEnum<T>>(inspectedObject, serializedProperty);
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<T> toryValue)
		{
			toryValue.Value = (T)System.Enum.ToObject(typeof(T), valueProperty.enumValueIndex);
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		void SyncSerializedPropertyWithValue(UnityEditor.SerializedProperty serializedProperty, T inspectedToryValue)
		{
			serializedProperty.enumValueIndex = (int)(object)inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<T> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.intValue);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<T> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.intValue);
			SyncSerializedPropertyWithValue(savedValueProperty, (T)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) &&
					PlayerPrefsElite.GetInt(keyProperty.stringValue).Equals(savedValueProperty.intValue));
		}

		protected override void SaveInspectedToryValue(ToryValue<T> toryValue)
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
