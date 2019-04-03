using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_1 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.DoPillar_1 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.DoPillar_1 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
