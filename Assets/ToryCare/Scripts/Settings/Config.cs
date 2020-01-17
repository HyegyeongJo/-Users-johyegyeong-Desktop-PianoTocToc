using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvoEx;
using UnityEngine;

namespace ToryCare
{
	public class Config
	{
		#region Singleton
		static volatile Config instance;
		static readonly object syncRoot = new object();

		public static Config Instance
		{
			get
			{
				if (instance == null)
				{
					lock(syncRoot)
					{
						if (instance == null)
						{
							instance = new Config();
						}
					}
				}
				return instance;
			}
		}
		#endregion

		ConfigJson json;

		#region Data Path

		const string configFilename = "SharedSettings.json";

		static string configFileDirectory = "";
		static string ConfigFileDirectory
		{
			get
			{
				if (string.IsNullOrEmpty(configFileDirectory))
				{
					configFileDirectory = Path.Combine(DataRootDirectory, "ToryCare");
					if (!Directory.Exists(configFileDirectory))
					{
						Directory.CreateDirectory(configFileDirectory);
					}
				}
				return configFileDirectory;
			}
		}
		static string ConfigFilePath
		{
			get
			{
				return Path.Combine(ConfigFileDirectory, configFilename);
			}
		}

		// Root data directory for ToryCare and ToryFramework
		private static string dataRootDirectory = "";
		public static string DataRootDirectory
		{
			get
			{
				if (string.IsNullOrEmpty(dataRootDirectory))
				{
#if UNITY_ANDROID && !UNITY_EDITOR
					dataRootDirectory = Path.Combine(Application.persistentDataPath, "ToryCare");
#elif UNITY_IOS && !UNITY_EDITOR
					dataRootDirectory = Application.persistentDataPath;
#else
					dataRootDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Envisible");
#endif

					if (!Directory.Exists(dataRootDirectory))
					{
						Directory.CreateDirectory(dataRootDirectory);
					}

#if !UNITY_IOS
					dataRootDirectory = Path.Combine(dataRootDirectory, Application.identifier);
					if (!Directory.Exists(dataRootDirectory))
					{
						Directory.CreateDirectory(dataRootDirectory);
					}
#endif
				}
				return dataRootDirectory;
			}
		}

		// ToryLog directory
		private static string toryLogDirectory = "";
		public static string ToryLogDirectory
		{
			get
			{
				if (string.IsNullOrEmpty(toryLogDirectory))
				{
					toryLogDirectory = Path.Combine(DataRootDirectory, "ToryLog");
					if (!Directory.Exists(toryLogDirectory))
					{
						Directory.CreateDirectory(toryLogDirectory);
					}
				}
				return toryLogDirectory;
			}
		}

		// Toryward directory
		private static string torywardDirectory = "";
		public static string TorywardDirectory
		{
			get
			{
				if (string.IsNullOrEmpty(torywardDirectory))
				{
					torywardDirectory = Path.Combine(DataRootDirectory, "Toryward");
					if (!Directory.Exists(torywardDirectory))
					{
						Directory.CreateDirectory(torywardDirectory);
					}
				}
				return torywardDirectory;
			}
		}

#endregion

#region ToryCare

		// ToryLog vital period
		public static int SendVitalPeriod
		{
			get
			{
				return Mathf.Max(1, Instance.json.sendVitalPeriod);
			}
		}

#endregion

#region Toryward

		// Toryward appery DBID
		private static string torywardApperyDatabaseID = "";
		public static string TorywardApperyDatabaseID
		{
			get
			{
				if (string.IsNullOrEmpty(torywardApperyDatabaseID))
				{
					torywardApperyDatabaseID = AesEncryptor.DecryptString(Instance.json.torywardApperyDatabaseID);
				}

				return torywardApperyDatabaseID;
			}
		}

		// Toryward appery master key
		private static string torywardApperyMasterKey = "";
		public static string TorywardApperyMasterKey
		{
			get
			{
				if (string.IsNullOrEmpty(torywardApperyMasterKey))
				{
					torywardApperyMasterKey = AesEncryptor.DecryptString(Instance.json.torywardApperyMasterKey);
				}
				return torywardApperyMasterKey;
			}
		}

