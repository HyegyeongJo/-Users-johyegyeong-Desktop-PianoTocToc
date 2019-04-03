using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPressedAnimation : MonoBehaviour
{
  public Animator[] pianoKeyPressed;


    void Start()
    {
        for (int i =0; i<pianoKeyPressed.Length; i++)
        {
           pianoKeyPressed[i].GetComponent<Animator>();
        }

    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.A))
        {
            pianoKeyPressed[0].SetTrigger("Pressed");

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            pianoKeyPressed[1].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            pianoKeyPressed[2].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            pianoKeyPressed[3].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            pianoKeyPressed[4].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            pianoKeyPressed[5].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            pianoKeyPressed[6].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            pianoKeyPressed[7].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            pianoKeyPressed[8].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            pianoKeyPressed[9].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            pianoKeyPressed[10].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            pianoKeyPressed[11].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            pianoKeyPressed[12].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            pianoKeyPressed[13].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            pianoKeyPressed[14].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pianoKeyPressed[15].SetTrigger("Pressed");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pianoKeyPressed[16].SetTrigger("Pressed");
        }
    }
}
