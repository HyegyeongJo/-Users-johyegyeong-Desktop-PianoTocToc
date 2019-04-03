using System;
using System.Collections;
using System.Reflection;
using System.Text;
using BestHTTP;
using UnityEngine;

namespace ToryUX
{
	public class TorywardManager : MonoBehaviour
	{
		#region Singleton
		static volatile TorywardManager instance;
		static readonly object syncRoot = new object();

		public static TorywardManager Instance
		{
			get
			{
				if (instance == null)
				{
					lock(syncRoot)
					{
						if (instance == null)
						{
							instance = FindObjectOfType<TorywardManager>();
						}
					}
				}
				return instance;
			}
		}
		#endregion

		// [Header("Appery Setting")]
		// public string userLoginID = "funtory";
		// public string userLoginPW = "house";

		// [Space]
		// string apperyDatabaseID = "5ad82e7b2e22d74827efafd8";	// old DB (Guestory)
		// string apperyMasterKey = "9dec0bb0-447d-4487-8a92-3555540ff242";

		// string apperyDatabaseID = "5b7e304200c48f6fa7cfa8c2";
		// string apperyMasterKey = "cc288fa3-c4c4-4096-b89b-36efbb1658b5";
		// db setting is moved to ToryCare.Config

		public string Record
		{
			get;
			private set;
		}
		public int Rank
		{
			get;
			private set;
		}

		private Texture2D snap;
		public Texture2D Snap
		{
			get
			{
				return snap;
			}
			set
			{
				snap = value;
			}
		}

		private Texture2D clippedSnap;
		public Texture2D ClippedSnap
		{
			get
			{
				return clippedSnap;
			}
			set
			{
				clippedSnap = value;
			}
		}

		public PlayerSnapshotObject playerSnapshotPrefab;

		#if TORY_FRAMEWORK
		public bool autoTakePhotoOnGameFinish = true;
		#endif

		void Awake()
		{
			#region Create Singleton Instance
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
			#endregion
		}

		#if TORY_FRAMEWORK
		void OnEnable()
		{
			if (ToryCare.Config.AutoTakeTorywardPhoto)
			{
				TF.Scene.Play.Ended += TakeSnap;
			}
		}

		void OnDisable()
		{
			if (ToryCare.Config.AutoTakeTorywardPhoto)
			{
				TF.Scene.Play.Ended -= TakeSnap;
			}
		}
		#endif

		#if !UNITY_EDITOR
		void Start()
		{
			if (string.IsNullOrEmpty(ToryCare.Config.BranchCode))
			{
				if (ToryUX.SettingsUI.Instance != null)
				{
					Debug.LogWarning("Branch Code is not set. Please insert Branch Code.");
					ToryUX.SettingsUI.ShowAdvancedPanel();
				}
			}
		}
		#endif

		// Upload record entry data only.
		public void UploadEntry(int rank, string record)
		{
			Debug.Log(string.Format("Uplaod toryward record entry. {0}.{1} Rank:{2} Record:{3}", ToryCare.Config.ContentName, ToryCare.Config.BranchCode, rank, record));

			Record = record;
			Rank = rank;

			HTTPRequest request = new HTTPRequest(new Uri("https://api.appery.io/rest/1/db/collections/RecordEntry"), HTTPMethods.Post, OnUploadEntryInfoFinished);
			request.SetHeader("X-Appery-Database-Id", ToryCare.Config.TorywardApperyDatabaseID);
			request.SetHeader("Content-Type", "application/json");
			request.RawData = Encoding.UTF8.GetBytes(HttpEntryPostData(ToryCare.Config.BranchCode, Rank, Record));
			request.Send();
		}

		// Upload record entry with photo.
		public void UploadEntry(byte[] imageData, int rank, string record)
		{
			Debug.Log(string.Format("Uplaod toryward record entry with photo. {0}.{1} Rank:{2} Record:{3}", ToryCare.Config.ContentName, ToryCare.Config.BranchCode, rank, record));

			Record = record;
			Rank = rank;

			string filename = DateTime.Now.ToString() + ".jpg";
			filename = filename.Replace('/', '_');
			filename = filename.Replace(':', '_');
			filename = filename.Replace(' ', '_');

			Debug.Log("# Upload photo : " + filename);

			HTTPRequest fileRequest = new HTTPRequest(new Uri("https://api.appery.io/rest/1/db/files/" + filename), HTTPMethods.Post, OnUploadEntryFinished);
			fileRequest.SetHeader("X-Appery-Database-Id", ToryCare.Config.TorywardApperyDatabaseID);
			fileRequest.SetHeader("X-Appery-Master-Key", ToryCare.Config.TorywardApperyMasterKey);
			fileRequest.SetHeader("Content-Type", "image/jpeg");

			fileRequest.RawData = imageData;
			fileRequest.Send();
		}

