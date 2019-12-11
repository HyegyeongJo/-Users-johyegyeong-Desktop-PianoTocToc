using System.Collections.Generic;
using System.Linq;

namespace ToryValue
{
	public static class PropertyDrawerUtility
	{
		/// <summary>
		/// Gets the actual inspected object, regardless of whether the <paramref name="property"/>'s target object is an array, generic list, or not.
		/// </summary>
		/// <returns>The actual object.</returns>
		/// <param name="fieldInfo">Field info.</param>
		/// <param name="property">Property.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		// Getting actual objects: http://sketchyventures.com/2015/08/07/unity-tip-getting-the-actual-object-from-a-custom-property-drawer/
		public static T GetActualObject<T>(System.Reflection.FieldInfo fieldInfo, UnityEditor.SerializedProperty property) where T : class
		{
			return GetActualObject<T>(fieldInfo.GetValue(property.serializedObject.targetObject), property);
		}

		/// <summary>
		/// Gets the actual inspected object, regardless of whether the <paramref name="obj"/> is an array, generic list, or not.
		/// </summary>
		/// <returns>The actual object.</returns>
		/// <param name="obj">Object.</param>
		/// <param name="property">Property.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetActualObject<T>(object obj, UnityEditor.SerializedProperty property) where T : class
		{
			try
			{
				return GetActualObjectOfArrayOrGenericList<T>(obj, property);
			}
			catch (System.Exception e)
			{
				if (!(e is System.NullReferenceException ||
					  e is System.IndexOutOfRangeException ||
					  e is System.ArgumentOutOfRangeException))
				{
					UnityEngine.Debug.LogException(e);
				}
				return null;
			}
		}

		static T GetActualObjectOfArrayOrGenericList<T>(object obj, UnityEditor.SerializedProperty property) where T : class
		{
			return IsArray(obj) ? GetActualObjectOfArray((T[])obj, property)
					: IsGenericList(obj) ? GetActualObjectOfGenericList((List<T>)obj, property)
					: obj as T;
		}

		static bool IsArray(object obj)
		{
			return obj.GetType().IsArray;
		}

		// Checking if a value is a generic list: https://stackoverflow.com/questions/794198/how-do-i-check-if-a-given-value-is-a-generic-list
		static bool IsGenericList(object obj)
		{
			return obj is System.Collections.IList && obj.GetType().IsGenericType;
		}

		static T GetActualObjectOfArray<T>(T[] arrayObject, UnityEditor.SerializedProperty property) where T : class
		{
			int index = GetIndexOf(property.propertyPath);
			return arrayObject[index];
		}

		static int GetIndexOf(string propertyPath)
		{
			var split = propertyPath.Split('[');
			int index = System.Convert.ToInt32(new string(split.Last().Where(c => char.IsDigit(c)).ToArray()));
			return index;
		}

		static T GetActualObjectOfGenericList<T>(List<T> genericListObject, UnityEditor.SerializedProperty property) where T : class
		{
			int index = GetIndexOf(property.propertyPath);
			return genericListObject[index];
		}
	}
}