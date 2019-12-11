using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accompaniment_Note : MonoBehaviour
{
    public AudioClip tone;
    float speed = 0.025f;

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
        GetComponent<AudioSource>().Play();
        Destroy(this.gameObject, 1);
    }
}
