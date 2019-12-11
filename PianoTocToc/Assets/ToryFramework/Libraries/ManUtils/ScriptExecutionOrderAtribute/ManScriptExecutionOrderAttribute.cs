using System;

namespace ManUtils
{
	// Script execution order attribute reference: https://github.com/kwnetzwelt/ugb-source/blob/UGB-3.0/UnityGameBase/ScriptExecutionOrderAttribute.cs
	// Using it reference: http://minhhh.github.io/posts/when-to-use-script-execution-order
	public class ManScriptExecutionOrder : Attribute
	{
		public int value;

		public ManScriptExecutionOrder(int id)
		{
			this.value = id;
		}
	}
}