using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LitJson;
using UnityEngine;

namespace ToryCare
{
	public enum LogType
	{
		STATUS = 0, // Log for application and ToryEngine status. ie: Application Start, Play End, Idle Start...
		PLAY, // Log for game play events. ie: Game Start, Player Left, Game Cleared, 
		DEV, // TODO : Log for dev message.
		CONSOLE // TODO : Cloned log for console message.
	}

	public class ToryLog : MonoBehaviour
	{
		#region Singleton

		static volatile ToryLog instance;
		static readonly object syncRoot = new object();

		public static ToryLog Instance
		{
			get
			{
				if (instance == null)
				{
					lock(syncRoot)
					{
						if (instance == null)
						{
							instance = FindObjectOfType<ToryLog>();
						}
					}
				}
				return instance;
			}
		}

		#endregion

		#region Fields

		Dictionary<LogType, JSONObject> jsons;

		float applicationStartTime;
		float gameStartTime;
		bool toryLogFilePrepared = false;

		#endregion

		#region Properties

		// Flag for holding if application is started or new toryScene is loaded after game finished.
		private bool IsApplicationIdle
		{
			get
			{
				return PlayerPrefs.GetInt(MethodBase.GetCurrentMethod().Name.Substring(4)) > 0;
			}
			set
			{
				PlayerPrefs.SetInt(MethodBase.GetCurrentMethod().Name.Substring(4), value? 1 : 0);
			}
		}

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

			jsons = new Dictionary<LogType, JSONObject>();

			// TF.Scene.Interacted += OnPlayerInteracted;
		}

		IEnumerator Start()
		{
			CheckDataDirectories();
			yield return new WaitUntil(() => System.IO.Directory.Exists(Config.ToryLogDirectory));

			LoadToryLogFiles();
			yield return new WaitForEndOfFrame();

			toryLogFilePrepared = true;

			// If application is just started, assume application is idle and log application started.
			if (Time.timeSinceLevelLoad < 1f)
			{
				IsApplicationIdle = true;
				LogApplicationStarted();
			}

		}

		#if TORY_FRAMEWORK

		void OnEnable()
		{
			TF.Scene.PlayerLeft += OnPlayerLeft;
			TF.Scene.Interacted += OnPlayerInteracted;
			TF.Scene.First.Ended += OnGameStarted;
			TF.Scene.Play.Ended += OnGameFinished;
		}

		void OnDisable()
		{
			TF.Scene.PlayerLeft -= OnPlayerLeft;
			TF.Scene.Interacted -= OnPlayerInteracted;
			TF.Scene.First.Ended -= OnGameStarted;
			TF.Scene.Play.Ended -= OnGameFinished;
		}

		#endif

		void OnApplicationQuit()
		{
			LogApplicationEnded();
		}

		#endregion

		#region Private Methods

		void CheckDataDirectories()
		{
			string checkFolder = Config.ToryLogDirectory;
			if (!System.IO.Directory.Exists(checkFolder))
			{
				Debug.Log(string.Format("ToryLog folder is not exist. Create folder {0}", checkFolder));
				System.IO.Directory.CreateDirectory(checkFolder);
			}

			foreach (LogType type in System.Enum.GetValues(typeof(LogType)))
			{
				string subFolderName = Utils.ToFirstCharUpperLowerCase(type.ToString());
				checkFolder = Config.ToryLogDirectory + Path.DirectorySeparatorChar + subFolderName;
				if (!System.IO.Directory.Exists(checkFolder))
				{
					Debug.Log(string.Format("ToryLog {0} folder is not exist. Create folder {1}", subFolderName, checkFolder));
					System.IO.Directory.CreateDirectory(checkFolder);
				}
			}
		}

		void LoadToryLogFiles()
		{
			foreach (LogType type in System.Enum.GetValues(typeof(LogType)))
			{
				if (File.Exists(GetJsonFilePath(type)))
				{
					jsons[type] = new JSONObject(File.ReadAllText(GetJsonFilePath(type)));
				}
				else
				{
					jsons[type] = new JSONObject(JSONObject.Type.OBJECT);
					jsons[type].AddField("Data", new JSONObject(JSONObject.Type.ARRAY));
				}
			}
		}

