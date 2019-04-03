using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_13 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.RaPillar_13 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.RaPillar_13 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
