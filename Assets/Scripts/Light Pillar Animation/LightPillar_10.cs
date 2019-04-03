using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_10 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.MiPillar_10 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.MiPillar_10 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
