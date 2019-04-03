using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_12 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.SolPillar_12 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.SolPillar_12 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
