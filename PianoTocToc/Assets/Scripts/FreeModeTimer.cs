using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class FreeModeTimer : MonoBehaviour
{
    private void OnEnable()
    {
        TF.Scene.Play.Started += StartPlay;
        TF.Scene.Play.Updated += UpdatePlay;
    }

    private void OnDisable()
    {
        TF.Scene.Play.Started -= StartPlay;
        TF.Scene.Play.Updated -= UpdatePlay;
    }

    void StartPlay()
    {
        Timer.ShowUI();
        Timer.Start();
    }

    void UpdatePlay()
    {
        if(Timer.CurrentTime == 0)
        {
            Timer.HideUI();
            TF.Scene.Proceed();

        }

    }
}
