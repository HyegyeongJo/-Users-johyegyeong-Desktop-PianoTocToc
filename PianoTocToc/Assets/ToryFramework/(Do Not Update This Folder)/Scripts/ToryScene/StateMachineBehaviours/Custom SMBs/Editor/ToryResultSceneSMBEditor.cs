using UnityEditor;
using ToryFramework.SMB;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(ToryResultSceneSMB))]
	[CanEditMultipleObjects]
	public class ToryResultSceneSMBEditor : ToryCustomSceneSMBEditor
	{
		#region PROPERTIES

		protected override string Name 			{ get { return "RESULT"; }}

		#endregion
	}
}