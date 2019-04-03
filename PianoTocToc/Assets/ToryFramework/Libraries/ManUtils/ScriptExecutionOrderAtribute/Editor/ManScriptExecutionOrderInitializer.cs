using System;
using UnityEngine;
using UnityEditor;

namespace ManUtils
{
	// Setting script execution order reference: https://github.com/kwnetzwelt/ugb-source/blob/UGB-3.0/UnityGameBase/Core/Editor/GameScriptExecutionOrder.cs
	// Using it reference: http://minhhh.github.io/posts/when-to-use-script-execution-order
	[InitializeOnLoad]
	class ManScriptExecutionOrderInitializer
	{
		static ManScriptExecutionOrderInitializer()
		{
			// Get all MonoScripts.
			foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
			{
				var currentClass = monoScript.GetClass();
				if (currentClass != null)
				{
					// Find the appropriate script.
					foreach (var attribute in Attribute.GetCustomAttributes(currentClass, typeof(ManScriptExecutionOrder)))
					{
						int currentOrder = MonoImporter.GetExecutionOrder(monoScript);
						int targetOrder = ((ManScriptExecutionOrder)attribute).value;

						if (currentOrder != targetOrder)
						{
							// Set the order.
							MonoImporter.SetExecutionOrder(monoScript, targetOrder);

							// Log
							Debug.Log("The script execution order of \"" + currentClass + "\" changed from " + currentOrder + " to " + targetOrder + ".");
						}
					}
				}
			}
		}
	}
}