using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class PianoKeyPressedEffect : MonoBehaviour
{
    public ParticleSystem[] pianoKeyPressedParticle; 
    
    private void OnEnable()
    {
        PianoKeyCollisionEvent.DoOn_1 += Play_1;
        PianoKeyCollisionEvent.ReOn_2 += Play_2;
        PianoKeyCollisionEvent.MiOn_3 += Play_3;
        PianoKeyCollisionEvent.FaOn_4 += Play_4;
        PianoKeyCollisionEvent.SolOn_5 += Play_5;
        PianoKeyCollisionEvent.RaOn_6 += Play_6;
        PianoKeyCollisionEvent.SiOn_7 += Play_7;
        PianoKeyCollisionEvent.DoOn_8 += Play_8;
        PianoKeyCollisionEvent.ReOn_9 += Play_9;
        PianoKeyCollisionEvent.MiOn_10 += Play_10;
        PianoKeyCollisionEvent.FaOn_11 += Play_11;
        PianoKeyCollisionEvent.SolOn_12 += Play_12;
        PianoKeyCollisionEvent.RaOn_13 += Play_13;
        PianoKeyCollisionEvent.SiOn_14 += Play_14;
        PianoKeyCollisionEvent.DoOn_15 += Play_15;
        PianoKeyCollisionEvent.ReOn_16 += Play_16;
        PianoKeyCollisionEvent.MiOn_17 += Play_17;
    }

    private void OnDisable()
    {
        PianoKeyCollisionEvent.DoOn_1 -= Play_1;
        PianoKeyCollisionEvent.ReOn_2 -= Play_2;
        PianoKeyCollisionEvent.MiOn_3 -= Play_3;
        PianoKeyCollisionEvent.FaOn_4 -= Play_4;
        PianoKeyCollisionEvent.SolOn_5 -= Play_5;
        PianoKeyCollisionEvent.RaOn_6 -= Play_6;
        PianoKeyCollisionEvent.SiOn_7 -= Play_7;
        PianoKeyCollisionEvent.DoOn_8 -= Play_8;
        PianoKeyCollisionEvent.ReOn_9 -= Play_9;
        PianoKeyCollisionEvent.MiOn_10 -= Play_10;
        PianoKeyCollisionEvent.FaOn_11 -= Play_11;
        PianoKeyCollisionEvent.SolOn_12 -= Play_12;
        PianoKeyCollisionEvent.RaOn_13 -= Play_13;
        PianoKeyCollisionEvent.SiOn_14 -= Play_14;
        PianoKeyCollisionEvent.DoOn_15 -= Play_15;
        PianoKeyCollisionEvent.ReOn_16 -= Play_16;
        PianoKeyCollisionEvent.MiOn_17 -= Play_17;
    }


    private void Start()
    {
        for(int i = 0; i < pianoKeyPressedParticle.Length; i++)
        {
            pianoKeyPressedParticle[i].GetComponent<ParticleSystem>();
        }
    }

    void Play_1()
    {
        pianoKeyPressedParticle[0].Play();
        Score.Gain(1);
    }
    void Play_2()
    {
        pianoKeyPressedParticle[1].Play();
        Score.Gain(1);
    }
    void Play_3()
    {
        pianoKeyPressedParticle[2].Play();
        Score.Gain(1);
    }
    void Play_4()
    {
        pianoKeyPressedParticle[3].Play();
        Score.Gain(1);
    }
    void Play_5()
    {
        pianoKeyPressedParticle[4].Play();
        Score.Gain(1);
    }
    void Play_6()
    {
        pianoKeyPressedParticle[5].Play();
        Score.Gain(1);
    }
    void Play_7()
    {
        pianoKeyPressedParticle[6].Play();
        Score.Gain(1);
    }
    void Play_8()
    {
        pianoKeyPressedParticle[7].Play();
        Score.Gain(1);
    }
    void Play_9()
    {
        pianoKeyPressedParticle[8].Play();
        Score.Gain(1);
    }
    void Play_10()
    {
        pianoKeyPressedParticle[9].Play();
        Score.Gain(1);
    }
    void Play_11()
    {
        pianoKeyPressedParticle[10].Play();
        Score.Gain(1);
    }
    void Play_12()
    {
        pianoKeyPressedParticle[11].Play();
        Score.Gain(1);
    }
    void Play_13()
    {
        pianoKeyPressedParticle[12].Play();
        Score.Gain(1);
    }
    void Play_14()
    {
        pianoKeyPressedParticle[13].Play();
        Score.Gain(1);
    }
    void Play_15()
    {
        pianoKeyPressedParticle[14].Play();
        Score.Gain(1);
    }
    void Play_16()
    {
        pianoKeyPressedParticle[15].Play();
        Score.Gain(1);
    }
    void Play_17()
    {
        pianoKeyPressedParticle[16].Play();
        Score.Gain(1);
    }
}
