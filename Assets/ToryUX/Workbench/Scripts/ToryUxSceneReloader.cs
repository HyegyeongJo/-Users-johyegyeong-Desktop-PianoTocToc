using UnityEngine;
using UnityEngine.SceneManagement;

public class ToryUxSceneReloader : MonoBehaviour
{
	void Start()
	{
		ToryUX.Score.Reset();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			SceneManager.LoadScene(0);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ToryUX.TitleUI.Hide();
			ToryUX.Score.ShowUI();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ToryUX.Score.Gain();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ToryUX.Score.HideUI();
			ToryUX.TorywardManager.Instance.TakeSnap();
			ToryUX.ResultUI.Show();
		}
	}
}
