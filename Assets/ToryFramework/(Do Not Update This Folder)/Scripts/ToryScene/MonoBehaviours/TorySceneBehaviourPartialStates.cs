using System.Reflection;
using UnityEngine;
using ToryFramework.SMB;

namespace ToryFramework.Behaviour
{
	public partial class TorySceneBehaviour : MonoBehaviour
	{
		void InitStates()
		{
			// Scene
			System.Type type = ToryScene.Instance.GetType();
			MethodInfo method = type.GetMethod("Init", (BindingFlags.NonPublic | BindingFlags.Instance));
			if (method != null)
			{
				object[] parameters = { animator.GetBehaviour<TorySceneSMB>() };
				method.Invoke(ToryScene.Instance, parameters);
			}

			// Title
			type = ToryTitleScene.Instance.GetType();
			method = type.GetMethod("Init", (BindingFlags.NonPublic | BindingFlags.Instance));
			if (method != null)
			{
				object[] parameters = { animator.GetBehaviour<ToryTitleSceneSMB>() };
				method.Invoke(ToryTitleScene.Instance, parameters);
			}

			// Play
			type = ToryPlayScene.Instance.GetType();
			method = type.GetMethod("Init", (BindingFlags.NonPublic | BindingFlags.Instance));
			if (method != null)
			{
				object[] parameters = { animator.GetBehaviour<ToryPlaySceneSMB>() };
				method.Invoke(ToryPlayScene.Instance, parameters);
			}

			// Result
			type = ToryResultScene.Instance.GetType();
			method = type.GetMethod("Init", (BindingFlags.NonPublic | BindingFlags.Instance));
			if (method != null)
			{
				object[] parameters = { animator.GetBehaviour<ToryResultSceneSMB>() };
				method.Invoke(ToryResultScene.Instance, parameters);
			}
		}

		void ResetEvents()
		{
			// Scene
			System.Type type = ToryScene.Instance.GetType();
			MethodInfo method = type.GetMethod("ResetEvents", (BindingFlags.NonPublic | BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryScene.Instance, null);
			}

			// Title
			type = ToryTitleScene.Instance.GetType();
			method = type.GetMethod("ResetEvents", (BindingFlags.NonPublic | BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryTitleScene.Instance, null);
			}

			// Play
			type = ToryPlayScene.Instance.GetType();
			method = type.GetMethod("ResetEvents", (BindingFlags.NonPublic | BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryPlayScene.Instance, null);
			}

			// Result
			type = ToryResultScene.Instance.GetType();
			method = type.GetMethod("ResetEvents", (BindingFlags.NonPublic | BindingFlags.Instance));
			if (method != null)
			{
				method.Invoke(ToryResultScene.Instance, null);
			}
		}
	}
}