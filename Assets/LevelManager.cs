using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
   int level = 1;

    public GameObject level1;
    public GameObject level2;
  //  public GameObject level3;


    private void OnEnable()
    {
        Player.ClearMusic += PlayerClearMusic;
    }


    private void Update()
    {
     if(level == 2)
        {
            level1.SetActive(false);
            level2.SetActive(true);
        }
     if(level == 3)
        {

        }

    }

    void PlayerClearMusic()
    {
        level += 1;
        Debug.Log("LEVEL MANAGER~~~~");
    }

}
