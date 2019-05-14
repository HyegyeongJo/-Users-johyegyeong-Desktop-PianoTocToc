using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManagerParticle : MonoBehaviour
{
    public ParticleSystem[] gauageFillParticle;
    public ParticleSystem noteSignParticle;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gauageFillParticle.Length; i++)
        {
            gauageFillParticle[i].GetComponent<ParticleSystem>();
        }
        
        noteSignParticle.GetComponent<ParticleSystem>();

    }

    private void OnEnable()
    {
        LifeManager.LoseLifeNum1 += CircleParticle_1;
        LifeManager.LoseLifeNum2 += CircleParticle_2;
        LifeManager.LoseLifeNum3 += CircleParticle_3;
        LifeManager.LoseLifeNum4 += CircleParticle_4;
        LifeManager.LoseLifeNum5 += CircleParticle_5;
        LifeManager.LoseLifeNum6 += CircleParticle_6;
        LifeManager.LoseLifeNum7 += CircleParticle_7;
        LifeManager.LoseLifeNum8 += CircleParticle_8;
        LifeManager.LoseLifeNum9 += CircleParticle_9;
        LifeManager.LoseLifeNum10 += CircleParticle_10;
        LifeManager.LoseLifeNum11 += CircleParticle_11;
        LifeManager.LoseLifeNum12 += CircleParticle_12;
        LifeManager.LoseLifeNum13 += CircleParticle_13;
        LifeManager.LoseLifeNum14 += CircleParticle_14;
        LifeManager.LoseLifeNum15 += CircleParticle_15;
        LifeManager.LoseLifeNum16 += CircleParticle_15;
    }

    private void OnDisable()
    {
        LifeManager.LoseLifeNum1 -= CircleParticle_1;
        LifeManager.LoseLifeNum2 -= CircleParticle_2;
        LifeManager.LoseLifeNum3 -= CircleParticle_3;
        LifeManager.LoseLifeNum4 -= CircleParticle_4;
        LifeManager.LoseLifeNum5 -= CircleParticle_5;
        LifeManager.LoseLifeNum6 -= CircleParticle_6;
        LifeManager.LoseLifeNum7 -= CircleParticle_7;
        LifeManager.LoseLifeNum8 -= CircleParticle_8;
        LifeManager.LoseLifeNum9 -= CircleParticle_9;
        LifeManager.LoseLifeNum10 -= CircleParticle_10;
        LifeManager.LoseLifeNum11 -= CircleParticle_11;
        LifeManager.LoseLifeNum12 -= CircleParticle_12;
        LifeManager.LoseLifeNum13 -= CircleParticle_13;
        LifeManager.LoseLifeNum14 -= CircleParticle_14;
        LifeManager.LoseLifeNum15 -= CircleParticle_15;
        LifeManager.LoseLifeNum16 -= CircleParticle_15;
    }

    void CircleParticle_1()
    {
        gauageFillParticle[0].Play();
    }
    void CircleParticle_2()
    {
        gauageFillParticle[1].Play();
    }
    void CircleParticle_3()
    {
        gauageFillParticle[2].Play();

    }
    void CircleParticle_4()
    {
        gauageFillParticle[3].Play();

    }
    void CircleParticle_5()
    {
        gauageFillParticle[4].Play();

    }
    void CircleParticle_6()
    {
        gauageFillParticle[5].Play();

    }
    void CircleParticle_7()
    {
        gauageFillParticle[6].Play();

    }
    void CircleParticle_8()
    {
        gauageFillParticle[7].Play();
        noteSignParticle.Play();
    }
    void CircleParticle_9()
    {
        gauageFillParticle[8].Play();

    }
    void CircleParticle_10()
    {
        gauageFillParticle[9].Play();

    }
    void CircleParticle_11()
    {
        gauageFillParticle[10].Play();

    }
    void CircleParticle_12()
    {
        gauageFillParticle[11].Play();
        noteSignParticle.Play();
    }
    void CircleParticle_13()
    {
        gauageFillParticle[12].Play();
    }
    void CircleParticle_14()
    {
        gauageFillParticle[13].Play();
        noteSignParticle.Play();
    }
    void CircleParticle_15()
    {
        gauageFillParticle[14].Play();
        noteSignParticle.Play();
    }
    void CircleParticle_16()
    {
        gauageFillParticle[15].Play();
    }

}
