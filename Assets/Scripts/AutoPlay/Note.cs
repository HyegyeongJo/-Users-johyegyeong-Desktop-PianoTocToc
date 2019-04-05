﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public AudioClip tone;
    public float speed = 0.01f;




    private void Start()
    {

        GetComponent<AudioSource>().clip = tone;


    }

    private void Update()
    {
        transform.Translate(0, -speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MainNoteDestroy") 
        {
            Destroy(this.gameObject, 1);
        }
        //else
        //{
        //    GetComponent<AudioSource>().Play();
        //}
    }
}