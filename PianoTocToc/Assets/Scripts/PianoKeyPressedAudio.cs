using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyPressedAudio : MonoBehaviour
{
    public AudioClip[] pianoNote;
    private AudioSource pianoNoteSource;


    private void Start()
    {
        pianoNoteSource = GetComponent<AudioSource>();

        for (int i = 0; i < pianoNote.Length; i++)
        {
            pianoNoteSource.clip = pianoNote[i];
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            pianoNoteSource.PlayOneShot(pianoNote[0]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            pianoNoteSource.PlayOneShot(pianoNote[1]);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            pianoNoteSource.PlayOneShot(pianoNote[2]);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            pianoNoteSource.PlayOneShot(pianoNote[3]);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            pianoNoteSource.PlayOneShot(pianoNote[4]);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            pianoNoteSource.PlayOneShot(pianoNote[5]);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            pianoNoteSource.PlayOneShot(pianoNote[6]);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            pianoNoteSource.PlayOneShot(pianoNote[7]);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            pianoNoteSource.PlayOneShot(pianoNote[8]);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            pianoNoteSource.PlayOneShot(pianoNote[9]);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            pianoNoteSource.PlayOneShot(pianoNote[10]);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            pianoNoteSource.PlayOneShot(pianoNote[11]);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            pianoNoteSource.PlayOneShot(pianoNote[12]);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            pianoNoteSource.PlayOneShot(pianoNote[13]);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            pianoNoteSource.PlayOneShot(pianoNote[14]);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pianoNoteSource.PlayOneShot(pianoNote[15]);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pianoNoteSource.PlayOneShot(pianoNote[16]);
        }
    }
}
