using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
   int level = 1;

    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;
    public GameObject level5;


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
          //  TF.Scene.Proceed();

            level1.SetActive(false);
            level2.SetActive(true);

        }
        if (level == 3)
        {
            level2.SetActive(false);
            level3.SetActive(true);
        }
     if(level == 4)
        {
            level3.SetActive(false);
            level4.SetActive(true);
        }
     if(level == 5)
        {
            level4.SetActive(false);
            level5.SetActive(true);
        }
     if(level == 6)
        {
            TF.Scene.Proceed();
        }
    }


    void PlayerClearMusic()
    {
        level += 1;
        Debug.Log("LEVEL~~~~" + level);
    }

}
