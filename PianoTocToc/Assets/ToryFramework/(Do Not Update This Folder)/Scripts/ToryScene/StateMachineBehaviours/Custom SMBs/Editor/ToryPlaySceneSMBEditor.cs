using UnityEditor;
using ToryFramework.SMB;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(ToryPlaySceneSMB))]
	[CanEditMultipleObjects]
	public class ToryPlaySceneSMBEditor : ToryCustomSceneSMBEditor
	{
		#region PROPERTIES

		protected override string Name 			{ get { return "PLAY"; }}

		#endregion
	}
}