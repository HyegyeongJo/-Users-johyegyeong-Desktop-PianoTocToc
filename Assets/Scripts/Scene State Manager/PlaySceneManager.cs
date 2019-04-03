using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class PlaySceneManager : MonoBehaviour
{
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
    }

    void UpdatePlay()
    {

    }

    void EndedPlay()
    {

    }
}
