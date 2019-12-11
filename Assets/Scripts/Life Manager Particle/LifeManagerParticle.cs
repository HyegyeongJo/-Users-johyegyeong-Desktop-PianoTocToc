using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManagerParticle : MonoBehaviour
{
    public ParticleSystem noteSignParticle;

    // Start is called before the first frame update
    void Start()
    {
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
    }
    void CircleParticle_2()
    {
    }
    void CircleParticle_3()
    {
    }
    void CircleParticle_4()
    {
    }
    void CircleParticle_5()
    {
    }
    void CircleParticle_6()
    {
    }
    void CircleParticle_7()
    {
    }
    void CircleParticle_8()
    {
        noteSignParticle.Play();
    }
    void CircleParticle_9()
    {
    }
    void CircleParticle_10()
    {
    }
    void CircleParticle_11()
    {
    }
    void CircleParticle_12()
    {
        noteSignParticle.Play();
    }
    void CircleParticle_13()
    {
    }
    void CircleParticle_14()
    {
        noteSignParticle.Play();
    }
    void CircleParticle_15()
    {
        noteSignParticle.Play();
    }
    void CircleParticle_16()
    {
    }

}
