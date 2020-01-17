using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	public class WebcamTextureManager : MonoBehaviour
	{
		#region Singleton
		static volatile WebcamTextureManager instance;
		static readonly object syncRoot = new object();

		public static WebcamTextureManager Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = FindObjectOfType<WebcamTextureManager>();
						}
					}
				}
				return instance;
			}
		}
		#endregion

		WebCamTexture webcamTexture;
#if TORY_EYE_V3
		ToryEye.ToryEye toryEye;
#endif
		public Texture Texture
		{
			get
			{
#if TORY_EYE_V3
				if (webcamTexture != null)
				{
					return webcamTexture;
				}
				if (toryEye == null || toryEye.Texture.Textures.Length == 0)
				{
					return null;
				}
				return toryEye.Texture.Textures[0];
#else
				return webcamTexture;
#endif
			}
		}

		public bool IsReady
		{
			get
			{
#if TORY_EYE_V3
				if (webcamTexture != null && webcamTexture.width > 32)
				{
					return true;
				}
				else if (toryEye == null || toryEye.Webcam == null || toryEye.Texture.Textures.Length == 0)
				{
					return false;
				}
				return toryEye.Webcam.IsReady;
#else
				return webcamTexture != null;
#endif
			}
		}

		int WebcamIndex
		{
			get
			{
				return Mathf.Clamp(CamAdjustment.Instance.CameraIndex, 0, WebCamTexture.devices.Length - 1);
			}
			set
			{
				CamAdjustment.Instance.CameraIndex.Value = Mathf.Clamp(value, 0, WebCamTexture.devices.Length - 1);
			}
		}

		[HideInInspector]
		public List<Graphic> referencingGraphics;

		void Awake()
		{
			CreateSingleton();
			referencingGraphics = new List<Graphic>();
#if TORY_EYE_V3
			toryEye = FindObjectOfType<ToryEye.ToryEye>();
#endif

			CamAdjustment.Instance.CameraIndex.Load();
#if TORY_EYE_V3
			toryEye.Webcam.DeviceIndex.Load();
			if (WebcamIndex != toryEye.Webcam.DeviceIndex.Value)
#endif
			{
				webcamTexture = new WebCamTexture(WebCamTexture.devices[WebcamIndex].name);
			}
		}

		private void CreateSingleton()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
		}

		IEnumerator Start()
		{
			// HACK: 웹캠을 미리 Play()해야 정상적으로 텍스쳐를 가지고 오는 문제가 있어 넣은 코드
			if (webcamTexture != null)
			{
				webcamTexture.Play();
				yield return new WaitForSeconds(3f);
				webcamTexture.Stop();
			}
		}

		public void StartCamera(Graphic graphic)
		{
			if (graphic != null && !referencingGraphics.Contains(graphic))
			{
				if (graphic.GetType().Equals(typeof(RawImage)) ||
					graphic.GetType().Equals(typeof(Image)))
				{
					referencingGraphics.Add(graphic);
				}
			}

			PlayCamera();
		}

		void PlayCamera()
		{
			if (webcamTexture != null)
			{
				webcamTexture.Play();
			}
			ApplyReferencingTexture();
		}

		void ApplyReferencingTexture()
		{
			foreach (Graphic graphic in referencingGraphics)
			{
				if (graphic.GetType().Equals(typeof(RawImage)))
				{
					((RawImage)graphic).texture = Texture;
					((RawImage)graphic).material.mainTexture = Texture;
				}
				else if (graphic.GetType().Equals(typeof(Image)))
				{
					((Image)graphic).material.mainTexture = Texture;
				}
			}
		}

		public void StopCamera(Graphic graphic, bool keepTexture = false)
		{
			if (webcamTexture != null)
			{
				webcamTexture.Stop();
			}

			if (referencingGraphics.Contains(graphic))
			{
				referencingGraphics.Remove(graphic);

				if (graphic.GetType().Equals(typeof(RawImage)))
				{
					if (!keepTexture)
					{
						((RawImage)graphic).texture = null;
						((RawImage)graphic).material.mainTexture = null;
					}
				}
				else if (graphic.GetType().Equals(typeof(Image)))
				{
					if (!keepTexture)
					{
						((Image)graphic).material.mainTexture = null;
					}
				}
			}
		}

		void OnDestroy()
		{
			if (webcamTexture != null)
			{
				if (webcamTexture.isPlaying)
				{
					webcamTexture.Stop();
				}
				Destroy(webcamTexture);
			}
		}
	}
}