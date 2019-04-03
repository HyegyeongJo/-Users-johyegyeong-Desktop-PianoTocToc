using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_16: MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.RePillar_16 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.RePillar_16 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
