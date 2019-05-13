using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public GameObject[] circleFill;
    public GameObject[] noteSign;
    public GameObject[] restSign;

    int loseLifeNum;

    private void OnEnable()
    {
        PianoKeyCollisionEvent.IDo_1 += LoseLife;
        PianoKeyCollisionEvent.IRe_2 += LoseLife;
        PianoKeyCollisionEvent.IMi_3 += LoseLife;
        PianoKeyCollisionEvent.IFa_4 += LoseLife;
        PianoKeyCollisionEvent.ISol_5 += LoseLife;
        PianoKeyCollisionEvent.IRa_6 += LoseLife;
        PianoKeyCollisionEvent.ISi_7 += LoseLife;
        PianoKeyCollisionEvent.IDo_8 += LoseLife;
        PianoKeyCollisionEvent.IRe_9 += LoseLife;
        PianoKeyCollisionEvent.IMi_10 += LoseLife;
        PianoKeyCollisionEvent.IFa_11 += LoseLife;
        PianoKeyCollisionEvent.ISol_12 += LoseLife;
        PianoKeyCollisionEvent.IRa_13 += LoseLife;
        PianoKeyCollisionEvent.ISi_14 += LoseLife;
        PianoKeyCollisionEvent.IDo_15 += LoseLife;

        TF.Scene.Play.Started += StartPlay;
        TF.Scene.Play.Updated += UpdatePlay;
        TF.Scene.Play.Ended += EndedPlay;
    }


    private void OnDisable()
    {
        PianoKeyCollisionEvent.IDo_1 -= LoseLife;
        PianoKeyCollisionEvent.IRe_2 -= LoseLife;
        PianoKeyCollisionEvent.IMi_3 -= LoseLife;
        PianoKeyCollisionEvent.IFa_4 -= LoseLife;
        PianoKeyCollisionEvent.ISol_5 -= LoseLife;
        PianoKeyCollisionEvent.IRa_6 -= LoseLife;
        PianoKeyCollisionEvent.ISi_7 -= LoseLife;
        PianoKeyCollisionEvent.IDo_8 -= LoseLife;
        PianoKeyCollisionEvent.IRe_9 -= LoseLife;
        PianoKeyCollisionEvent.IMi_10 -= LoseLife;
        PianoKeyCollisionEvent.IFa_11 -= LoseLife;
        PianoKeyCollisionEvent.ISol_12 -= LoseLife;
        PianoKeyCollisionEvent.IRa_13 -= LoseLife;
        PianoKeyCollisionEvent.ISi_14 -= LoseLife;
        PianoKeyCollisionEvent.IDo_15 -= LoseLife;

        TF.Scene.Play.Started -= StartPlay;
        TF.Scene.Play.Updated -= UpdatePlay;
        TF.Scene.Play.Ended -= EndedPlay;
    }


    private void StartPlay()
    {
        loseLifeNum = 0;
    }

    private void UpdatePlay()
    {

    }

    private void EndedPlay()
    {

    }

    private void LoseLife()
    {
        loseLifeNum += 1;

        if(loseLifeNum == 1)
        {
            circleFill[0].SetActive(false);
            noteSign[0].SetActive(false);
            noteSign[1].SetActive(true);
        }
        if (loseLifeNum == 2)
        {
            circleFill[1].SetActive(false);
            noteSign[1].SetActive(false);
            noteSign[2].SetActive(true);
        }
        if (loseLifeNum == 3)
        {
            circleFill[2].SetActive(false);
            noteSign[2].SetActive(false);
            noteSign[3].SetActive(true);
        }
        if (loseLifeNum == 4)
        {
            circleFill[3].SetActive(false);
            noteSign[3].SetActive(false);
            noteSign[4].SetActive(true);
        }
        if (loseLifeNum == 5)
        {
            circleFill[4].SetActive(false);
            noteSign[4].SetActive(false);
            noteSign[5].SetActive(true);
        }
        if (loseLifeNum == 6)
        {
            circleFill[5].SetActive(false);
            noteSign[5].SetActive(false);
            noteSign[6].SetActive(true);
        }
        if (loseLifeNum == 7)
        {
            circleFill[6].SetActive(false);
            noteSign[6].SetActive(false);
            noteSign[7].SetActive(true);
        }
        if (loseLifeNum == 8)
        {
            circleFill[7].SetActive(false);
            noteSign[7].SetActive(false);
        }

        if (loseLifeNum == 9)
        {
            circleFill[8].SetActive(false);
            restSign[0].SetActive(false);
            restSign[1].SetActive(true);
        }
        if (loseLifeNum == 10)
        {
            circleFill[9].SetActive(false);
            restSign[1].SetActive(false);
            restSign[2].SetActive(true);
        }
        if (loseLifeNum == 11)
        {
            circleFill[10].SetActive(false);
            restSign[2].SetActive(false);
            restSign[3].SetActive(true);
        }
        if (loseLifeNum == 12)
        {
            circleFill[11].SetActive(false);
            restSign[3].SetActive(false);
            restSign[4].SetActive(true);
        }
        if (loseLifeNum == 13)
        {
            circleFill[12].SetActive(false);
            restSign[4].SetActive(false);
            restSign[5].SetActive(true);
        }
        if (loseLifeNum == 14)
        {
            circleFill[13].SetActive(false);
            restSign[5].SetActive(false);
            restSign[6].SetActive(true);
        }
        if (loseLifeNum == 15)
        {
            circleFill[14].SetActive(false);
            restSign[6].SetActive(false);
            restSign[7].SetActive(true);
        }
        if (loseLifeNum == 16)
        {
            circleFill[15].SetActive(false);
            restSign[7].SetActive(false);
            TF.Scene.Proceed();
        }
    }
}
