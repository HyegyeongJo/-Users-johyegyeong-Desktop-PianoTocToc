using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_5 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.SolPillar_5 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.SolPillar_5 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
