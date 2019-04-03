namespace ToryValue 
{
	public static class KeyFormatter
	{
		readonly static string keyPrefix = "";

		// Not used anymore.
		//readonly static string defaultKeySuffix = "_Default";

		readonly static string savedKeySuffix = "";

		// Not used anymore.
		//public static string GetDefaultKey(string key)
		//{
		//	return keyPrefix + key + defaultKeySuffix;
		//}

		public static string GetSavedKey(string key)
		{
			return keyPrefix + key + savedKeySuffix;
		}
	}
}