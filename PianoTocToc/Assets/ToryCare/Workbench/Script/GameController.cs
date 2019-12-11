using System.Collections;
using System.Collections.Generic;
using ToryUX;
using UnityEngine;

namespace ToryCare
{
	public class GameController : MonoBehaviour
	{
		#region Singleton

		static volatile GameController instance;
		static readonly object syncRoot = new object();

		public static GameController Instance
		{
			get
			{
				if (instance == null)
				{
					lock(syncRoot)
					{
						if (instance == null)
						{
							instance = FindObjectOfType<GameController>();
						}
					}
				}
				return instance;
			}
		}

		#endregion

		public ToryValue.ToryBool sendVital;
		public ToryValue.ToryBool SendVital
		{
			get
			{
				return sendVital;
			}
			set
			{
				sendVital = value;
			}
		}

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

			SendVital.Value = true;
			SendVital.Save();
		}

		#if TORY_FRAMEWORK
		/*
		void Start()
		{
			ToryScene.Instance.Title.Started += () =>
			{
				TitleUI.Show();
			};

			ToryScene.Instance.Title.Ended += () =>
			{
				TitleUI.Hide();
			};

			ToryScene.Instance.Result.Started += () =>
			{
				ResultUI.Show();
			};

			ToryScene.Instance.Result.Ended += () =>
			{
				if (ResultUI.Instance.gameObject.activeInHierarchy)
				{
					ResultUI.FadeOut();
				}
			};

			ToryInput.Instance.Interacted += () =>
			{
				if (ToryScene.Instance.Current == ToryScene.Instance.Title)
				{
					ToryScene.Instance.Proceed();
				}
			};

			ToryInput.Instance.Interacted += () =>
			{
				if (ToryScene.Instance.Current == ToryScene.Instance.Title)
				{
					ToryScene.Instance.Proceed();
				}
			};

			ToryInput.Instance.PlayerLeft += () =>
			{
				// Debug.Log("Player has left!");
			};
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.Space))
			{
				ToryInput.Instance.SetRawValue(1f);
			}
			else
			{
				ToryInput.Instance.SetRawValue(0f);
			}

			if (Input.GetKey(KeyCode.Alpha1))
			{
				ToryScene.Instance.Start(ToryScene.Instance.Title);
			}
			if (Input.GetKey(KeyCode.Alpha2))
			{
				ToryScene.Instance.Start(ToryScene.Instance.Play);
			}
			if (Input.GetKey(KeyCode.Alpha3))
			{
				ToryScene.Instance.Start(ToryScene.Instance.Result);
			}
		}
		*/
		#endif
	}
}