		public static string TorywardFilename
		{
			get
			{
				return Instance.json.torywardFilename;
			}
		}

		public static string TorywardImageFormat
		{
			get
			{
				return Instance.json.torywardImageFormat;
			}
		}

		// Do upload Toryward entry to server in Unity?
		public static bool DoUploadTorywardEntry
		{
			get
			{
				return Instance.json.doUploadTorywardEntryFromUnity;
			}
		}

		// Toryward min rank to upload entry to server
		public static int TorywardUploadEntryLimitRank
		{
			get
			{
				return Mathf.Min(10, Instance.json.torywardUploadEntryLimitRank);
			}
		}

		public static float TorywardTakePhotoDelay
		{
			get
			{
				return Mathf.Max(1f, Instance.json.torywardTakePhotoDelay);
			}
		}

		public static bool PlayBackgroundWebcam
		{
			get
			{
				return Instance.json.playBackgroundWebcam;
			}
		}

#if TORY_FRAMEWORK
		public static bool AutoTakeTorywardPhoto
		{
			get
			{
				return Instance.json.autoTakeTorywardPhoto;
			}
		}
#endif

		public static string BranchCode
		{
			get
			{
				return Instance.json.branchCode;
			}
			set
			{
				Instance.json.branchCode = value;
			}
		}

		public static string ContentName
		{
			get
			{
				return Instance.json.contentName;
			}
			set
			{
				Instance.json.contentName = value;
			}
		}

		// Do make up Toryward entries in Unity?
		public static bool DoMakeUpToryward
		{
			get
			{
				return Instance.json.doMakeUpLocalTorywardEntriesInUnity;
			}
		}

		// Toryward minimum record when make up
		public static int TorywardMinRecordCount
		{
			get
			{
				return Mathf.Max(5, Instance.json.torywardMinRecordCount);
			}
		}

		// Toryward max record when make up
		public static int TorywardMaxRecordCount
		{
			get
			{
				return Mathf.Max(5, Instance.json.torywardMaxRecordCount);
			}
		}

		// Toryward maximum life of record as day
		public static int TorywardRecordLifeAsDay
		{
			get
			{
				return Mathf.Max(1, Instance.json.torywardRecordLifeAsDay);
			}
		}

#endregion

#region Reward Card

		public static bool ShowRewardCardSequence
		{
			get
			{
				return Instance.json.showRewardCardSequence;
			}
		}
		public static int LimitRankForShowRewardCard
		{
			get
			{
				return Mathf.Max(1, Instance.json.limitRankForShowRewardCard);
			}
		}
		public static int MinScoreForShowRewardCard
		{
			get
			{
				return Instance.json.minScoreForShowRewardCard;
			}
			set
			{
				Instance.json.minScoreForShowRewardCard = value;
			}
		}

		public static int RewardCardShowDuration
		{
			get
			{
				return Mathf.Max(5, Instance.json.rewardCardShowDuration);
			}
		}

		public static string RewardCardImageFilename
		{
			get
			{
				return Instance.json.rewardCardImageFilename;
			}
		}

#endregion

#region Sound Control

		public static float MasterVolume
		{
			get
			{
				return Mathf.Clamp01(Instance.json.masterVolume);
			}
			set
			{
				Instance.json.masterVolume = value;
			}
		}

		public static float BgmVolume
		{
			get
			{
				return Mathf.Clamp01(Instance.json.bgmVolume);
			}
			set
			{
				Instance.json.bgmVolume = value;
			}
		}

		public static float SfxVolume
		{
			get
			{
				return Mathf.Clamp01(Instance.json.sfxVolume);
			}
			set
			{
				Instance.json.sfxVolume = value;
			}
		}

#endregion

#region Network Settings

		public static int ToryMessageOscReceivePort
		{
			get
			{
				return Instance.json.oscCareToUnityPort;
			}
		}

