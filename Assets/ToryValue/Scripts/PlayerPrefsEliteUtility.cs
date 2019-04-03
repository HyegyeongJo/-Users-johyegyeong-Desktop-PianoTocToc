namespace ToryValue 
{
	public static class PlayerPrefsEliteUtility
	{
		static SecureKeysManager secureKeysManager;

		public static void MakePlayerPrefsEliteAvailableInEditMode()
		{
#if UNITY_EDITOR
			try
			{
				TryToMakeItAvailable();
			}
			catch (System.Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
#endif
		}

		static void TryToMakeItAvailable()
		{
			if (!HasSecureKeysManager())
			{
				secureKeysManager = FindSecureKeysManagerInScene();
			}
			CopyKeyFromSecureKeysManagerToPlayerPrefsElite();
		}

		static bool HasSecureKeysManager()
		{
			return (secureKeysManager != null);
		}

		static SecureKeysManager FindSecureKeysManagerInScene()
		{
			SecureKeysManager found = UnityEngine.Object.FindObjectOfType<SecureKeysManager>();
			if (found == null)
			{
				throw new System.NullReferenceException("Three is no SecureKeysManager component in your scene. " +
				                                        "Please add the component manually, " +
				                                        "or add the SecureKeysManager prefab in your scene to use the ToryValue.");
			}
			return found;
		}

		static void CopyKeyFromSecureKeysManagerToPlayerPrefsElite()
		{
			if (SecureKeysManagerHasKey() && !PlayerPrefsEliteHasKey())
			{
				SetKeysOfPlayerPrefsElite();
			}
		}

		static bool SecureKeysManagerHasKey()
		{
			bool hasKey = (secureKeysManager.keys.Length > 0);
			if (!hasKey)
			{
				throw new System.Exception("There is no key in the SecureKeysManager. " +
				                           "Please generate at least one key in the SecureKeysManager component in your scene to use the ToryValue.");
			}
			return hasKey;
		}

		static bool PlayerPrefsEliteHasKey()
		{
			return (PlayerPrefsElite.key != null && PlayerPrefsElite.key.Length > 0);
		}

		static void SetKeysOfPlayerPrefsElite()
		{
			PlayerPrefsElite.setKeys(secureKeysManager.keys);
		}
	}
}