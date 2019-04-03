using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class FinishLine : MonoBehaviour
{
    bool resultSceneGo;

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
        resultSceneGo = false;
    }

    void UpdatePlay()
    {
        if(resultSceneGo)
        {
            TF.Scene.Proceed();
        }
    }

    void EndedPlay()
    {
        TF.Scene.Proceed();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "FinishCollider")
        {
            Debug.Log("Finish Line Collision!!!");
            resultSceneGo = true;
        }
    }
}
