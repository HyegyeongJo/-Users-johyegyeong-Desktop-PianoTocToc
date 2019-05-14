using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class ResultSceneManager : MonoBehaviour
{
    public GameObject[] stageLevelManager;

    private void OnEnable()
    {
        TF.Scene.Result.Started += StartResult;
        TF.Scene.Result.Updated += UpdateResult;
        TF.Scene.Result.Ended += EndedResult;
        ResultUI.OnCountDownFinish += EndedResult;

    }

    private void OnDisable()
    {
        TF.Scene.Result.Started -= StartResult;
        TF.Scene.Result.Updated -= UpdateResult;
        TF.Scene.Result.Ended -= EndedResult;
        ResultUI.OnCountDownFinish -= EndedResult;
    }

    void StartResult()
    {
        ResultUI.Show();
        Score.HideUI();

        for (int i = 0; i < stageLevelManager.Length; i++)
        {
            stageLevelManager[i].SetActive(false);
        }
    }

    void UpdateResult()
    {
        if(Input.anyKey && ResultUI.countDownStarted)
        {
            TF.Scene.LoadToryScene();
        }
    }

    void EndedResult()
    {
        TF.Scene.LoadToryScene();
    }

}