		void OnUploadEntryFinished(HTTPRequest request, HTTPResponse response)
		{
			Log(response);

			try
			{
				JSONObject jso = new JSONObject(response.DataAsText.ToString());
				string fileUrl = jso.GetField("fileurl").ToString();
				// Debug.Log(fileUrl);

				if (fileUrl.Length > 0)
				{
					UploadEntryInfo(fileUrl.Substring(1, fileUrl.Length - 2));
				}
			}
			catch (Exception e)
			{
				print(e.ToString());
			}
		}

		void UploadEntryInfo(string fileUrl)
		{
			HTTPRequest request = new HTTPRequest(new Uri("https://api.appery.io/rest/1/db/collections/RecordEntry"), HTTPMethods.Post, OnUploadEntryInfoFinished);
			request.SetHeader("X-Appery-Database-Id", ToryCare.Config.TorywardApperyDatabaseID);
			request.SetHeader("Content-Type", "application/json");
			request.RawData = Encoding.UTF8.GetBytes(HttpEntryPostData(ToryCare.Config.BranchCode, Rank, Record, fileUrl));
			request.Send();
		}

		String HttpEntryPostData(string branchCode, int rank, string record, string fileUrl = "")
		{
			string rawData = "{";
			if (!string.IsNullOrEmpty(fileUrl))
			{
				rawData += "\"fileUrl\":\"" + fileUrl + "\",";
			}
			rawData += "\"branch\":\"" + branchCode + "\",";
			rawData += "\"content\":\"" + ToryCare.Config.ContentName + "\",";
			rawData += "\"rank\":" + Rank + ",";
			rawData += "\"record\":\"" + Record + "\",";
			rawData += "\"isRewarded\":false,";
			#if UNITY_EDITOR
			rawData += "\"isTest\":true";
			#else
			rawData += "\"isTest\":false";
			#endif
			rawData += "}";

			return rawData;
		}

		void OnUploadEntryInfoFinished(HTTPRequest request, HTTPResponse response)
		{
			Log(response);
		}

		public void TakeSnap()
		{
			if (takeSnapCoroutine == null)
			{
				takeSnapCoroutine = StartCoroutine(TakeSnapCoroutine());
			}
		}

		Coroutine takeSnapCoroutine;
		IEnumerator TakeSnapCoroutine()
		{
			float waitTime = Time.timeSinceLevelLoad;

			UnityEngine.UI.RawImage tempRawImage = gameObject.GetComponent<UnityEngine.UI.RawImage>();
			if (tempRawImage == null)
			{
				tempRawImage = gameObject.AddComponent<UnityEngine.UI.RawImage>();
			}
			WebcamTextureManager.Instance.StartCamera(tempRawImage);

			yield return new WaitUntil(() => WebcamTextureManager.Instance.IsReady || Time.timeSinceLevelLoad > waitTime + 3f);

			if (WebcamTextureManager.Instance.IsReady)
			{
				if (Snap == null || Snap.width != WebcamTextureManager.Instance.Texture.width || Snap.height != WebcamTextureManager.Instance.Texture.height)
				{
					Snap = null;
					Resources.UnloadUnusedAssets();
					System.GC.Collect();

					yield return new WaitForEndOfFrame();
					Snap = new Texture2D(WebcamTextureManager.Instance.Texture.width, WebcamTextureManager.Instance.Texture.height);
				}

				yield return new WaitForSeconds(ToryCare.Config.TorywardTakePhotoDelay);

				RenderTexture currentRT = RenderTexture.active;

				RenderTexture renderTexture = new RenderTexture(WebcamTextureManager.Instance.Texture.width, WebcamTextureManager.Instance.Texture.height, 32);
				Graphics.Blit(WebcamTextureManager.Instance.Texture, renderTexture);
				RenderTexture.active = renderTexture;

				Snap.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
				Snap.Apply();

				RenderTexture.active = currentRT;

				yield return new WaitForSeconds(.1f);

				renderTexture = null;
				Resources.UnloadUnusedAssets();
				System.GC.Collect();

				// Create and get clipped snapshot photo.
				GameObject.Instantiate(playerSnapshotPrefab);
			}
			else
			{
				Debug.LogWarning("WebcamTexture seems to be null. Cannot take snap.");
			}

			yield return new WaitForSeconds(1f);
			WebcamTextureManager.Instance.StopCamera(tempRawImage, true);

			Destroy(tempRawImage, .1f);
			yield return new WaitForSeconds(.5f);

			takeSnapCoroutine = null;
		}

		void Log(HTTPResponse r)
		{
			Debug.Log(r.DataAsText);
		}

		void OnDestroy()
		{
			instance = null;
		}
	}
}