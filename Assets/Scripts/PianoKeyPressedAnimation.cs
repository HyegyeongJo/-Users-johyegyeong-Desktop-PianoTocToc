using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyPressedAnimation : MonoBehaviour
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
            pianoKeyPressed[0].SetTrigger("KeyPressed");

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            pianoKeyPressed[1].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            pianoKeyPressed[2].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            pianoKeyPressed[3].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            pianoKeyPressed[4].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            pianoKeyPressed[5].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            pianoKeyPressed[6].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            pianoKeyPressed[7].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            pianoKeyPressed[8].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            pianoKeyPressed[9].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            pianoKeyPressed[10].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            pianoKeyPressed[11].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            pianoKeyPressed[12].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            pianoKeyPressed[13].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            pianoKeyPressed[14].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pianoKeyPressed[15].SetTrigger("KeyPressed");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pianoKeyPressed[16].SetTrigger("KeyPressed");
        }
    }
}
