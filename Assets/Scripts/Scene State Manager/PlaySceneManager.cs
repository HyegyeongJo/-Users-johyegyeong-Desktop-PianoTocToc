using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class PlaySceneManager : MonoBehaviour
{
    public GameObject lightPillar;
    public GameObject lifeManager;

    private void OnEnable()
    {
        TF.Scene.Play.Started += StartPlay;
        TF.Scene.Play.Updated += UpdatePlay;
        TF.Scene.Play.Ended += EndedPlay;
    }

    private void OnDisable()
    {
        TF.Scene.Play.Started -= StartPlay;
        TF.Scene.Play.Updated -= UpdatePlay;
        TF.Scene.Play.Ended -= EndedPlay;
    }

    void StartPlay()
    {
        ToryUX.TitleUI.Hide();
        lightPillar.SetActive(true);
        lifeManager.SetActive(true);
    }

    void UpdatePlay()
    {

    }

    void EndedPlay()
    {

    }
}
