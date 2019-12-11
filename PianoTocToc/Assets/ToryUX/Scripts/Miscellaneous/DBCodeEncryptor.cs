using System.Collections;
using System.Collections.Generic;
using AvoEx;
using ToryUX;
using UnityEngine;
using UnityEngine.UI;

public class DBCodeEncryptor : MonoBehaviour
{
	[SerializeField]
	InputField dbCodeInputField, resultInputField;

	void Awake()
	{
		dbCodeInputField.onEndEdit.AddListener((s) =>
		{
			if (!string.IsNullOrEmpty(s))
			{
				string result = "";
				result = AesEncryptor.Encrypt(s);
				resultInputField.text = result;

				Debug.Log(string.Format("\"{0}\" is encrypted to \"{1}\"", s, result));

				Debug.Log(AesEncryptor.DecryptString(result));
			}
		});
	}

	void Update()
	{
		if (!SettingsUI.IsShown)
		{
			SettingsUI.Show();
		}
	}
}