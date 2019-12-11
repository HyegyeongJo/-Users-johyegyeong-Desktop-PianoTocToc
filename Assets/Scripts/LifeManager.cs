using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public delegate void LifeManagerEventHandler();
    public static event LifeManagerEventHandler LoseLifeNum1 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum2 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum3 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum4 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum5 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum6 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum7 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum8 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum9 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum10 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum11 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum12 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum13 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum14 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum15 = () => { };
    public static event LifeManagerEventHandler LoseLifeNum16 = () => { };

    public GameObject[] noteSign;

    int loseLifeNum;

    [SerializeField] Animator lifeBarAnim;

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
        lifeBarAnim = GetComponent<Animator>();
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
            lifeBarAnim.SetTrigger("LoseLife1");
            LoseLifeNum1();
        }
        if (loseLifeNum == 2)
        {
            lifeBarAnim.SetTrigger("LoseLife2");
            LoseLifeNum2();
        }
        if (loseLifeNum == 3)
        {
            lifeBarAnim.SetTrigger("LoseLife3");
            LoseLifeNum3();
        }
        if (loseLifeNum == 4)
        {
            lifeBarAnim.SetTrigger("LoseLife4");
            LoseLifeNum4();
        }
        if (loseLifeNum == 5)
        {
            lifeBarAnim.SetTrigger("LoseLife5");
            LoseLifeNum5();
        }
        if (loseLifeNum == 6)
        {
            lifeBarAnim.SetTrigger("LoseLife6");
            LoseLifeNum6();
        }
        if (loseLifeNum == 7)
        {
            lifeBarAnim.SetTrigger("LoseLife7");
            LoseLifeNum7();
        }
        if (loseLifeNum == 8)
        {
            lifeBarAnim.SetTrigger("LoseLife8");
            noteSign[0].SetActive(false);
            noteSign[1].SetActive(true);
            LoseLifeNum8();
        }

        if (loseLifeNum == 9)
        {
            lifeBarAnim.SetTrigger("LoseLife9");
            noteSign[1].SetActive(true);

            LoseLifeNum9();
        }
        if (loseLifeNum == 10)
        {
            lifeBarAnim.SetTrigger("LoseLife10");
            noteSign[1].SetActive(true);

            LoseLifeNum10();
        }
        if (loseLifeNum == 11)
        {
            lifeBarAnim.SetTrigger("LoseLife11");
            noteSign[1].SetActive(true);

            LoseLifeNum11();
        }
        if (loseLifeNum == 12)
        {
            lifeBarAnim.SetTrigger("LoseLife12");
            noteSign[1].SetActive(false);
            noteSign[2].SetActive(true);
            LoseLifeNum12();

        }
        if (loseLifeNum == 13)
        {
            lifeBarAnim.SetTrigger("LoseLife13");
            noteSign[2].SetActive(true);
            LoseLifeNum13();

        }
        if (loseLifeNum == 14)
        {
            lifeBarAnim.SetTrigger("LoseLife14");
            noteSign[2].SetActive(false);
            noteSign[3].SetActive(true);
            LoseLifeNum14();

        }
        if (loseLifeNum == 15)
        {
            lifeBarAnim.SetTrigger("LoseLife15");
            noteSign[3].SetActive(false);
            noteSign[4].SetActive(true);
            LoseLifeNum15();

        }
        if (loseLifeNum == 16)
        {
            lifeBarAnim.SetTrigger("LoseLife16");
            noteSign[4].SetActive(false);

            TF.Scene.Proceed();
            LoseLifeNum16();
        }
    }
}
