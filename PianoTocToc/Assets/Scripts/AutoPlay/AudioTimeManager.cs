using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTimeManager : MonoBehaviour
{
    AudioSource audio;
    float time;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            time += 2f;
            time %= 20;
            audio.volume = 1;
            audio.time = time;
        }

        audio.volume -= Time.deltaTime;
    }


}
