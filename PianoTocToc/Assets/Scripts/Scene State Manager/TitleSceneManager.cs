﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject title;

    public GameObject piano;

    public GameObject pond_1;
    public GameObject spider_2;
    public GameObject rabbit_3;
    public GameObject bear_4;
    public GameObject funtorySonge_5;
    public GameObject freeMode_6;


    public GameObject[] stageBalloon;
    int stageNum = 0;
   // public GameObject scene_02;

    private void OnEnable()
    {
        TF.Scene.Title.Started += StartTitle;
        TF.Scene.Title.Updated += UpdateTitle;
        TF.Scene.Title.Ended += EndedTitle;

        TitleAnimation.PondClear += PondClearEvent;
        TitleAnimation.SpiderClear += SpiderClearEvent;
        TitleAnimation.RabbitClear += RabbitClearEvent;
        TitleAnimation.BearClear += BearClearEvent;
        TitleAnimation.FuntoryClear += FuntoryClearEvent;
        TitleAnimation.FreeClear+= FreeClearEvent;
    }

    private void OnDisable()
    {
        TF.Scene.Title.Started -= StartTitle;
        TF.Scene.Title.Updated -= UpdateTitle;
        TF.Scene.Title.Ended -= EndedTitle;

        TitleAnimation.PondClear -= PondClearEvent;
        TitleAnimation.SpiderClear -= SpiderClearEvent;
        TitleAnimation.RabbitClear -= RabbitClearEvent;
        TitleAnimation.BearClear -= BearClearEvent;
        TitleAnimation.FuntoryClear -= FuntoryClearEvent;
        TitleAnimation.FreeClear -= FreeClearEvent;
    }

    void StartTitle()
    {
        Score.Reset();
       // if(TF)
        //InvokeRepeating("StageBalloonAnimation", 1f, 6f);
        Debug.Log(TF.Scene.Title.StepIndex);
    }

    void UpdateTitle()
    {  

    }

    void PondClearEvent()
    {
        title.SetActive(false);
        piano.SetActive(true);

        pond_1.SetActive(true);

        ToryUX.Score.ShowUI();

        TF.Scene.Proceed();
    }

    void SpiderClearEvent()
    {
        title.SetActive(false);
        piano.SetActive(true);

        spider_2.SetActive(true);

        ToryUX.Score.ShowUI();

        TF.Scene.Proceed();
    }

    void RabbitClearEvent()
    {
        title.SetActive(false);
        piano.SetActive(true);

        rabbit_3.SetActive(true);

        ToryUX.Score.ShowUI();

        TF.Scene.Proceed();
    }

    void BearClearEvent()
    {
        title.SetActive(false);
        piano.SetActive(true);

        bear_4.SetActive(true);

        ToryUX.Score.ShowUI();

        TF.Scene.Proceed();
    }

    void FuntoryClearEvent()
    {
        title.SetActive(false);
        piano.SetActive(true);

        funtorySonge_5.SetActive(true);

        ToryUX.Score.ShowUI();

        TF.Scene.Proceed();
    }

    void FreeClearEvent()
    {
        title.SetActive(false);
        piano.SetActive(true);

        freeMode_6.SetActive(true);

        TF.Scene.Proceed();
    }

    void EndedTitle()
    {
        TF.Scene.Proceed();
    }
}
