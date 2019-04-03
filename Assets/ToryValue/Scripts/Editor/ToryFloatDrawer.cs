using System.Reflection;

namespace ToryValue 
{
	[UnityEditor.CustomPropertyDrawer(typeof(ToryFloat))]
	public class ToryFloatDrawer : ToryValueDrawer<float>
	{
		protected override ToryValue<float> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryFloat>(inspectedObject, serializedProperty);
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<float> toryValue)
		{
			toryValue.Value = valueProperty.floatValue;
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		void SyncSerializedPropertyWithValue(UnityEditor.SerializedProperty serializedProperty, float inspectedToryValue)
		{
			serializedProperty.floatValue = inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<float> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.floatValue);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<float> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.floatValue);
			SyncSerializedPropertyWithValue(savedValueProperty, (float)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        PlayerPrefsElite.GetFloat(keyProperty.stringValue).Equals(savedValueProperty.floatValue));
		}

		protected override void SaveInspectedToryValue(ToryValue<float> toryValue)
		{
			PlayerPrefsElite.SetFloat(toryValue.Key, savedValueProperty.floatValue);
		}

		protected override bool SavedButInconsistent()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        !PlayerPrefsElite.GetFloat(keyProperty.stringValue).Equals(savedValueProperty.floatValue));
		}

		protected override void FetchInspectedToryValueSavedValue()
		{
			savedValueProperty.floatValue = PlayerPrefsElite.GetFloat(keyProperty.stringValue);
		}
	}
}
