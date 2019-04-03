using UnityEngine;
using ToryFramework.Value;

namespace ToryFramework.Behaviour
{
	public class ToryValueGenerator : MonoBehaviour
	{
		public CustomValue[] values;
	}
}

namespace ToryFramework.Value
{
	public enum ValueType { Float, Int, Bool, String, Vector2, Vector3, Vector4 }

	// ReorderableList with struct: http://unityindepth.tistory.com/56
	[System.Serializable]
	public struct CustomValue
	{
		public ValueType valueType;
		public string valueString;

		public CustomValue(ValueType type, string value)
		{
			this.valueType = type;
			this.valueString = value;
		}
	}
}