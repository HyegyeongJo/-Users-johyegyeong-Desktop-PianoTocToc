using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ToryCare
{
	public class ToryCareBehaviour : MonoBehaviour
	{
		#region Singleton
		static volatile ToryCareBehaviour instance;
		static readonly object syncRoot = new object();

		public static ToryCareBehaviour Instance
		{
			get
			{
				if (instance == null)
				{
					lock(syncRoot)
					{
						if (instance == null)
						{
							instance = FindObjectOfType<ToryCareBehaviour>();
						}
					}
				}
				return instance;
			}
		}
		#endregion

		// [Header("Content Info")]
		public string contentNameInKorean = "한글 콘텐츠명";

		static List<KeyCode> observingKeycodes;
		public event Action<KeyCode> SpecialKeyPressed = (k) =>
		{};

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

			observingKeycodes = new List<KeyCode>();

			foreach (string s in ToryCare.Config.ObservingKeycode)
			{
				observingKeycodes.Add((KeyCode) Enum.Parse(typeof(KeyCode), s));
			}
		}

		void Update()
		{
			foreach (KeyCode keyCode in observingKeycodes)
			{
				if (Input.GetKeyDown(keyCode))
				{
					SpecialKeyPressed(keyCode);
				}
			}
		}
	}
}