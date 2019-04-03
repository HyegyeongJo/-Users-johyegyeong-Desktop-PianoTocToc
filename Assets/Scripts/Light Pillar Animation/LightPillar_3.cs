using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_3 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.MiPillar_3 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.MiPillar_3 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
