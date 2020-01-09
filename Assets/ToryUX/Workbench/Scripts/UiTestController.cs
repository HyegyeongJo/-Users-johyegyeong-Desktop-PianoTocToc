using UnityEngine;
using UnityEngine.SceneManagement;

public class UiTestController : MonoBehaviour
{
	[SerializeField] KeyCode proceedTonextUi = KeyCode.F;
	[SerializeField] KeyCode gainScore = KeyCode.G;
	int status;

	void OnEnable()
	{
		ToryUX.ResultUI.OnCountDownStart += OnCountDownStarted;
		ToryUX.ResultUI.OnFadedOut += ReloadScene;
	}

	void OnDisable()
	{
		ToryUX.ResultUI.OnCountDownStart -= OnCountDownStarted;
		ToryUX.ResultUI.OnFadedOut -= ReloadScene;
	}

	void OnCountDownStarted()
	{
		if (status == 2)
		{
			status++;
		}
	}

	void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		ToryUX.Score.Reset();
	}

	void Update()
	{
		if (Input.GetKeyDown(proceedTonextUi))
		{
			switch (status)
			{
				case 0:
					ToryUX.TitleUI.Hide();
					ToryUX.Score.ShowUI();
					status++;
					break;

				case 1:
					ToryUX.Score.HideUI();
					ToryUX.TorywardManager.Instance.TakeSnap();
					ToryUX.ResultUI.Show();
					status++;
					break;

				case 3:
					ToryUX.ResultUI.FadeOut();
					break;

				default:
					break;
			}
		}
		else if (Input.GetKeyDown(gainScore))
		{
			if (status == 1)
			{
				ToryUX.Score.Gain();
			}
		}
	}
}
