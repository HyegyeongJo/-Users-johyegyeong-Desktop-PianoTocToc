using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
	public enum ShieldTVRemoteButton
	{
		None,
		Confirm,
		Back,
		Voice,
		Up,
		Down,
		Left,
		Right
	}

	public static class ShieldTVRemoteInputExtensionMethods
	{
		public static KeyCode ToKeyCode(this ShieldTVRemoteButton button)
		{
			switch (button)
			{
			case ShieldTVRemoteButton.Confirm:
				return KeyCode.Joystick1Button0;
			case ShieldTVRemoteButton.Back:
				return KeyCode.Escape;
			case ShieldTVRemoteButton.Voice:
				return KeyCode.F2;
			case ShieldTVRemoteButton.Up:
				return KeyCode.UpArrow;
			case ShieldTVRemoteButton.Down:
				return KeyCode.DownArrow;
			case ShieldTVRemoteButton.Left:
				return KeyCode.LeftArrow;
			case ShieldTVRemoteButton.Right:
				return KeyCode.RightArrow;
			case ShieldTVRemoteButton.None:
			default:
				return KeyCode.None;
			}
		}
	}
}