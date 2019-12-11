using UnityEngine;

namespace ToryValue 
{
	public static class SecureKeysChecker
	{
		public static SecureKeysManager secureKeysManager;

		public static bool CheckSecureKeys()
		{
			if (secureKeysManager == null)
			{
				secureKeysManager = Object.FindObjectOfType<SecureKeysManager>();
				if (secureKeysManager == null)
				{
					Debug.LogError("ToryValue needs a SecureKeysManager component in your scene. " +
					               "So please add the component manually, or add the SecureKeysManager prefab, which includes it, in your scene.");
					return false;
				}
				if (secureKeysManager.keys.Length == 0)
				{
					Debug.LogError("Please generate at least one key in the SecureKeysManager component in your scene.");
					return false;
				}
				if (PlayerPrefsElite.key == null)
				{
					PlayerPrefsElite.setKeys(secureKeysManager.keys);
				}
			}
			else
			{
				if (secureKeysManager.keys.Length == 0)
				{
					Debug.LogError("Please generate at least one key in the SecureKeysManager component in your scene.");
					return false;
				}
				if (PlayerPrefsElite.key == null)
				{
					PlayerPrefsElite.setKeys(secureKeysManager.keys);
				}
			}
			return true;
		}
	}
}

