using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RewardCardObject : MonoBehaviour
{
	Texture2D cardTexture;
	Animator animator;
	string cardUrl;

	void OnEnable()
	{
		if (loadCardTextureCoroutine == null)
		{
			loadCardTextureCoroutine = StartCoroutine(LoadCardTextureCoroutine());
		}
	}

	void OnDisable()
	{
		if (loadCardTextureCoroutine != null)
		{
			StopCoroutine(LoadCardTextureCoroutine());
			loadCardTextureCoroutine = null;
		}

		if (cardTexture != null)
		{
			cardTexture = null;
			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}
	}

	Coroutine loadCardTextureCoroutine;
	IEnumerator LoadCardTextureCoroutine()
	{
		if (string.IsNullOrEmpty(cardUrl))
		{
			string cardImageFolder = Path.Combine(ToryCare.Config.DataRootDirectory, "RewardCard");
			if (!Directory.Exists(cardImageFolder))
			{
				Directory.CreateDirectory(cardImageFolder);
			}

			yield return new WaitUntil(() => Directory.Exists(cardImageFolder));
			cardUrl = Path.Combine(cardImageFolder, ToryCare.Config.RewardCardImageFilename);
		}

		if (File.Exists(cardUrl))
		{
			byte[] fileData;
			fileData = File.ReadAllBytes(cardUrl);

			cardTexture = new Texture2D(2, 2);
			cardTexture.LoadImage(fileData);

			GetComponent<Renderer>().material.mainTexture = cardTexture;
		}

		loadCardTextureCoroutine = null;
	}
}