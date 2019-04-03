using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_14 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.SiPillar_14 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.SiPillar_14 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
