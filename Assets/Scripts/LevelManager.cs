using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
   int level = 1;

    public GameObject level1;
   


    private void OnEnable()
    {
        TF.Scene.Play.Started += StartPlay;
        TF.Scene.Play.Updated += UpdatePlay;
        TF.Scene.Play.Ended += EndedPlay;

        NoteCreator.ClearMusic += PlayerClearMusic;
    }

    private void OnDisable()
    {
        TF.Scene.Play.Started -= StartPlay;
        TF.Scene.Play.Updated -= UpdatePlay;
        TF.Scene.Play.Ended -= EndedPlay;

        NoteCreator.ClearMusic-= PlayerClearMusic;
    }


    void StartPlay()
    {

    }

    void EndedPlay()
    {

    }


    private void UpdatePlay()
    {
      Debug.Log(level);

     if(level == 2)
        {
            TF.Scene.Proceed();
        }
    }


    void PlayerClearMusic()
    {
        level += 1;
    }

}