		public static int ToryMessageOscSendPort
		{
			get
			{
				return Instance.json.oscUnityToCarePort;
			}
		}

		public static bool DoNetworkPingTest
		{
			get
			{
				return Instance.json.doNetworkPingTest;
			}
		}

		public static string[] NetworkTestPingIP
		{
			get
			{
				return Instance.json.networkPingTestIP;
			}
		}

		public static int NetworkPingTestFailDelay
		{
			get
			{
				return Instance.json.networkPingTestFailDelay;
			}
		}

#endregion

		public static string[] ObservingKeycode
		{
			get
			{
				return Instance.json.observingKeycode;
			}
		}

		Config()
		{
			if (File.Exists(ConfigFilePath))
			{
				try
				{
					json = JsonUtility.FromJson<ConfigJson>(File.ReadAllText(ConfigFilePath));
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
			else
			{
				json = new ConfigJson();
			}

#if UNITY_EDITOR
			json.LoadFromEditorInspector();
#endif

			SaveToFile();
		}

		public bool SaveToFile()
		{
			try
			{
				File.WriteAllText(ConfigFilePath, JsonUtility.ToJson(json, true));
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				return false;
			}

			return true;
		}
	}

	[Serializable]
	public class ConfigJson
	{
		// appery db setting (encrypted)
		public string torywardApperyDatabaseID = AesEncryptor.Encrypt("5b7e304200c48f6fa7cfa8c2");
		public string torywardApperyMasterKey = AesEncryptor.Encrypt("cc288fa3-c4c4-4096-b89b-36efbb1658b5");

		// vital sign setting
		public int sendVitalPeriod = 1;

		// toryward local file setting
		public string torywardFilename = "Leaderboard.json";
		public string torywardImageFormat = "LeaderboardPhoto_{0}.png";

		// toryward basic setting
		public bool doUploadTorywardEntryFromUnity = true;
		public int torywardUploadEntryLimitRank = 5;

#if UNITY_ANDROID
		public float torywardTakePhotoDelay = 2.5f;
#else
		public float torywardTakePhotoDelay = 1f;
#endif

		public bool playBackgroundWebcam = true;

#if TORY_FRAMEWORK
		public bool autoTakeTorywardPhoto = false;
#endif

		public string branchCode = "";
		public string contentName = "한글콘텐츠명";

		// toryward make up setting
		public bool doMakeUpLocalTorywardEntriesInUnity = true;
		public int torywardMinRecordCount = 20;
		public int torywardMaxRecordCount = 100;
		public int torywardRecordLifeAsDay = 7;

		// reward card setting
		public bool showRewardCardSequence = true;
		public int limitRankForShowRewardCard = 5;
		public int minScoreForShowRewardCard = 1;
		public int rewardCardShowDuration = 15;
		public string rewardCardImageFilename = "RewardCard.png";

		// volume setting
		public float masterVolume = .7f;
		public float bgmVolume = .7f;
		public float sfxVolume = .7f;

		// osc port setting for torycare
		public int oscCareToUnityPort = 6237;
		public int oscUnityToCarePort = 6241;

		// network ping test setting
		public bool doNetworkPingTest = true;
		public string[] networkPingTestIP = {
			"8.8.8.8", // google-public-dns.a.google.com
			"8.8.4.4" // google-public-dns.b.google.com
		};
		public int networkPingTestFailDelay = 5;

		// keycode for send to torycare app setting
		public string[] observingKeycode = {
			"F11",
			"F12",
		};

		public ConfigJson()
		{
			LoadFromEditorInspector();
		}

		public void LoadFromEditorInspector()
		{
			if (ToryCareBehaviour.Instance != null)
			{
				contentName = ToryCareBehaviour.Instance.contentNameInKorean;
			}

#if TORY_FRAMEWORK
			if (ToryUX.TorywardManager.Instance != null)
			{
				autoTakeTorywardPhoto = ToryUX.TorywardManager.Instance.autoTakePhotoOnGameFinish;
			}
#endif
		}
	}
}