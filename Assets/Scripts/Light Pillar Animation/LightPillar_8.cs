using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_8 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.DoPillar_8 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.DoPillar_8 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
