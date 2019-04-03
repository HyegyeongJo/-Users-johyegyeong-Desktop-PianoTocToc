using System.Collections;
using UnityEngine;

namespace ManUtils
{
	public static class ManCoroutine
	{
		#region FIELDS

		#endregion



		#region PROPERTIES

		#endregion



		#region EVENTS

		#endregion



		#region UNITY_FRAMEWORK

		#endregion



		#region CUSTOM_FRAMEWORK

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		// http://kieuns.com/wiki/doku.php?id=language:csharp:unity3d
		// http://www.blockypixel.com/2012/09/c-in-unity3d-dynamic-methods-with-lambda-expressions/
		public static IEnumerator WaitAndAction(int frame, System.Action action)
		{
			for (int i = 0; i < frame; i++)
			{
				yield return null;
			}
			action();
		}

		public static IEnumerator WaitAndAction<T>(int frame, System.Action<T> action, T arg)
		{
			for (int i = 0; i < frame; i++)
			{
				yield return null;
			}
			action(arg);
		}

		public static IEnumerator WaitAndAction<T1, T2>(int frame, System.Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			for (int i = 0; i < frame; i++)
			{
				yield return null;
			}
			action(arg1, arg2);
		}

		public static IEnumerator WaitAndAction<T1, T2, T3>(int frame, System.Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			for (int i = 0; i < frame; i++)
			{
				yield return null;
			}
			action(arg1, arg2, arg3);
		}

		public static IEnumerator WaitAndAction<T1, T2, T3, T4>(int frame, System.Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			for (int i = 0; i < frame; i++)
			{
				yield return null;
			}
			action(arg1, arg2, arg3, arg4);
		}

		public static IEnumerator WaitAndAction(float time, System.Action action)
		{
			yield return new WaitForSeconds(time);
			action();
		}

		public static IEnumerator WaitAndAction<T>(float time, System.Action<T> action, T arg)
		{
			yield return new WaitForSeconds(time);
			action(arg);
		}

		public static IEnumerator WaitAndAction<T1, T2>(float time, System.Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			yield return new WaitForSeconds(time);
			action(arg1, arg2);
		}

		public static IEnumerator WaitAndAction<T1, T2, T3>(float time, System.Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			yield return new WaitForSeconds(time);
			action(arg1, arg2, arg3);
		}

		public static IEnumerator WaitAndAction<T1, T2, T3, T4>(float time, System.Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			yield return new WaitForSeconds(time);
			action(arg1, arg2, arg3, arg4);
		}

		#endregion
	}
}