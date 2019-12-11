using UnityEngine;
using UnityEditor;

public class ToryFrameworkScriptingDefineSymbolGenerator : Editor
{
	/// <summary>
	/// Adds a symbol to the scripting define symbols when Unity editor is loaded.
	/// </summary>
	[InitializeOnLoadMethod]
	static void AddScriptingDefineSymbol()
	{
		const string symbol = "TORY_FRAMEWORK";

		string scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
		if (!scriptingDefineSymbols.Contains(symbol))
		{
			PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, 
			                                                     scriptingDefineSymbols + ";" + symbol);

			// Console
			Debug.Log("\"" + symbol + "\" symbol added to the scripting define symbols.\n" +
			          "Current symbols: " + 
			          PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup) + ".");
		}
	}
}