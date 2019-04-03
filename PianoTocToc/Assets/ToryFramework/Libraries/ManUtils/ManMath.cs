using UnityEngine;

namespace ManUtils
{
	public class ManMath : MonoBehaviour
	{
		#region SINGLETON

		private static volatile ManMath instance;
		private static readonly object sync_root = new object();

		private ManMath(){}

		public static ManMath Instance
		{
			get
			{
				if (instance == null)
				{
					lock (sync_root)
					{
						if (instance == null)
						{
							instance = new ManMath();
						}
					}
				}

				return instance;
			}
		}

		#endregion


		#region FIELDS

		#endregion



		#region PROPERTIES

		#endregion



		#region EVENTS

		#endregion



		#region UNITY_FRAMEWORK

		#endregion



		#region MAN_FRAMEWORK

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		// Map

		public static float Map(float value, float min, float max, float minTarget, float maxTarget)
		{
			return minTarget + (value - min) / (max - min) * (maxTarget - minTarget);
		}

		public static Vector2 Map(Vector2 value, Vector2 min, Vector2 max, Vector2 minTarget, Vector2 maxTarget)
		{
			return new Vector2(minTarget.x + (value.x - min.x) / (max.x - min.x) * (maxTarget.x - minTarget.x),
			               minTarget.y + (value.y - min.y) / (max.y - min.y) * (maxTarget.y - minTarget.y));
		}

		public static Vector3 Map(Vector3 value, Vector3 min, Vector3 max, Vector3 minTarget, Vector3 maxTarget)
		{
			return new Vector3(minTarget.x + (value.x - min.x) / (max.x - min.x) * (maxTarget.x - minTarget.x),
			               minTarget.y + (value.y - min.y) / (max.y - min.y) * (maxTarget.y - minTarget.y),
			               minTarget.z + (value.z - min.z) / (max.z - min.z) * (maxTarget.z - minTarget.z));
		}

		public static Vector4 Map(Vector4 value, Vector4 min, Vector4 max, Vector4 minTarget, Vector4 maxTarget)
		{
			return new Vector4(minTarget.x + (value.x - min.x) / (max.x - min.x) * (maxTarget.x - minTarget.x),
			               minTarget.y + (value.y - min.y) / (max.y - min.y) * (maxTarget.y - minTarget.y),
			               minTarget.z + (value.z - min.z) / (max.z - min.z) * (maxTarget.z - minTarget.z),
			               minTarget.w + (value.w - min.w) / (max.w - min.w) * (maxTarget.w - minTarget.w));
		}

		// Map Clamped

		public static float MapClamped(float value, float min, float max, float minTarget, float maxTarget)
		{
			return Mathf.Clamp(minTarget + (value - min) / (max - min) * (maxTarget - minTarget), minTarget, maxTarget);
		}

		public static Vector2 MapClamped(Vector2 value, Vector2 min, Vector2 max, Vector2 minTarget, Vector2 maxTarget)
		{
			return new Vector2(Mathf.Clamp(minTarget.x + (value.x - min.x) / (max.x - min.x) * (maxTarget.x - minTarget.x), minTarget.x, maxTarget.x),
			               Mathf.Clamp(minTarget.y + (value.y - min.y) / (max.y - min.y) * (maxTarget.y - minTarget.y), minTarget.y, maxTarget.y));
		}

		public static Vector3 MapClamped(Vector3 value, Vector3 min, Vector3 max, Vector3 minTarget, Vector3 maxTarget)
		{
			return new Vector3(Mathf.Clamp(minTarget.x + (value.x - min.x) / (max.x - min.x) * (maxTarget.x - minTarget.x), minTarget.x, maxTarget.x),
			               Mathf.Clamp(minTarget.y + (value.y - min.y) / (max.y - min.y) * (maxTarget.y - minTarget.y), minTarget.y, maxTarget.y),
			               Mathf.Clamp(minTarget.z + (value.z - min.z) / (max.z - min.z) * (maxTarget.z - minTarget.z), minTarget.z, maxTarget.z));
		}

		public static Vector4 MapClamped(Vector4 value, Vector4 min, Vector4 max, Vector4 minTarget, Vector4 maxTarget)
		{
			return new Vector4(Mathf.Clamp(minTarget.x + (value.x - min.x) / (max.x - min.x) * (maxTarget.x - minTarget.x), minTarget.x, maxTarget.x),
			               Mathf.Clamp(minTarget.y + (value.y - min.y) / (max.y - min.y) * (maxTarget.y - minTarget.y), minTarget.y, maxTarget.y),
			               Mathf.Clamp(minTarget.z + (value.z - min.z) / (max.z - min.z) * (maxTarget.z - minTarget.z), minTarget.z, maxTarget.z),
			               Mathf.Clamp(minTarget.w + (value.w - min.w) / (max.w - min.w) * (maxTarget.w - minTarget.w), minTarget.w, maxTarget.w));
		}

		#endregion
	}	
}