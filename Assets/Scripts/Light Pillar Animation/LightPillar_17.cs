using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_17 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.MiPillar_17 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.MiPillar_17 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
