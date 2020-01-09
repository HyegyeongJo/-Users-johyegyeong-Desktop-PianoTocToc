using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
	public class InputMonitorForSettingsUI : MonoBehaviour
	{
		// Keyboard inputs.
		[Header("Keyboard Inputs")]
		public KeyCode keyForBasicSettings = KeyCode.Tab;
		public KeyCode[] keyCombinationForAdvancedSettings = new KeyCode[5]
		{
			KeyCode.S,
				KeyCode.Y,
				KeyCode.S,
				KeyCode.O,
				KeyCode.P
		};
		private int keyCombiIndex = 0;
		private float lastKeyCombiTime = 0;
		private const float keyCombiLife = 1f;

		// Mouse inputs.
		[System.Flags]
		public enum HotCorner
		{
			NA = 0,

			TopLeft = (1 << 0),
			TopRight = (1 << 1),
			BottomRight = (1 << 2),
			BottomLeft = (1 << 3),

			Top = (1 << 0) + (1 << 1),
			Right = (1 << 1) + (1 << 2),
			Bottom = (1 << 2) + (1 << 3),
			Left = (1 << 0) + (1 << 3),

			AnyCorner = (1 << 0) + (1 << 1) + (1 << 2) + (1 << 3)
		}

		[System.Serializable]
		public class HotCornerMouseInput
		{
			public HotCorner hotCorner;
			public int numberOfClicks;

			public bool Contains(HotCornerMouseInput other)
			{
				return (hotCorner != HotCorner.NA) && ((hotCorner & other.hotCorner) == other.hotCorner) && (numberOfClicks == other.numberOfClicks);
			}

			public bool BelongsTo(HotCornerMouseInput other)
			{
				return (hotCorner != HotCorner.NA) && ((hotCorner & hotCorner) == other.hotCorner) && (numberOfClicks == other.numberOfClicks);
			}
		}
		private const float cornerCoeff = 0.2f;
		private HotCornerMouseInput lastMouseInput = new HotCornerMouseInput();
		private int lastMouseInputFrame = 0;
		private float lastMouseInputTime = 0;
		private const float mouseInputLife = 0.5f;

		[Header("Mouse Inputs")]
		public bool autoShowAndHidePointer = true;
		private Vector3 lastCursorPosition;
		private float lastShowCursorInputTime = 0;
		private const float timeToHideCursor = 3f;

		public HotCornerMouseInput mouseInputForBasicSettings;
		public HotCornerMouseInput[] mouseInputCombinationForAdvancedSettings;
		private int mouseCombiIndex = 0;
		private float lastMouseCombiTime = 0;
		private const float mouseCombiLife = 2f;

#if UNITY_ANDROID

		// Shield TV Remote inputs.
		[Header("ShieldTV Remote Inputs")]
		public ShieldTVRemoteButton stvButtonForBasicSettings = ShieldTVRemoteButton.Back;
		public ShieldTVRemoteButton[] stvButtonCombinationForAdvancedSettings = new ShieldTVRemoteButton[4]
		{
			ShieldTVRemoteButton.Up,
				ShieldTVRemoteButton.Down,
				ShieldTVRemoteButton.Left,
				ShieldTVRemoteButton.Right
		};
#pragma warning disable 0414
		private int stvButtonCombiIndex = 0;
		private float lastStvButtonCombiTime = 0;
		private const float stvButtonCombiLife = 1f;

#endif

		void Awake()
		{
			if (autoShowAndHidePointer)
			{
				Cursor.visible = false;
				lastCursorPosition = Input.mousePosition;
			}
		}

		void Update()
		{
			if (autoShowAndHidePointer)
			{
				if (Cursor.visible)
				{
					if (Time.timeSinceLevelLoad > lastShowCursorInputTime + timeToHideCursor)
					{
						lastCursorPosition = Input.mousePosition;
						Cursor.visible = false;
					}
				}
				else
				{
					if (!lastCursorPosition.Equals(Input.mousePosition))
					{
						lastShowCursorInputTime = Time.timeSinceLevelLoad;
						Cursor.visible = true;
					}
				}
			}

			if (SettingsUI.Instance == null)
			{
				return;
			}

			if (SettingsUI.IsShown)
			{
				// ESC to kill settings UI.
				if ((Input.GetKeyDown(keyForBasicSettings) || Input.GetButtonDown("Cancel")) &&
					SettingsUI.IsShown &&
					!SettingsUI.PreventEscapeKeyFromClosingSettingsUI)
				{
					SettingsUI.Hide();
				}
			}
			else
			{
				// Check if the key for basic settings is pressed.
#if UNITY_ANDROID
				if (Input.GetKeyDown(keyForBasicSettings) ||
					Input.GetKeyDown(stvButtonForBasicSettings.ToKeyCode()) ||
					GetHotCornerMouseInput().BelongsTo(mouseInputForBasicSettings))
				{
					SettingsUI.Show();
				}
#else
				if (Input.GetKeyDown(keyForBasicSettings) ||
					GetHotCornerMouseInput().BelongsTo(mouseInputForBasicSettings))
				{
					SettingsUI.Show();
				}
#endif
			}

			// Check key combination input for advanced settings.
			if (keyCombinationForAdvancedSettings.Length > 0)
			{
				if (keyCombiIndex > 0 && Time.realtimeSinceStartup - lastKeyCombiTime > keyCombiLife)
				{
					keyCombiIndex = 0;
				}
				if (Input.GetKeyDown(keyCombinationForAdvancedSettings[keyCombiIndex]))
				{
					keyCombiIndex += 1;
					lastKeyCombiTime = Time.realtimeSinceStartup;
					if (keyCombiIndex >= keyCombinationForAdvancedSettings.Length)
					{
						keyCombiIndex = 0;
						SettingsUI.ShowAdvancedPanel();
					}
				}
				else if (Input.anyKeyDown)
				{
					keyCombiIndex = 0;
				}
			}

			// Check mouse input combination for advanced settings.
			if (mouseInputCombinationForAdvancedSettings.Length > 0)
			{
				if (mouseCombiIndex > 0 && Time.realtimeSinceStartup - lastMouseCombiTime > mouseCombiLife)
				{
					mouseCombiIndex = 0;
				}
				if (GetHotCornerMouseInput().BelongsTo(mouseInputCombinationForAdvancedSettings[mouseCombiIndex]))
				{
					mouseCombiIndex += 1;
					lastMouseCombiTime = Time.realtimeSinceStartup;
					if (mouseCombiIndex >= mouseInputCombinationForAdvancedSettings.Length)
					{
						mouseCombiIndex = 0;
						SettingsUI.ShowAdvancedPanel();
					}
				}
			}

#if UNITY_ANDROID
			// Check Shield TV remote button combination for advanced settings.
			if (stvButtonCombinationForAdvancedSettings.Length > 0)
			{
				if (stvButtonCombiIndex > 0 && Time.realtimeSinceStartup - lastStvButtonCombiTime > stvButtonCombiLife)
				{
					stvButtonCombiIndex = 0;
				}
				if (Input.GetKeyDown(stvButtonCombinationForAdvancedSettings[stvButtonCombiIndex].ToKeyCode()))
				{
					stvButtonCombiIndex += 1;
					lastStvButtonCombiTime = Time.realtimeSinceStartup;
					if (stvButtonCombiIndex >= stvButtonCombinationForAdvancedSettings.Length)
					{
						stvButtonCombiIndex = 0;
						SettingsUI.ShowAdvancedPanel();
					}
				}
			}
#endif
		}

		HotCornerMouseInput GetHotCornerMouseInput()
		{
			if (lastMouseInputFrame == Time.frameCount)
			{
				return lastMouseInput;
			}

			HotCornerMouseInput input = new HotCornerMouseInput();

			bool receivedInput = false;
			Vector2 inputPosition = Vector2.zero;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
			receivedInput = Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
#else
			receivedInput = Input.GetMouseButtonDown(0);
#endif

			if (receivedInput)
			{
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
				inputPosition = Input.touches[0].position;
#else
				inputPosition = Input.mousePosition;
#endif

				if (inputPosition.x < Screen.width * cornerCoeff)
				{
					if (inputPosition.y < Screen.height * cornerCoeff)
					{
						switch (UIOrientationSetter.CurrentOrientation)
						{
							case UIOrientation.Landscape:
								input.hotCorner = HotCorner.BottomLeft;
								break;
							case UIOrientation.LandscapeUpsideDown:
								input.hotCorner = HotCorner.TopRight;
								break;
							case UIOrientation.PortraitLeft:
								input.hotCorner = HotCorner.TopLeft;
								break;
							case UIOrientation.PortraitRight:
								input.hotCorner = HotCorner.BottomRight;
								break;
							default:
								input.hotCorner = HotCorner.NA;
								break;
						}
					}
					else if (inputPosition.y >= Screen.height * (1f - cornerCoeff))
					{
						switch (UIOrientationSetter.CurrentOrientation)
						{
							case UIOrientation.Landscape:
								input.hotCorner = HotCorner.TopLeft;
								break;
							case UIOrientation.LandscapeUpsideDown:
								input.hotCorner = HotCorner.BottomRight;
								break;
							case UIOrientation.PortraitLeft:
								input.hotCorner = HotCorner.TopRight;
								break;
							case UIOrientation.PortraitRight:
								input.hotCorner = HotCorner.BottomLeft;
								break;
							default:
								input.hotCorner = HotCorner.NA;
								break;
						}
					}
					else
					{
						switch (UIOrientationSetter.CurrentOrientation)
						{
							case UIOrientation.Landscape:
								input.hotCorner = HotCorner.Left;
								break;
							case UIOrientation.LandscapeUpsideDown:
								input.hotCorner = HotCorner.Right;
								break;
							case UIOrientation.PortraitLeft:
								input.hotCorner = HotCorner.Top;
								break;
							case UIOrientation.PortraitRight:
								input.hotCorner = HotCorner.Bottom;
								break;
							default:
								input.hotCorner = HotCorner.NA;
								break;
						}
					}
				}
				else if (inputPosition.x >= Screen.width * (1f - cornerCoeff))
				{
					if (inputPosition.y < Screen.height * cornerCoeff)
					{
						switch (UIOrientationSetter.CurrentOrientation)
						{
							case UIOrientation.Landscape:
								input.hotCorner = HotCorner.BottomRight;
								break;
							case UIOrientation.LandscapeUpsideDown:
								input.hotCorner = HotCorner.TopLeft;
								break;
							case UIOrientation.PortraitLeft:
								input.hotCorner = HotCorner.BottomLeft;
								break;
							case UIOrientation.PortraitRight:
								input.hotCorner = HotCorner.TopRight;
								break;
							default:
								input.hotCorner = HotCorner.NA;
								break;
						}
					}
					else if (inputPosition.y >= Screen.height * (1f - cornerCoeff))
					{
						switch (UIOrientationSetter.CurrentOrientation)
						{
							case UIOrientation.Landscape:
								input.hotCorner = HotCorner.TopRight;
								break;
							case UIOrientation.LandscapeUpsideDown:
								input.hotCorner = HotCorner.BottomLeft;
								break;
							case UIOrientation.PortraitLeft:
								input.hotCorner = HotCorner.BottomRight;
								break;
							case UIOrientation.PortraitRight:
								input.hotCorner = HotCorner.TopLeft;
								break;
							default:
								input.hotCorner = HotCorner.NA;
								break;
						}
					}
					else
					{
						switch (UIOrientationSetter.CurrentOrientation)
						{
							case UIOrientation.Landscape:
								input.hotCorner = HotCorner.Right;
								break;
							case UIOrientation.LandscapeUpsideDown:
								input.hotCorner = HotCorner.Left;
								break;
							case UIOrientation.PortraitLeft:
								input.hotCorner = HotCorner.Bottom;
								break;
							case UIOrientation.PortraitRight:
								input.hotCorner = HotCorner.Top;
								break;
							default:
								input.hotCorner = HotCorner.NA;
								break;
						}
					}
				}
				else if (inputPosition.y < Screen.height * cornerCoeff)
				{
					switch (UIOrientationSetter.CurrentOrientation)
					{
						case UIOrientation.Landscape:
							input.hotCorner = HotCorner.Bottom;
							break;
						case UIOrientation.LandscapeUpsideDown:
							input.hotCorner = HotCorner.Top;
							break;
						case UIOrientation.PortraitLeft:
							input.hotCorner = HotCorner.Left;
							break;
						case UIOrientation.PortraitRight:
							input.hotCorner = HotCorner.Right;
							break;
						default:
							input.hotCorner = HotCorner.NA;
							break;
					}
				}
				else if (inputPosition.y >= Screen.height * (1f - cornerCoeff))
				{
					switch (UIOrientationSetter.CurrentOrientation)
					{
						case UIOrientation.Landscape:
							input.hotCorner = HotCorner.Top;
							break;
						case UIOrientation.LandscapeUpsideDown:
							input.hotCorner = HotCorner.Bottom;
							break;
						case UIOrientation.PortraitLeft:
							input.hotCorner = HotCorner.Right;
							break;
						case UIOrientation.PortraitRight:
							input.hotCorner = HotCorner.Left;
							break;
						default:
							input.hotCorner = HotCorner.NA;
							break;
					}
				}
				else
				{
					input.hotCorner = HotCorner.NA;
				}

				if (lastMouseInput.numberOfClicks > 0 &&
					Time.realtimeSinceStartup - lastMouseInputTime <= mouseInputLife &&
					(lastMouseInput.hotCorner & input.hotCorner) == input.hotCorner)
				{
					input.numberOfClicks = lastMouseInput.numberOfClicks + 1;
				}
				else
				{
					input.numberOfClicks = 1;
				}

				lastMouseInput = input;
				lastMouseInputTime = Time.realtimeSinceStartup;
				lastMouseInputFrame = Time.frameCount;
			}
			else
			{
				input.hotCorner = HotCorner.NA;
				input.numberOfClicks = 0;
			}

			return input;
		}
	}
}