using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_15 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.DoPillar_15 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.DoPillar_15 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