		void Write(LogType type, params string[] log)
		{
			if (!toryLogFilePrepared || log.Length < 1)
			{
				return;
			}

			JSONObject nodeData = CreateNodeData(type, log);

			if (nodeData.Count > 0)
			{
				jsons[type].GetField("Data").Add(nodeData);
				File.WriteAllText(GetJsonFilePath(type), jsons[type].ToString(true));

				// Debug.Log(nodeData);
			}

			// Send ToryMessage too.
			switch (type)
			{
				case LogType.STATUS:
					ToryMessage.Instance.SendStatusLog(log);
					break;
				case LogType.PLAY:
					ToryMessage.Instance.SendPlayLog(log);
					break;
				default:
					break;
			}
		}

		JSONObject CreateNodeData(LogType type, params string[] log)
		{
			JSONObject node = new JSONObject(JSONObject.Type.OBJECT);

			try
			{
				switch (type)
				{
					case LogType.STATUS:
						{
							node.AddField("Type", log[0]);
							node.AddField("Log", log[1]);
							node.AddField("RunningTime", GetEllapsedTimeStringFrom(applicationStartTime));
							node.AddField("When", DateTime.Now.ToString());
						}
						break;

					case LogType.PLAY:
						{
							node.AddField("Log", log[0]);
							node.AddField("Scene", log[1]);
							node.AddField("PlayTime", GetEllapsedTimeStringFrom(gameStartTime));
							node.AddField("Score", log[2]);
							node.AddField("When", DateTime.Now.ToString());
						}
						break;

					default:
						break;
				}
			}
			catch (NullReferenceException)
			{
				Debug.LogWarning("param log[] lenght is less than expected. Cannot write " + type.ToString());
			}

			return node;
		}

		void WritePlayLog(string logText)
		{
			#if TORY_FRAMEWORK
			Write(LogType.PLAY, logText, ToryFramework.ToryScene.Instance.Current.ToString(), ToryUX.Score.CurrentScorePoint.ToString());
			#else
			Write(LogType.PLAY, logText, "Non-ToryScene", ToryUX.Score.CurrentScorePoint.ToString());
			#endif
		}

		string GetJsonFilePath(LogType type)
		{
			string filename = string.Format("{0}_{1}.json", type.ToString().ToLower(), DateTime.Today.ToString("yyMMdd"));
			return Path.Combine(Config.ToryLogDirectory, Path.Combine(Utils.ToFirstCharUpperLowerCase(type.ToString()), filename));
		}

		string GetEllapsedTimeStringFrom(float referenceTime)
		{
			TimeSpan t = TimeSpan.FromSeconds(Time.timeSinceLevelLoad - referenceTime);
			return string.Format("{0:D2}h {1:D2}m {2:D2}s", t.Hours, t.Minutes, t.Seconds);
		}

		void OnPlayerLeft()
		{
			if (!IsApplicationIdle) // If application was playing,
			{
				IsApplicationIdle = true;
				LogPlayerLeft();
				LogPlayEnded();
				LogIdleStarted();
			}
		}

		void OnPlayerInteracted()
		{
			if (IsApplicationIdle) // If application was idle,
			{
				IsApplicationIdle = false;
				LogIdleEnded();
				LogPlayStarted();
				LogPlayerJoined();
			}

			// TODO : Log user interaction if necessary.
		}
		void OnGameStarted()
		{
			LogGameStarted();
		}

		void OnGameFinished()
		{
			LogGameFinished();
		}

		#endregion

		#region Public Methods

		public void LogApplicationStarted()
		{
			applicationStartTime = Time.timeSinceLevelLoad;
			Write(LogType.STATUS, "Application", "Start");
		}

		public void LogApplicationEnded()
		{
			if (IsApplicationIdle)
			{
				LogIdleEnded();
			}
			else
			{
				LogPlayEnded();
			}
			Write(LogType.STATUS, "Application", "End");
		}

		public void LogPlayStarted()
		{
			Write(LogType.STATUS, "Play", "Start");
		}

		public void LogPlayEnded()
		{
			Write(LogType.STATUS, "Play", "End");
		}

		public void LogIdleStarted()
		{
			Write(LogType.STATUS, "Idle", "Start");
		}

		public void LogIdleEnded()
		{
			Write(LogType.STATUS, "Idle", "End");
		}

		public void LogPlayerJoined()
		{
			WritePlayLog("PlayerJoin");
		}

		public void LogPlayerLeft()
		{
			WritePlayLog("PlayerLeft");
		}

		public void LogGameStarted()
		{
			gameStartTime = Time.timeSinceLevelLoad;
			WritePlayLog("GameStart");
		}

		public void LogGameFinished()
		{
			WritePlayLog("GameFinish");
		}

		public void LogGameCleared()
		{
			WritePlayLog("GameFail");
		}

		public void LogGameFailed()
		{
			WritePlayLog("GameFail");
		}

		#endregion
	}
}