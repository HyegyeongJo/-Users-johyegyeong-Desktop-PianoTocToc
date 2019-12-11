using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviour
{
	[SerializeField]
	ToryUX.ShowAndHideAnimationPlayer networkErrorSpritePrefab;
	ToryUX.ShowAndHideAnimationPlayer networkErrorSprite;
	
	public UnityEvent onNetworkUnreachable;
	public UnityEvent onNetworkRecovered;

	// static string[] pingIPs = {
	// 	"8.8.8.8", // google-public-dns.a.google.com
	// 	"8.8.4.4" // google-public-dns.b.google.com
	// };

	static bool[] pingSuccess;
	static int pingIPIndex = 0;

	static bool AllPingFailed
	{
		get
		{
			for (int i = 0; i < pingSuccess.Length; i++)
			{
				if (pingSuccess[i])
				{
					return false;
				}
			}
			return true;
		}
	}
	static bool showingNetworkAlertMessage = false;

	void Awake()
	{
		if (!ToryCare.Config.DoNetworkPingTest)
		{
			Destroy(this);
		}
		pingSuccess = new bool[ToryCare.Config.NetworkTestPingIP.Length];
	}

	Coroutine pingCoroutine;
	IEnumerator Ping()
	{
		Ping ping;
		float pingTime;

		while (true)
		{
			ping = new Ping(ToryCare.Config.NetworkTestPingIP[pingIPIndex]);
			pingTime = Time.timeSinceLevelLoad;

			yield return new WaitUntil(() => ping.isDone || Time.timeSinceLevelLoad > pingTime + ToryCare.Config.NetworkPingTestFailDelay);
			pingSuccess[pingIPIndex] = ping.isDone;

			ping = null;

			if (!pingSuccess[pingIPIndex])
			{
				if (AllPingFailed && !showingNetworkAlertMessage)
				{
					if (networkErrorSpritePrefab != null)
					{
						if (networkErrorSprite == null)
						{
							networkErrorSprite = GameObject.Instantiate(networkErrorSpritePrefab, GetComponentInChildren<ToryUX.UIOrientationSetter>().transform);
						}
						else if (!networkErrorSprite.gameObject.activeInHierarchy)
						{
							networkErrorSprite.gameObject.SetActive(true);
						}
					}

					onNetworkUnreachable.Invoke();
					showingNetworkAlertMessage = true;
					Debug.LogWarning(string.Format("[{0}] Network connectivity seems to be unstable. Ping delay is over {1} seconds.", System.DateTime.Now, ToryCare.Config.NetworkPingTestFailDelay * ToryCare.Config.NetworkTestPingIP.Length));
				}
				else
				{
					pingIPIndex = (pingIPIndex + 1) % ToryCare.Config.NetworkTestPingIP.Length;
				}
			}
			else
			{
				for (int i = 0; i < pingSuccess.Length; i++)
				{
					pingSuccess[i] = true;
				}

				if (showingNetworkAlertMessage)
				{
					if (networkErrorSprite != null && networkErrorSprite.gameObject.activeInHierarchy)
					{
						networkErrorSprite.PlayHideAnimation();
					}

					onNetworkRecovered.Invoke();
					showingNetworkAlertMessage = false;
					Debug.LogWarning(string.Format("[{0}] Network connectivity recovered.", System.DateTime.Now));
				}
			}

			// randomize wait delay for not being banned from dns server (...will it works???)
			yield return new WaitForSeconds(Random.Range(1f, 10f));
		}
	}

	void OnEnable()
	{
		pingIPIndex = 0;
		for (int i = 0; i < pingSuccess.Length; i++)
		{
			pingSuccess[i] = true;
		}

		if (pingCoroutine != null)
		{
			StopCoroutine(pingCoroutine);
		}
		pingCoroutine = StartCoroutine(Ping());
	}

	void OnDisable()
	{
		if (pingCoroutine != null)
		{
			StopCoroutine(pingCoroutine);
		}
		pingCoroutine = null;
	}

	public static void Reset()
	{
		showingNetworkAlertMessage = false;
	}
}