using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
	public class ResetRecordSetter : MonoBehaviour
	{
		public void ResetRecord()
		{
			Leaderboard.Clear();
		}
	}
}