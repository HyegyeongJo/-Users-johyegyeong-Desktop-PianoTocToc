using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
	public class SettingsUIHider : MonoBehaviour
	{
		public void HideSettingsUI()
		{
			SettingsUI.Hide();
		}
	}
}