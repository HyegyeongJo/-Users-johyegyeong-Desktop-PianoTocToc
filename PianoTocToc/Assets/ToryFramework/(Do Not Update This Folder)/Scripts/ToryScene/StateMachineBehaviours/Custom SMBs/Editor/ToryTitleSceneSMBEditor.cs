using UnityEditor;
using ToryFramework.SMB;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(ToryTitleSceneSMB))]
	[CanEditMultipleObjects]
	public class ToryTitleSceneSMBEditor : ToryCustomSceneSMBEditor
	{
		#region PROPERTIES

		protected override string Name 			{ get { return "TITLE"; }}

		#endregion
	}
}