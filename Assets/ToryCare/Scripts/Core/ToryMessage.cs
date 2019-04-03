using System;
using System.Collections;
using System.Collections.Generic;
using OscSimpl;
using UnityEngine;

namespace ToryCare
{
	[RequireComponent(typeof(OscIn), typeof(OscOut))]
	public class ToryMessage : MonoBehaviour
	{
		#region Singleton

		static volatile ToryMessage instance;
		static readonly object syncRoot = new object();

		public static ToryMessage Instance
		{
			get
			{
				if (instance == null)
				{
					lock(syncRoot)
					{
						if (instance == null)
						{
							instance = FindObjectOfType<ToryMessage>();
						}
					}
				}
				return instance;
			}
		}

		#endregion

		#region Fields

		// Osc Receive Address
		// const int oscReceivePort = 6237; -> managed with ToryCare.Config
		const string oscCommandAddress = "/torycare/command"; // Receive some command. ie: restart game
		const string oscMessageAddress = "/torycare/message"; // Receive some message. ie: show network problem alert... 
		const string oscNoticeAddress = "/torycare/notice"; // Show notice. ie: reboot soon message

		// Osc Send Address
		// const int oscSendPort = 6241; -> managed with ToryCare.Config
		const string oscVitalAddress = "/torycare/vital"; // Send vital
		const string oscKeyCodeAddress = "/torycare/keycode"; // Send specific keyboard input

		// TODO : Send same message when ToryLog logs.
		const string oscStatusLogAddress = "/torycare/status";
		const string oscPlayLogAddress = "/torycare/play";
		// const string oscDevLogAddress = "/torycare/dev";
		// const string oscConsoleLogAddress = "/torycare/console";

		OscIn oscReceive;
		OscOut oscSend;

		#endregion

		#region Events

		// public Action<string> onCommandReceived = (string s) =>
		// {};
		// public Action<string> onMessageReceived = (string s) =>
		// {};

		#endregion

		#region Unity Frameworks

		void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}

			oscReceive = GetComponent<OscIn>();
			oscSend = GetComponent<OscOut>();
		}

		void Start()
		{
			try
			{
				oscReceive.Open(ToryCare.Config.ToryMessageOscReceivePort);
				oscReceive.Map(oscCommandAddress, ParseReceivedCommand);
				oscReceive.Map(oscMessageAddress, ParseRecievedMessage);
				oscReceive.Map(oscNoticeAddress, ParseRecievedNotice);

				oscSend.Open(ToryCare.Config.ToryMessageOscSendPort);
			}
			catch (Exception e)
			{
				Debug.LogWarning("Cannot open osc port : " + e);
			}
		}

		void OnEnable()
		{
			if (vitalCoroutine == null)
			{
				vitalCoroutine = StartCoroutine(VitalCoroutine());
			}

			ToryCareBehaviour.Instance.SpecialKeyPressed += SendKeyboardEvent;
		}

		void OnDisable()
		{
			if (vitalCoroutine != null)
			{
				StopCoroutine(vitalCoroutine);
				vitalCoroutine = null;
			}

			ToryCareBehaviour.Instance.SpecialKeyPressed -= SendKeyboardEvent;
		}

		void OnApplicationQuit()
		{
			if (oscReceive.isOpen)
			{
				oscReceive.Close();
			}
			if (oscSend.isOpen)
			{
				oscSend.Close();
			}
		}

		#endregion

		#region Private Methods

		Coroutine vitalCoroutine;
		IEnumerator VitalCoroutine()
		{
			while (true)
			{
				// Debug for build test ToryCareGameApp.
				// Should be called when exported to UnityPackage.
				// if (GameController.Instance.sendVital.Value)
				{
					SendVital();
				}
				yield return new WaitForSeconds(Config.SendVitalPeriod);
			}
		}

		// Send Time.realtimeSinceStartup as vital sign.
		void SendVital()
		{
			OscMessage message = new OscMessage(oscVitalAddress);
			message.Add(Time.realtimeSinceStartup);
			oscSend.Send(message);

			// Debug.Log(message.ToString());
		}

		// Send some keyboard event such as : F1, F2, ...
		void SendKeyboardEvent(KeyCode keyCode)
		{
			OscMessage message = new OscMessage(oscKeyCodeAddress);
			message.Add(keyCode.ToString());
			oscSend.Send(message);

			Debug.LogFormat("Sending keyboard event with ToryMessage : {0}", message.ToString());
		}

		// TODO
		void ParseReceivedCommand(OscMessage message)
		{
			OscPool.Recycle(message);
		}

		// TODO
		void ParseRecievedMessage(OscMessage message)
		{
			OscPool.Recycle(message);
		}

		void ParseRecievedNotice(OscMessage message)
		{
			string type = "";
			if (message.TryGet(0, ref type))
			{
				byte[] data = new byte[0];
				switch (type)
				{
					case "text":
						if (message.TryGet(1, ref data))
						{
							int duration = 10;
							message.TryGet(2, out duration);
							ToryUX.ToryNotification.Instance.Show(System.Text.Encoding.UTF8.GetString(data), duration);
						}
						break;

					case "backgroundColor":
						if (message.TryGet(1, ref data))
						{
							if (data.Length >= 4)
							{
								ToryUX.ToryNotification.Instance.SetBackgroundColor(new Color32(data[0], data[1], data[2], data[3]));
							}
						}
						break;

					case "textColor":
						if (message.TryGet(1, ref data))
						{
							if (data.Length >= 4)
							{
								ToryUX.ToryNotification.Instance.SetTextColor(new Color32(data[0], data[1], data[2], data[3]));
							}
						}
						break;

					default:
						break;
				}
			}
			OscPool.Recycle(message);
		}

		#endregion

		#region Public Methods

		public void SendStatusLog(params string[] log)
		{
			OscMessage message = new OscMessage(oscStatusLogAddress);
			foreach (string s in log)
			{
				message.Add(s);
			}
			oscSend.Send(message);

			// Debug.Log(message.ToString());
		}

		public void SendPlayLog(params string[] log)
		{
			OscMessage message = new OscMessage(oscPlayLogAddress);
			foreach (string s in log)
			{
				message.Add(s);
			}
			oscSend.Send(message);

			// Debug.Log(message.ToString());
		}

		#endregion
	}
}