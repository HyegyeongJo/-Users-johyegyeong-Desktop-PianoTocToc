using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleArrowAnimation : MonoBehaviour
{
    public Animator blueArrow;
    public Animator orangeArrow;
    public Animator yellowArrow;
    public Animator pinkArrow;
    public Animator greenArrow;
    public Animator redArrow;


    private void OnEnable()
    {
        TF.Scene.Title.Started += StartTitle;
        TF.Scene.Title.Updated += UpdateTitle;
    }

    private void OnDisable()
    {
        TF.Scene.Title.Started -= StartTitle;
        TF.Scene.Title.Updated -= UpdateTitle;
    }


    private void StartTitle()
    {
        blueArrow.GetComponent<Animator>();
        orangeArrow.GetComponent<Animator>();
        yellowArrow.GetComponent<Animator>();
        pinkArrow.GetComponent<Animator>();
        greenArrow.GetComponent<Animator>();
        redArrow.GetComponent<Animator>();
    }


    private void UpdateTitle()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            blueArrow.SetTrigger("BtPressed");

            orangeArrow.SetTrigger("AnotherBtPressed");
            yellowArrow.SetTrigger("AnotherBtPressed");
            pinkArrow.SetTrigger("AnotherBtPressed");
            greenArrow.SetTrigger("AnotherBtPressed");
            redArrow.SetTrigger("AnotherBtPressed");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            orangeArrow.SetTrigger("BtPressed");

            blueArrow.SetTrigger("AnotherBtPressed");
            yellowArrow.SetTrigger("AnotherBtPressed");
            pinkArrow.SetTrigger("AnotherBtPressed");
            greenArrow.SetTrigger("AnotherBtPressed");
            redArrow.SetTrigger("AnotherBtPressed");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            yellowArrow.SetTrigger("BtPressed");

            blueArrow.SetTrigger("AnotherBtPressed");
            orangeArrow.SetTrigger("AnotherBtPressed");
            pinkArrow.SetTrigger("AnotherBtPressed");
            greenArrow.SetTrigger("AnotherBtPressed");
            redArrow.SetTrigger("AnotherBtPressed");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            pinkArrow.SetTrigger("BtPressed");

            blueArrow.SetTrigger("AnotherBtPressed");
            orangeArrow.SetTrigger("AnotherBtPressed");
            yellowArrow.SetTrigger("AnotherBtPressed");
            greenArrow.SetTrigger("AnotherBtPressed");
            redArrow.SetTrigger("AnotherBtPressed");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            greenArrow.SetTrigger("BtPressed");

            blueArrow.SetTrigger("AnotherBtPressed");
            orangeArrow.SetTrigger("AnotherBtPressed");
            yellowArrow.SetTrigger("AnotherBtPressed");
            pinkArrow.SetTrigger("AnotherBtPressed");
            redArrow.SetTrigger("AnotherBtPressed");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            redArrow.SetTrigger("BtPressed");

            blueArrow.SetTrigger("AnotherBtPressed");
            orangeArrow.SetTrigger("AnotherBtPressed");
            yellowArrow.SetTrigger("AnotherBtPressed");
            pinkArrow.SetTrigger("AnotherBtPressed");
            greenArrow.SetTrigger("AnotherBtPressed");
        }
    }





}
