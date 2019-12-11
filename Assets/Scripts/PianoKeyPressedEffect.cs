using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class PianoKeyPressedEffect : MonoBehaviour
{
    public ParticleSystem[] pianoKeyPressedParticle;
    public ParticleSystem[] incorrectParticle;

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

        PianoKeyCollisionEvent.IDo_1 += IPlay_1;
        PianoKeyCollisionEvent.IRe_2 += IPlay_2;
        PianoKeyCollisionEvent.IMi_3 += IPlay_3;
        PianoKeyCollisionEvent.IFa_4 += IPlay_4;
        PianoKeyCollisionEvent.ISol_5 += IPlay_5;
        PianoKeyCollisionEvent.IRa_6 += IPlay_6;
        PianoKeyCollisionEvent.ISi_7 += IPlay_7;
        PianoKeyCollisionEvent.IDo_8 += IPlay_8;
        PianoKeyCollisionEvent.IRe_9 += IPlay_9;
        PianoKeyCollisionEvent.IMi_10 += IPlay_10;
        PianoKeyCollisionEvent.IFa_11 += IPlay_11;
        PianoKeyCollisionEvent.ISol_12 += IPlay_12;
        PianoKeyCollisionEvent.IRa_13 += IPlay_13;
        PianoKeyCollisionEvent.ISi_14 += IPlay_14;
        PianoKeyCollisionEvent.IDo_15 += IPlay_15;

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


        PianoKeyCollisionEvent.IDo_1 -= IPlay_1;
        PianoKeyCollisionEvent.IRe_2 -= IPlay_2;
        PianoKeyCollisionEvent.IMi_3 -= IPlay_3;
        PianoKeyCollisionEvent.IFa_4 -= IPlay_4;
        PianoKeyCollisionEvent.ISol_5 -= IPlay_5;
        PianoKeyCollisionEvent.IRa_6 -= IPlay_6;
        PianoKeyCollisionEvent.ISi_7 -= IPlay_7;
        PianoKeyCollisionEvent.IDo_8 -= IPlay_8;
        PianoKeyCollisionEvent.IRe_9 -= IPlay_9;
        PianoKeyCollisionEvent.IMi_10 -= IPlay_10;
        PianoKeyCollisionEvent.IFa_11 -= IPlay_11;
        PianoKeyCollisionEvent.ISol_12 -= IPlay_12;
        PianoKeyCollisionEvent.IRa_13 -= IPlay_13;
        PianoKeyCollisionEvent.ISi_14 -= IPlay_14;
        PianoKeyCollisionEvent.IDo_15 -= IPlay_15;
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



    void IPlay_1()
    {
        incorrectParticle[0].Play();
        Score.Lose(1);
    }
    void IPlay_2()
    {
        incorrectParticle[1].Play();
        Score.Lose(1);
    }
    void IPlay_3()
    {
        incorrectParticle[2].Play();
        Score.Lose(1);
    }
    void IPlay_4()
    {
        incorrectParticle[3].Play();
        Score.Lose(1);
    }
    void IPlay_5()
    {
        incorrectParticle[4].Play();
        Score.Lose(1);
    }
    void IPlay_6()
    {
        incorrectParticle[5].Play();
        Score.Lose(1);
    }
    void IPlay_7()
    {
        incorrectParticle[6].Play();
        Score.Lose(1);
    }
    void IPlay_8()
    {
        incorrectParticle[7].Play();
        Score.Lose(1);
    }
    void IPlay_9()
    {
        incorrectParticle[8].Play();
        Score.Lose(1);
    }
    void IPlay_10()
    {
        incorrectParticle[9].Play();
        Score.Lose(1);
    }
    void IPlay_11()
    {
        incorrectParticle[10].Play();
        Score.Lose(1);
    }
    void IPlay_12()
    {
        incorrectParticle[11].Play();
        Score.Lose(1);
    }
    void IPlay_13()
    {
        incorrectParticle[12].Play();
        Score.Lose(1);
    }
    void IPlay_14()
    {
        incorrectParticle[13].Play();
        Score.Lose(1);
    }
    void IPlay_15()
    {
        incorrectParticle[14].Play();
        Score.Lose(1);
    }
}
