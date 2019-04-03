using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_7 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.SiPillar_7 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.SiPillar_7 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
