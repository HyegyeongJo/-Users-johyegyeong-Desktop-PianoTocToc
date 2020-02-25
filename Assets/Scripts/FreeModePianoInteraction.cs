using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FreeModePianoInteraction : MonoBehaviour
{
    public ParticleSystem[] pianoPressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            pianoPressed[0].Play();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            pianoPressed[1].Play();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            pianoPressed[2].Play();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            pianoPressed[3].Play();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            pianoPressed[4].Play();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            pianoPressed[5].Play();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            pianoPressed[6].Play();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            pianoPressed[7].Play();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            pianoPressed[8].Play();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            pianoPressed[9].Play();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            pianoPressed[10].Play();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            pianoPressed[11].Play();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            pianoPressed[12].Play();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            pianoPressed[13].Play();
        }

    }
}
