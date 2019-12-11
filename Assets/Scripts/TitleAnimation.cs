using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public delegate void TitleClearEvent();
    public static event TitleClearEvent PondClear = () => { };
    public static event TitleClearEvent SpiderClear = () => { };
    public static event TitleClearEvent RabbitClear = () => { };
    public static event TitleClearEvent BearClear = () => { };
    public static event TitleClearEvent FuntoryClear = () => { };
    public static event TitleClearEvent FreeClear = () => { };

    public Animator pond;
    public Animator[] pondCircle;

    public Animator spider;
   // public Animator[] spiderCircle;

    public Animator rabbit;
    //public Animator[] rabbitCircle;

    public Animator bear;
    //public Animator[] bearCircle;

    public Animator funtorySong;
    //public Animator[] funtorySongcircle;

    public Animator freeMode;
    //public Animator[] freemodeCircle;


    int pondNum = 0;
    int spiderNum = 0;
    int rabbitNum = 0;
    int bearNum = 0;
    int funtoryNum = 0;
    int freeNum = 0;

    private void OnEnable()
    {
        TF.Scene.Title.Started += StartTitle;
        TF.Scene.Title.Updated += UpdateTitle;
        TF.Scene.Title.Ended += EndedTitle;
    }

    private void OnDisable()
    {
        TF.Scene.Title.Started -= StartTitle;
        TF.Scene.Title.Updated -= UpdateTitle;
        TF.Scene.Title.Ended -= EndedTitle;
    }


    void StartTitle()
    {
        pond.GetComponent<Animator>();
        spider.GetComponent<Animator>();
        rabbit.GetComponent<Animator>();
        bear.GetComponent<Animator>();
        funtorySong.GetComponent<Animator>();
        freeMode.GetComponent<Animator>();

        for(int i =0; i< pondCircle.Length; i++)
        {
            pondCircle[i].GetComponent<Animator>();
        }


        //for (int i = 0; i < spiderCircle.Length; i++)
        //{
        //    spiderCircle[i].GetComponent<Animator>();
        //}
        //for (int i = 0; i < rabbitCircle.Length; i++)
        //{
        //    rabbitCircle[i].GetComponent<Animator>();
        //}
        //for (int i = 0; i < bearCircle.Length; i++)
        //{
        //    bearCircle[i].GetComponent<Animator>();
        //}
        //for (int i = 0; i < funtorySongcircle.Length; i++)
        //{
        //    funtorySongcircle[i].GetComponent<Animator>();
        //}
        //for (int i = 0; i < freemodeCircle.Length; i++)
        //{
        //    freemodeCircle[i].GetComponent<Animator>();
        //}
    }

    void UpdateTitle()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(pondNum);
            pondNum += 1;

            pond.SetTrigger("BtPressed");

            spider.SetTrigger("AnotherBtPressedSpider");
            rabbit.SetTrigger("AnotherBtPressedRabbit");
            bear.SetTrigger("AnotherBtPressedBear");
            funtorySong.SetTrigger("AnotherBtPressedFuntory");
            freeMode.SetTrigger("AnotherBtPressedFree");

            spiderNum = 0;
            rabbitNum = 0;
            bearNum = 0;
            funtoryNum = 0;
            freeNum = 0;

            if(pondNum == 2)
            {
                PondClear();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(spiderNum);
            spiderNum += 1;

            spider.SetTrigger("BtPressedSpider");

            pond.SetTrigger("AnotherBtPressed");
            rabbit.SetTrigger("AnotherBtPressedRabbit");
            bear.SetTrigger("AnotherBtPressedBear");
            funtorySong.SetTrigger("AnotherBtPressedFuntory");
            freeMode.SetTrigger("AnotherBtPressedFree");

            pondNum = 0;
            rabbitNum = 0;
            bearNum = 0;
            funtoryNum = 0;
            freeNum = 0;

            if (spiderNum == 2)
            {
                SpiderClear();
            }
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(rabbitNum);
            rabbitNum += 1;

            rabbit.SetTrigger("BtPressedRabbit");

            pond.SetTrigger("AnotherBtPressed");
            spider.SetTrigger("AnotherBtPressedSpider");
            bear.SetTrigger("AnotherBtPressedBear");
            funtorySong.SetTrigger("AnotherBtPressedFuntory");
            freeMode.SetTrigger("AnotherBtPressedFree");

            pondNum = 0;
            spiderNum = 0;
            bearNum = 0;
            funtoryNum = 0;
            freeNum = 0;

            if (rabbitNum == 2)
            {
                RabbitClear();
            }
        }


        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(bearNum);
            bearNum += 1;

            bear.SetTrigger("BtPressedBear");

            pond.SetTrigger("AnotherBtPressed");
            spider.SetTrigger("AnotherBtPressedSpider");
            rabbit.SetTrigger("AnotherBtPressedRabbit");
            funtorySong.SetTrigger("AnotherBtPressedFuntory");
            freeMode.SetTrigger("AnotherBtPressedFree");

            pondNum = 0;
            spiderNum = 0;
            rabbitNum = 0;
            funtoryNum = 0;
            freeNum = 0;

            if (bearNum == 2)
            {
                BearClear();
            }
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(funtoryNum);
            funtoryNum += 1;

            funtorySong.SetTrigger("BtPressedFuntory");

            pond.SetTrigger("AnotherBtPressed");
            spider.SetTrigger("AnotherBtPressedSpider");
            rabbit.SetTrigger("AnotherBtPressedRabbit");
            bear.SetTrigger("AnotherBtPressedBear");
            freeMode.SetTrigger("AnotherBtPressedFree");

            pondNum = 0;
            spiderNum = 0;
            rabbitNum = 0;
            bearNum = 0;
            freeNum = 0;

            if (funtoryNum == 2) 
            {
                FuntoryClear();
            }
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(freeNum);
            freeNum += 1;

            freeMode.SetTrigger("BtPressedFree");

            pond.SetTrigger("AnotherBtPressed");
            spider.SetTrigger("AnotherBtPressedSpider");
            rabbit.SetTrigger("AnotherBtPressedRabbit");
            bear.SetTrigger("AnotherBtPressedBear");
            funtorySong.SetTrigger("AnotherBtPressedFuntory");

            pondNum = 0;
            spiderNum = 0;
            rabbitNum = 0;
            bearNum = 0;
            funtoryNum = 0;

            if (freeNum == 2)
            {
                FreeClear();
            }
        }
    }


    void EndedTitle()
    {
        TF.Scene.Proceed();
    }
}
